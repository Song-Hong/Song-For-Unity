using System.Collections;
using System.Collections.Generic;
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
        #region assets patg
        private readonly string bgcstyle_path = "Assets/Song/Extend/UMLEditor/Editor/View/UMLEditorBackGround.uss";
        #endregion

        #region private field 
        private UMLView view;
        #endregion

        [MenuItem("Song/UML")]
        public static void ShowWindow()
        {
            UMLEditorWindow window = GetWindow<UMLEditorWindow>();
            window.titleContent = new GUIContent("UML editor");
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
            AddDialogueNode();//ass node
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

        public void AddDialogueNode()
        {
            // 1. 创建Node
            UMLNode node = new UMLNode();
            node.title = "node1";
            node.SetPosition(new Rect(x: 100, y: 200, width: 100, height: 150));
            var iport = node.InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(float));
            iport.portName = "next";
            node.outputContainer.Add(iport);
            node.RefreshExpandedState();
            node.RefreshPorts();
            view.AddElement(node);

            //2. 创建Node
            UMLNode node1 = new UMLNode();
            node1.title = "node2";
            node1.SetPosition(new Rect(x: 200, y: 200, width: 100, height: 150));
            var iport1 = node1.InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(float));
            iport1.portName = "input";
            node1.inputContainer.Add(iport1);
            node1.RefreshExpandedState();
            node1.RefreshPorts();
            view.AddElement(node1);
        }

    }
}