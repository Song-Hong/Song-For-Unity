using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Song.Editor.Core.Tools;
using Song.Runtime.Core.Data;
using Song.Editor.Core.Data;
using System;
using UnityEditor;

namespace Song.Extend.SongSet
{
    /// <summary>
    /// Utility set 
    /// </summary>
    public class GeneralPage : SongSetPage
    {
        public override string Name() => "General";

        private VisualElement root;
        private Color panel_bgc = new Color(0.2f, 0.2f, 0.2f);
        private int top = 0;
        private Set set;
        private Lang lang;

        public override VisualElement Show()
        {
            top = 0;
            root = new VisualElement();
            set = Config.LoadSet("Assets/Song/Editor/Others/Config/Set/init.songset");
            lang = Config.LoadLang("Assets/Song/Editor/Others/Config/Lang/Init.songlang");
            var _lang = songset.lang;
            List<string> langchoices = new List<string>();
            var index = 0;
            var default_index = 0;
            foreach (var key in lang.datas.Keys)
            {
                switch (key)
                {
                    case "en":
                        langchoices.Add("English");
                        break;
                    case "zh":
                        langchoices.Add("简体中文");
                        break;
                    case "cht":
                        langchoices.Add("繁體中文");
                        break;
                    default:
                        langchoices.Add(key);
                        break;
                }
                if (string.Compare(_lang, key) == 0)
                    default_index = index;
                index++;
            }
            Panel("Song", 90, (VisualElement cpanel) =>
            {
                Label set_version = new Label($"{lang[_lang]["Version"]} : {set["version"]}");
                set_version.name = "version";
                set_version.style.left = 26;
                set_version.style.top = 16;
                cpanel.Add(set_version);

                Label lang_name = new Label($"{lang[_lang]["Lang"]} :");
                lang_name.style.position = Position.Absolute;
                lang_name.style.left = 26;
                lang_name.style.top = 58;
                lang_name.style.width = 60;
                cpanel.Add(lang_name);

                DropdownField lang_choose = new DropdownField(langchoices, default_index);
                lang_choose.style.position = Position.Absolute;
                lang_choose.style.left = 80;
                lang_choose.style.top = 56;
                lang_choose.style.width = 80;
                lang_choose.style.height = 20;
                lang_choose.RegisterValueChangedCallback(x =>
                {
                    string new_lang;
                    switch (x.newValue)
                    {
                        case "English":
                            new_lang = "en";
                            break;
                        case "简体中文":
                            new_lang = "zh";
                            break;
                        case "繁體中文":
                            new_lang = "cht";
                            break;
                        default:
                            new_lang = x.newValue;
                            break;
                    }
                    songset.lang = new_lang;
                    songset.RepaintRightPanel();
                    songset.ReloadLeftButtonName();
                });
                cpanel.Add(lang_choose);
            });

            Panel(lang[_lang]["Project Setting"], 100, (VisualElement cpanel) =>
            {
                Label ps_iconTip = new Label(lang[_lang]["Icon"]);
                ps_iconTip.style.position = Position.Absolute;
                ps_iconTip.style.left = 26;
                ps_iconTip.style.top = 40;
                cpanel.Add(ps_iconTip);

                VisualElement ps_icon = new VisualElement();
                ps_icon.style.position = Position.Absolute;
                ps_icon.style.left = 64;
                ps_icon.style.height = 30;
                ps_icon.style.width = 30;
                ps_icon.style.top = 32;
                ps_icon.style.borderTopLeftRadius = 2;
                ps_icon.style.borderTopRightRadius = 2;
                ps_icon.style.borderBottomLeftRadius = 2;
                ps_icon.style.borderBottomRightRadius = 2;
                if (!string.IsNullOrWhiteSpace(set["icon"]))
                    ps_icon.style.backgroundImage = AssetDatabase.LoadAssetAtPath<Texture2D>(set["icon"]);
                else
                {
                    Texture2D texture = new Texture2D(1, 1);
                    texture.SetPixel(1, 1, Color.white);
                    texture.Apply();
                    ps_icon.style.backgroundImage = texture;
                }
                ps_icon.RegisterCallback<MouseDownEvent>(x =>
                {
                    songset.GetAssets("png,jpg", delegate (string path)
                    {
                        set["icon"] = path;
                        ps_icon.style.backgroundImage = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                    });
                });
                cpanel.Add(ps_icon);

                Label ps_nameTip = new Label(lang[_lang]["Name"]);
                ps_nameTip.style.position = Position.Absolute;
                ps_nameTip.style.left = 26;
                ps_nameTip.style.top = 70;
                cpanel.Add(ps_nameTip);

                TextField ps_name = new TextField();
                ps_name.style.position = Position.Absolute;
                ps_name.style.marginLeft = 64;
                ps_name.style.marginRight = 10;
                ps_name.style.top = 68;
                ps_name.value = set["name"];
                ps_name.RegisterValueChangedCallback(x =>
                {
                    set["name"] = x.newValue;
                    //set.Save();
                });
                cpanel.Add(ps_name);
            });
            return root;
        }

        public void Panel(string name,int height,Action<VisualElement> cpanel)
        {
            VisualElement panel = new VisualElement();
            panel.name = name;
            panel.style.borderTopLeftRadius     = 6;
            panel.style.borderTopRightRadius    = 6;
            panel.style.borderBottomLeftRadius  = 6;
            panel.style.borderBottomRightRadius = 6;
            panel.style.backgroundColor         = panel_bgc;
            panel.style.marginLeft              = 20;
            panel.style.marginRight             = 20;
            panel.style.marginTop               = 10 + top;
            panel.style.height                  = height;
            Label panel_name                    = new Label(name);
            panel_name.style.left               = 10;
            panel_name.style.top                = 10;
            panel.Add(panel_name);
            cpanel?.Invoke(panel);
            root.Add(panel);
            panel.StretchToParentWidth();
            top += height + 10;
        }

        public override void OnClose()
        {
            set.SaveAsync();
        }
    }
}
