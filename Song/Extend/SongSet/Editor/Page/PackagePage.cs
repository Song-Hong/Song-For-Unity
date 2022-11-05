using System.Collections;
using System.Collections.Generic;
using System.IO;
using Song.Editor.Core.Data;
using Song.Runtime.Core.Data;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using UnityEngine.UIElements;

namespace Song.Extend.SongSet
{
    /// <summary>
    /// package manager page
    /// </summary>
    public class PackagePage : SongSetPage
    {
        private Color node_bgc = new Color(0.2f, 0.2f, 0.2f);

        public override VisualElement Show()
        {
            var root = new VisualElement();
            var set = new Set();
            var index = 0;
            foreach (var item in Directory.GetDirectories("Assets/Song/Extend"))
            {
                set = null;
                var path = item + "/Editor/info.songset";
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
            VisualElement node = new VisualElement();
            node.style.marginLeft  = 20;
            node.style.marginRight = 20;
            node.style.backgroundColor = node_bgc;
            node.style.height = 100;
            node.style.marginTop = 10 + index*110;
            node.style.borderTopLeftRadius     = 6;
            node.style.borderTopRightRadius    = 6;
            node.style.borderBottomLeftRadius  = 6;
            node.style.borderBottomRightRadius = 6;
            node.name = set["name"];

            Label node_name = new Label($"{set["name"]}");
            node.Add(node_name);
            node_name.style.marginLeft = 12;
            node_name.style.marginTop = 10;

            Label node_version = new Label($"Version : {set["version"]}");
            node.Add(node_version);
            node_version.style.marginLeft = 32;
            node_version.style.marginTop = 6;

            Label node_author = new Label($"Author : {set["author"]}");
            node.Add(node_author);
            node_author.style.marginLeft = 32;
            node_author.style.marginTop = 6;

            Label node_describe = new Label($"Describe : {set["describe"]}");
            node.Add(node_describe);
            node_describe.style.marginLeft = 32;
            node_describe.style.marginTop = 6;

            if (string.Compare(set["access"], "read/write") == 0)
            {
                Button btn_del = new Button();
                btn_del.text = "delete";
                btn_del.style.marginLeft = Length.Percent(78);
                btn_del.style.position   = Position.Absolute;
                btn_del.style.marginTop  = 72;
                btn_del.style.height     = 20;
                node.Add(btn_del);
            }

            Button btn_update = new Button();
            btn_update.text = "update";
            btn_update.style.marginLeft = Length.Percent(88);
            btn_update.style.position   = Position.Absolute;
            btn_update.style.marginTop  = 72;
            btn_update.style.height     = 20;
            node.Add(btn_update);

            return node;
        }
    }
}
