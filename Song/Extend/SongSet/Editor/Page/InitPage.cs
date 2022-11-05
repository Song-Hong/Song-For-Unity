using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Song.Editor.Core.Tools;
using Song.Runtime.Core.Data;
using Song.Editor.Core.Data;
using System;
using UnityEditor;
using static UnityEngine.Rendering.DebugUI.MessageBox;

namespace Song.Extend.SongSet
{
    /// <summary>
    /// Utility set 
    /// </summary>
    public class InitPage : SongSetPage
    {
        private VisualElement root;
        private Color panel_bgc = new Color(0.2f, 0.2f, 0.2f);
        private int top = 0;
        private Set set;

        public override VisualElement Show()
        {
            top = 0;
            root = new VisualElement();
            set = Config.LoadSet("Assets/Song/Extend/SongSet/Editor/Config/init.songset");

            Panel("Song",60, (VisualElement cpanel) =>
            {
                Label set_version       = new Label($"Version : {set["version"]}");
                set_version.name        = "version";
                set_version.style.left  = 26;
                set_version.style.top   = 16;
                cpanel.Add(set_version);
            });

            Panel("Project Setting", 100, (VisualElement cpanel) =>
            {
                Label ps_iconTip = new Label("Icon");
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
                        //set.Save();
                    });
                });
                cpanel.Add(ps_icon);

                Label ps_nameTip = new Label("name");
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
