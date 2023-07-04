using BatterySimulator.Base;
using BatterySimulator.EMS;
using BatterySimulator.Interface;
using BatterySimulator.Model;
using BMS.Communication;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BatterySimulator
{
    public class MainDev : DevBase
    {
        public BatteryTotalBase BatteryTotal { get; set; }

        public MainDev()
            : base()
        {
            BatteryTotal = new BatteryTotalBase(1);

        }

        public void InitBattery()
        {
            for (int i = 0; i < 3; i++)
            {
                BatterySeriesBase series = new BatterySeriesBase(i + 1);
                for (int j = 0; j < 5; j++)
                {
                    EMS.BatteryBase battery = new EMS.BatteryBase(j + 1);
                    series.Batteries.Add(battery);
                }
                BatteryTotal.Series.Add(series);
            }
        }

        //public bool InBattery(ref string Ero, int ChannelIndex, BatteryBase Battery)
        //{
        //    if (ChannelIndex >= Batteries.Length)
        //    {
        //        Ero = "指定通道不存在";
        //        return false;
        //    }
        //    else
        //    {
        //        if (Batteries[ChannelIndex - 1] != null)
        //        {
        //            Ero = "指定通道已存在电池";
        //            return false;
        //        }
        //        else
        //        {
        //            Batteries[ChannelIndex - 1] = Battery;
        //            return true;
        //        }
        //    }
        //}

        public bool OutBattery(ref string Ero, int ChannelIndex)
        {
            if (ChannelIndex >= Batteries.Length)
            {
                Ero = "指定通道不存在";
                return false;
            }
            else
            {
                if (Batteries[ChannelIndex - 1] == null)
                {
                    Ero = "指定通道不存在电池";
                    return false;
                }
                else
                {
                    Batteries[ChannelIndex - 1] = null;
                    return true;
                }
            }
        }

        ModbusServer server;
        public override bool StartDev()
        {
            server = new ModbusServer();
            if (server.Connect(Modbus_tcp_server_RequestReceived, Modbus_tcp_server_WriteComplete))
            {
                InitDev();
                return true;
            }
            else
            {
                return false;
            }
        }

        // 创建一个监视器，监视寄存器的变化
        private void Modbus_tcp_server_WriteComplete(object sender, ModbusSlaveRequestEventArgs e)
        {
            Console.WriteLine("(WriteComplete)" + DateTime.Now.ToString("yyyy-MM-dd hh:ss:fff") + ":" + e.Message.ToString());

            // 监听系统控制寄存器
            //for (int i = 0; i < 4; i++)
            //{
            //    ushort value = server.GetValue(3, 8000 + i);
            //    switch (value)
            //    {
            //        case 0:
            //            break;
            //        case 1:
            //            RunChannel(i + 1);
            //            break;
            //        case 2:
            //            PauseChannel(i + 1);
            //            break;
            //        case 3:
            //            ContinueChannel(i + 1);
            //            break;
            //        case 4:
            //            StopChannel(i + 1);
            //            break;
            //        default:
            //            break;
            //    }
            //}
        }

        // 请求监听
        private void Modbus_tcp_server_RequestReceived(object sender, ModbusSlaveRequestEventArgs e)
        {
            Console.WriteLine("(RequestReceived)" + DateTime.Now.ToString("yyyy-MM-dd hh:ss:fff") + ":" + e.Message.ToString());
            byte[] data = e.Message.MessageFrame;
            byte[] byteStartAddress = new byte[] { data[3], data[2] };
            byte[] byteNum = new byte[] { data[5], data[4] };
            Int16 StartAddress = BitConverter.ToInt16(byteStartAddress, 0);
            Int16 NumOfPoint = BitConverter.ToInt16(byteNum, 0);
            if (StartAddress == 12002)
            {
                if (NumOfPoint == 0)
                {
                    Console.WriteLine("Test");
                }
            }
            //if (StartAddress >= 8000 && StartAddress < 8010)
            //{
            //    switch (NumOfPoint)
            //    {
            //        case 0:
            //            break;
            //        case 1:
            //            RunChannel(StartAddress - 8000 + 1);
            //            break;
            //        case 2:
            //            PauseChannel(StartAddress - 8000 + 1);
            //            break;
            //        case 3:
            //            ContinueChannel(StartAddress - 8000 + 1);
            //            break;
            //        case 4:
            //            StopChannel(StartAddress - 8000 + 1);
            //            break;
            //        default:
            //            break;
            //    }
            //}
        }

        private void InitDev()
        {
            // 10000 => ID
            server.SetValue(3, 10000 + 0, (ushort)BatteryTotal.TotalID);
            server.SetValue(3, 10000 + 1, (ushort)(BatteryTotal.Voltage * 1000));
            server.SetValue(3, 10000 + 2, (ushort)(BatteryTotal.Current * 1000));
            server.SetValue(3, 10100, (ushort)BatteryTotal.Series.Count);
            for (int j = 0; j < BatteryTotal.Series.Count; j++)
            {
                server.SetValue(3, 11000 + j * 10 + 0, (ushort)BatteryTotal.Series[j].SeriesID);
                server.SetValue(3, 11000 + j * 10 + 1, (ushort)BatteryTotal.Series[j].Voltage * 1000);
                server.SetValue(3, 11000 + j * 10 + 2, (ushort)BatteryTotal.Series[j].Current * 1000);
                server.SetValue(3, 10200 + j * 10, (ushort)BatteryTotal.Series[j].Batteries.Count);
                for (int l = 0; l < BatteryTotal.Series[j].Batteries.Count; l++)
                {
                    server.SetValue(3, 12000 + l * 10 + 0, (ushort)BatteryTotal.Series[j].Batteries[l].BatteryID);
                    server.SetValue(3, 12000 + l * 10 + 1, (ushort)BatteryTotal.Series[j].Batteries[l].Voltage * 1000);
                    server.SetValue(3, 12000 + l * 10 + 2, (ushort)BatteryTotal.Series[j].Batteries[l].Current * 1000);
                }
            }
            BatteryRun();
            // 同时创建线程，监听系统控制部分寄存器
            StartListener();
        }

        public override bool StopDev()
        {
            return server.Disconnect();
        }

        public void Run(int ChannelIndex)
        {
            // 根据通道号来解析第一条任务
            // ThreadPool.QueueUserWorkItem(new WaitCallback(RunChannel), ChannelIndex);
        }

        private void StopChannel(object index)
        {
            if ((int)index >= Batteries.Length)
            {
                Console.WriteLine("指定通道不存在");
                return;
            }
            else
            {
                if (Batteries[(int)index - 1] == null)
                {
                    Console.WriteLine("指定通道不存在电池");
                    return;
                }
                else
                {
                    tokens[(int)index - 1].Cancel();
                }
            }
        }

        private void PauseChannel(object index)
        {
            if ((int)index >= Batteries.Length)
            {
                Console.WriteLine("指定通道不存在");
                return;
            }
            else
            {
                if (Batteries[(int)index - 1] == null)
                {
                    Console.WriteLine("指定通道不存在电池");
                    return;
                }
                else
                {
                    tokens[(int)index - 1].Pause();
                }
            }
        }

        private void ContinueChannel(object index)
        {
            if ((int)index >= Batteries.Length)
            {
                Console.WriteLine("指定通道不存在");
                return;
            }
            else
            {
                if (Batteries[(int)index - 1] == null)
                {
                    Console.WriteLine("指定通道不存在电池");
                    return;
                }
                else
                {
                    tokens[(int)index - 1].Continue();
                }
            }
        }

        DateTime startTime;
        DateTime PhTime;
        MyToken[] tokens = new MyToken[4];
        private void RunChannel(object index)
        {
            if ((int)index >= Batteries.Length)
            {
                Console.WriteLine("指定通道不存在");
                return;
            }
            else
            {
                if (Batteries[(int)index - 1] == null)
                {
                    Console.WriteLine("指定通道不存在电池");
                    return;
                }
                else
                {
                    startTime = DateTime.Now;
                    tokens[(int)index - 1] = new MyToken();
                    ThreadPool.QueueUserWorkItem(t => RunStep((int)index, 1));

                    //RunStep((int)index, 1);
                }
            }
        }

        private void RunStep(int ChannelIndex, int StepIndex)
        {
            Step step = GetStep(ChannelIndex, StepIndex);
            if (step.Mode == WorkMode.Stop)
            {
                return;
            }
            else if (step.Mode == WorkMode.Goto)
            {
                RunStep(ChannelIndex, step.NextStep);
            }
            else
            {
                // 按照参数运行
                if (Batteries[ChannelIndex - 1] != null)
                {
                    // 测试电池的同时监控电池的参数
                    CancellationTokenSource cts = new CancellationTokenSource();
                    ThreadPool.QueueUserWorkItem(t => ListenBattery(cts.Token, ChannelIndex));
                    PhTime = DateTime.Now;
                    if (!TestStep(step, ChannelIndex))
                    {
                        cts.Cancel();
                        return;
                    }
                }
            }
        }

        private void BatteryRun()
        {
            BatteryTotal.Run();
            for (int i = 0; i < BatteryTotal.Series.Count; i++)
            {
                BatteryTotal.Series[i].Run();
                for(int j = 0; j < BatteryTotal.Series[i].Batteries.Count; j++)
                {
                    BatteryTotal.Series[i].Batteries[j].Run();
                }
            }
        }

        public void StartListener()
        {
            // 测试电池的同时监控电池的参数
            CancellationTokenSource cts = new CancellationTokenSource();
            ThreadPool.QueueUserWorkItem(t => ListenBattery(cts.Token));
        }

        private void ListenBattery(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }
                // 将电池的实时数据写入到指定寄存器位置
                server.SetValue(3, 10000 + 1, (ushort)(BatteryTotal.Voltage * 1000));
                server.SetValue(3, 10000 + 2, (ushort)(BatteryTotal.Current * 1000));
                for (int j = 0; j < BatteryTotal.Series.Count; j++)
                {
                    server.SetValue(3, 11000 + j * 10 + 1, (ushort)BatteryTotal.Series[j].Voltage * 1000);
                    server.SetValue(3, 11000 + j * 10 + 2, (ushort)BatteryTotal.Series[j].Current * 1000);
                    for (int l = 0; l < BatteryTotal.Series[j].Batteries.Count; l++)
                    {
                        server.SetValue(3, 12000 + l * 10 + 1, (ushort)BatteryTotal.Series[j].Batteries[l].Voltage * 1000);
                        server.SetValue(3, 12000 + l * 10 + 2, (ushort)BatteryTotal.Series[j].Batteries[l].Current * 1000);
                    }
                }
            }
        }

        private Step GetStep(int ChannelIndex, int StepIndex)
        {
            int start = 3000 + (ChannelIndex - 1) * 100 + (StepIndex - 1) * 10;
            var res = server.GetValue(3, start, 10);
            return new Step(StepIndex, res);
        }

        private bool TestStep(Step step, int ChannelIndex)
        {
            if (step.Mode == WorkMode.Stand)
            {
                Batteries[ChannelIndex - 1].Standing(tokens[ChannelIndex - 1], step.StopTime);
            }
            else if (step.Mode == WorkMode.Charge_CC)
            {
                Batteries[ChannelIndex - 1].Charge(tokens[ChannelIndex - 1], step.StopTime);
            }
            else if (step.Mode == WorkMode.Discharge_CD)
            {
                Batteries[ChannelIndex - 1].Discharge(tokens[ChannelIndex - 1], step.StopTime);
            }
            else
            {
                Console.WriteLine("nothing happened");
            }

            if (tokens[ChannelIndex - 1].IsCancel)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ListenBattery(CancellationToken token, object index)
        {
            int i = (int)index;
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    break;
                }
                // 将电池的实时数据写入到指定寄存器位置
                server.SetValue(3, 2000 + (i - 1) * 100, (int)(Batteries[i - 1].Vol * 1000));
                server.SetValue(3, 2002 + (i - 1) * 100, (int)(Batteries[i - 1].Elc * 1000));
                server.SetValue(3, 2004 + (i - 1) * 100, (UInt16)0);
                server.SetValue(3, 2005 + (i - 1) * 100, (UInt16)24);
                server.SetValue(3, 2006 + (i - 1) * 100, (DateTime.Now - startTime).Seconds);
                server.SetValue(3, 2008 + (i - 1) * 100, (UInt16)(DateTime.Now - PhTime).Seconds);
                server.SetValue(3, 2009 + (i - 1) * 100, (UInt16)(Batteries[i - 1].SOC * 10));
                server.SetValue(3, 2010 + (i - 1) * 100, (UInt16)(Batteries[i - 1].Vol * Batteries[i - 1].Elc));
                server.SetValue(3, 2011 + (i - 1) * 100, (UInt16)((DateTime.Now - PhTime).TotalHours * Batteries[i - 1].Vol * Batteries[i - 1].Elc));
                server.SetValue(3, 2012 + (i - 1) * 100, (UInt16)((DateTime.Now - PhTime).TotalHours * Batteries[i - 1].Elc));
                server.SetValue(3, 2013 + (i - 1) * 100, (UInt16)1);
                server.SetValue(3, 2014 + (i - 1) * 100, (UInt16)0);
            }
        }
    }
}
