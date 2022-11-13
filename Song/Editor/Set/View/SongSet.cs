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
using Song.Editor.Core.Tools;
using Song.Runtime.Core.Data;
using Song.Editor.Core.Data;

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
        private VisualElement root;
        private ScrollView leftmenu;
        private VisualElement rightpanel;
        private Button lastbtn;
        private SongSetPage nowpage;
        private Set set;
        #endregion

        #region Style
        private Color btn_click = new Color(0.133f, 0.650f, 0.949f);
        private Color btn_default = new Color(0.227f, 0.227f, 0.227f);
        #endregion

        #region MyRegion
        public string lang
        {
            get => set["Lang"];
            set => set["Lang"] = value;
        }
        private Lang langdata;
        #endregion

        [MenuItem("Song/Set")]
        public static void ShowWindow()
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
            set = Config.LoadSet("Assets/Song/Editor/Others/Config/Set/songset.songset");
            if (string.IsNullOrWhiteSpace(set["Lang"])) set["Lang"] = "en";
            langdata = Config.LoadLang("Assets/Song/Editor/Others/Config/Lang/songset.songlang");
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
                page.Value.songset = this;
                Button left_btn = new Button();
                left_btn.name = page.Key;
                if (langdata[lang].datas.ContainsKey(page.Key))
                {
                    left_btn.text = langdata[lang][page.Key];
                }
                else
                {
                    left_btn.text = page.Value.Name();
                }
                leftmenu.Add(left_btn);
                left_btn.AddToClassList("LeftItem");
                left_btn.clicked += delegate
                {
                    ShowRightPanel(left_btn, page);
                };
                if (lastbtn == null)
                {
                    ShowRightPanel(left_btn, page);
                }
            }
        }

        public void ShowRightPanel(Button left_btn, KeyValuePair<string, SongSetPage> page)
        {
            if (lastbtn != null && string.Compare(left_btn.name, lastbtn.name) == 0) return;
            if (lastbtn != null)
            {
                nowpage.OnClose();
                lastbtn.style.backgroundColor = btn_default;
            }
            left_btn.style.backgroundColor = btn_click;
            var new_rightpanel = page.Value.Show();
            rightpanel.Clear();
            rightpanel.Add(new_rightpanel);
            new_rightpanel.StretchToParentSize();
            lastbtn = left_btn;
            nowpage = page.Value;
        }

        public void GetAssets(string format, Action<string> CallBack = null)
        {
            SongFileSelection.ShowWindow(format, (string path) =>
            {
                CallBack?.Invoke(path);
            }, delegate { GetWindow<SongSet>().Show(); });
        }

        public void RepaintRightPanel()
        {
            var new_rightpanel = nowpage.Show();
            rightpanel.Clear();
            rightpanel.Add(new_rightpanel);
            new_rightpanel.StretchToParentSize();
        }

        public void ReloadLeftButtonName()
        {
            foreach (var item in langdata[lang].datas)
            {
                Button btn = leftmenu.Q<Button>(item.Key);
                if (btn != null)
                {
                    btn.text = item.Value;
                }
            }
        }

        public void OnDestroy()
        {
            if (nowpage != null)
                nowpage.OnClose();
            set.SaveAsync();
        }
    }
}