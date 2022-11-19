using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Song.Extend.UML
{
    /// <summary>
    /// UML view
    /// </summary>
    public class UMLView : GraphView
    {
        /// <summary>
        /// initialize
        /// </summary>
        public UMLView()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new FreehandSelector());

            GridBackground grid = new GridBackground();
            Insert(0, grid);
            this.StretchToParentSize();
        }

        /// <summary>
        /// custom Contextual Menu
        /// </summary>
        /// <param name="evt"></param>
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("test",
                delegate(DropdownMenuAction action)
                {
                    
                },
                delegate(DropdownMenuAction action)
                {
                    return DropdownMenuAction.Status.None;
                });
            // evt.
            base.BuildContextualMenu(evt);
        }

        /// <summary>
        /// Connecting two ports
        /// </summary>
        /// <param name="_outputPort">out port</param>
        /// <param name="_inputPort">in port</param>
        public void AddEdgeByPorts(Port outputPort, Port inputPort)
        {
            Edge tempEdge = new Edge()
            {
                output = outputPort,
                input = inputPort
            };
            tempEdge.input.Connect(tempEdge);
            tempEdge.output.Connect(tempEdge);
            Add(tempEdge);
        }

        /// <summary>
        /// Handles the cables between nodes
        /// </summary>
        /// <param name="startPort"></param>
        /// <param name="nodeAdapter"></param>
        /// <returns></returns>
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
            var startPortView   = startPort;

            ports.ForEach((port) =>
            {
                var portView = port;
                if (startPortView != portView && startPortView.node != portView.node)
                    compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }
    }
}