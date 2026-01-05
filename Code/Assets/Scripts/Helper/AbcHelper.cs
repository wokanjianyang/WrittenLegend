using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = System.Random;

namespace Game
{
    public class AbcHelper
    {

        private static int[] data = new int[] { 1800, 1200, 600 };



        public static long GetRecord(int x)
        {
            if (data.Length < x)
            {
                return 0;
            }

            return data[x - 1];
        }
    }

    public enum AbcType
    {
        Stone = 1,
        Relic = 2,
        Talent = 3,
    }

}