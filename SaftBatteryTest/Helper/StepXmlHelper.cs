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
        public void StepSaveAs(string path, string newPath, StepModel[] steps)
        {
            // open .xml file
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);
            XmlElement xmlElement = xmlDocument.DocumentElement;
            XmlNodeList xmlNodeList = xmlElement.ChildNodes;

            // step add into steplist
            for (int i = 0; i < steps.Length; i++)
            {
                InsertStepNode(xmlDocument, (XmlElement)xmlNodeList[0], steps[i]);
            }

            // safe model


            // save file as newPath
            xmlDocument.Save(newPath);
        }

        /// <summary>
        /// 在xml文件里记录工步信息
        /// </summary>
        /// <param name="xmlDocument">xml文件对象(用来创建节点)</param>
        /// <param name="xmlNode">父节点</param>
        /// <param name="step">工步信息</param>
        private void InsertStepNode(XmlDocument xmlDocument, XmlElement xmlNode, StepModel step)
        {
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
        }


        private void ModifySafeModel(XmlElement xmlNode, StepSafeModel model)
        {
            //XmlDocument xmlDocument = new XmlDocument();
            //xmlDocument.Load(path);
            //XmlElement xmlElement = xmlDocument.DocumentElement;
            //XmlNodeList xmlNodeList = xmlElement.ChildNodes;
            //XmlElement xmlNode = (XmlElement)xmlNodeList[0];

            //XmlNodeList xmlNodeList1 = xmlNode.ChildNodes;
            //for (int i = 0; i < xmlNodeList1.Count; i++)
            //{
            //    if(xmlNodeList1[i].ChildNodes[0].InnerText == ID)
            //    {
            //        for (int l = 0; l < xmlNodeList1[i].ChildNodes.Count; l++)
            //        {
            //            if(xmlNodeList1[i].ChildNodes[l].Name == propertyName)
            //            {
            //                xmlNodeList1[i].ChildNodes[l].InnerText = newValue;
            //            }
            //        }
            //    }
            //}
        }
    }
}
