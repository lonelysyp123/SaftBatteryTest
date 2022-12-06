using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using SaftBatteryTest.Helper;
using SaftBatteryTest.Model;
using SaftBatteryTest.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace SaftBatteryTest.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private ObservableCollection<BatteryTestDev> _devList;
        public ObservableCollection<BatteryTestDev> DevList
        {
            get => _devList;
            set
            {
                SetProperty(ref _devList, value);
            }
        }

        private int _devIndex;
        public int DevIndex
        {
            get => _devIndex;
            set
            {
                SetProperty(ref _devIndex, value);
                // 改变选择的IP时，会改变主界面
                DevChange();
            }
        }

        public RelayCommand SetAutoOnlineCommand { get; set; }
        public RelayCommand OpenFileCommand { get; set; }
        public RelayCommand SetSavePathCommand { get; set; }
        public RelayCommand StepModifyCommand { get; set; }
        public RelayCommand AddIPCommand { get; set; }
        public RelayCommand AddIPsCommand { get; set; }
        public RelayCommand DeleteAllIPCommand { get; set; }
        public RelayCommand StartChannelCommand { get; set; }
        public RelayCommand StopChannelCommand { get; set; }
        public RelayCommand QueryStatusCommand { get; set; }
        public RelayCommand ShowInfomationCommand { get; set; }
        public RelayCommand SystemSetCommand { get; set; }

        public AppStateViewModel AppStateVM;
        private SystemSetViewModel SysVM;
        private IPXmlHelper helper = new IPXmlHelper();

        private const string IPConfigFilePath = "./Resource/Config/IPConfig.xml";
        private const string SysConfigFilePath = "./Resource/Config/SystemConfig.xml";
        private string DataPath = "";

        public MainViewModel()
        {
            SetAutoOnlineCommand = new RelayCommand(SetAutoOnline);
            OpenFileCommand = new RelayCommand(OpenFile);
            SetSavePathCommand = new RelayCommand(SetSavePath);
            StepModifyCommand = new RelayCommand(StepModify);
            AddIPCommand = new RelayCommand(AddIP);
            AddIPsCommand = new RelayCommand(AddIPs);
            DeleteAllIPCommand = new RelayCommand(DeleteAllIP);
            StartChannelCommand = new RelayCommand(StartChannel);
            StopChannelCommand = new RelayCommand(StopChannel);
            QueryStatusCommand = new RelayCommand(QueryStatus);
            ShowInfomationCommand = new RelayCommand(ShowInfomation);
            SystemSetCommand = new RelayCommand(SystemSet);

            DevList = new ObservableCollection<BatteryTestDev>();
            AppStateVM = new AppStateViewModel();
            SysVM = new SystemSetViewModel(SysConfigFilePath);

            // 初始化界面
            InitContent();
            // 初始化文件资源
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/Data");
        }


        #region Command
        private void SystemSet()
        {
            SystemSetView view = new SystemSetView(SysVM);
            view.ShowDialog();
        }

        private void ShowInfomation()
        {
            ChannelInfomationView view = new ChannelInfomationView();
            view.ShowDialog();
        }

        private void QueryStatus()
        {
            string msg = "未检测到设备!";
            // 判断是否有设备连接
            foreach (var item in DevList)
            {
                if (item.CommunicationState == "Connected")
                {
                    msg = "";
                    msg = msg + "IP:" + item.Address + "(" + item.Channels.Count + ")\n";
                }
            }
            MessageBox.Show(msg);
        }

        private void StopChannel()
        {
            if (DevList[DevIndex].CommunicationState == "Connected")
            {
                DevList[DevIndex].StopDaq();
            }
        }

        private void StartChannel()
        {
            if (DevList[DevIndex].CommunicationState == "Connected")
            {
                DevList[DevIndex].StartDaq();
            }
            else
            {
                MessageBox.Show("未连接设备");
            }
        }

        public void DeleteIP(string ip)
        {
            var objs = DevList.Where(dev => dev.Address == ip).ToList();
            for (int i = 0; i < objs.Count; i++)
            {
                DeleteIPByView(objs[i]);
                helper.DeleteIP(IPConfigFilePath, ip);
            }
        }

        public void DeleteAllIP()
        {
            DeleteAllIPByView();
            helper.DeleteAllIP(IPConfigFilePath);
        }

        AddIPsView IPsview;
        public void AddIPs()
        {
            IPsview = new AddIPsView();
            //! 根据配置文件来设置网段
            IPsview.IP1.SetAddressText("192.168.0.1");
            IPsview.IP2.SetAddressText("192.168.0.3");
            if (IPsview.ShowDialog() == true)
            {
                for (int i = IPsview.beforeN; i <= IPsview.afterN; i++)
                {
                    string ip = IPsview.segment + i.ToString();
                    //! 判断该IP是否存在
                    var objs = DevList.Where(dev => dev.Address == ip).ToList();
                    if (objs.Count == 0)
                    {
                        //! 界面上新增IP
                        AddIPInView(ip);
                        //! TODO 配置文件中新增IP
                        helper.InsertIP(IPConfigFilePath, ip);
                    }
                }
            }
        }

        AddIPView IPview;
        public void AddIP()
        {
            IPview = new AddIPView();
            if (IPview.ShowDialog() == true)
            {
                string ip = IPview.IPText.AddressText;
                //! 判断该IP是否存在
                var objs = DevList.Where(dev => dev.Address == ip).ToList();
                if (objs.Count == 0)
                {
                    //! 界面上新增IP
                    AddIPInView(ip);
                    //! TODO 配置文件中新增IP
                    helper.InsertIP(IPConfigFilePath, ip);
                }
            }
        }

        
        private void StepModify()
        {
            DevList[DevIndex].StartDaq();
        }

        private void SetSavePath()
        {
            PathSettingView view = new PathSettingView();
            if (view.ShowDialog() == true)
            {
                //! TODO 保存这个地址到本地的配置文件
                DataPath = view.PathTxt.Text;
                using (FileStream fs = new FileStream("./Resource/Config/DataPath.txt", FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                    {
                        sw.Write(DataPath);
                    }
                }
            }
        }

        private void OpenFile()
        {
            Microsoft.Win32.OpenFileDialog file = new Microsoft.Win32.OpenFileDialog();
            file.Filter = "数据文件|*.cds";
            file.ShowDialog();
            if (file.FileName != null && file.FileName != "")
            {
                //! 加载文档，根据规则生成数据
            }
        }


        private void SetAutoOnline()
        {
            AutoOnLineView view = new AutoOnLineView();
            view.ShowDialog();
        }
        #endregion

        /// <summary>
        /// 新增IP到界面
        /// </summary>
        /// <param name="ip"></param>
        private void AddIPInView(string ip)
        {
            BatteryTestDev dev = new BatteryTestDev(ip);
            DevList.Add(dev);
        }

        /// <summary>
        /// 从界面上删除IP
        /// </summary>
        /// <param name="dev">ip对应的设备对象</param>
        private void DeleteIPByView(BatteryTestDev dev)
        {
            DevList.Remove(dev);
        }

        /// <summary>
        /// 从界面上删除全部IP
        /// </summary>
        private void DeleteAllIPByView()
        {
            DevList.Clear();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void InitContent()
        {
            //! 根据"./Resource/Config/IPConfig.xml"文件中的IPList来初始化IP部分
            InitIP(IPConfigFilePath);
        }

        /// <summary>
        /// 初始化IP界面的内容
        /// </summary>
        /// <param name="path">IP相关文件地址</param>
        private void InitIP(string path)
        {
            var IPs = helper.ReadIPList(path);
            for (int i = 0; i < IPs.Length; i++)
            {
                AddIPInView(IPs[i].Value);
            }
        }

        private void DevChange()
        {
            
        }
    }
}
