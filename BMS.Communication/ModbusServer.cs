using Modbus.Data;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BMS.Communication
{
    public class ModbusServer
    {
        private DataStore data;
        private ModbusSlave modbus_tcp_server;
        private ModbusBase modbus;
        public ModbusServer() 
        {
        }

        public bool Connect(EventHandler<ModbusSlaveRequestEventArgs> receiveEvent, EventHandler<ModbusSlaveRequestEventArgs> writeEvent)
        {
            try
            {
                data = DataStoreFactory.CreateDefaultDataStore();
                modbus_tcp_server = ModbusTcpSlave.CreateTcp(1, new TcpListener(IPAddress.Parse("127.0.0.1"), 502));
                modbus_tcp_server.DataStore = data;
                modbus_tcp_server.ModbusSlaveRequestReceived += receiveEvent;
                modbus_tcp_server.WriteComplete += writeEvent;
                modbus_tcp_server.Listen();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Disconnect()
        {
            try
            {
                modbus_tcp_server.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        /// <summary>
        /// 向指定寄存器写入一个int值
        /// 因为int值占4个字节，而寄存器中一个地址只能存储2个字节，所以需要分解int值
        /// </summary>
        /// <param name="groupindex">寄存器类型</param>
        /// <param name="offset">寄存器位置</param>
        /// <param name="value">int值</param>
        public void SetValue(int groupindex, int offset, int value)
        {
            byte[] valueBuf = BitConverter.GetBytes(value);
            ushort lowOrderValue = BitConverter.ToUInt16(valueBuf, 0);
            ushort highOrderValue = BitConverter.ToUInt16(valueBuf, 2);

            ModbusDataCollection<ushort> objs = getRegisterGroup(groupindex);
            objs[offset + 1] = lowOrderValue;
            objs[offset + 2] = highOrderValue;
        }

        /// <summary>
        /// 批量向指定寄存器写入int值
        /// </summary>
        /// <param name="groupindex">寄存器类型</param>
        /// <param name="offset">寄存器位置</param>
        /// <param name="value">int值</param>
        public void SetValue(int groupindex, int offset, int[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                byte[] valueBuf = BitConverter.GetBytes(values[i]);
                ushort lowOrderValue = BitConverter.ToUInt16(valueBuf, 0);
                ushort highOrderValue = BitConverter.ToUInt16(valueBuf, 2);

                ModbusDataCollection<ushort> objs = getRegisterGroup(groupindex);
                objs[offset + 1 + i * 2] = lowOrderValue;
                objs[offset + 2 + i * 2] = highOrderValue;
            }
        }

        /// <summary>
        /// 向指定寄存器写入一个ushort值
        /// </summary>
        /// <param name="groupindex">寄存器类型</param>
        /// <param name="offset">寄存器位置</param>
        /// <param name="value">ushort值</param>
        public void SetValue(int groupindex, int offset, UInt16 value)
        {
            ModbusDataCollection<ushort> objs = getRegisterGroup(groupindex);
            objs[offset + 1] = value;
        }

        /// <summary>
        /// 批量向指定寄存器写入ushort值
        /// </summary>
        /// <param name="groupindex">寄存器类型</param>
        /// <param name="offset">寄存器位置</param>
        /// <param name="values">ushort数组</param>
        public void SetValue(int groupindex, int offset, UInt16[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                ModbusDataCollection<ushort> objs = getRegisterGroup(groupindex);
                objs[offset + 1 + i] = values[i];
            }
        }

        /// <summary>
        /// 向指定寄存器写入一个string值
        /// </summary>
        /// <param name="groupindex">寄存器类型</param>
        /// <param name="offset">寄存器位置</param>
        /// <param name="values">string值</param>
        public void SetValue(int groupindex, int offset, char[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                byte[] valueBuf = BitConverter.GetBytes(values[i]);
                ushort lowOrderValue = BitConverter.ToUInt16(valueBuf, 0);
                ushort highOrderValue = BitConverter.ToUInt16(valueBuf, 2);

                ModbusDataCollection<ushort> objs = getRegisterGroup(groupindex);
                objs[offset + 1] = lowOrderValue;
                objs[offset + 2] = highOrderValue;
            }
        }

        public UInt16 GetValue(int groupindex, int offset)
        {
            ModbusDataCollection<ushort> objs = getRegisterGroup(groupindex);
            return objs[offset + 1];
        }

        public UInt16[] GetValue(int groupindex, int offset, int num)
        {
            UInt16[] values = new UInt16[num];
            ModbusDataCollection<ushort> objs = getRegisterGroup(groupindex);
            for (int i = 0; i < num; i++)
            {
                values[i] = objs[offset + 1 + i];
            }
            return values;
        }

        private ModbusDataCollection<ushort> getRegisterGroup(int groupindex)//根据3或4返回适合的寄存器
        {
            switch (groupindex)
            {
                case 3: return data.HoldingRegisters; //可修改
                case 4: return data.InputRegisters;   //不可通过modbus修改
                default: return data.InputRegisters;
            }
        }
    }
}
