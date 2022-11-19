using SaftBatteryTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SaftBatteryTest.Helper
{
    public class XmlBase
    {
        public XmlBase()
        {

        }

        /// <summary>
        /// 添加子节点
        /// </summary>
        /// <param name="xmlDoc">Xml文档</param>
        /// <param name="xmlNode">父节点</param>
        /// <param name="node">子节点</param>
        protected void AddChildNode(XmlDocument xmlDoc, XmlElement parentNode, NodeBase node)
        {
            XmlElement element = xmlDoc.CreateElement(node.Name);
            element.SetAttribute("Type", node.Type);
            element.InnerText = node.Value;
            parentNode.AppendChild(element);
        }

        /// <summary>
        /// 增加子节点
        /// </summary>
        /// <param name="xmlDoc">Xml文档</param>
        /// <param name="parentNode">父节点</param>
        /// <param name="name">子节点名称</param>
        /// <param name="type">子节点类型</param>
        /// <param name="value">子节点值</param>
        protected void AddChildNode(XmlDocument xmlDoc, XmlElement parentNode, string name, string type = null, string value = null)
        {
            XmlElement element = xmlDoc.CreateElement(name);
            if (type != null)
            {
                if(type.Contains("Int"))
                {
                    type = "Int";
                }
                element.SetAttribute("Type", type.ToLower());
            }
            if (value != null)
            {
                element.InnerText = value;
            }
            parentNode.AppendChild(element);
        }

        /// <summary>
        /// 删除指定节点
        /// </summary>
        /// <param name="Node">父节点</param>
        /// <param name="node">指定节点</param>
        protected void DeleteNode(XmlElement parentNode, NodeBase node)
        {
            XmlNodeList NodeList = parentNode.ChildNodes;
            for (int i = 0; i < NodeList.Count; i++)
            {
                if (NodeList[i].Name == node.Name && NodeList[i].InnerText == node.Value)
                {
                    parentNode.RemoveChild(NodeList[i]);
                }
            }
        }

        /// <summary>
        /// 删除指定节点
        /// </summary>
        /// <param name="parentNode">父节点</param>
        /// <param name="name">指定节点名称</param>
        /// <param name="value">指定节点值</param>
        protected void DeleteNode(XmlElement parentNode, string name, string value)
        {
            XmlNodeList NodeList = parentNode.ChildNodes;
            for (int i = 0; i < NodeList.Count; i++)
            {
                if (NodeList[i].Name == name && NodeList[i].InnerText == value)
                {
                    parentNode.RemoveChild(NodeList[i]);
                }
            }
        }

        /// <summary>
        /// 删除所有节点
        /// </summary>
        /// <param name="parentNode"></param>
        protected void DeleteAll(XmlElement parentNode)
        {
            XmlNodeList NodeList = parentNode.ChildNodes;
            int count = NodeList.Count;
            for (int i = 0; i < count; i++)
            {
                if (NodeList[0].Name == "Address")
                {
                    parentNode.RemoveChild(NodeList[0]);
                }
            }
        }

        /// <summary>
        /// 读取当前节点
        /// </summary>
        /// <param name="nodeName">节点</param>
        /// <returns></returns>
        protected NodeBase ReadNode(XmlElement node)
        {
            NodeBase nodeb = new NodeBase();
            nodeb.Name = node.Name;
            nodeb.Type = node.GetAttribute("Type");
            nodeb.Value = node.InnerText;
            return nodeb;
        }

        /// <summary>
        /// 读取子节点
        /// </summary>
        /// <param name="parentNode">父节点名称</param>
        /// <returns></returns>
        protected NodeBase[] ReadChildrenNode(XmlElement parentNode)
        {
            List<NodeBase> nodes = new List<NodeBase>();
            XmlNodeList NodeList = parentNode.ChildNodes;
            for (int i = 0; i < NodeList.Count; i++)
            {
                XmlElement Node = (XmlElement)NodeList[i];
                nodes.Add(new NodeBase() { Name=Node.Name, Value=Node.InnerText, Type=Node.GetAttribute("Type")});
            }
            return nodes.ToArray();
        }

        /// <summary>
        /// 读取指定节点信息
        /// </summary>
        /// <param name="path">xml文件地址</param>
        /// <param name="index">节点序号</param>
        /// <param name="nodeName">节点名称</param>
        /// <returns>节点内容</returns>
        public string ReadSpecifyInfo(string path, int index, string nodeName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);
            XmlElement xmlElement = xmlDocument.DocumentElement;
            XmlNodeList xmlNodeList1 = xmlElement.ChildNodes[index].ChildNodes;
            for (int i = 0; i < xmlNodeList1.Count; i++)
            {
                if (xmlNodeList1[i].Name == nodeName)
                {
                    return xmlNodeList1[i].InnerText;
                }
            }
            return "";
        }
    }
}
