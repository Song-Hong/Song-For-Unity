using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Song.Runtime.Core.Data;
using UnityEditor;
using System.Threading;
using System.Threading.Tasks;

namespace Song.Editor.Core.Data
{
    public static class Config 
    {
        #region Set
        /// <summary>
        /// quick load set file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Set LoadSet(string path)
        {
            if (File.Exists(path))
                return new Set(File.ReadAllLines(path)) { savepath = path};
            else
            {
                File.Create(path).Dispose();
                return new Set() { savepath = path };
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
        /// async save set file
        /// </summary>
        /// <param name="set"></param>
        public static void SaveAsync(this Set set)
        {
            Thread thread = null;
            thread = new Thread(() =>
            {
                if (!string.IsNullOrWhiteSpace(set.savepath))
                    File.WriteAllText(set.savepath, set.ToString());
                if (thread != null) thread.Abort();
            });
            thread.Start();
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
        #endregion

        #region Lang
        /// <summary>
        /// quick load lang file
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Lang LoadLang(string path)
        {
            if (File.Exists(path))
                return new Lang(File.ReadAllText(path)) { savepath = path };
            else
            {
                File.Create(path).Dispose();
                return new Lang() { savepath = path };
            }
        }

        /// <summary>
        /// quick save lang file
        /// </summary>
        public static void Save(this Lang lang)
        {
            if (!string.IsNullOrWhiteSpace(lang.savepath))
                File.WriteAllText(lang.savepath, lang.ToString());
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// async save set file
        /// </summary>
        /// <param name="set"></param>
        public static void SaveAsync(this Lang lang)
        {
            Thread thread = null; 
            thread = new Thread(() =>
            {
                if (!string.IsNullOrWhiteSpace(lang.savepath))
                    File.WriteAllText(lang.savepath, lang.ToString());
                if (thread != null) thread.Abort();
            });
            thread.Start();
        }

        /// <summary>
        /// quick save set file
        /// </summary>
        /// <param name="savepath">file path</param>
        public static void Save(this Lang lang, string savepath)
        {
            if (!string.IsNullOrWhiteSpace(savepath))
                lang.savepath = savepath;
            Save(lang);
        }
        #endregion
    }
}
