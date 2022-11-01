using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Song.Extend.UML
{
    /// <summary>
    /// UML节点数据
    /// </summary>
    public class UMLNodeData 
    {
        /// <summary>
        /// 节点类型
        /// </summary>
        public UMLNodeType NodeType;
        /// <summary>
        /// 全部属性
        /// </summary>
        public List<string> Attributes;
        /// <summary>
        /// 全部方法
        /// </summary>
        public List<string> Methods;

        /// <summary>
        /// UML节点数据
        /// </summary>
        public UMLNodeData()
        {

        }
    }
}