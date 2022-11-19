using System.Collections.Generic;
using System.IO;
using Song.Runtime.Module;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Song.Editor.Style.Inspector
{
    /// <summary>
    /// Init Module Style For InitModule
    /// </summary>
    [CustomEditor(typeof(InitModule))]
    public class InitModuleStyle : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            //style
            StyleColor themeColor = new Color(0.968f, 0.503f, 0.223f);

            //root
            VisualElement root = new VisualElement();

            //title
            Label initname = new Label("Song");
            initname.style.alignSelf = Align.Center;
            initname.style.color = themeColor;
            root.Add(initname);

            //fps
            Label fpsName = new Label("FPS");
            root.Add(fpsName);
            

                 
            return root;
        }
    }
}

