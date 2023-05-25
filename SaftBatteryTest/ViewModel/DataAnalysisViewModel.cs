using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using SaftBatteryTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaftBatteryTest.ViewModel
{
    public class DataAnalysisViewModel : ObservableObject
    {
        private PlotModel _batteryData;
        public PlotModel BatteryData
        {
            get => _batteryData;
            set
            {
                SetProperty(ref _batteryData, value);
            }
        }

        public RelayCommand DisplayVolDataCommand { get; set; }
        public RelayCommand DisplayElcDataCommand { get; set; }

        private StoreModel storeModel;
        private StepSettingViewModel stepSettingViewModel;

        public DataAnalysisViewModel(StoreModel store, StepSettingViewModel step) 
        {
            DisplayVolDataCommand = new RelayCommand(DisplayVolData);
            DisplayElcDataCommand = new RelayCommand(DisplayElcData);

            BatteryData = new PlotModel();
            storeModel = store;
            stepSettingViewModel = step;

            InitChart("电压", "时间");
            ChartShowNow(storeModel.VolCollect.ToArray());
        }

        // 事件绑定
        private void DisplayElcData()
        {
            InitChart("电流", "时间");
            ChartShowNow(storeModel.ElcCollect.ToArray());
        }

        private void DisplayVolData()
        {
            InitChart("电压", "时间");
            ChartShowNow(storeModel.VolCollect.ToArray());
        }

        /// <summary>
        /// 初始化图表控件（定义X，Y轴）
        /// </summary>
        private void InitChart(string LeftName, string BottomName)
        {
            //! Axes
            BatteryData.Axes.Clear();
            BatteryData.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = LeftName });
            BatteryData.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = BottomName });
        }

        /// <summary>
        /// 绘制光谱
        /// </summary>
        /// <param name="Data">数据</param>
        private void ChartShowNow(double[] Data)
        {
            try
            {
                //! Series
                LineSeries lineSeries = new LineSeries();
                for (int i = 0; i < Data.Length; i++)
                {
                    lineSeries.Points.Add(new DataPoint(i, Data[i]));
                }

                BatteryData.Series.Clear();
                BatteryData.Series.Add(lineSeries);
                BatteryData.InvalidatePlot(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
