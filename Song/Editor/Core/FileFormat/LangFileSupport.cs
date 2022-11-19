using System.Collections;
using System.Collections.Generic;
using Song.Editor.Core;
using Song.Editor.Core.Data;
using Song.Editor.Core.Tools;
using Song.Runtime.Core.Data;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Song.Editor.FileFormat
{
    [ScriptedImporter(1, ".songlang")]
    public class LangFileSupport : CustomFileSupport
    {
        protected override string iconpath() => "Assets/Song/Editor/Others/Art/Icons/songlang.png";
        protected override string fileformat() => "songset";
    }

    /// <summary>
    /// create set file
    /// </summary>
    public class CreateLangFile
    {
        [MenuItem("Assets/Song/CreateLangFile")]
        public static void Create()
        {
            string path = SongEditorUtility.GetAssetsOpenDirPath() + "/newset.songlang";
            Set set = new Set();
            path = SongEditorUtility.GetNewFileName(path);
            set.Save(path);
        }
    }
}