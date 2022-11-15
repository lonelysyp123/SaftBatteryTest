using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using SaftBatteryTest.Helper;
using SaftBatteryTest.Model;
using SaftBatteryTest.View;
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

        private StepSafeModel _stepSafe;
        public StepSafeModel StepSafe
        { 
            get => _stepSafe; 
            set
            {
                SetProperty(ref _stepSafe, value);
            }
        }

        private DataRecordModel _dataRecord;
        public DataRecordModel DataRecord
        {
            get => _dataRecord;
            set
            {
                SetProperty(ref _dataRecord, value);
            }
        }

        private StepInfoModel _stepInfo;
        public StepInfoModel StepInfo
        {
            get => _stepInfo;
            set
            {
                SetProperty(ref _stepInfo, value);
            }
        }

        private int _runIndex;
        public int RunIndex
        {
            get => _runIndex;
            set
            {
                SetProperty(ref _runIndex, value);
            }
        }

        private string _dataFileSavePath;
        public string DataFileSavePath
        {
            get => _dataFileSavePath;
            set
            {
                SetProperty(ref _dataFileSavePath, value);
            }
        }

        private bool _isBackup;
        public bool IsBackup
        {
            get => _isBackup;
            set
            {
                SetProperty(ref _isBackup, value);
            }
        }

        private string _filePath;
        public string FilePath
        {
            get => _filePath;
            set
            {
                SetProperty(ref _filePath, value);
            }
        }

        public RelayCommand AddRowCommand { get; set; }
        public RelayCommand DeleteRowCommand { get; set; }
        public RelayCommand SaveStepCommand { get; set; }
        public RelayCommand LoadStepCommand { get; set; }
        public RelayCommand SetSavePathCommand { get; set; }
        public RelayCommand SetFilePathCommand { get; set; }

        private StepXmlHelper helper = new StepXmlHelper();
        private const string StepConfigFilePath = "./Resource/Config/StepConfig.xml";


        public StepSettingViewModel()
        {
            AddRowCommand = new RelayCommand(AddRow);
            DeleteRowCommand = new RelayCommand(DeleteRow);
            SaveStepCommand = new RelayCommand(SaveStep);
            LoadStepCommand = new RelayCommand(LoadStep);
            SetSavePathCommand = new RelayCommand(SetSavePath);
            SetFilePathCommand = new RelayCommand(SetFilePath);

            // 初始化DataGrid
            StepList = new ObservableCollection<StepModel>();
            StepSafe = new StepSafeModel();
            DataRecord = new DataRecordModel();
            StepInfo = new StepInfoModel();

            // 初始化文件资源
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/Data/Step");
        }

        private void SetFilePath()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FilePath = dialog.SelectedPath;
            }
        }

        private void SetSavePath()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataFileSavePath = dialog.SelectedPath;
            }
        }

        private void LoadStep()
        {
            Console.WriteLine("Loading Step");
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
                    helper.StepSaveAs(StepConfigFilePath, dialog.FileName, StepList.ToArray());
                }
            }
        }

        private void DeleteRow()
        {
            if (StepList.Count > 0)
            {
                StepList.RemoveAt(StepList.Count - 1);
            }
        }

        private void AddRow()
        {
            StepList.Add(new StepModel() { ID = StepList.Count + 1 });
        }
    }
}
