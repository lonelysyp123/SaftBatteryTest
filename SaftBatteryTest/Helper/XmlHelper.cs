using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SaftBatteryTest.Helper
{
    public static class XmlHelper
    {
        /// <summary>
        /// 读取Xml文件中的IP列表
        /// </summary>
        /// <param name="Path">IP列表文件地址</param>
        /// <returns>IP列表</returns>
        public static string[] ReadIPList(string Path)
        {
            List<string> IPList = new List<string>();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Path);
            XmlElement xmlElement = xmlDocument.DocumentElement;
            XmlNodeList xmlNodeList = xmlElement.ChildNodes;
            if (xmlNodeList.Count == 1)
            {
                XmlElement xmlNode = (XmlElement)xmlNodeList[0];
                if (xmlNode.Name == "IPList")
                {
                    XmlNodeList NodeList = xmlNode.ChildNodes;
                    for (int i = 0; i < NodeList.Count; i++)
                    {
                        if (NodeList[i].Name == "Address")
                        {
                            IPList.Add(NodeList[i].InnerText);
                        }
                    }
                }
            }
            return IPList.ToArray();
        }

        /// <summary>
        /// 新增IP地址子节点
        /// </summary>
        /// <param name="Path">IP列表文件地址</param>
        /// <param name="IP">新增的IP地址</param>
        public static void InsertIP(string Path, string IP)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Path);
            XmlElement xmlElement = xmlDocument.DocumentElement;
            XmlNodeList xmlNodeList = xmlElement.ChildNodes;
            for (int l = 0; l < xmlNodeList.Count; l++)
            {
                XmlElement xmlNode = (XmlElement)xmlNodeList[l];
                string NodeName = xmlNode.Name;
                if (NodeName == "IPList")
                {
                    XmlElement node = xmlDocument.CreateElement("Address");
                    node.InnerText = IP;
                    xmlNode.AppendChild(node);
                }
            }
            xmlDocument.Save(Path);
        }

        /// <summary>
        /// 删除指定IP
        /// </summary>
        /// <param name="Path">IP列表文件地址</param>
        /// <param name="IP">删除的IP地址</param>
        public static void DeleteIP(string Path, string IP)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Path);
            XmlElement xmlElement = xmlDocument.DocumentElement;
            XmlNodeList xmlNodeList = xmlElement.ChildNodes;
            for (int l = 0; l < xmlNodeList.Count; l++)
            {
                XmlElement xmlNode = (XmlElement)xmlNodeList[l];
                string NodeName = xmlNode.Name;
                if (NodeName == "IPList")
                {
                    XmlNodeList NodeList = xmlNode.ChildNodes;
                    for (int i = 0; i < NodeList.Count; i++)
                    {
                        if (NodeList[i].Name == "Address" && NodeList[i].InnerText == IP)
                        {
                            xmlNode.RemoveChild(NodeList[i]);
                        }
                    }
                }
            }
            xmlDocument.Save(Path);
        }

        /// <summary>
        /// 删除所有IP
        /// </summary>
        /// <param name="Path">IP列表文件地址</param>
        public static void DeleteAllIP(string Path)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Path);
            XmlElement xmlElement = xmlDocument.DocumentElement;
            XmlNodeList xmlNodeList = xmlElement.ChildNodes;
            for (int l = 0; l < xmlNodeList.Count; l++)
            {
                XmlElement xmlNode = (XmlElement)xmlNodeList[l];
                string NodeName = xmlNode.Name;
                if (NodeName == "IPList")
                {
                    XmlNodeList NodeList = xmlNode.ChildNodes;
                    int count = NodeList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        if (NodeList[0].Name == "Address")
                        {
                            xmlNode.RemoveChild(NodeList[0]);
                        }
                    }
                }
            }
            xmlDocument.Save(Path);
        }
    }
}
