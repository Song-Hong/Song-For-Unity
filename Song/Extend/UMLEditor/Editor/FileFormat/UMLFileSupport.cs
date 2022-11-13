using System.Collections;
using System.Collections.Generic;
using System.IO;
using Song.Editor.Core;
using Song.Editor.Core.Tools;
using Song.Extend.UML;
using Song.Runtime.Core.Data;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Song.Editor.FileFormat
{
    [ScriptedImporter(1, ".songuml")]
    public class UMLFileSupport : CustomFileSupport
    {
        protected override string iconpath() => "Assets/Song/Extend/UMLEditor/Editor/Art/songuml.png";
        protected override string fileformat() => "songuml";
    }

    /// <summary>
    /// create set file
    /// </summary>
    public class CreateUMLFile
    {
        [MenuItem("Assets/Song/CreateUMLFile")]
        public static void Create()
        {
            UMLData uml = new UMLData();
            var path = SongEditorUtility.GetAssetsOpenDirPath() + "/newuml.songuml";
            path = SongEditorUtility.GetNewFileName(path);
            uml.path = path;
            uml.name = "newuml";
            uml.Save();
            AssetDatabase.Refresh();
        }
    }
}
