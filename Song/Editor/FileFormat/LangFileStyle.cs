using Song.Editor.Core.Tools;
using Song.Runtime.Core.Data;
using UnityEditor;
using UnityEngine;
using Song.Editor.Core.Base;
using Song.Editor.Core.Data;

namespace Song.Editor.FileFormat
{
    [CustomEditor(typeof(LangFileSupport))]
    public class LangFileStyle : CustomFileStyle
    {
        protected override string IconPath() => "Assets/Song/Editor/Others/Art/Icons/songlang.png";
        protected override string FileFormat() => "lang file";

        private Lang _lang;
        private Lang _langData;
        private string _langName;

        protected override void Init()
        {
            _lang  = Config.LoadLang(Path);
            _langName = SongEditorUtility.GetLangName();
            _langData = Config.LoadLang("Assets/Song/Editor/Others/Config/Lang/LangFileSupport.songlang");
        }

        protected override void InspectorStyle()
        {
            GUILayout.Space(26);
            var width = (Screen.width - 100) / 2;
            var newlang = new Lang(_lang.ToString());
            foreach (var lang_key in newlang.datas.Keys)
            {
                var old_name = lang_key;
                var new_name = GUILayout.TextField(lang_key, GUILayout.Width((lang_key.Length * 8) + 10));
                if (string.Compare(old_name, new_name) != 0)
                {
                    if (string.IsNullOrWhiteSpace(new_name))
                        new_name = "null";
                    if (old_name == "null" && new_name.Length > 4)
                        new_name = new_name.Replace("null","");
                    var set = _lang[old_name];
                    _lang.datas.Remove(old_name);
                    _lang.datas.Add(new_name,set);
                }
                foreach (var data in newlang[lang_key].datas)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    var old_key = data.Key;
                    var new_key = GUILayout.TextField(data.Key, GUILayout.Width(width));
                    if (string.Compare(old_key, new_key) != 0)
                    {
                        foreach (var key in newlang.datas.Keys)
                        {
                            var _value = _lang[key][old_key];
                            _lang[key].datas.Remove(old_key);
                            _lang[key].datas.Add(new_key, _value);
                        }
                    }
                    GUILayout.Space(10);
                    var old_value = data.Value;
                    var new_value = GUILayout.TextField(data.Value, GUILayout.Width(width));
                    if (string.Compare(old_value, new_value) != 0)
                    {
                        _lang[lang_key][data.Key] = new_value;
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.Space(6);
            }
            GUILayout.Space(6);
            GUILayout.BeginHorizontal();
            if (GUI.Button(new Rect(10,0,50,20),_langData[_langName]["new"]))
            {
                foreach (var lang_key in newlang.datas.Keys)
                {
                    if (_lang[lang_key].datas.ContainsKey(_langData[_langName]["NewProperty"])) continue;
                    _lang[lang_key].datas.Add(_langData[_langName]["NewProperty"], "");
                }
            }
            if (GUI.Button(new Rect(65, 0, 80, 20), _langData[_langName]["new_lang"]))
            {
                if (_lang.datas.ContainsKey(_langData[_langName]["new_lang"]))
                    return;
                Set set = new Set();
                foreach (var item in _lang.datas.Values)
                {
                    foreach (var data in item.datas)
                    {
                        set.datas.Add(data.Key,"");
                    }
                    break;
                }
                _lang.datas.Add(_langData[_langName]["new_lang"], set);
            }
            GUILayout.FlexibleSpace();
            if (GUI.Button(new Rect(Screen.width - 60,0,40,20),_langData[_langName]["save"]))
            {
                _lang.Save(Path);
            }
            if (GUI.Button(new Rect(Screen.width - 120, 0, 40, 20), _langData[_langName]["open"]))
            {
                LangEditorWindow.LangPath = Path;
                LangEditorWindow.ShowLangEditor();
            }
            GUILayout.EndHorizontal();
        }

        protected override void OnHeaderGUI()
        {
            base.OnHeaderGUI();
        }
    }
}