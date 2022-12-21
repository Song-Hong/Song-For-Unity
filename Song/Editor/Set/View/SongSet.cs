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
        private readonly Color _btnClick = new Color(0.133f, 0.650f, 0.949f);
        private readonly Color _btnDefault = new Color(0.227f, 0.227f, 0.227f);
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
                Button leftBtn = new Button();
                leftBtn.name = page.Key;
                if (langdata[lang].datas.ContainsKey(page.Key))
                {
                    leftBtn.text = langdata[lang][page.Key];
                }
                else
                {
                    leftBtn.text = page.Value.Name();
                }
                leftmenu.Add(leftBtn);
                leftBtn.AddToClassList("LeftItem");
                leftBtn.clicked += delegate
                {
                    ShowRightPanel(leftBtn, page);
                };
                if (lastbtn == null)
                {
                    ShowRightPanel(leftBtn, page);
                }
            }
        }

        public void ShowRightPanel(Button leftBtn, KeyValuePair<string, SongSetPage> page)
        {
            if (lastbtn != null && String.CompareOrdinal(leftBtn.name, lastbtn.name) == 0) return;
            if (lastbtn != null)
            {
                nowpage.OnClose();
                lastbtn.style.backgroundColor = _btnDefault;
            }
            leftBtn.style.backgroundColor = _btnClick;
            var newRightpanel = page.Value.Show();
            rightpanel.Clear();
            rightpanel.Add(newRightpanel);
            newRightpanel.StretchToParentSize();
            lastbtn = leftBtn;
            nowpage = page.Value;
        }

        public void GetAssets(string format, Action<string> callBack = null)
        {
            SongFileSelection.ShowWindow(format, (string path) =>
            {
                callBack?.Invoke(path);
            }, delegate { GetWindow<SongSet>().Show(); });
        }

        public void RepaintRightPanel()
        {
            var panel = nowpage.Show();
            rightpanel.Clear();
            rightpanel.Add(panel);
            panel.StretchToParentSize();
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