using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SaftBatteryTest.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public StepSettingViewModel()
        {
            AddRowCommand = new RelayCommand(AddRow);
            DeleteRowCommand = new RelayCommand(DeleteRow);

            // 初始化DataGrid
            StepList = new ObservableCollection<StepModel>();
            StepList.Add(new StepModel() { ID = StepList.Count + 1});
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
