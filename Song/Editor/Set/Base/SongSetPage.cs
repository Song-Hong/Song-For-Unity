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
        public virtual string Name() => "NewPage";

        /// <summary>
        /// set 
        /// </summary>
        public SongSet songset;

        /// <summary>
        /// show man page
        /// </summary>
        /// <returns></returns>
        public abstract VisualElement Show();

        /// <summary>
        /// on page change and window close call back
        /// </summary>
        public virtual void OnClose() { }
    }
}
