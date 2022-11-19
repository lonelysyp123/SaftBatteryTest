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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

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

        private StepDataRecordModel _dataRecord;
        public StepDataRecordModel DataRecord
        {
            get => _dataRecord;
            set
            {
                SetProperty(ref _dataRecord, value);
            }
        }

        private StepBarCodeModel _barCode;
        public StepBarCodeModel BarCode
        {
            get => _barCode;
            set
            {
                SetProperty(ref _barCode, value);
            }
        }

        private StepNoteModel _stepNote;
        public StepNoteModel StepNote
        {
            get => _stepNote;
            set
            {
                SetProperty(ref _stepNote, value);
            }
        }

        private StepOrderModel _stepOrder;
        public StepOrderModel StepOrder
        {
            get => _stepOrder;
            set
            {
                SetProperty(ref _stepOrder, value);
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
            DataRecord = new StepDataRecordModel();
            BarCode = new StepBarCodeModel();
            StepNote = new StepNoteModel();
            StepOrder = new StepOrderModel();

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
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "工步文件|*.xml";
            file.ShowDialog();
            if (file.FileName != null && file.FileName != "")
            {
                // 工程步骤更新
                var steps = helper.ReadStepFromXml(file.FileName);
                for (int i = 0; i < steps.Length; i++)
                {
                    StepList.Add(steps[i]);
                }
                // 更新 safemodel datarecord info note barcode order
                StepSafeModel safemodel = new StepSafeModel();
                helper.ModifyModelToView(file.FileName, 1, ref safemodel);
                StepSafe = safemodel;

                // 因为 datarecord 和 info有和其他节点不同的地方所以单独处理
                StepDataRecordModel datarecordmodel = new StepDataRecordModel();
                helper.ModifyDataRecordModelToView(file.FileName, 2, ref datarecordmodel);
                DataRecord = datarecordmodel;
                if (helper.ReadSpecifyInfo(file.FileName, 3, "RunIndex") != "")
                {
                    RunIndex = int.Parse(helper.ReadSpecifyInfo(file.FileName, 3, "RunIndex"));
                }
                DataFileSavePath = helper.ReadSpecifyInfo(file.FileName, 3, "DataFileSavePath");
                if (helper.ReadSpecifyInfo(file.FileName, 3, "IsBackup") != "")
                {
                    IsBackup = bool.Parse(helper.ReadSpecifyInfo(file.FileName, 3, "IsBackup"));
                }
                FilePath = helper.ReadSpecifyInfo(file.FileName, 3, "FilePath");

                StepNoteModel notemodel = new StepNoteModel();
                helper.ModifyModelToView(file.FileName, 4, ref notemodel);
                StepNote = notemodel;

                StepBarCodeModel barcodemodel = new StepBarCodeModel();
                helper.ModifyModelToView(file.FileName, 5, ref barcodemodel);
                BarCode = barcodemodel;

                StepOrderModel ordermodel = new StepOrderModel();
                helper.ModifyModelToView(file.FileName, 6, ref ordermodel);
                StepOrder = ordermodel;
            }
        }

        private void SaveStep()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "Data\\Step\\";
            dialog.Filter = "工步文件|*.xml";
            dialog.DefaultExt = "工步文件|*.xml";
            if (dialog.ShowDialog() == true)
            {
                if (StepList.Count > 0)
                {
                    helper.StepSaveAs(StepConfigFilePath, dialog.FileName, StepList.ToArray());
                }
                else
                {
                    helper.StepSaveAs(StepConfigFilePath, dialog.FileName);
                }
                // 更新 safemodel datarecord info note barcode order
                helper.ModifyModel(dialog.FileName, 1, StepSafe);

                // 因为 datarecord 和 info有和其他节点不同的地方所以单独处理
                helper.ModifyDataRecordModel(dialog.FileName, 2, DataRecord);
                helper.ModifyInfoModel(dialog.FileName, 3, "RunIndex", RunIndex.ToString());
                helper.ModifyInfoModel(dialog.FileName, 3, "DataFileSavePath", DataFileSavePath);
                helper.ModifyInfoModel(dialog.FileName, 3, "IsBackup", IsBackup.ToString());
                helper.ModifyInfoModel(dialog.FileName, 3, "FilePath", FilePath);

                helper.ModifyModel(dialog.FileName, 4, StepNote);
                helper.ModifyModel(dialog.FileName, 5, BarCode);
                helper.ModifyModel(dialog.FileName, 6, StepOrder);
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
