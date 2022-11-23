using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SaftBatteryTest.Model
{
    public class ModbusTcpClient : TcpClientBase
    {
        public ModbusTcpClient()
        {

        }

        /// <summary>
        /// 通用读取函数
        /// </summary>
        /// <param name="msg">读取字节</param>
        /// <param name="type">数据类型</param>
        /// <returns></returns>
        public object ReadFunc(byte[] msg, string type)
        {
            object obj = null;
            byte[] bytes = Request(msg);
            if (bytes != null && bytes.Length > 8)
            {
                int count = (short)bytes[8];
                byte[] result = new byte[count];
                if (Compare(bytes, msg, 8))
                {
                    Buffer.BlockCopy(bytes, 9, result, 0, count);
                    if ("UInt16" == type)
                    {
                        obj = ReadByteToInt16(result);
                    }
                    else if ("UInt32" == type)
                    {
                        obj = ReadByteToInt32(result);
                    }
                    else if ("String" == type)
                    {
                        obj = ReadByteToString(result);
                    }
                }
            }
            return obj;
        }

        /// <summary>
        /// 通用指定通道读取函数
        /// </summary>
        /// <param name="index">通道序号</param>
        /// <param name="msg">读取字节</param>
        /// <param name="type">数据类型</param>
        /// <returns></returns>
        public object ReadFunc(int index, byte[] msg, string type)
        {
            // TODO 根据这个index改变寄存器地址字节
            object obj = null;
            byte[] bytes = Request(msg);
            if (bytes != null && bytes.Length > 8)
            {
                int count = (short)bytes[8];
                byte[] result = new byte[count];
                if (Compare(bytes, msg, 8))
                {
                    Buffer.BlockCopy(bytes, 9, result, 0, count);
                    if ("UInt16" == type)
                    {
                        obj = ReadByteToInt16(result);
                    }
                    else if ("UInt32" == type)
                    {
                        obj = ReadByteToInt32(result);
                    }
                    else if ("String" == type)
                    {
                        obj = ReadByteToString(result);
                    }
                }
            }
            return obj;
        }

        /// <summary>
        /// 通用指定通道写入函数
        /// </summary>
        /// <param name="index">通道序号</param>
        /// <param name="msg">写入字节</param>
        /// <param name="value">写入内容</param>
        public void WriteFunc(int index, int stepNum, int value)
        {
            // TODO 根据index改变寄存器地址字节, 根据value改变数据内容
            byte[] msg = { };
            byte[] bytes = Request(msg);
            if (bytes != null && bytes.Length > 8)
            {
                // 验证响应是否正确
            }
        }

        /// <summary>
        /// 将读取到的Byte转成String
        /// </summary>
        /// <param name="bytes">字节流</param>
        /// <returns>字符串</returns>
        private string ReadByteToString(byte[] bytes)
        {
            byte[] lengh = { bytes[1], bytes[0] };
            int count = BitConverter.ToInt32(lengh, 0);
            if (count > 0)
            {
                return Encoding.UTF8.GetString(bytes, 2, count);
            }
            return "";
        }

        /// <summary>
        /// 将读取到的Byte转成ushort
        /// </summary>
        /// <param name="bytes">字节流</param>
        /// <returns>整数</returns>
        private int ReadByteToInt16(byte[] bytes)
        {
            bool bigEndian = true;
            short count = BitConverter.ToInt16(bytes, 0);
            if (bigEndian)
            {
                count = IPAddress.NetworkToHostOrder(count);
            }
            return count;
        }

        /// <summary>
        /// 将读取到的Byte转成uint
        /// </summary>
        /// <param name="bytes">字节流</param>
        /// <returns>整数</returns>
        private int ReadByteToInt32(byte[] bytes)
        {
            bool bigEndian = true;
            int count = BitConverter.ToInt32(bytes, 0);
            if (bigEndian)
            {
                count = IPAddress.NetworkToHostOrder(count);
            }
            return count;
        }

        /// <summary>
        /// 字节流对比
        /// </summary>
        /// <param name="newb">新的字节流</param>
        /// <param name="oldb">旧的字节流</param>
        /// <param name="index">对比序列</param>
        /// <returns>一样:true;不一样:false</returns>
        public bool Compare(byte[] newb, byte[] oldb, int index)
        {
            bool IsEqual = true;
            for (int i = 0; i < index; i++)
            {
                if (i != 4 && i != 5)
                {
                    if (newb[i] == oldb[i])
                    {
                        IsEqual &= true;
                    }
                    else
                    {
                        IsEqual &= false;
                    }
                }
            }
            return IsEqual;
        }
    }
}
