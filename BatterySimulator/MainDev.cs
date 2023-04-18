using BatterySimulator.Base;
using BatterySimulator.Interface;
using BatterySimulator.Model;
using BMS.Communication;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BatterySimulator
{
    public class MainDev : DevBase, IDev
    {
        public MainDev()
            :base()
        {

        }

        public bool InBattery(ref string Ero, int ChannelIndex, BatteryBase Battery)
        {
            if (ChannelIndex >= Batteries.Length)
            {
                Ero = "指定通道不存在";
                return false;
            }
            else
            {
                if (Batteries[ChannelIndex-1] != null)
                {
                    Ero = "指定通道已存在电池";
                    return false;
                }
                else
                {
                    Batteries[ChannelIndex-1] = Battery;
                    return true;
                }
            }
        }

        public bool OutBattery(ref string Ero, int ChannelIndex)
        {
            if (ChannelIndex >= Batteries.Length)
            {
                Ero = "指定通道不存在";
                return false;
            }
            else
            {
                if (Batteries[ChannelIndex-1] == null)
                {
                    Ero = "指定通道不存在电池";
                    return false;
                }
                else
                {
                    Batteries[ChannelIndex-1] = null;
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
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:ss:fff") + ":" + e.Message.ToString());

            // 监听系统控制寄存器
            for (int i = 0; i < 4; i++)
            {
                ushort value = server.GetValue(3, 8000 + i);
                switch (value)
                {
                    case 0:
                        break;
                    case 1:
                        RunChannel(i + 1);
                        break;
                    case 2:
                        PauseChannel(i + 1);
                        break;
                    case 3:
                        ContinueChannel(i + 1);
                        break;
                    case 4:
                        StopChannel(i + 1);
                        break;
                    default:
                        break;
                }
            }
        }

        // 请求监听
        private void Modbus_tcp_server_RequestReceived(object sender, ModbusSlaveRequestEventArgs e)
        {
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:ss:fff") + ":" + e.Message.ToString());

        }

        private void InitDev()
        {
            Console.WriteLine("基础信息维护");
            server.SetValue(3, 1000, IntSwVersion[0]);
            server.SetValue(3, 1001, IntSwVersion[1]);
            Console.WriteLine("内部版本号为1010");
            server.SetValue(3, 1002, ExtSwVersion);
            Console.WriteLine("外部版本号为1");
            for (int i = 0; i < 6; i++)
            {
                if (i < ProjectCode.Length)
                {
                    server.SetValue(3, 1003 + i, ProjectCode[i]);
                }
            }
            Console.WriteLine("项目编号为Test01");
            server.SetValue(3, 1009, Year);
            server.SetValue(3, 1010, Month * 100 + Day);
            Console.WriteLine("创建时间为年：" + Year + "月：" + Month + "日：" + Day);
            server.SetValue(3, 1011, Sort[0]);
            server.SetValue(3, 1012, Sort[1]);
            Console.WriteLine("序列号为01");
            server.SetValue(3, 1013, ChannelNums);
            Console.WriteLine("通带数量为"+ ChannelNums);
            for (int i = 0; i < 6; i++)
            {
                if (i < Model.Length)
                {
                    server.SetValue(3, 1014 + i, Model[i]);
                }
            }
            Console.WriteLine("型号为V01");
            for (int i = 0; i < 11; i++)
            {
                if (i < ProductBarCode.Length)
                {
                    server.SetValue(3, 1020 + i, ProductBarCode[i]);
                }
            }
            Console.WriteLine("二维码信息为zxcvbn");
            for (int i = 0; i < 5; i++)
            {
                if (i < ManufacturerName.Length)
                {
                    server.SetValue(3, 1040 + i, ManufacturerName[i]);
                }
            }
            Console.WriteLine("智造商为SYP");
            server.SetValue(3, 1045, HardWareVersion_Major * 100 + HardWareVersion_Minor);
            Console.WriteLine("硬件编号为1020");
            for (int i = 0; i < 15; i++)
            {
                if (i < PCBA_BarCode.Length)
                {
                    server.SetValue(3, 1046 + i, PCBA_BarCode[i]);
                }
            }
            Console.WriteLine("PCBA编号为MCU");

            // 系统配置
            server.SetValue(3, 7000, "127.0.0.1".ToArray());
            // 同时创建线程，监听系统控制部分寄存器

        }

        public override bool StopDev()
        {
            return server.Disconnect();
        }

        public void Run(int ChannelIndex)
        {
            // 根据通道号来解析第一条任务
            ThreadPool.QueueUserWorkItem(new WaitCallback(RunChannel), ChannelIndex);
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
                    RunStep((int)index, 1);
                    tokens[(int)index - 1] = new MyToken();
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
                if (Batteries[ChannelIndex-1] != null)
                {
                    // 测试电池的同时监控电池的参数
                    CancellationTokenSource cts = new CancellationTokenSource();
                    ThreadPool.QueueUserWorkItem(t=>ListenBattery(cts.Token, Batteries[ChannelIndex - 1]));
                    PhTime = DateTime.Now;
                    TestStep(step, ChannelIndex);
                    cts.Cancel();
                }

                RunStep(ChannelIndex, step.NextStep);
            }
        }

        private Step GetStep(int ChannelIndex, int StepIndex)
        {
            int start = 3000 + (ChannelIndex - 1) * 100 + (StepIndex - 1) * 10;
            var res = server.GetValue(3, start, 10);
            return new Step(StepIndex, res);
        }

        private void TestStep(Step step, int ChannelIndex)
        {
            if (step.Mode == WorkMode.Stand)
            {
                Batteries[ChannelIndex - 1].Standing(tokens[ChannelIndex-1], step.StopTime);
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
