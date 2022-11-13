using System.Collections;
using System.Collections.Generic;
using System.IO;
using Song.Editor.Core.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.VisualScripting.Icons;

namespace Song.Editor.Core.Tools
{
    public class SongEditorUtility
    {
        /// <summary>
        /// get Assets now show path 
        /// </summary>
        /// <returns></returns>
        public static string GetAssetsNowShowPath()
        {
            Object[] arr = Selection.GetFiltered(typeof(Object), SelectionMode.TopLevel);
            return AssetDatabase.GetAssetPath(arr[0]);
        }

        /// <summary>
        /// get assets now show dir path
        /// </summary>
        /// <returns></returns>
        public static string GetAssetsOpenDirPath()
        {
            string dir = "Assets";
            foreach (var obj in Selection.GetFiltered<Object>(SelectionMode.Assets))
            {
                var path = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(path))
                    continue;
                if (Directory.Exists(path))
                    dir = path;
                else if (File.Exists(path))
                    dir = Path.GetDirectoryName(path);
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

        /// <summary>
        /// get set lang name
        /// </summary>
        /// <returns>lang name</returns>
        public static string GetLangName()
        {
            string langname = Config.LoadSet("Assets/Song/Editor/Others/Config/Set/songset.songset")["Lang"];
            if (string.IsNullOrWhiteSpace(langname)) langname = "en";
            return langname;
        }

        /// <summary>
        /// Returns a draggable box
        /// </summary>
        /// <param name="index">box index</param>
        /// <returns>draggable box</returns>
        public static IMGUIContainer GetDragBox(int index)
        {
            var box = new IMGUIContainer();
            box.onGUIHandler = () =>
            {
                var controlId = GUIUtility.GetControlID(index, FocusType.Passive);
                var rvt = Event.current.GetTypeForControl(controlId);
                if (rvt == EventType.MouseDown)
                {
                    GUIUtility.hotControl = controlId;
                }
                else if(rvt == EventType.MouseDrag)
                {
                    if (GUIUtility.hotControl == controlId)
                    {
                        box.style.left = box.style.left.value.value + Event.current.delta.x;
                        box.style.top = box.style.top.value.value + Event.current.delta.y;
                    }
                }
                else if (rvt == EventType.MouseUp)
                {
                    if (GUIUtility.hotControl == controlId)
                    {
                        GUIUtility.hotControl = -1;
                    }
                }
            };
            return box;
        }
    }
}
