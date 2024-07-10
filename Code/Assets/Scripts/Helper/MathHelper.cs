using System;
using UnityEngine;

namespace Game
{
    public static class MathHelper
    {
        public static float RadToDeg(float radians)
        {
            return (float)(radians * 180 / System.Math.PI);
        }

        public static float DegToRad(float degrees)
        {
            return (float)(degrees * System.Math.PI / 180);
        }

        public static long GetSequence1(long level)
        {
            return level * (level + 1) / 2;
        }

        public static double ConvertionDropRate(long rate)
        {
            double r = 0;

            for (int i = 1; i < 1000; i++)
            {
                int pr = i * 100;

                if (rate >= pr)
                {
                    r += 1;
                }
                else
                {
                    r += rate * 1.0 / pr;
                    break;
                }
                rate -= pr;
            }
            return r;
        }

        public static int CalOfflineDropCount(double killRecord, double killCount, double rate)
        {
            int oldCount = (int)(killRecord / rate);

            int newCount = (int)((killRecord + killCount) / rate);

            return newCount - oldCount;
        }

        public static int CalRefineStone(int equipLevel, int riseStone)
        {
            int count = (equipLevel * 3 / 20 + riseStone);
            //Debug.Log("RefineStone:" + count);
            return count;
        }

        public static int RandomBurstMul(double rs)
        {
            if (rs <= 0)
            {
                return 0;
            }

            if (rs >= 300)
            {
                return 3;
            }

            int count = (int)(rs / 100);
            int rate = (int)(rs - count * 100);

            if (RandomHelper.RandomRate(rate))
            {
                count++;
            }

            return count;
        }

        public static double CalRealResist(double val) {
            double r = 0;

            while (val > 0)
            {
                double temp = Math.Min(val, 70);
                val = val - temp;

                r += (100 - r) * temp / 100;
            }

            return r;
        }
    }
}