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

namespace Song.Editor.FileFormat
{
    [CustomEditor(typeof(SetFileSupport))]
    public class SetFileFormatStyle : ScriptedImporterEditor
    {
        public  override bool showImportedObject => false;
        public  override bool HasPreviewGUI() => false;

        private string   path;
        private Set      set;

        public override void OnEnable()
        {
            base.OnEnable();
            path = SongEditorUtility.GetAssetsNowShowPath();
            set = new Set(File.ReadAllText(path));
        }

        public override void OnInspectorGUI()
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
            if (GUILayout.Button("new",GUILayout.Width(40),GUILayout.Height(20)))
            {
                set["new_property"]= "";
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("save", GUILayout.Width(60), GUILayout.Height(20)))
            {
                set.Save(path);
            }
            GUILayout.EndHorizontal();
            base.ApplyRevertGUI();
        }

        protected override void OnHeaderGUI()
        {
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Song/Editor/Art/Icons/songset.png");
            if (texture != null)
            {
                GUI.Label(new Rect(10,10,32,32),new GUIContent(texture));
                GUI.Label(new Rect(50, 4, Screen.width - 50, 32), Path.GetFileNameWithoutExtension(path)+(" (set file)"));
            }
            //if (GUI.Button(new Rect(Screen.width - 80, 16, 60, 20), "reload"))
            //{
            //    try
            //    {
            //        AssetDatabase.DeleteAsset(path);
            //    }
            //    catch (System.Exception ex)
            //    {
            //        System.Console.Write(ex);
            //    }
            //    finally
            //    {
            //        set.Save(path);
            //        AssetDatabase.Refresh();
            //    }
            //}
            GUILayout.Space(64);
        }
    }
}
