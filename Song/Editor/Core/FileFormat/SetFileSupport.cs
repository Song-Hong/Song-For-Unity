using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;
using Song.Runtime.Core.Data;
using Song.Editor.Core.Data;
using Song.Editor.Core.Tools;
using Song.Editor.Core;

namespace Song.Editor.FileFormat
{
    [ScriptedImporter(1, ".songset")]
    public class SetFileSupport : CustomFileSupport
    {
        protected override string iconpath() => "Assets/Song/Editor/Others/Art/Icons/songset.png";
        protected override string fileformat() => "songset";
    }

    /// <summary>
    /// create set file
    /// </summary>
    public class CreateSetFile
    {
        [MenuItem("Assets/Song/CreateSetFile")]
        public static void Create()
        {
            string path = SongEditorUtility.GetAssetsOpenDirPath() + "/newset.songset";
            Set set = new Set();
            path = SongEditorUtility.GetNewFileName(path);
            set.Save(path);
        }
    }
}
