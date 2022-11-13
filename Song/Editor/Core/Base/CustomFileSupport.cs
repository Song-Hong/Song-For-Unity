using System.Collections;
using System.Collections.Generic;
using Song.Editor.Core.Data;
using Song.Editor.Core.Tools;
using Song.Runtime.Core.Data;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Song.Editor.Core
{
    public class CustomFileSupport : ScriptedImporter
    {
        /// <summary>
        /// icon file path
        /// </summary>
        protected virtual string iconpath() => "";
        /// <summary>
        /// file format
        /// </summary>
        protected virtual string fileformat() => "main";

        public override void OnImportAsset(AssetImportContext ctx)
        {
            if (string.IsNullOrWhiteSpace(iconpath())) return;
            Texture2D asset = AssetDatabase.LoadAssetAtPath<Texture2D>(iconpath());
            ctx.AddObjectToAsset(fileformat(), asset);
            ctx.SetMainObject(asset);
        }
    }
}