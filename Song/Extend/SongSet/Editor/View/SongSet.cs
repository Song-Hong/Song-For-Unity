using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Song.Editor.Core;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Unity.VisualScripting;
using System;

namespace Song.Extend.SongSet
{
    /// <summary>
    /// 
    /// </summary>
    public class SongSet : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        #region datas
        /// <summary>
        /// root page
        /// </summary>
        private VisualElement  root;
        private ScrollView     leftmenu;
        private VisualElement  rightpanel;
        private Button         lastbtn;
        #endregion

        #region Style
        private Color btn_click   = new Color(0.133f, 0.650f, 0.949f);
        private Color btn_default = new Color(0.227f, 0.227f, 0.227f);
        #endregion

        [MenuItem("Song/Set")]
        public static void ShowExample()
        {
            SongSet wnd = GetWindow<SongSet>();
            wnd.titleContent = new GUIContent("SongSet");
            wnd.minSize = new Vector2(600, 400);
        }

        public void CreateGUI()
        {
            root = m_VisualTreeAsset.Instantiate();
            rootVisualElement.Add(root);
            root.StretchToParentSize();
            leftmenu = root.Q<ScrollView>("LeftMenu");
            rightpanel = root.Q<VisualElement>("RightPanel");
            InitLeftMenu();
        }

        public void InitLeftMenu()
        {
            List<KeyValuePair<string, SongSetPage>> pages = new List<KeyValuePair<string, SongSetPage>>();
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var item in types)
            {
                if (item.IsSubclassOf(typeof(SongSetPage)))
                {
                    pages.Add(new KeyValuePair<string, SongSetPage>(item.Name.Replace("Page", ""),
                        Activator.CreateInstance(item) as SongSetPage));
                }
            }
            foreach (var page in pages)
            {
                Button left_btn = new Button();
                left_btn.name = page.Key;
                left_btn.text = page.Key;
                leftmenu.Add(left_btn);
                left_btn.AddToClassList("LeftItem");
                left_btn.clicked += delegate
                {
                    if (lastbtn != null && string.Compare(left_btn.name, lastbtn.name) == 0) return;
                    if (lastbtn != null)
                    {
                        lastbtn.style.unityBackgroundImageTintColor = btn_default;
                    }
                    left_btn.style.unityBackgroundImageTintColor = btn_click;
                    var new_rightpanel = page.Value.Show();
                    rightpanel.Clear();
                    rightpanel.Add(new_rightpanel);
                    new_rightpanel.StretchToParentSize();
                    lastbtn = left_btn;
                };
            }
        }
    }
}