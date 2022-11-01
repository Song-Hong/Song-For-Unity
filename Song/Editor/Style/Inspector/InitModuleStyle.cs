using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Song.Core;
using UnityEngine.UIElements;
using Song.Editor.Core.Data;
using Song.Core.Data;

namespace Song.Editor.Style
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
            StyleColor theme_color = new Color(0.968f, 0.503f, 0.223f);

            //root
            VisualElement root = new VisualElement();

            //title
            Label initname = new Label("Song");
            initname.style.alignSelf = Align.Center;
            initname.style.color = theme_color;
            root.Add(initname);

            //fps
            Label fpsName = new Label("FPS");
            root.Add(fpsName);

            Set set = Config.LoadSet(Application.streamingAssetsPath+"/Config/InitModule.set");
            return root;
        }
    }
}
