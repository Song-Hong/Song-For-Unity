using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Song.Extend.UML
{
    /// <summary>
    /// UML Node Data
    /// </summary>
    public class UMLNodeData
    {
        public string name;
        public List<string> Attribute;
        public List<string> Method;
        public Rect   rect;
        public UMLNodeData()
        {
            Attribute = new List<string>();
            Method    = new List<string>();
        }
    }
}