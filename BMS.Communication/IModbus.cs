using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Communication
{
    public interface IModbus
    {
        bool Connect();
        bool Disconnect();
        bool ReadRegister(ref byte[] ret, Address address, int offset, int length);
        bool WriteRegister(ref byte[] ret, Address address, int offset, byte[] value);
    }
}
