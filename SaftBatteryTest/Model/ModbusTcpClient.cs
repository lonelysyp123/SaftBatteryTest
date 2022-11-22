using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaftBatteryTest.Model
{
    public class ModbusTcpClient : TcpClientBase
    {
        // MBAP 头描述 {|传输标志(2byte)|协议标志(2byte)|长度(2byte)|单元号标志(1byte)|}
        byte[] IntSwVersion =   { 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x01, 0x03, 0xe8, 0x03, 0x02, 0x00 };
        byte[] ChNums =         { 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x01, 0x03, 0xf5, 0x03, 0x01, 0x00 };

        public ModbusTcpClient()
        {

        }

        /// <summary>
        /// 获取软件版本号
        /// </summary>
        /// <returns>版本号</returns>
        public string ReadIntSwVersion()
        {
            string Version = "";
            byte[] bytes = Request(IntSwVersion);
            byte[] result = new byte[bytes.Length-8];
            if (bytes.Length > 10)
            {
                if (Compare(bytes, IntSwVersion, 8))
                {
                    bytes.CopyTo(result, 8);
                    Version = ReadByteToString(result);
                }
            }
            return Version;
        }

        /// <summary>
        /// 获取通道数量
        /// </summary>
        /// <returns>通道数量</returns>
        public int ReadChNums()
        {
            int Nums = 0;
            byte[] bytes = Request(ChNums);
            byte[] result = new byte[bytes.Length - 8];
            if (bytes.Length > 10)
            {
                if (Compare(bytes, IntSwVersion, 8))
                {
                    bytes.CopyTo(result, 8);
                    Nums = ReadByteToInt(result);
                }
            }
            return Nums;
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
        /// 将读取到的Byte转成String
        /// </summary>
        /// <param name="bytes">字节流</param>
        /// <returns>整数</returns>
        private int ReadByteToInt(byte[] bytes)
        {
            byte[] lengh = { bytes[1], bytes[0] };
            int count = BitConverter.ToInt32(lengh, 0);
            if (count > 0)
            {
                return (int)bytes[0];
            }
            return 0;
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
                        IsEqual &= IsEqual;
                    }
                }
            }
            return IsEqual;
        }
    }
}
