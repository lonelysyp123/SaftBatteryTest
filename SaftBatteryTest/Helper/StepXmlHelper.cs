using SaftBatteryTest.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SaftBatteryTest.Helper
{
    public class StepXmlHelper : XmlBase
    {
        /// <summary>
        /// 保存工步内容
        /// </summary>
        /// <param name="path">模板地址</param>
        /// <param name="newPath">新地址</param>
        /// <param name="steps">工程步骤</param>
        /// <param name="safemodel">安全模块</param>
        /// <param name="datarecordmodel">数据记录模块</param>
        /// <param name="notemodel">备注模块</param>
        /// <param name="barcodemodel">二维码模块</param>
        /// <param name="ordermodel">工单模块</param>
        public void StepSaveAs(string path, string newPath, StepModel[] steps, StepSafeModel safemodel, StepDataRecordModel datarecordmodel, StepNoteModel notemodel, StepBarCodeModel barcodemodel, StepOrderModel ordermodel)
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
            ModifyModel((XmlElement)xmlNodeList[1], safemodel);
            // datarecord model
            ModifyDataRecordModel((XmlElement)xmlNodeList[2], datarecordmodel);
            // note model
            ModifyModel((XmlElement)xmlNodeList[4], notemodel);
            // barcode model
            ModifyModel((XmlElement)xmlNodeList[5], barcodemodel);
            // order model
            ModifyModel((XmlElement)xmlNodeList[5], ordermodel);
            // save file as newPath
            xmlDocument.Save(newPath);
        }

        /// <summary>
        /// 保存工程步骤
        /// </summary>
        /// <param name="path">模板地址</param>
        /// <param name="newPath">存储位置</param>
        /// <param name="steps">工步</param>
        public void StepSaveAs(string path, string newPath, StepModel[] steps)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);
            XmlElement xmlElement = xmlDocument.DocumentElement;
            XmlNodeList xmlNodeList = xmlElement.ChildNodes;

            for (int i = 0; i < steps.Length; i++)
            {
                InsertStepNode(xmlDocument, (XmlElement)xmlNodeList[0], steps[i]);
            }
            xmlDocument.Save(newPath);
        }

        /// <summary>
        /// 复制模板到新的地址
        /// </summary>
        /// <param name="path">模板地址</param>
        /// <param name="newPath">存储位置</param>
        public void StepSaveAs(string path, string newPath)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);
            xmlDocument.Save(newPath);
        }

        /// <summary>
        /// 在xml文件里添加工步信息
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

        /// <summary>
        /// 获取step模板xml，然后新增工步，保存到新位置
        /// </summary>
        /// <param name="path">模板地址</param>
        /// <param name="newPath">新地址</param>
        /// <param name="step">新增工步</param>
        public void InsertStepNode(string path, string newPath, StepModel step)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);
            XmlElement xmlElement = xmlDocument.DocumentElement;
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
            xmlElement.ChildNodes[0].AppendChild(stepelement);
            xmlDocument.Save(newPath);
        }

        /// <summary>
        /// 数据记录模块内容修改
        /// </summary>
        /// <param name="xmlNode">指定节点</param>
        /// <param name="model">模块</param>
        private void ModifyDataRecordModel(XmlElement xmlNode, StepDataRecordModel model)
        {
            XmlNodeList xmlNodeList1 = xmlNode.ChildNodes;
            for (int i = 0; i < xmlNodeList1.Count; i++)
            {
                foreach (System.Reflection.PropertyInfo info in model.GetType().GetProperties())
                {
                    if (xmlNodeList1[i].Name.Contains(info.Name))
                    {
                        if (model.GetType().GetProperty(info.Name).PropertyType == typeof(bool))
                        {
                            ((XmlElement)xmlNodeList1[i]).SetAttribute("IsEnable", model.GetType().GetProperty(info.Name).GetValue(model, null).ToString());
                        }
                        else
                        {
                            xmlNodeList1[i].InnerText = model.GetType().GetProperty(info.Name).GetValue(model, null).ToString();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 修改指定的xml文件中数据记录模块内容
        /// </summary>
        /// <param name="path">xml文件地址</param>
        /// <param name="index">节点序号</param>
        /// <param name="model">模块</param>
        public void ModifyDataRecordModel(string path, int index, StepDataRecordModel model)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);
            XmlElement xmlElement = xmlDocument.DocumentElement;
            XmlNodeList xmlNodeList1 = xmlElement.ChildNodes[index].ChildNodes;
            for (int i = 0; i < xmlNodeList1.Count; i++)
            {
                foreach (System.Reflection.PropertyInfo info in model.GetType().GetProperties())
                {
                    if (info.Name.Contains(xmlNodeList1[i].Name))
                    {
                        if (model.GetType().GetProperty(info.Name).PropertyType == typeof(bool))
                        {
                            ((XmlElement)xmlNodeList1[i]).SetAttribute("IsEnable", model.GetType().GetProperty(info.Name).GetValue(model, null).ToString());
                        }
                        else
                        {
                            xmlNodeList1[i].InnerText = model.GetType().GetProperty(info.Name).GetValue(model, null).ToString();
                        }
                    }
                }
            }
            xmlDocument.Save(path);
        }

        public void ModifyDataRecordModelToView(string path, int index, ref StepDataRecordModel model)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);
            XmlElement xmlElement = xmlDocument.DocumentElement;
            XmlNodeList xmlNodeList1 = xmlElement.ChildNodes[index].ChildNodes;
            for (int i = 0; i < xmlNodeList1.Count; i++)
            {
                foreach (System.Reflection.PropertyInfo info in model.GetType().GetProperties())
                {
                    if (info.Name.Contains(xmlNodeList1[i].Name))
                    {
                        if (model.GetType().GetProperty(info.Name).PropertyType == typeof(bool))
                        {
                            model.GetType().GetProperty(info.Name).SetValue(model, bool.Parse(((XmlElement)xmlNodeList1[i]).GetAttribute("IsEnable")));
                        }
                        else
                        {
                            if (xmlNodeList1[i].InnerText != null)
                            {
                                model.GetType().GetProperty(info.Name).SetValue(model, double.Parse(xmlNodeList1[i].InnerText));
                            }
                            else
                            {
                                model.GetType().GetProperty(info.Name).SetValue(model, 0);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 通用节点内容修改
        /// </summary>
        /// <typeparam name="TEntity">模块类型</typeparam>
        /// <param name="xmlNode">指定节点</param>
        /// <param name="model">模块</param>
        private void ModifyModel<TEntity>(XmlElement xmlNode, TEntity model)
        {
            XmlNodeList xmlNodeList1 = xmlNode.ChildNodes;
            for (int i = 0; i < xmlNodeList1.Count; i++)
            {
                foreach (System.Reflection.PropertyInfo info in model.GetType().GetProperties())
                {
                    if (info.Name == xmlNodeList1[i].Name)
                    {
                        xmlNodeList1[i].InnerText = model.GetType().GetProperty(info.Name).GetValue(model, null).ToString();
                    }
                }
            }
        }

        /// <summary>
        /// 通用节点内容修改
        /// </summary>
        /// <typeparam name="TEntity">模块类型</typeparam>
        /// <param name="path">xml文件地址</param>
        /// <param name="index">节点序号</param>
        /// <param name="model">模块</param>
        public void ModifyModel<TEntity>(string path, int index, TEntity model)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);
            XmlElement xmlElement = xmlDocument.DocumentElement;
            XmlNodeList xmlNodeList1 = xmlElement.ChildNodes[index].ChildNodes;
            for (int i = 0; i < xmlNodeList1.Count; i++)
            {
                foreach (System.Reflection.PropertyInfo info in model.GetType().GetProperties())
                {
                    if (info.Name == xmlNodeList1[i].Name)
                    {
                        if (model.GetType().GetProperty(info.Name).GetValue(model, null) != null)
                        {
                            xmlNodeList1[i].InnerText = model.GetType().GetProperty(info.Name).GetValue(model, null).ToString();
                        }
                    }
                }
            }
            xmlDocument.Save(path);
        }

        /// <summary>
        /// 通用工步设置内容修改
        /// </summary>
        /// <typeparam name="TEntity">模块类型</typeparam>
        /// <param name="path">xml文件地址</param>
        /// <param name="index">节点序号</param>
        /// <param name="model">模块</param>
        public void ModifyModelToView<TEntity>(string path, int index,ref TEntity model)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);
            XmlElement xmlElement = xmlDocument.DocumentElement;
            var nodes = ReadChildrenNode((XmlElement)xmlElement.ChildNodes[index]);
            for (int p = 0; p < nodes.Length; p++)
            {
                if (nodes[p].Type == "double")
                {
                    if (nodes[p].Value == "")
                    {
                        model.GetType().GetProperty(nodes[p].Name).SetValue(model, 0);
                    }
                    else
                    {
                        model.GetType().GetProperty(nodes[p].Name).SetValue(model, double.Parse(nodes[p].Value));
                    }
                }
                else if (nodes[p].Type == "int")
                {
                    if (nodes[p].Value == "")
                    {
                        model.GetType().GetProperty(nodes[p].Name).SetValue(model, 0);
                    }
                    else
                    {
                        model.GetType().GetProperty(nodes[p].Name).SetValue(model, int.Parse(nodes[p].Value));
                    }
                }
                else if (nodes[p].Type == "bool")
                {
                    model.GetType().GetProperty(nodes[p].Name).SetValue(model, bool.Parse(nodes[p].Value));
                }
                else
                {
                    model.GetType().GetProperty(nodes[p].Name).SetValue(model, nodes[p].Value);
                }
            }
        }

        /// <summary>
        /// 修改指定xml文档中的指定节点内容
        /// </summary>
        /// <param name="path">xml文件地址</param>
        /// <param name="index">节点序号</param>
        /// <param name="nodeName">子节点名称</param>
        /// <param name="value">子节点值</param>
        public void ModifyInfoModel(string path, int index, string nodeName, string value)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);
            XmlElement xmlElement = xmlDocument.DocumentElement;
            XmlNodeList xmlNodeList1 = xmlElement.ChildNodes[index].ChildNodes;
            for (int i = 0; i < xmlNodeList1.Count; i++)
            {
                if (xmlNodeList1[i].Name == nodeName)
                {
                    xmlNodeList1[i].InnerText = value;
                }
            }
            xmlDocument.Save(path);
        }

        /// <summary>
        /// 从xml文件中读取工程步骤
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public StepModel[] ReadStepFromXml(string path)
        {
            List<StepModel> steps = new List<StepModel>();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(path);
            XmlElement xmlElement = xmlDocument.DocumentElement;
            for (int l = 0; l < xmlElement.ChildNodes[0].ChildNodes.Count; l++)
            {
                StepModel step = new StepModel();
                var nodes = ReadChildrenNode((XmlElement)xmlElement.ChildNodes[0].ChildNodes[l]);
                for (int p = 0; p < nodes.Length; p++)
                {
                    if (nodes[p].Type == "double")
                    {
                        double.TryParse(nodes[p].Value, out double value);
                        step.GetType().GetProperty(nodes[p].Name).SetValue(step, value);
                    }
                    else if (nodes[p].Type == "int")
                    {
                        int.TryParse(nodes[p].Value, out int value);
                        step.GetType().GetProperty(nodes[p].Name).SetValue(step, value);
                    }
                    else if (nodes[p].Type == "bool")
                    {
                        bool.TryParse(nodes[p].Value,out bool value);
                        step.GetType().GetProperty(nodes[p].Name).SetValue(step, value);
                    }
                    else if (nodes[p].Type == "workmode")
                    {
                        Enum.TryParse(nodes[p].Value, out WorkMode value);
                        step.GetType().GetProperty(nodes[p].Name).SetValue(step, value);
                    }
                    else if (nodes[p].Type == "string")
                    {
                        step.GetType().GetProperty(nodes[p].Name).SetValue(step, nodes[p].Value.ToString());
                    }
                    else
                    {
                        step.GetType().GetProperty(nodes[p].Name).SetValue(step, nodes[p].Value);
                    }
                }
                steps.Add(step);
            }

            return steps.ToArray();
        }
    }
}
