using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Song.Extend.UML
{
    /// <summary>
    /// UML 视图
    /// </summary>
    public class UMLView : GraphView
    {
        /// <summary>
        /// 初始化
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
        /// 构建右键菜单
        /// </summary>
        /// <param name="evt"></param>
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
            
        }

        /// <summary>
        /// 处理节点之间的连线
        /// </summary>
        /// <param name="startPort"></param>
        /// <param name="nodeAdapter"></param>
        /// <returns></returns>
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();
            var startPortView = startPort;

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