using CommunityToolkit.Mvvm.ComponentModel;
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

        private StoreModel storeModel;
        private StepSettingViewModel stepSettingViewModel;

        public DataAnalysisViewModel(StoreModel store, StepSettingViewModel step) 
        {
            BatteryData = new PlotModel();

            storeModel = store;
            stepSettingViewModel = step;

            InitChart();
        }

        /// <summary>
        /// 初始化图表控件（定义X，Y轴）
        /// </summary>
        private void InitChart()
        {
            //! Axes
            BatteryData.Axes.Clear();
            BatteryData.Axes.Add(new LinearAxis() { Position = AxisPosition.Left, Title = "电压" });
            BatteryData.Axes.Add(new LinearAxis() { Position = AxisPosition.Bottom, Title = "时间" });

            ChartShowNow(storeModel.VolCollect.ToArray());
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
