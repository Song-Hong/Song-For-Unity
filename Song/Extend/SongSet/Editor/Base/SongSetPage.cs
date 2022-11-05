using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Song.Extend.SongSet
{
    /// <summary>
    /// set page
    /// </summary>
    public abstract class SongSetPage
    {
        public SongSet songset;

        /// <summary>
        /// shou1
        /// </summary>
        /// <returns></returns>
        public abstract VisualElement Show();

        /// <summary>
        /// on page change and window close call back
        /// </summary>
        public virtual void OnClose() { }
    }
}
