using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public LargeNumber Mul(double val)
        {
            double d = ExtractExponent(val, out int s);

            this.data *= d;
            this.size += s;

            return this;
        }

        public LargeNumber div(double val)
        {
            double d = ExtractExponent(val, out int s);

            this.data = this.data / d;
            this.size = this.size - s;

            return this;
        }

        public LargeNumber add(double val)
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

            return this;
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

        private void ReExponent()
        {
            if (this.data > 10)
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
        }

        public string FormatUnit()
        {
            this.ReExponent();

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
