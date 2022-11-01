using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Song.Editor.Core.Tools
{
    public class SongEditorUtility
    {
        /// <summary>
        /// get assets now show path
        /// </summary>
        /// <returns></returns>
        public static string GetAssetsOpenDirPath()
        {
            string dir = "Assets";
            foreach (var obj in Selection.GetFiltered<UnityEngine.Object>(SelectionMode.Assets))
            {
                var path = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(path))
                    continue;
                if (System.IO.Directory.Exists(path))
                    dir = path;
                else if (System.IO.File.Exists(path))
                    dir = System.IO.Path.GetDirectoryName(path);
            }
            return dir;
        }

        /// <summary>
        /// Avoid creating new files repeatedly
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns></returns>
        public static string GetNewFileName(string path)
        {
            if (!File.Exists(path)) return path;
            string dirpath = path.Substring(0,path.LastIndexOf("/"))+"/";
            path = path.Replace(dirpath,"");
            string name = path.Substring(0, path.LastIndexOf("."));
            path = path.Replace(name, "");
            string extension = path;
            int index = 1;
            while (true)
            {
                path = dirpath + name + index + extension;
                if (!File.Exists(path)) break;
                else index++;
            }
            return path;
        }
    }
}
