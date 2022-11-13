using System.Collections;
using System.Collections.Generic;
using System.IO;
using Song.Editor.Core;
using Song.Editor.Core.Base;
using Song.Editor.Core.Data;
using Song.Editor.Core.Tools;
using Song.Extend.UML;
using Song.Runtime.Core.Data;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;
using static Unity.VisualScripting.Icons;

namespace Song.Editor.FileFormat
{
    [CustomEditor(typeof(UMLFileSupport))]
    public class UMLFileStyle : CustomFileStyle
    {
        protected override string IconPath() => "Assets/Song/Extend/UMLEditor/Editor/Art/songuml.png";
        protected override string FileFormat() => "uml file";

        private UMLData uml;
        private Lang langdata;
        private string langname;

        protected override void Init()
        {
            uml = UMLData.Load(Path);
            langname = SongEditorUtility.GetLangName();
            langdata = Config.LoadLang("Assets/Song/Extend/UMLEditor/Editor/UMLEditor.songlang");
        }

        protected override void InspectorStyle()
        {
            if (uml == null)
            {
                GUILayout.Label(langdata[langname]["1"]);
                GUILayout.Label(langdata[langname]["2"]);
                GUILayout.Label(langdata[langname]["3"]);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(langdata[langname]["Override"], GUILayout.Width(60), GUILayout.Height(20)))
                {
                    UMLData uml = new UMLData();
                    uml.path = Path;
                    uml.name = System.IO.Path.GetFileNameWithoutExtension(Path);
                    uml.Save();
                    AssetDatabase.Refresh();
                }
                ApplyRevertGUI();
                return;
            }
            GUILayout.BeginHorizontal();
            GUILayout.Label(langdata[langname]["UMLName"]+" : ");
            uml.name = GUILayout.TextField(uml.name);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(langdata[langname]["NoteCount"]+" : "+ uml.Data.Count);
            GUILayout.EndHorizontal();

            GUILayout.Space(6);
            GUILayout.BeginHorizontal();
            //if (GUILayout.Button("new", GUILayout.Width(40), GUILayout.Height(20)))
            //{
            //set["new_property"] = "";
            //}
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(langdata[langname]["save"], GUILayout.Width(60), GUILayout.Height(20)))
            {
                uml.Save();
            }
            GUILayout.EndHorizontal();
        }
    }

}