using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public class WorldData
    {

        public Dictionary<int, int> Record { get; set; } = new Dictionary<int, int>();

        public long Ticket { get; set; }

        public Dictionary<int, List<int>> DictItemList = new Dictionary<int, List<int>>();


        public Dictionary<int, List<int>> DictItemListNew = new Dictionary<int, List<int>>();

        public bool Check()
        {
            long nt = TimeHelper.ClientNowSeconds();

            List<WorldConfig> worlds = WorldConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

            int seed = AppHelper.GetDeviceIdentifier().GetHashCode();
            User user = GameProcessor.Inst.User;
            if (user != null && user.Account != null)
            {
                seed = user.Account.GetHashCode();
            }
            seed += TimeHelper.WeekSeed();

            //如果新增了仙兽，配置掉落
            if (worlds.Count > DictItemListNew.Count)
            {
                for (int i = 0; i < worlds.Count; i++)
                {
                    int mapId = worlds[i].Id;
                    if (!this.DictItemList.ContainsKey(mapId))
                    {
                        List<int> list = WorldDropConfigCategory.Instance.GetAllDropIdList(mapId, seed);

                        DictItemListNew.Add(mapId, list);
                    }
                }
            }

            if (Ticket == 0 || nt - Ticket >= 86400 * 10)
            {
                if (Ticket == 0)
                {
                    Ticket = nt;
                }
                else
                {
                    Ticket += 86400 * 10;
                }

                Record.Clear();
                DictItemList = DictItemListNew;

                DictItemListNew.Clear();

                for (int mapId = 1; mapId <= 4; mapId++)
                {
                    List<int> list = WorldDropConfigCategory.Instance.GetAllDropIdList(mapId, 0);

                    DictItemListNew.Add(mapId, list);
                }

                return true;
            }

            //如果新增了仙兽，配置掉落
            if (worlds.Count > DictItemList.Count)
            {
                for (int i = 0; i < worlds.Count; i++)
                {
                    int mapId = worlds[i].Id;
                    if (!this.DictItemList.ContainsKey(mapId))
                    {
                        List<int> list = WorldDropConfigCategory.Instance.GetAllDropIdList(mapId, seed);

                        DictItemList.Add(mapId, list);
                    }
                }

                return true;
            }

            return false;
        }

        public int GetLayer(int id)
        {
            if (!Record.ContainsKey(id))
            {
                Record[id] = 0;
            }

            return Record[id] + 1;
        }

        public void SetOver(int id, int ap)
        {
            this.Record[id] += ap;
        }

        public int GetDropId(int mapId, int level)
        {
            List<int> dropList = DictItemList[mapId];

            if (level > dropList.Count)
            {
                return 0;
            }
            else
            {
                return dropList[level - 1];
            }
        }
    }
}
