using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Data
{

    public class RecordData
    {
        public Dictionary<int, long> RecordList { get; } = new Dictionary<int, long>();

        public string Text { get; set; }

        public void AddRecord(RecordType type, long val)
        {
            int key = (int)type;

            if (!RecordList.ContainsKey(key))
            {
                RecordList[key] = 0;
            }

            RecordList[key] += val;
            Encryption();
        }

        public bool Check()
        {
            if (RecordList.Count <= 0)
            {
                return true;
            }

            //序列化
            string str_json = JsonConvert.SerializeObject(RecordList, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            string md5 = EncryptionHelper.Md5(str_json);

            if (Text != md5)
            {
                //数据校检失败
                //GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "您已经修改了存档", ToastType = ToastTypeEnum.Failure });
                //GameProcessor.Inst.isCheckError = true;
                return false;
            }

            return true;
        }

        private void Encryption()
        {
            string str_json = JsonConvert.SerializeObject(RecordList, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            string md5 = EncryptionHelper.Md5(str_json);

            this.Text = md5;
        }

        public long GetRecord(int type)
        {
            if (RecordList.ContainsKey(type))
            {
                return RecordList[type];
            }
            return 0;
        }
    }

    public enum RecordType
    {
        AdReal = 0,
        AdVirtual = 1,
    }
}
