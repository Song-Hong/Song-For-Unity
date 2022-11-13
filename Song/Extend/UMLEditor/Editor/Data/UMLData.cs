using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using Unity.Jobs;
using UnityEngine;
using System;
using Codice.Client.BaseCommands;
using Unity.Burst;
using Unity.Collections;
using UnityEditor.TestTools.TestRunner.Api;

namespace Song.Extend.UML
{
    /// <summary>
    /// UML Data
    /// </summary>
    public class UMLData
    {
        #region Info
        public string name;
        public string path;
        #endregion

        #region Data
        public List<UMLNodeData> Data;
        #endregion

        public UMLData()
        {
            Data = new List<UMLNodeData>();
        }

        #region IO
        public static UMLData Load(string path)
        {
            UMLData umldata = new UMLData();
            XmlDocument uml = new XmlDocument();
            if (!File.Exists(path)) return null;
            try
            {
                uml.LoadXml(File.ReadAllText(path));
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return null;
            }
            var root = uml.SelectSingleNode("UML");
            umldata.name = root.SelectSingleNode("Name").InnerText;
            foreach (XmlNode item in root.SelectNodes("Node"))
            {
                UMLNodeData umlnodedata = new UMLNodeData();
                umlnodedata.name = item.SelectSingleNode("Name").InnerText;
                var rect = item.SelectSingleNode("Rect").InnerText.Split(",");
                if (rect.Length >= 4)
                {
                    try
                    {
                        umlnodedata.rect = new Rect(
                            float.Parse(rect[0]),
                            float.Parse(rect[1]),
                            float.Parse(rect[2]),
                            float.Parse(rect[3])
                        );
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                        umlnodedata.rect = Rect.zero;
                    }
                }
                else umlnodedata.rect = Rect.zero;
                foreach (XmlNode attribute in item.SelectSingleNode("Attributes").SelectNodes("Attribute"))
                {
                    umlnodedata.Attribute.Add(attribute.InnerText);
                }
                foreach (XmlNode method in item.SelectSingleNode("Methods").SelectNodes("Method"))
                {
                    umlnodedata.Attribute.Add(method.InnerText);
                }
                umldata.Data.Add(umlnodedata);
            }
            return umldata;
        }

        public static void Load(string path, Action<UMLNodeData> CallBack = null, Action<UMLData> EndCallBack = null)
        {
            UMLData umldata = new UMLData();
            XmlDocument uml = new XmlDocument();
            if (!File.Exists(path.ToString())) return;
            uml.LoadXml(File.ReadAllText(path.ToString()));
            var root = uml.SelectSingleNode("UML");
            umldata.name = root.SelectSingleNode("Name").InnerText;
            foreach (XmlNode item in root.SelectNodes("Node"))
            {
                UMLNodeData umlnodedata = new UMLNodeData();
                umlnodedata.name = item.SelectSingleNode("Name").InnerText;
                var rect = item.SelectSingleNode("Rect").InnerText.Split(",");
                if (rect.Length >= 4)
                {
                    try
                    {
                        umlnodedata.rect = new Rect(
                            float.Parse(rect[0]),
                            float.Parse(rect[1]),
                            float.Parse(rect[2]),
                            float.Parse(rect[3])
                        );
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex);
                        umlnodedata.rect = Rect.zero;
                    }
                }
                else umlnodedata.rect = Rect.zero;
                foreach (XmlNode attribute in item.SelectSingleNode("Attributes").SelectNodes("Attribute"))
                {
                    umlnodedata.Attribute.Add(attribute.InnerText);
                }
                foreach (XmlNode method in item.SelectSingleNode("Methods").SelectNodes("Method"))
                {
                    umlnodedata.Attribute.Add(method.InnerText);
                }
                CallBack?.Invoke(umlnodedata);
                umldata.Data.Add(umlnodedata);
            }
            EndCallBack?.Invoke(umldata);
        }

        /// <summary>
        /// executes the IO stream on the new thread
        /// </summary>
        public void SaveAsync() => SaveAsync(path);

        /// <summary>
        /// custome save path executes the IO stream on new thread
        /// </summary>
        /// <param name="path">file path</param>
        public void SaveAsync(string path)
        {
            Thread thread = new Thread(() =>
            {
                using (FileStream file = File.Open(path, FileMode.OpenOrCreate))
                {
                    byte[] data = new UTF8Encoding(true).GetBytes(ToString());
                    file.Write(data, 0, data.Length);
                }
            });
            thread.Start();
        }

        /// <summary>
        /// default path storage
        /// </summary>
        public void Save() => Save(path);

        /// <summary>
        /// custom path storage
        /// </summary>
        /// <param name="path">file path</param>
        public void Save(string path)
        {
            this.path = path;
            File.WriteAllText(path, ToString());
        }

        public override string ToString()
        {
            var uml = new XmlDocument();
            uml.AppendChild(uml.CreateXmlDeclaration("1.0", "utf-8", null));
            var root = uml.CreateElement("UML");
            uml.AppendChild(root);
            var uml_name = uml.CreateElement("Name");
            uml_name.InnerText = name;
            root.AppendChild(uml_name);
            foreach (var item in Data)
            {
                //node
                var node = uml.CreateElement("Node");
                //node name
                var node_name = uml.CreateElement("Name");
                node_name.InnerText = item.name;
                node.AppendChild(node_name);
                //node rect
                var node_rect = uml.CreateElement("Rect");
                node_rect.InnerText = $"{item.rect.x},{item.rect.y},{item.rect.width},{item.rect.height}";
                node.AppendChild(node_rect);
                //node attribute
                var node_attribute = uml.CreateElement("Attributes");
                foreach (var attribute in item.Attribute)
                {
                    var attribute_node = uml.CreateElement("Attribute");
                    attribute_node.InnerText = attribute;
                    node_attribute.AppendChild(attribute_node);
                }
                node_attribute.InnerText = item.name;
                node.AppendChild(node_attribute);
                //node method
                var node_method = uml.CreateElement("Methods");
                foreach (var method in item.Method)
                {
                    var method_node = uml.CreateElement("Method");
                    method_node.InnerText = method;
                    node_method.AppendChild(method_node);
                }
                node_method.InnerText = item.name;
                node.AppendChild(node_method);

                root.AppendChild(node);
            }
            return uml.InnerXml;
        }
        #endregion
    }
}
