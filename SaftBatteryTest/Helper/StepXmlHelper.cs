using SaftBatteryTest.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SaftBatteryTest.Helper
{
    public class StepXmlHelper : XmlBase
    {
        #region Step
        /// <summary>
        /// 新增步骤
        /// </summary>
        /// <param name="path"></param>
        /// <param name="step"></param>
        public void InsertStepNode(string path, StepModel step)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);
            XmlElement xmlElement = xmlDocument.DocumentElement;
            XmlNodeList xmlNodeList = xmlElement.ChildNodes;
            XmlElement xmlNode = (XmlElement)xmlNodeList[0];
            XmlElement stepelement = xmlDocument.CreateElement("Step");
            if (step != null)
            {
                foreach (System.Reflection.PropertyInfo info in step.GetType().GetProperties())
                {
                    if (step.GetType().GetProperty(info.Name).GetValue(step, null) != null)
                    {
                        AddChildNode(xmlDocument, stepelement, info.Name, info.PropertyType.Name, step.GetType().GetProperty(info.Name).GetValue(step, null).ToString());
                    }
                    else
                    {
                        AddChildNode(xmlDocument, stepelement, info.Name, info.PropertyType.Name);
                    }
                }
            }
            xmlNode.AppendChild(stepelement);
            xmlDocument.Save(path);
        }

        public void ModifyStep(string path, string ID, string propertyName, string newValue)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);
            XmlElement xmlElement = xmlDocument.DocumentElement;
            XmlNodeList xmlNodeList = xmlElement.ChildNodes;
            XmlElement xmlNode = (XmlElement)xmlNodeList[0];

            XmlNodeList xmlNodeList1 = xmlNode.ChildNodes;
            for (int i = 0; i < xmlNodeList1.Count; i++)
            {
                if(xmlNodeList1[i].ChildNodes[0].InnerText == ID)
                {
                    for (int l = 0; l < xmlNodeList1[i].ChildNodes.Count; l++)
                    {
                        if(xmlNodeList1[i].ChildNodes[l].Name == propertyName)
                        {
                            xmlNodeList1[i].ChildNodes[l].InnerText = newValue;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 将Step文件保存到指定位置
        /// </summary>
        /// <param name="oldPath">模板位置</param>
        /// <param name="newPath">指定位置</param>
        public void SaveXml(string oldPath, string newPath)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(oldPath);
            xmlDocument.Save(newPath);
        }
        #endregion
    }
}
