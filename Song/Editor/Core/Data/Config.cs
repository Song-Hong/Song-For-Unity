using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Song.Core.Data;
using UnityEditor;

namespace Song.Editor.Core.Data
{
    public static class Config 
    {
        public static Set LoadSet(string path)
        {
            if (File.Exists(path))
                return new Set(File.ReadAllLines(path));
            else
            {
                File.Create(path).Dispose();
                return new Set();
            }
        }

        /// <summary>
        /// quick save set file
        /// </summary>
        public static void Save(this Set set)
        {
            if (!string.IsNullOrWhiteSpace(set.savepath))
                File.WriteAllText(set.savepath, set.ToString());
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// quick save set file
        /// </summary>
        /// <param name="savepath">file path</param>
        public static void Save(this Set set,string savepath)
        {
            if(!string.IsNullOrWhiteSpace(savepath))
                set.savepath = savepath;
            Save(set);
        }
    }
}
