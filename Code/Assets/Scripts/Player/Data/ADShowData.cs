using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Data
{
    public enum ADTypeEnum
    {
        GoldCount = 0,
        ExpCount,
        CopyTicketCount,
        StoneCount,
        ExpAdd,
        ExpTime,
        GoldAdd,
        GoldTime,
        ErrorCount,
    }
    [Serializable]
    public class ADData
    {
        public int ADType;
        public int CurrentShowCount;
        public int MaxShowCount;
    }
    [Serializable]
    public class ADShowData
    {
        public int LastShowADMonth = 0;
        public int LastShowADDay = 0;
        public List<ADData> ADDatas;

        public ADShowData()
        {

        }

        public void Reset()
        {
            this.LastShowADMonth = DateTime.Now.Month;
            this.LastShowADDay = DateTime.Now.Day;

            ADDatas = new List<ADData>();
            ADDatas.Add(new ADData()
            {
                ADType = (int)ADTypeEnum.GoldCount,
                CurrentShowCount = 0,
                MaxShowCount = 6
            });
            ADDatas.Add(new ADData()
            {
                ADType = (int)ADTypeEnum.ExpCount,
                CurrentShowCount = 0,
                MaxShowCount = 6
            });
            ADDatas.Add(new ADData()
            {
                ADType = (int)ADTypeEnum.CopyTicketCount,
                CurrentShowCount = 0,
                MaxShowCount = 6
            });
            ADDatas.Add(new ADData()
            {
                ADType = (int)ADTypeEnum.StoneCount,
                CurrentShowCount = 0,
                MaxShowCount = 6
            });
            ADDatas.Add(new ADData()
            {
                ADType = (int)ADTypeEnum.ErrorCount,
                CurrentShowCount = 0,
                MaxShowCount = 6
            });
        }

        public bool CheckDate()
        {
            
            return this.LastShowADDay < DateTime.Now.Day || this.LastShowADMonth < DateTime.Now.Month;
        }

        public ADData GetADShowStatus(ADTypeEnum adType)
        {
            if (this.CheckDate())
            {
                this.Reset();
            }

            if (this.ADDatas == null)
            {
                this.Reset();
            }

            //ADData ret = null;
            var data = this.ADDatas.FirstOrDefault(d => d.ADType == (int)adType);
            return data;
        }
    }
}
