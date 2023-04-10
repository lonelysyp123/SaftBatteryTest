using BatterySimulator.Base;
using BMS.Communication;
using Modbus.Data;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BatterySimulator
{
    internal class Program
    {
        private static List<Battery> batteries = new List<Battery>();

        static void Main(string[] args)
        {
            TestBattery battery = new TestBattery(12, 3, 3);
            MainDev mainDev = new MainDev();
            mainDev.StartDev();
            string Er = "";
            if (mainDev.InBattery(ref Er, 1, battery))
            {
                Console.WriteLine("1通道内放入电池：" + battery.ToString());
            }
            else
            {
                Console.WriteLine(Er);
            }
            Console.ReadKey();
        }

        private static void ReadBatteryNums()
        {
            string obj = Console.ReadLine();

            if (obj == "")
            {
                Console.WriteLine("Please select the number of batteries you want to generate!");
                ReadBatteryNums();
            }
            else
            {
                Regex reg = new Regex(@"^[0-9]*$");
                if (!reg.IsMatch(obj))
                {
                    Console.WriteLine("Please select the number of batteries you want to generate!");
                    ReadBatteryNums();
                }
                else
                {
                    Console.WriteLine(obj + " battery will be generated");
                    // 创建对应数量的模拟电池
                    batteries.Clear();
                    for (int i = 0; i < int.Parse(obj); i++)
                    {
                        batteries.Add(new Battery(i));
                    }
                }
            }
        }

        //private static void ReadCommunicationMode()
        //{
        //    string obj = Console.ReadLine();

        //    if (obj == "")
        //    {
        //        Console.WriteLine("Please select a communication method!");
        //        ReadCommunicationMode();
        //    }
        //    else
        //    {
        //        if (obj == "1")
        //        {
        //            Console.WriteLine("battery will communication by Modbus");
        //            StartModbusServer();
        //        }
        //        else if (obj == "2")
        //        {
        //            Console.WriteLine("battery will communication by BT3562");
        //        }
        //        else
        //        {
        //            Console.WriteLine("Please select a communication method!");
        //            ReadCommunicationMode();
        //        }
        //    }
        //}

        //private static void StartModbusServer()
        //{
        //    ModbusServer server = new ModbusServer();
        //    server.Connect();
        //}

        private static bool IsOnline = false;
        //private static void BatteryParamSync()
        //{
            //while (IsOnline)
            //{
            //    for (int i = 0; i < batteries.Count; i++)
            //    {
            //        setValue32(3, 2000, (int)(batteries[i].Vol * 1000));
            //        setValue32(3, 2002, (int)(batteries[i].Elc * 1000));
            //        Thread.Sleep(1000);
            //    }
            //}
        //}
    }
}
