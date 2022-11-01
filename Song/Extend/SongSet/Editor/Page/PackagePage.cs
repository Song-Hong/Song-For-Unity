using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Song.Extend.SongSet
{
    /// <summary>
    /// package manager page
    /// </summary>
    public class PackagePage : SongSetPage
    {
        public override VisualElement Show()
        {
            VisualElement root = new VisualElement();
            return root;
        }

        /// <summary>
        /// create packagenode
        /// </summary>
        /// <returns></returns>
        public VisualElement Node()
        {
            VisualElement node = new VisualElement();
            return node;
        }
    }
}
