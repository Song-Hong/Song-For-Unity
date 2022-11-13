using System;
using System.Collections.Generic;
using System.IO;
using Song.Editor.Core.Data;
using Song.Editor.Core.Tools;
using Song.Runtime.Core.Data;
using Song.Runtime.Support;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Song.Editor.FileFormat
{
    public class LangEditorWindow : EditorWindow
    {
        #region Field
        public static string LangPath;
        private Runtime.Core.Data.Lang _lang;
        private Runtime.Core.Data.Lang _langData;
        private string _langName;
        private bool _isUndo;
        private VisualElement _topMenu;
        private VisualElement _bottomPanel;
        private VisualElement _bottomRowTools;
        private VisualElement _bottomColTools;
        private GroupBox _table;
        private Color _bgc1;
        private Color _choose1;
        private int _maxRow;
        private int _maxCol;
        private Label _nowChoose;
        private string _colStr;
        private string _rowStr;
        #endregion
        
        [MenuItem("Song/Lang")]
        public static void ShowLangEditor()
        {
            var wnd = GetWindow<LangEditorWindow>();
            if (string.IsNullOrWhiteSpace(LangPath))
            {
                wnd.Close();
            }

            wnd.titleContent = new GUIContent(Path.GetFileName(LangPath));
        }

        private void CreateGUI()
        {
            _langName = SongEditorUtility.GetLangName();
            _langData = Config.LoadLang("Assets/Song/Editor/Others/Config/Lang/LangFileSupport.songlang");
            if (string.IsNullOrWhiteSpace(LangPath))
            {
                SongFileSelection.ShowWindow("songlang", delegate(string path)
                {
                    LangPath = path;
                    ShowLangEditor();
                }, delegate { }, _langData[_langName]["FindTip"]);
                return;
            }

            Init();
            CreateTable();
            CreateTopTools();
            InitTable();
            BottomPlane();
        }

        private bool Init()
        {
            _lang = Config.LoadLang(LangPath);
            _bgc1 = new Color(0.176f, 0.176f, 0.176f);
            _choose1 = new Color(0.141f, 0.647f, 0.949f);
            _maxRow = 0;
            _maxCol = 0;
            _colStr = _langData[_langName]["Col"];
            _rowStr = _langData[_langName]["Row"];
            return true;
        }
        
        private void CreateTopTools()
        {
            _topMenu = new VisualElement()
            {
                name = "TopMenu",
                style =
                {
                    width = Length.Percent(100),
                    height = 28,
                    backgroundColor = _bgc1
                }
            };
            
            //add new property
            var add = new Button()
            {
                text = _langData[_langName]["new"],
                style =
                {
                    marginTop = 4,
                    marginLeft = 10,
                    position = Position.Absolute,
                    width = 60,
                    height = 20,
                }
            };
            add.clicked += delegate
            {
                _maxRow++;
                _table.Add(CreateNode(_maxRow, 1, _langData[_langName]["NewProperty"]));
                for (int i = 2; i < _maxCol+1; i++)
                {
                    _table.Add(CreateNode(_maxRow, i, ""));
                }
                CreateRowManager(_maxRow);
            };
            
            //add new lang
            var addlang = new Button()
            {
                text = _langData[_langName]["new_lang"],
                style =
                {
                    marginTop = 4,
                    marginLeft = 80,
                    position = Position.Absolute,
                    width = 60,
                    height = 20
                }
            };
            addlang.clicked += delegate
            {
                _maxCol++;
                _table.Add(CreateNode(1, _maxCol, _langData[_langName]["NewLange"]));
                for (int i = 2; i < _maxRow+1; i++)
                {
                    _table.Add(CreateNode(i, _maxCol, ""));
                }
                CreateColManager(_maxCol);
            };
            
            //save file
            var save = new Button()
            {
                text = _langData[_langName]["save"],
                style =
                {
                    marginTop = 4,
                    marginLeft = 150,
                    position = Position.Absolute,
                    width = 60,
                    height = 20
                }
            };
            save.clicked += delegate
            {
                var pres = new List<string>();
                for (int r = 1; r <= _maxRow; r++)
                {
                    var node = _table.Q<TextField>($"({r},{1})");
                    if (node != null)
                    {
                        pres.Add(node.text.Trim());
                    }
                }

                var langNames = new List<string>();
                for (int c = 1; c <= _maxCol; c++)
                {
                    var node = _table.Q<TextField>($"({1},{c})");
                    if (node != null)
                    {
                        langNames.Add(node.text.Trim());
                    }
                }
                
                var lang = new Runtime.Core.Data.Lang();
                for (int c = 2; c <= _maxCol; c++)
                {
                    var data = new Set();
                    for (int r = 2; r <= _maxRow; r++)
                    {
                        var pre = pres[r - 1];
                        if(string.IsNullOrWhiteSpace(pre))break;
                        var node = _table.Q<TextField>($"({r},{c})");
                        if (node != null)
                        {
                            data[pres[r-1]] = node.text.Trim();
                        }
                    }
                    lang[langNames[c-1]] = data;
                }
                lang.savepath = LangPath;
                lang.SaveAsync();
            };
            _topMenu.Add(add);
            _topMenu.Add(addlang);
            _topMenu.Add(save);
            rootVisualElement.Add(_topMenu);
        }

        private void CreateTable()
        {
            ScrollView scroll = new ScrollView();
            _table = new GroupBox();
            _table.name = "Table";
            scroll.style.marginTop = 28;
            scroll.style.position = Position.Absolute;
            scroll.Add(_table);
            _table.StretchToParentSize();
            rootVisualElement.Add(scroll);
            scroll.StretchToParentSize();
        }

        private void InitTable()
        {
            //table row 1
            _table.Add(CreateNode(1, 1, _langData[_langName]["Attribute"]));
            CreateRowManager(1);
            CreateColManager(1);
            int h = 2;
            string langname = null;
            foreach (var item in _lang.datas.Keys)
            {
                _table.Add(CreateNode(1, h, item));
                h++;
                if(langname == null)
                    langname = item;
            }

            if (langname == null)
            {
                _maxCol = 1;
                _maxRow = 1;
                return;
            }
            
            //table col 1
            int i = 2;
            foreach (var item in _lang[langname].datas.Keys)
            {
                _table.Add(CreateNode(i++, 1, item));
            }
            
            //fill table value
            int c = 2;
            // CreateRowManager(2);
            foreach (var item in _lang.datas)
            {
                if (c > _maxCol)
                {
                    CreateColManager(c);
                    _maxCol = c;
                }
                int r = 2;
                var key = item.Key;
                foreach (var data in _lang[key].datas)
                {
                    if (r > _maxRow)
                    {
                        CreateRowManager(r);
                        _maxRow = r;
                    }
                    _table.Add(CreateNode(r, c, data.Value));
                    r++;
                }
                c++;
            }
        }

        private TextField CreateNode(int row, int col, string value)
        {
            var node = new TextField
            {
                style =
                {
                    position = Position.Absolute,
                    top = 40 + 30 * (row-1),
                    left = 32 + 124 * (col-1),
                    height = 26,
                    width = 120,
                    backgroundColor = _bgc1,
                    borderTopLeftRadius = 8,
                    borderTopRightRadius = 8,
                    borderBottomLeftRadius = 8,
                    borderBottomRightRadius = 8
                },
                name = $"({row},{col})",
                value = " " + value
            };
            node.RegisterCallback<MouseDownEvent>(x =>
            {
                var panel = _table.Q<VisualElement>("choosePanel");
                if (panel != null) _table.Remove(panel);
                _nowChoose.text = $"{_rowStr} { node.name.Substring(1,1)}," +
                                  $" {_colStr} {node.name.Substring(3,1)}";
                _bottomRowTools.style.display = DisplayStyle.None;
                _bottomColTools.style.display = DisplayStyle.None;
            });
            return node;
        }

        private void CreateRowManager(int row)
        {
            var rowbtn = new Label()
            {
                text = row.ToString(),
                name = "row" + row,
                style=
                {
                    position = Position.Absolute,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    left = 2,
                    top = 40 + 30 * (row - 1),
                    width = 26,
                    height = 26,
                    backgroundColor = _bgc1,
                    borderTopLeftRadius = 8,
                    borderTopRightRadius = 8,
                    borderBottomLeftRadius = 8,
                    borderBottomRightRadius = 8
                }
            };
            rowbtn.RegisterCallback<MouseDownEvent>(x =>
            {
                RowClick(row);
            });
            _table.Add(rowbtn);
        }

        public void RowClick(int row)
        {
            var rowbtn = _table.Q<Label>("row" + row);
            if(rowbtn==null) return;
            // _bottomColTools.focusable = false;
            // _bottomRowTools.focusable = true;
            _bottomColTools.style.display = DisplayStyle.None;
            _bottomRowTools.style.display = DisplayStyle.Flex;
                var panel = _table.Q<VisualElement>("choosePanel");
                if (panel != null) _table.Remove(panel);
                _nowChoose.text = $"{_rowStr} {row}, {_colStr} *";
                var box = new VisualElement()
                {
                    name = "choosePanel",
                    style =
                    {
                        position = Position.Absolute,
                        unityTextAlign = TextAnchor.MiddleCenter,
                        left = 2,
                        top = 39+30 * (row - 1),
                        width = 30+126 * _maxCol,
                        height = 30,
                        backgroundColor = _choose1,
                        borderTopLeftRadius = 8,
                        borderTopRightRadius = 8,
                        borderBottomLeftRadius = 8,
                        borderBottomRightRadius = 8
                    }
                };
                _table.Add(box);
                box.SendToBack();
                var boxName = new TextField()
                {
                    value = row.ToString(),
                    style =
                    {
                        unityTextAlign = TextAnchor.MiddleCenter,
                        left = 2,
                        top = 2,
                        width = 26,
                        height = 26,
                    }
                };
                box.Add(boxName);
                boxName.RegisterValueChangedCallback(x =>
                {
                    int.TryParse(x.newValue, out int result);
                    if(String.CompareOrdinal(result.ToString(),x.newValue)!=0) return;;
                    if (x.previousValue != x.newValue)
                    {
                        MoveRowNode(int.Parse(x.previousValue),result);
                    }

                    box.style.top = 10 + 30 * (result - 1);
                });
                if (row == 1) boxName.focusable = false;
                rowbtn.SendToBack();
        }

        public void CreateColManager(int col)
        {
            var colbtn = new Label()
            {
                text = col.ToString(),
                name = "col" + col,
                style =
                {
                    position = Position.Absolute,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    left = 34 + 124 * (col-1),
                    top = 10,
                    width = 120,
                    height = 26,
                    backgroundColor = _bgc1,
                    borderTopLeftRadius = 8,
                    borderTopRightRadius = 8,
                    borderBottomLeftRadius = 8,
                    borderBottomRightRadius = 8
                }
            };
            colbtn.RegisterCallback<MouseDownEvent>(x =>
            {
                ColClick(col);
            });
            _table.Add(colbtn);
        }

        public void ColClick(int col)
        {
            var colbtn = _table.Q<Label>("col" + col);
            if(colbtn==null) return;
            // _bottomColTools.focusable = true;
            // _bottomRowTools.focusable = false;
            _bottomRowTools.style.display = DisplayStyle.None;
            _bottomColTools.style.display = DisplayStyle.Flex;
                var panel = _table.Q<VisualElement>("choosePanel");
            if (panel != null) _table.Remove(panel);
            _nowChoose.text = $"{_rowStr} *, {_colStr} {col}";
            var box = new VisualElement()
            {
                name = "choosePanel",
                style =
                {
                    position = Position.Absolute,
                    unityTextAlign = TextAnchor.MiddleCenter,
                    left = 32 + 124*(col-1),
                    top = 8,
                    width = 126,
                    height = 30+31*_maxRow,
                    backgroundColor = _choose1,
                    borderTopLeftRadius = 8,
                    borderTopRightRadius = 8,
                    borderBottomLeftRadius = 8,
                    borderBottomRightRadius = 8
                }
            };
            _table.Add(box);
            box.SendToBack();
            var boxName = new TextField()
            {
                value = col.ToString(),
                style =
                {
                    unityTextAlign = TextAnchor.MiddleCenter,
                    left = 2,
                    top = 2,
                    width = 116,
                    height = 26,
                }
            };
            box.Add(boxName);
            boxName.RegisterValueChangedCallback(x =>
            {
                int.TryParse(x.newValue, out int result);
                if(String.CompareOrdinal(result.ToString(),x.newValue)!=0) return;;
                if (x.previousValue != x.newValue)
                {
                    MoveColNode(int.Parse(x.previousValue),result);
                }

                box.style.left = 32 + 124 * (result - 1);
            });
            if (col == 1) boxName.focusable = false;
            colbtn.SendToBack();
        }

        private void BottomPlane()
        {
            _bottomPanel = new VisualElement
            {
                style =
                {
                    width = Length.Percent(100),
                    top = Length.Percent(96),
                    height = 30,
                    backgroundColor = _bgc1,
                    position = Position.Absolute,
                    flexDirection = FlexDirection.RowReverse
                }
            };
            _nowChoose = new Label()
            {
                text = $"{_rowStr} 0, {_colStr} 0",
                style =
                {
                    height = 20,
                    top = 5,
                    right = 5
                }
            };
            _bottomPanel.Add(_nowChoose);
            rootVisualElement.Add(_bottomPanel);

            #region Bottom Row Tools
            _bottomRowTools = new VisualElement();
            _bottomPanel.Add(_bottomRowTools);
            _bottomRowTools.StretchToParentWidth();
            _bottomRowTools.style.flexDirection = FlexDirection.Column;
            var moveUp = new Button()
            {
                text = "↑",
                style =
                {
                    position = Position.Absolute,
                    width = 20,
                    height = 20,
                    left = 5
                }
            };
            var moveDown = new Button()
            {
                text = "↓",
                style =
                {
                    position = Position.Absolute,
                    width = 20,
                    height = 20,
                    left = 30
                }
            };
            var del = new Button()
            {
                text = "-",
                style =
                {
                    position = Position.Absolute,
                    width = 20,
                    height = 20,
                    left = 55
                }
            };
            _bottomRowTools.Add(del);
            _bottomRowTools.Add(moveUp);
            _bottomRowTools.Add(moveDown);

            moveUp.clicked += delegate
            {
                var value = _nowChoose.text;
                var now = value.Substring(value.IndexOf(',') - 1, 1);
                int.TryParse(now, out int result);
                if (String.CompareOrdinal(result.ToString(), now) == 0)
                {
                    MoveRowNode(result, result - 1);
                }
            };
            moveDown.clicked += delegate
            {
                var value = _nowChoose.text;
                var now = value.Substring(value.IndexOf(',') - 1, 1);
                int.TryParse(now, out int result);
                if (String.CompareOrdinal(result.ToString(), now) == 0)
                {
                    MoveRowNode(result, result + 1);
                }
            };
            del.clicked += delegate
            {
                TipWindow(
                    _langData[_langName]["DelTip"],
                    _langData[_langName]["Cancel"],
                    _langData[_langName]["Agree"],
                    delegate { },
                    delegate
                    {
                        var value = _nowChoose.text;
                        var now = value.Substring(value.IndexOf(',') - 1, 1);
                        int.TryParse(now, out int result);
                        if (String.CompareOrdinal(result.ToString(), now) == 0)
                        {
                            MoveRowNode(result, _maxRow);
                        }

                        for (int i = 1; i <= _maxCol; i++)
                        {
                            var node = _table.Q<TextField>($"({_maxRow},{i})");
                            if (node != null)
                            {
                                _table.Remove(node);
                            }
                        }

                        var rowi = _table.Q<Label>($"row{_maxRow}");
                        if (rowi != null)
                        {
                            _table.Remove(rowi);
                            _maxRow--;
                            var panel = _table.Q<VisualElement>("choosePanel");
                            if (panel != null) _table.Remove(panel);
                            _nowChoose.text = "";
                        }
                    }
                );
            };
            _bottomRowTools.style.display = DisplayStyle.None;
            #endregion

            #region Bottom Col Tools
            _bottomColTools = new VisualElement();
            _bottomPanel.Add(_bottomColTools);
            _bottomColTools.StretchToParentWidth();
            _bottomColTools.style.flexDirection = FlexDirection.Column;
            var moveLeft = new Button()
            {
                text = "←",
                style =
                {
                    position = Position.Absolute,
                    width = 20,
                    height = 20,
                    left = 5
                }
            };
            var moveRight = new Button()
            {
                text = "→",
                style =
                {
                    position = Position.Absolute,
                    width = 20,
                    height = 20,
                    left = 30
                }
            };
            var delLang = new Button()
            {
                text = "-",
                style =
                {
                    position = Position.Absolute,
                    width = 20,
                    height = 20,
                    left = 55
                }
            };
            _bottomColTools.Add(delLang);
            _bottomColTools.Add(moveLeft);
            _bottomColTools.Add(moveRight);

            moveLeft.clicked += delegate
            {
                var value = _nowChoose.text;
                var now = value.Substring(value.Length  - 1, 1);
                int.TryParse(now, out int result);
                if (String.CompareOrdinal(result.ToString(), now) == 0)
                {
                    MoveColNode(result, result - 1);
                }
            };
            moveRight.clicked += delegate
            {
                var value = _nowChoose.text;
                var now = value.Substring(value.Length  - 1, 1);
                int.TryParse(now, out int result);
                if (String.CompareOrdinal(result.ToString(), now) == 0)
                {
                    MoveColNode(result, result + 1);
                }
            };
            delLang.clicked += delegate
            {
                TipWindow(
                    _langData[_langName]["DelTip"],
                    _langData[_langName]["Cancel"],
                    _langData[_langName]["Agree"],
                    delegate { },
                    delegate
                    {
                        var value = _nowChoose.text;
                        var now = value.Substring(value.Length- 1, 1);
                        int.TryParse(now, out int result);
                        if (String.CompareOrdinal(result.ToString(), now) == 0)
                        {
                            MoveColNode(result, _maxCol);
                        }

                        for (int i = 1; i <= _maxRow; i++)
                        {
                            var node = _table.Q<TextField>($"({i},{_maxCol})");
                            if (node != null)
                            {
                                _table.Remove(node);
                            }
                        }

                        var coli = _table.Q<Label>($"col{_maxCol}");
                        if (coli != null)
                        {
                            _table.Remove(coli);
                            _maxCol--;
                            var panel = _table.Q<VisualElement>("choosePanel");
                            if (panel != null) _table.Remove(panel);
                            _nowChoose.text = "";
                        }
                    }
                );
            };
            _bottomColTools.style.display = DisplayStyle.None;
            #endregion
        }

        private void MoveRowNode(int now,int target)
        {
            if(target< 1 || target >_maxRow) return;
            var tTable = new Dictionary<string, VisualElement>();
            for (int c = 0; c <= _maxCol; c++)
            {
                var node = _table.Q<VisualElement>($"({now},{c})");
                if (node == null) continue;
                node.style.top = 10 + 30 * (target);
                node.name = "";
                tTable.Add($"({target},{c})",node);
            }
            if (target > now)
            {
                for (int r = target; r > now; r--)
                {
                    for (int c = 0; c <= _maxCol; c++)
                    {
                        var node = _table.Q<VisualElement>($"({r},{c})");
                        if (node == null) continue;
                        node.style.top = 10 + 30 * (r - 1);
                        node.name = $"({r-1},{c})";
                    }
                }
            }
            else
            {
                for (int r = target; r < now; r++)
                {
                    for (int c = 0; c <= _maxCol; c++)
                    {
                        var node = _table.Q<VisualElement>($"({r},{c})");
                        if (node == null) continue;
                        node.style.top = 10 + 30 * (r+1);
                        node.name = $"({r+1},{c})";
                    }
                }
            }
            foreach (var value in tTable)
            {
                value.Value.name = value.Key;
            }
            RowClick(target);
        }

        public void MoveColNode(int now,int target)
        {
            if(target< 1 || target >_maxCol) return;
            var tTable = new Dictionary<string, VisualElement>();
            for (int r = 0; r <= _maxRow; r++)
            {
                var node = _table.Q<VisualElement>($"({r},{now})");
                if (node == null) continue;
                node.style.left = 32 + 124 * (target-1);
                node.name = "";
                tTable.Add($"({r},{target})",node);
            }
            if (target > now)
            {
                for (int c = target; c > now; c--)
                {
                    for (int r = 0; r <= _maxRow; r++)
                    {
                        var node = _table.Q<VisualElement>($"({r},{c})");
                        if (node == null) continue;
                        node.style.left = 32 + 124 * (c-2);
                        node.name = $"({r},{c-1})";
                    }
                }
            }
            else
            {
                for (int c = target; c < now; c++)
                {
                    for (int r = 0; r <= _maxRow; r++)
                    {
                        var node = _table.Q<VisualElement>($"({r},{c})");
                        if (node == null) continue;
                        node.style.left = 32 + 124 * (c);
                        node.name = $"({r},{c+1})";
                    }
                }
            }
            foreach (var value in tTable)
            {
                value.Value.name = value.Key;
            }
            ColClick(target);
        }
        
        private void TipWindow(string tip,string lstr,string rstr,Action lCallBack = null,Action rCallBack = null)
        {
            if(rootVisualElement.Q<IMGUIContainer>("TipWindow")!=null) return;
            var box = SongEditorUtility.GetDragBox(100);
            box.name = "TipWindow";
            box.style.width = Length.Percent(40);
            box.style.height = Length.Percent(36);
            box.style.backgroundColor = new Color(0.156f,0.172f,0.203f);
            box.style.borderTopLeftRadius = 6;
            box.style.borderTopRightRadius = 6;
            box.style.borderBottomLeftRadius = 6;
            box.style.borderBottomRightRadius = 6;
            box.style.left = 200;
            box.style.top = 100;
            var tipcontent = new Label()
            {
                text = tip,
                focusable = false,
                style =
                {
                    unityTextAlign = TextAnchor.MiddleCenter,
                    top = Length.Percent(10),
                    left = Length.Percent(10),
                    width = Length.Percent(80),
                    height = Length.Percent(60),
                    fontSize = 20,
                }
            };
            box.Add(tipcontent);
            var lbtn = new Button()
            {
                text = lstr,
                style =
                {
                    position = Position.Absolute,
                    width = Length.Percent(20),
                    height = 26,
                    left = Length.Percent(20),
                    top = Length.Percent(80),
                    borderTopLeftRadius = 6,
                    borderBottomLeftRadius = 6,
                    borderBottomRightRadius = 6,
                    borderTopRightRadius = 6,
                    backgroundColor = new Color(0.811f,0.298f,0.172f),
                }
            };
            box.Add(lbtn);
            var rbtn = new Button()
            {
                text = rstr,
                style =
                {
                    position = Position.Absolute,
                    width = Length.Percent(20),
                    height = 26,
                    left = Length.Percent(60),
                    top = Length.Percent(80),
                    borderTopLeftRadius = 6,
                    borderBottomLeftRadius = 6,
                    borderBottomRightRadius = 6,
                    borderTopRightRadius = 6,
                    backgroundColor = _choose1
                }
            };
            box.Add(rbtn);
            lbtn.clicked+=delegate { lCallBack?.Invoke(); rootVisualElement.Remove(box); };
            rbtn.clicked+=delegate { rCallBack?.Invoke(); rootVisualElement.Remove(box); };
            rootVisualElement.Add(box);
            box.BringToFront();
        }
        
        private void OnDestroy()
        {
            LangPath = "";
        }
    }
}