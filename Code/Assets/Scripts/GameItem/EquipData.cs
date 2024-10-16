﻿using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public class EquipData
    {
        public List<List<KeyValuePair<int, long>>> AttrList = new List<List<KeyValuePair<int, long>>>();
        public List<int> RuneIdList = new List<int>();
        public List<int> SuitIdList = new List<int>();

        public List<KeyValuePair<int, long>> GetAttrList()
        {
            return AttrList[0];
        }

        public int GetRuneId()
        {
            return RuneIdList[0];
        }

        public int GetSuitId()
        {
            return SuitIdList[0];
        }

        public void Refresh(int part, int level, int quality, int role)
        {
            if (RuneIdList.Count > 0)
            {
                RuneIdList.RemoveAt(0);
                SuitIdList.RemoveAt(0);
                AttrList.RemoveAt(0);
            }

            for (int i = 0; i < 20 - RuneIdList.Count; i++)
            {
                if (GameProcessor.Inst.Net)
                {
                    AttrList.Add(AttrEntryConfigCategory.Instance.Build(part, level, quality, role));
                }
                else
                {
                    AttrList.Add(AttrEntryConfigCategory.Instance.BuildNew(part, level, quality, role, GameProcessor.Inst.User.RandomRecord));
                }

                SkillRuneConfig config = SkillRuneConfigCategory.Instance.RandomRune(-1, -1, role, 1, quality, level);
                RuneIdList.Add(config.Id);
                SuitIdList.Add(SkillSuitHelper.RandomSuit(-1, config.SkillId, config.Type).Id);
            }
        }
    }
}
