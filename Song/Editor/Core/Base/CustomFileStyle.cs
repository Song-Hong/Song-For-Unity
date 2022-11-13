using Song.Editor.Core.Tools;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Song.Editor.Core.Base
{
    public class CustomFileStyle : ScriptedImporterEditor
    {
        #region Custom
        /// <summary>
        /// icon file path
        /// </summary>
        protected virtual string IconPath() => "";
        /// <summary>
        /// file format
        /// </summary>
        protected virtual string FileFormat() => "default file";
        /// <summary>
        /// init method
        /// </summary>
        protected virtual void Init() { }
        /// <summary>
        /// inspector style
        /// </summary>
        protected virtual void InspectorStyle() { }
        #endregion

        public override bool showImportedObject => false;
        public override bool HasPreviewGUI() => false;
        protected override bool OnApplyRevertGUI() => false;

        protected string Path { get; private set; }

        public override void OnEnable()
        {
            base.OnEnable();
            Path = SongEditorUtility.GetAssetsNowShowPath();
            Init();
        }

        public override void OnInspectorGUI()
        {
            InspectorStyle();
            ApplyRevertGUI();
        }

        protected override void OnHeaderGUI()
        {
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(IconPath());
            if (texture != null)
            {
                GUI.Label(new Rect(10, 10, 32, 32), new GUIContent(texture));
                GUI.Label(new Rect(50, 4, Screen.width - 50, 32), System.IO.Path.GetFileNameWithoutExtension(Path) + ($" ({FileFormat()})"));
            }
            GUILayout.Space(64);
        }
    }
}