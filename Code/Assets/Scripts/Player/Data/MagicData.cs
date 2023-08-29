using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Data
{

    public class MagicData
    {
        private const long MagicRate = 3;

        private const long MagicOff = 10211;

        private long data;

        private string text;

        public long Data
        {
            get
            {
                if (data <= 1)
                {
                    return data;
                }
                else
                {
                    return (data - MagicOff) / MagicRate;
                }
            }
            set
            {
                if (value <= 1)
                {
                    data = value;
                }
                else
                {
                    //if (!Check())
                    //{
                    //    return;
                    //}

                    data = value * MagicRate + MagicOff;

                    this.text = EncryptionHelper.Md5(data + "1o9&z");
                }
            }
        }



        public bool Check()
        {
            if (text == null || text == "")
            {
                return true;
            }

            string md5 = EncryptionHelper.Md5(data + "1o9&z");

            if (this.text != md5)
            {
                //数据校检失败
                GameProcessor.Inst.EventCenter.Raise(new CheckGameCheatEvent());
                return false;
            }

            return true;
        }
    }
}
