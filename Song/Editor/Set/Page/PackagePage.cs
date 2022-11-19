using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Song.Editor.Core.Data;
using Song.Runtime.Core.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace Song.Extend.SongSet
{
    /// <summary>
    /// package manager page
    /// </summary>
    public class PackagePage : SongSetPage
    {
        public override string Name() => "Package";

        private Color node_bgc = new Color(0.2f, 0.2f, 0.2f);
        private Lang lang;
        private string _lang;
        private VisualElement root;
        
        public override VisualElement Show()
        {
            root = new VisualElement();
            var set = new Set();
            lang = Config.LoadLang("Assets/Song/Editor/Others/Config/Lang/PackagePage.songlang");
            _lang = songset.lang;
            var index = 0;
            foreach (var item in Directory.GetDirectories("Assets/Song/Extend"))
            {
                set = null;
                var path = item + "/Editor/Config/info.songset";
                if (File.Exists(path))
                {
                    set = Config.LoadSet(path);
                }
                if (set == null) continue;
                var node = Node(set, index);
                root.Add(node);
                node.StretchToParentWidth();
                index++;
            }
            return root;
        }

        /// <summary>
        /// create packagenode
        /// </summary>
        /// <returns></returns>
        public VisualElement Node(Set set,int index =0)
        {
            var node = new VisualElement
            {
                style =
                {
                    marginLeft = 20,
                    marginRight = 20,
                    backgroundColor = node_bgc,
                    height = 100,
                    marginTop = 10 + index*110,
                    borderTopLeftRadius = 6,
                    borderTopRightRadius = 6,
                    borderBottomLeftRadius = 6,
                    borderBottomRightRadius = 6
                },
                name = set["name"]
            };

            Label node_name = new Label($"{set["name"]}");
            node.Add(node_name);
            node_name.style.marginLeft = 12;
            node_name.style.marginTop = 10;

            Label node_version = new Label($"{lang[_lang]["Version"]} : {set["version"]}");
            node.Add(node_version);
            node_version.style.marginLeft = 32;
            node_version.style.marginTop = 6;

            Label node_author = new Label($"{lang[_lang]["Author"]} : {set["author"]}");
            node.Add(node_author);
            node_author.style.marginLeft = 32;
            node_author.style.marginTop = 6;

            Label node_describe = new Label($"{lang[_lang]["Describe"]} : {set["describe"]}");
            node.Add(node_describe);
            node_describe.style.marginLeft = 32;
            node_describe.style.marginTop = 6;

            if (string.Compare(set["access"], "read/write") == 0)
            {
                var btn_del = new Button();
                btn_del.text = lang[_lang]["Delete"];
                btn_del.style.marginLeft = Length.Percent(78);
                btn_del.style.position   = Position.Absolute;
                btn_del.style.marginTop  = 72;
                btn_del.style.height     = 20;
                node.Add(btn_del);
            }
            
            if (!string.IsNullOrWhiteSpace(set["set"]))
            {
                var btn_set = new Button();
                btn_set.text = lang[_lang]["Set"];
                btn_set.style.marginLeft = Length.Percent(68);
                btn_set.style.position   = Position.Absolute;
                btn_set.style.marginTop  = 72;
                btn_set.style.height     = 20;
                node.Add(btn_set);
                btn_set.clicked+= delegate { ShowSetting(set["set"]);};
            }
            
            Button btn_update = new Button();
            btn_update.text = lang[_lang]["Update"];
            btn_update.style.marginLeft = Length.Percent(88);
            btn_update.style.position   = Position.Absolute;
            btn_update.style.marginTop  = 72;
            btn_update.style.height     = 20;
            node.Add(btn_update);

            return node;
        }

        public void ShowSetting(string path)
        {
            var settingPanel = new VisualElement();
            root.Add(settingPanel);
            settingPanel.StretchToParentSize();
            settingPanel.BringToFront();

            var set = Config.LoadSet(path);
            foreach (var item in set.datas)
            {
                var node = new VisualElement();
                // settingPanel.Add();
            }
        }
    }
}
