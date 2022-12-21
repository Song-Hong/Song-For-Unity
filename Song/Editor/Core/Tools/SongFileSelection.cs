using System;
using System.Collections.Generic;
using System.IO;
using Song.Editor.Core.Data;
using Song.Runtime.Core.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Song.Editor.Core.Tools
{
    public class SongFileSelection : EditorWindow
    {
        private Color panel_bgc = new Color(0.178f, 0.178f, 0.178f);
        private VisualElement root;
        private static Action<string> CallBack;
        private static Action WindowCloseCallBack;
        private static string fileformat;
        private static Set set;

        public static void ShowWindow(string format, Action<string> ClickCallBack = null, Action CloseCallBack = null,string title = "") 
        {
            CallBack = ClickCallBack;
            WindowCloseCallBack = CloseCallBack;
            fileformat = "";
            set = Config.LoadSet("Assets/Song/Editor/Others/Config/Set/SongFileSelection.songset");
            foreach(var item in format.Split(","))
            {
                if (!item.StartsWith("."))
                     fileformat += "."+item;
                else fileformat += item;
            }
            var wnd = GetWindow<SongFileSelection>();
            if (string.IsNullOrWhiteSpace(title)) title = "File Selection";
            wnd.titleContent = new GUIContent(title);
            wnd.Show();
        }

        private void CreateGUI()
        {
            root = rootVisualElement;
            TextField field = new TextField();
            field.style.width = Length.Percent(80);
            field.style.marginTop = 10;
            field.style.marginLeft = Length.Percent(10);

            ScrollView scroll       = new ScrollView();
            scroll.style.width      = Length.Percent(92);
            scroll.style.height     = Length.Percent(92);
            scroll.style.marginLeft = Length.Percent(4);
            scroll.style.marginTop  = Length.Percent(2);

            GroupBox group = new GroupBox();
            group.style.flexDirection = FlexDirection.Row;
            group.style.flexWrap = Wrap.Wrap;
            scroll.Add(group);
            group.StretchToParentSize();
            
            field.RegisterValueChangedCallback(x =>
            {
                var value = x.newValue;
                if (string.IsNullOrWhiteSpace(value))
                {
                    foreach (var node in group.Children())
                    {
                        node.style.display = DisplayStyle.Flex;
                    }
                }
                else
                {
                    foreach (var node in group.Children())
                    {
                        if (!node.name.Contains(value))
                        {
                            node.style.display = DisplayStyle.None;
                        }
                    }
                }
            });

            root.Add(field);
            root.Add(scroll);

            ShowAllNode(group,Application.dataPath ,fileformat);
        }

        // public void ShowAllNode(GroupBox group,List<string> paths)
        public void ShowAllNode(GroupBox group,string dirPath,string pattern)
        {
            group.Clear();
            SongEditorUtility.GetFilesAsync(dirPath,pattern, delegate(string fileName)
            {
                var name = Path.GetFileName(fileName);
                var item = node(name, "Assets/" + fileName.Replace(Application.dataPath, ""));
                group.Add(item);
                // item.AScale(new Vector2(1,1), 0.1f);
            });
        }

        public VisualElement node(string name,string path)
        {
            var node    = new VisualElement
            {
                style =
                {
                    width = 64,
                    height = 64,
                    marginLeft = 30,
                    marginTop = 30,
                    borderTopLeftRadius = 4,
                    borderTopRightRadius = 4,
                    borderBottomLeftRadius = 4,
                    borderBottomRightRadius = 4,
                    backgroundColor = Color.white,
                    scale = Vector2.one
                }
            };
            node.RegisterCallback<MouseDownEvent>(x =>
            {
                CallBack?.Invoke(path);
                GetWindow<SongFileSelection>().Close();
            });
            var extend = Path.GetExtension(path);
            if (".jpg.png.JPG.PNG".Contains(extend))
            {
                node.style.backgroundImage = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                node.style.backgroundColor = panel_bgc;
            }
            else if (set.datas.ContainsKey(extend))
            {
                node.style.backgroundImage = AssetDatabase.LoadAssetAtPath<Texture2D>(set[extend]);
                node.style.backgroundColor = panel_bgc;
            }
            node.name = name;
            var node_name = new Label();
            if (name.Length > 12) name = name.Substring(0, 12) + "...";
            node_name.text = name;
            node_name.style.marginTop = 72;
            node_name.style.unityTextAlign = TextAnchor.MiddleCenter;
            node.Add(node_name);
            node.tooltip = path;
            return node;
        }
        
        public void OnDestroy()
        {
            WindowCloseCallBack?.Invoke();
        }
    }
}
