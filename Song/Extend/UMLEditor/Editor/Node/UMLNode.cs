using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine.UIElements;
using Node = UnityEditor.Experimental.GraphView.Node;

namespace Song.Extend.UML
{
    /// <summary>
    /// UML node
    /// </summary>
    public class UmlNode:Node
    {
        public UmlNode(UMLNodeData data)
        {
            #region hidden default 
            var node = this.Q<VisualElement>("node-border");
            if (node != null)
            {
                Remove(node);
            }
            #endregion

            #region set self style
            var borderColor = new Color(0.125f,0.125f,0.125f);
            var bgc = new Color(0.219f,0.219f,0.219f);
            //set size
            style.height = (data.Attribute.Count+data.Method.Count)*30 + 100;
            style.width = 200;
            //set bgc
            style.backgroundColor = bgc;
            //set border
            style.borderBottomWidth = 2;
            style.borderRightWidth = 1;
            style.borderBottomColor = borderColor;
            style.borderRightColor = borderColor;
            style.borderBottomLeftRadius = 8;
            style.borderBottomRightRadius = 8;
            style.borderTopLeftRadius = 8;
            style.borderTopRightRadius = 8;
            #endregion

            var headName = new Label()
            {
                text = data.name,
                style =
                {
                    top = 2,
                    height = 26,
                    unityTextAlign = TextAnchor.MiddleCenter
                }
            };
            Add(headName);
            
            foreach (var attribute in  data.Attribute)
            {
                TextField(attribute);
            }

            foreach (var method in data.Method)
            {
                TextField(method);
            }
        }

        public void TextField(string value)
        {
            TextField text = new TextField()
            {
                value = value,
                style=
                {
                    top = 2,
                    height = 26,
                    width = Length.Percent(92),
                    left = Length.Percent(2),
                    right = Length.Percent(2)
                }
            };
            Add(text);
        }
    }
}
