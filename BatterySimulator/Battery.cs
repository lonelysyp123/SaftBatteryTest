using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BatterySimulator
{
    public class Battery
    {
        public int ChannelNum { get; set; }
        public double Vol { get; set; }// V
        public double Elc { get; set; }// mA

        public Battery(int i)
        {
            InitBattery(i);
        }

        private void InitBattery(int i)
        {
            ChannelNum = i;
            Vol = 12.0;
            Elc = 100;

            IsRun = true;
            Nothing();
        }

        bool IsRun = false;
        Random random = new Random();
        public void Charge()
        {
            Thread th = new Thread(ChargeTh);
            th.IsBackground = true;
            th.Start();
        }

        private void ChargeTh()
        {
            for (int i = 0; i < 100; i++)
            {
                if (IsRun)
                {
                    if (Vol < 12)
                    {
                        Thread.Sleep(1000);
                        Vol += 0.1;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            NothingTh();
        }

        public void Discharge()
        {
            Thread th = new Thread(DischargeTh);
            th.IsBackground = true;
            th.Start();
        }

        private void DischargeTh()
        {
            for (int i = 0; i < 100; i++)
            {
                if (IsRun)
                {
                    if (Vol > 2)
                    {
                        Thread.Sleep(1000);
                        Vol -= 0.1;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            NothingTh();
        }

        public void Nothing()
        {
            Thread th = new Thread(NothingTh);
            th.IsBackground = true;
            th.Start();
        }

        private void NothingTh()
        {
            double oldVol = Vol;
            double oldElc = Elc;

            while (IsRun)
            {
                Thread.Sleep(1000);
                if (Vol >= oldVol)
                {
                    Vol -= random.NextDouble();
                }
                else
                {
                    Vol += random.NextDouble();
                }

                if (Elc >= oldElc)
                {
                    Elc -= random.NextDouble();
                }
                else
                {
                    Elc += random.NextDouble();
                }
            }
        }
    }
}
