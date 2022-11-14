using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using SaftBatteryTest.Helper;
using SaftBatteryTest.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaftBatteryTest.ViewModel
{
    public class StepSettingViewModel : ObservableObject
    {
        private ObservableCollection<StepModel> _stepList;
        public ObservableCollection<StepModel> StepList
        { 
            get => _stepList; 
            set
            {
                SetProperty(ref _stepList, value);
            }
        }

        public RelayCommand AddRowCommand { get; set; }
        public RelayCommand DeleteRowCommand { get; set; }
        public RelayCommand SaveStepCommand { get; set; }

        private StepXmlHelper helper = new StepXmlHelper();
        private const string StepConfigFilePath = "./Resource/Config/StepConfig.xml";

        public StepSettingViewModel()
        {
            AddRowCommand = new RelayCommand(AddRow);
            DeleteRowCommand = new RelayCommand(DeleteRow);
            SaveStepCommand = new RelayCommand(SaveStep);

            // 初始化DataGrid
            StepList = new ObservableCollection<StepModel>();
            StepList.Add(new StepModel() { ID = StepList.Count + 1});

            // 初始化文件资源
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/Data/Step");
        }

        private void SaveStep()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Data\\Step\\";
            dialog.Filter = "数据文件|*.xml";
            dialog.DefaultExt = "数据文件|*.xml";
            if (dialog.ShowDialog() == true)
            {
                if (StepList.Count > 0)
                {
                    helper.InsertStepNode(StepConfigFilePath, StepList[0]);
                }

                helper.SaveXml(StepConfigFilePath, dialog.FileName);
            }
        }

        private void DeleteRow()
        {
            StepList.RemoveAt(StepList.Count - 1);
        }

        private void AddRow()
        {
            StepList.Add(new StepModel() { ID = StepList.Count + 1 });
        }
    }
}
