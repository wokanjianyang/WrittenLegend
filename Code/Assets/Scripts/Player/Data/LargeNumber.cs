using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class LargeNumber
    {
        public LargeNumber(double val)
        {
            double d = ExtractExponent(val, out int s);

            this.data = d;
            this.size = s;
        }

        public LargeNumber(double d, int s)
        {
            this.data = d;
            this.size = s;
            this.ReExponent();
        }

        public LargeNumber(LargeNumber lg)
        {
            this.data = lg.data;
            this.size = lg.size;
            this.ReExponent();
        }

        public LargeNumber Mul(double val)
        {
            double d = ExtractExponent(val, out int s);

            this.data *= d;
            this.size += s;

            return this.ReExponent();
        }

        public LargeNumber Mul(LargeNumber lg)
        {
            this.data *= lg.data;
            this.size += lg.size;

            return this.ReExponent();
        }

        public LargeNumber Div(double val)
        {
            double d = ExtractExponent(val, out int s);

            this.data = this.data / d;
            this.size = this.size - s;

            return this.ReExponent();
        }

        public LargeNumber Div(LargeNumber val)
        {
            this.data = this.data / val.data;
            this.size = this.size - val.size;

            return this.ReExponent();
        }

        public LargeNumber Add(double val)
        {
            double d = ExtractExponent(val, out int s);

            if (this.size > s)
            {
                int p = this.size - s;
                d = d / Math.Pow(10, p);
            }
            else
            {
                int p = s - this.size;
                this.data = this.data / Math.Pow(10, p);

                this.size = s;
            }

            this.data += d;

            return this.ReExponent();
        }

        public LargeNumber Add(LargeNumber val)
        {
            this.ReExponent();

            LargeNumber lg = new LargeNumber(val.data, val.size);

            int s = this.size - lg.size;

            if (s >= 4)
            {
                return this;
            }
            else if (s <= -4)
            {
                this.data = lg.data;
                this.size = lg.size;
            }
            else if (s < 4 && s >= 0)
            {
                //this 大
                lg.data = lg.data / Math.Pow(10, s);

                this.data = this.data + lg.data;
            }
            else if (s > -4 && s <= 0)
            {
                //lg大
                this.data = this.data / Math.Pow(10, -s);
                lg.data = lg.data + this.data;

                this.data = lg.data;
                this.size = lg.size;
            }

            return this.ReExponent();
        }

        public LargeNumber Sub(double val)
        {
            return this.Add(-val);
        }

        public LargeNumber Sub(LargeNumber lg)
        {
            return this.Add(new LargeNumber(-lg.data, lg.size));
        }

        public int Compare(LargeNumber b)
        {
            this.ReExponent();
            b.ReExponent();

            if (this.data >= 0 && b.data < 0)
            {
                return 1;
            }
            if (this.data < 0 && b.data >= 0)
            {
                return -1;
            }

            //判定正负
            int pre = this.data >= 0 ? 1 : -1;

            //再判定大小
            if (this.size > b.size)
            {
                return 1 * pre;
            }
            else if (this.size == b.size)
            {
                if (this.data > b.data)
                {
                    return 1 * pre;
                }
                else if (this.data == b.data)
                {
                    return 0;
                }
                else
                {
                    return -1 * pre;
                }
            }
            else
            {
                return -1 * pre;
            }
        }

        public int Compare(double b)
        {
            return this.Compare(new LargeNumber(b));
        }

        public LargeNumber SetZero()
        {
            this.data = 0;
            this.size = 0;

            return this;
        }

        public double ConvertToDouble()
        {
            if (this.data == 0)
            {
                return 0;
            }


            if (this.size >= 300)
            {
                return 1E300;
            }

            string text = this.data + "E" + this.size;

            try
            {
                return Convert.ToDouble(text);
            }
            catch (Exception ex)
            {
                Debug.Log("Error text:" + text);
            }

            return 1;
        }

        public double data = 0;

        public int size = 0;

        public double ExtractExponent(double val, out int s)
        {
            s = 0;

            string text = val.ToString("E");
            string[] parts = text.ToUpper().Split('E');

            if (parts.Length == 2)
            {
                s = Convert.ToInt32(parts[1]);
            }

            double exp = Math.Pow(10, s);
            double tmp = val / exp;

            return tmp;
        }

        public double GetMythScale()
        {
            double scale = Math.Log10(data) + size - 9;
            return scale;
        }

        public override string ToString()
        {
            return "data;" + data + " size:" + size;
        }

        private LargeNumber ReExponent()
        {
            if (this.data > 10 || (this.data < 1 && this.data > 0))
            {
                string text = this.data.ToString("E");
                string[] parts = text.ToUpper().Split('E');

                if (parts.Length == 2)
                {
                    int s = Convert.ToInt32(parts[1]);
                    double exp = Math.Pow(10, s);

                    this.data = this.data / exp;
                    this.size += s;
                }
            }

            return this;
        }

        public string FormatUnit()
        {
            this.ReExponent();

            if (this.size < -2)
            {
                return "0";
            }

            string[] UnitList = ConfigHelper.UnitList;

            string unit = "";

            int index = size / 4;
            double ld = (this.data * Math.Pow(10, size % 4));

            if (ld < 100)
            {
                ld = Math.Floor(ld * 100) / 100; //向下取整，并且保留2位小数
            }
            else
            {
                ld = Math.Floor(ld);
            }

            string text = ld >= 10 ? ld.ToString("0.#") : ld.ToString("0.##");

            while (index > 0)
            {
                int unitIndex = Math.Min(index, UnitList.Length);
                index -= unitIndex;
                unit = UnitList[unitIndex - 1] + unit;
            }

            return text + unit;
        }
    }
}
