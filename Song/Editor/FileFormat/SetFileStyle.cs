using System.Collections;
using System.Collections.Generic;
using System.IO;
using Song.Runtime.Core.Data;
using Song.Editor.Core.Data;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.UIElements;
using Song.Editor.Core.Tools;
using Song.Editor.Core;
using Song.Editor.Core.Base;

namespace Song.Editor.FileFormat
{
    [CustomEditor(typeof(SetFileSupport))]
    public class SetFileFormatStyle : CustomFileStyle
    {
        protected override string IconPath() => "Assets/Song/Editor/Others/Art/Icons/songset.png";
        protected override string FileFormat() => "set file";

        private Set set;
        private Lang langdata;
        private string langname;

        protected override void Init()
        {
            set = new Set(File.ReadAllText(Path));
            langname = SongEditorUtility.GetLangName();
            langdata = Config.LoadLang("Assets/Song/Editor/Others/Config/Lang/SetFileSupport.songlang");
        }

        protected override void InspectorStyle()
        {
            
            Dictionary<string, string> datas = new Dictionary<string, string>();
            float width = ((Screen.width - 90) / 2);
            foreach (var item in set.datas)
            {
                GUILayout.BeginHorizontal();
                string key = GUILayout.TextField(item.Key, GUILayout.Width(width));
                GUILayout.Label(" : ", GUILayout.Width(30));
                string value = GUILayout.TextField(item.Value, GUILayout.Width(width));
                datas.Add(key, value);
                GUILayout.Space(6);
                if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    datas.Remove(key);
                }
                GUILayout.EndHorizontal();
            }
            set.datas = datas;

            GUILayout.Space(6);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(langdata[langname]["new"],GUILayout.Width(50),GUILayout.Height(20)))
            {
                set["new_property"]= "";
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(langdata[langname]["save"], GUILayout.Width(60), GUILayout.Height(20)))
            {
                set.Save(Path);
            }
            GUILayout.EndHorizontal();
        }
    }
}
