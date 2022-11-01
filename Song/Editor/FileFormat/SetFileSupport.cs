using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;
using Song.Core.Data;
using Song.Editor.Core.Data;
using Song.Editor.Core.Tools;

namespace Song.Editor.FileFormat
{
    [ScriptedImporter(1, ".songset")]
    public class SetFileSupport : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {

        }
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
