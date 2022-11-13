using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using Song.Editor.Core.Data;
using Song.Runtime.Core.Data;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;
using Object = UnityEngine.Object;

namespace Song.Editor.Core.Tools
{
    public class SongFileSelection : EditorWindow
    {
        private Color panel_bgc = new Color(0.178f, 0.178f, 0.178f);
        private VisualElement root;
        private static Action<string> CallBack;
        private static Action WindowCloseCallBack;
        private static string fileformat;
        private List<string> assets;
        private static Set set;

        public static void ShowWindow(string format, Action<string> ClickCallBack = null, Action CloseCallBack = null,string title = "") 
        {
            CallBack = ClickCallBack;
            WindowCloseCallBack = CloseCallBack;
            fileformat = "";
            set = Config.LoadSet("Assets/Song/Editor/Others/Config/Set/SongFileSelection.songset");
            foreach(var item in format.Split(","))
            {
                if (!item.StartsWith("."))
                     fileformat += "."+item;
                else fileformat += item;
            }
            var wnd = GetWindow<SongFileSelection>();
            if (string.IsNullOrWhiteSpace(title)) title = "File Selection";
            wnd.titleContent = new GUIContent(title);
            wnd.Show();
        }

        private void CreateGUI()
        {
            root = rootVisualElement;
            TextField field = new TextField();
            field.style.width = Length.Percent(80);
            field.style.marginTop = 10;
            field.style.marginLeft = Length.Percent(10);

            ScrollView scroll       = new ScrollView();
            scroll.style.width      = Length.Percent(92);
            scroll.style.height     = Length.Percent(92);
            scroll.style.marginLeft = Length.Percent(4);
            scroll.style.marginTop  = Length.Percent(2);

            GroupBox group = new GroupBox();
            group.style.flexDirection = FlexDirection.Row;
            group.style.flexWrap = Wrap.Wrap;
            scroll.Add(group);
            group.StretchToParentSize();

            assets = GetFiles(Application.dataPath, fileformat);

            field.RegisterValueChangedCallback(x =>
            {
                var value = x.newValue;
                if (string.IsNullOrWhiteSpace(value))
                    ShowAllNode(group, assets);
                else
                {
                    List<string> newvalue = new List<string>();
                    foreach (var item in assets)
                    {
                        var name = Path.GetFileName(item);
                        if (name.Contains(value))
                            newvalue.Add(item);
                    }
                    ShowAllNode(group, newvalue);
                }
            });

            root.Add(field);
            root.Add(scroll);

            ShowAllNode(group, assets);
        }

        public void ShowAllNode(GroupBox group,List<string> paths)
        {
            group.Clear();
            foreach (var item in paths)
            {
                var name = Path.GetFileName(item);
                group.Add(node(name, "Assets/" + item.Replace(Application.dataPath, "")));
            }
        }

        public VisualElement node(string name,string path)
        {
            VisualElement node    = new VisualElement();
            node.style.width      = 64;
            node.style.height     = 64;
            node.style.marginLeft = 30;
            node.style.marginTop  = 30;
            node.style.borderTopLeftRadius = 4;
            node.style.borderTopRightRadius = 4;
            node.style.borderBottomLeftRadius = 4;
            node.style.borderBottomRightRadius = 4;
            node.style.backgroundColor = Color.white;
            node.RegisterCallback<MouseDownEvent>(x =>
            {
                CallBack?.Invoke(path);
                GetWindow<SongFileSelection>().Close();
            });
            var extend = Path.GetExtension(path);
            if (".jpg.png.JPG.PNG".Contains(extend))
            {
                node.style.backgroundImage = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                node.style.backgroundColor = panel_bgc;
            }
            else if (set.datas.ContainsKey(extend))
            {
                node.style.backgroundImage = AssetDatabase.LoadAssetAtPath<Texture2D>(set[extend]);
                node.style.backgroundColor = panel_bgc;
            }
            Label node_name = new Label();
            if (name.Length > 12) name = name.Substring(0, 12) + "...";
            node_name.text = name;
            node_name.style.marginTop = 72;
            node_name.style.unityTextAlign = TextAnchor.MiddleCenter;
            node.Add(node_name);
            node.tooltip = path;
            return node;
        }

        static List<string> GetFiles(string directory,string pattern = "*")
        {
            List<string> files = new List<string>();
            foreach (var item in Directory.GetFiles(directory))
            {
                if (item.EndsWith(".meta"))
                    continue;
                else if (pattern == "*")
                    files.Add(item);
                else if (pattern.Contains(Path.GetExtension(item)))
                {
                    files.Add(item);
                }
            }
            foreach (var item in Directory.GetDirectories(directory))
            {
                files.AddRange(GetFiles(item,pattern));
            }
            return files;
        }

        public void OnDestroy()
        {
            WindowCloseCallBack?.Invoke();
        }
    }

    public struct FileSelectionJob<T> : IJob where T :Object
    {

        public void Execute()
        {
            AssetDatabase.FindAssets("t:"+typeof(T).ToString());
        }
    }
}
