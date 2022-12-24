using SaftBatteryTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SaftBatteryTest.Helper
{
    public class SystemXmlHelper : XmlBase
    {
        public SystemSetModel ReadPart1FromXml(string path)
        {
            SystemSetModel model = new SystemSetModel();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);
            XmlElement xmlElement = xmlDocument.DocumentElement;
            XmlNodeList xmlNodeList1 = xmlElement.ChildNodes[0].ChildNodes;
            for (int i = 0; i < xmlNodeList1.Count; i++)
            {
                //foreach (System.Reflection.PropertyInfo info in model.GetType().GetProperties())
                //{
                //    if (info.Name(xmlNodeList1[i].Name))
                //    {
                //        if (model.GetType().GetProperty(info.Name).PropertyType == typeof(bool))
                //        {
                //            model.GetType().GetProperty(info.Name).SetValue(model, bool.Parse(((XmlElement)xmlNodeList1[i]).GetAttribute("IsEnable")));
                //        }
                //        else
                //        {
                //            if (xmlNodeList1[i].InnerText != null)
                //            {
                //                model.GetType().GetProperty(info.Name).SetValue(model, double.Parse(xmlNodeList1[i].InnerText));
                //            }
                //            else
                //            {
                //                model.GetType().GetProperty(info.Name).SetValue(model, 0);
                //            }
                //        }
                //    }
                //}
            }
            return model;
        }
    }
}
