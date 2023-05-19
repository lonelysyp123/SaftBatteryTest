using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SaftBatteryTest.Model
{
    public class ModbusTcpClient
    {
        public string IP { get; set; }
        public int Port { get; set; }
        private static ModbusTcpClient instance;
        private static readonly object locker = new object();

        private ModbusTcpClient()
        {

        }

        public static ModbusTcpClient GetInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new ModbusTcpClient();
                    }
                }
            }
            return instance;
        }

        private TcpClient client;
        private ModbusIpMaster master;
        //public CancellationTokenSource tokenSource = new CancellationTokenSource();

        /// <summary>
        /// 连接
        /// </summary>
        /// <returns>是否连接成功</returns>
        public bool Connect()
        {
            try
            {
                client = new TcpClient();
                client.Connect(IPAddress.Parse(IP), Port);
                master = ModbusIpMaster.CreateIp(client);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <returns>是否断开连接成功</returns>
        public bool Disconnect()
        {
            try
            {
                master.Dispose();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// Modbus通用读取函数
        /// </summary>
        /// <param name="address">寄存器地址</param>
        /// <param name="num">数据位数</param>
        /// <returns></returns>
        public byte[] ReadFunc(ushort address, ushort num)
        {
            ushort[] holding_register = master.ReadHoldingRegisters(address, num);
            byte[] bytes = new byte[holding_register.Length * 2];
            for (int i = 0; i < holding_register.Length; i++)
            {
                var objs = BitConverter.GetBytes(holding_register[i]);
                bytes[i * 2] = objs[0];
                bytes[i * 2 + 1] = objs[1];
            }
            return bytes;
        }

        /// <summary>
        /// Modbus通用写入函数
        /// </summary>
        /// <param name="address"> 寄存器地址</param>
        /// <param name="value">写入内容</param>
        public void WriteFunc(ushort address, ushort value)
        {
            master.WriteSingleRegister(address, value);
        }

        /// <summary>
        /// 写入指定通道，指定工步的工作模式
        /// </summary>
        /// <param name="ChIndex">指定通道</param>
        /// <param name="ID">指定工步</param>
        /// <param name="Mode">工作模式</param>
        public void WriteWorkMode(int ChIndex, int ID, WorkMode Mode)
        {
            int address = 3000 + ChIndex * 100 + ID * 10 + 0;
            WriteFunc((ushort)address, (ushort)Mode);
        }

        /// <summary>
        /// 写入指定通道，指定工步的工作模式
        /// </summary>
        /// <param name="ChIndex">指定通道</param>
        /// <param name="ID">指定工步</param>
        /// <param name="Step">工步信息</param>
        public void WriteStep(int ChIndex, int ID, StepModel Step)
        {
            int address = 3000 + (ChIndex-1) * 100 + ID * 10 + 0;
            WriteFunc((ushort)address, (ushort)Step.Mode);
            address++;
            WriteFunc((ushort)address, ushort.Parse(Step.Param1));
            address++;
            WriteFunc((ushort)address, ushort.Parse(Step.Param2));
            address++;
            WriteFunc((ushort)address, (ushort)Step.StopTime);
            address++;
            WriteFunc((ushort)address, (ushort)Step.StopVoltage);
            address++;
            WriteFunc((ushort)address, (ushort)Step.StopElectric);
            address++;
            WriteFunc((ushort)address, (ushort)Step.StopT);
            address++;
            WriteFunc((ushort)address, (ushort)Step.StopCap);
            address++;
            WriteFunc((ushort)address, (ushort)Step.StopEnergy);
            address++;
            WriteFunc((ushort)address, (ushort)Step.NextStep);
        }

        /// <summary>
        /// 写入指定通道，指定工步的下一步号
        /// </summary>
        /// <param name="ChIndex">指定通道</param>
        /// <param name="ID">指定工步</param>
        /// <param name="NextS">下一步号</param>
        public void WriteNextStep(int ChIndex, int ID, int NextS)
        {
            int address = 3000 + ChIndex * 100 + ID * 10 + 9;
            WriteFunc((ushort)address, (ushort)NextS);
        }

        /// <summary>
        /// 写入设备控制码
        /// </summary>
        /// <param name="ChIndex">指定通道</param>
        /// <param name="CtrlCode">控制码</param>
        public void WriteCtrlDev(int ChIndex, int CtrlCode)
        {
            int address = 8000 + ChIndex - 1;
            WriteFunc((ushort)address, (ushort)CtrlCode);
        }
    }
}
