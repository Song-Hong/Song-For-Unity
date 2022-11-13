using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Song.Runtime.Support;
using System;
using UnityEngine.UIElements;

namespace Song.Editor.Core.Extend
{
    /// <summary>
    /// UnDo Extend Method
    /// </summary>
    public static class UnDoExtends
    {
        public static void Bind<T>(this UnDo<T> undo,VisualElement root)
        {
            VisualElement ve = new VisualElement();
            ve.RegisterCallback<KeyDownEvent>(x => {
#if UNITY_EDITOR_OSX
                Debug.Log(1);
                if (x.commandKey && x.shiftKey && x.keyCode == KeyCode.Z)
                {
                    undo.Un();
                }
                else if (x.commandKey && x.keyCode == KeyCode.Z)
                {
                    undo.Re();
                }
#else
                if (x.ctrlKey && x.shiftKey && x.keyCode == KeyCode.Z)
                {
                    undo.Un();
                }
                else if (x.ctrlKey && x.keyCode == KeyCode.Z)
                {
                    undo.Re();
                }
#endif
            });
            ve.style.height = 0;
            ve.style.width  = 0;
            ve.name = "UnDoShortcutKey";
            root.Add(ve);
        }
    }
}
