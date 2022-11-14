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
    public class IPXmlHelper : XmlBase
    {
        /// <summary>
        /// 读取Xml文件中的IP列表
        /// </summary>
        /// <param name="Path">IP列表文件地址</param>
        /// <returns>IP列表</returns>
        public NodeBase[] ReadIPList(string Path)
        {
            List<string> IPList = new List<string>();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Path);
            XmlElement xmlElement = xmlDocument.DocumentElement;
            XmlNodeList xmlNodeList = xmlElement.ChildNodes;
            return ReadChildrenNode((XmlElement)xmlNodeList[0]);
        }

        /// <summary>
        /// 新增IP地址子节点
        /// </summary>
        /// <param name="Path">IP列表文件地址</param>
        /// <param name="IP">新增的IP地址</param>
        public void InsertIP(string Path, string IP)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Path);
            XmlElement xmlElement = xmlDocument.DocumentElement;
            XmlNodeList xmlNodeList = xmlElement.ChildNodes;
            XmlElement xmlNode = (XmlElement)xmlNodeList[0];
            AddChildNode(xmlDocument, xmlNode, "Address", "string", IP);
            xmlDocument.Save(Path);
        }

        /// <summary>
        /// 删除指定IP
        /// </summary>
        /// <param name="Path">IP列表文件地址</param>
        /// <param name="IP">删除的IP地址</param>
        public void DeleteIP(string Path, string IP)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Path);
            XmlElement xmlElement = xmlDocument.DocumentElement;
            XmlNodeList xmlNodeList = xmlElement.ChildNodes;
            XmlElement xmlNode = (XmlElement)xmlNodeList[0];
            DeleteNode(xmlNode, "Address", IP);
            xmlDocument.Save(Path);
        }

        /// <summary>
        /// 删除所有IP
        /// </summary>
        /// <param name="Path">IP列表文件地址</param>
        public void DeleteAllIP(string Path)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Path);
            XmlElement xmlElement = xmlDocument.DocumentElement;
            XmlNodeList xmlNodeList = xmlElement.ChildNodes;
            XmlElement xmlNode = (XmlElement)xmlNodeList[0];
            DeleteAll(xmlNode);
            xmlDocument.Save(Path);
        }
    }
}
