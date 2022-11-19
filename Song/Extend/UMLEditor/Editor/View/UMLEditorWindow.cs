using System.Collections;
using System.Collections.Generic;
using Song.Editor.Core.Tools;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace Song.Extend.UML
{
    /// <summary>
    /// UML editor window
    /// </summary>
    public class UMLEditorWindow:EditorWindow
    {
        #region assets
        private readonly string bgcstyle_path = "Assets/Song/Extend/UMLEditor/Editor/View/UMLEditorBackGround.uss";
        private UMLView view;
        private UMLData data;
        #endregion

        [MenuItem("Song/UML")]
        public static void ShowWindow()
        {
            UMLEditorWindow window = GetWindow<UMLEditorWindow>();
            window.titleContent = new GUIContent("UML Editor");
            window.Show();
        }

        public void CreateGUI()
        {
            Init();
            InitMiniMap();
        }

        #region initialize
        /// <summary>
        /// initialize
        /// </summary>
        public void Init()
        {
            StyleSheet bgc_style = AssetDatabase.LoadAssetAtPath<StyleSheet>(bgcstyle_path);
            view = new UMLView();
            rootVisualElement.Add(view);
            view.styleSheets.Add(bgc_style);
            UMLData.Load("Assets/Song/Extend/UMLEditor/newuml.songuml", (UMLNodeData data)=>
            {
                CreateNode(data);
            },(UMLData data) =>
            {
                this.data = data;
            });
        }

        /// <summary>
        /// create mini map
        /// </summary>
        public void InitMiniMap()
        {
            var miniMap = new MiniMap { anchored = true};
            view.Add(miniMap);
            miniMap.SetPosition(new Rect(20,20,200, 140));
            miniMap.maxWidth = 100;
            miniMap.maxHeight = 70;
        }
        #endregion

        /// <summary>
        /// create new node from UmlNodeData and add to UMLView                                  
        /// </summary>
        /// <param name="data"></param>
        public void CreateNode(UMLNodeData data)
        {
            UmlNode node = new UmlNode(data);
            // node.title = data.name;
            node.style.height = (data.Attribute.Count+data.Method.Count)*30 + 100;
            node.style.width = 200;
            node.style.backgroundColor = new Color(0.219f,0.219f,0.219f);
            node.SetPosition(data.rect);
            // var inPort = node.InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(float));
            // inPort.portName = "";
            // var outPort = node.InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(float));
            // outPort.portName = "";
            // node.inputContainer.Add(inPort);
            // inPort.highlight = true;
            // node.outputContainer.Add(outPort);
            // node.RefreshPorts();
            view.AddElement(node);
        }
    }
}