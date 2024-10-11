using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Game.Data;

public class BattleRule_Mine
{
    protected float currentRoundTime = 0f;

    public void OnUpdate()
    {
        this.currentRoundTime += Time.unscaledDeltaTime;

        if (this.currentRoundTime >= 1)
        {
            this.currentRoundTime = 0;

            this.BuildReward();
        }
    }

    public BattleRule_Mine()
    {

    }

    private void BuildReward()
    {
        //Debug.Log("Mine Build Reward ");

        User user = GameProcessor.Inst.User;

        long nt = TimeHelper.ClientNowSeconds();

        if (nt - user.MinerTime >= 3)
        {
            Debug.Log("Mine Build Reward");

            user.MinerTime = nt;
            Dictionary<int, int> metalList = MineConfigCategory.Instance.BuildMetal(true, 1);

            if (metalList.Count <= 0)
            {
                return;
            }

            metalList = metalList.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            Dictionary<int, MagicData> md = user.MetalData;

            string message = $"ÍÚµ½¿óÊ¯£º";

            foreach (var kv in metalList)
            {
                int key = kv.Key;
                if (!md.ContainsKey(key))
                {
                    md[key] = new MagicData();
                }

                md[key].Data += kv.Value;

                MetalConfig metalConfig = MetalConfigCategory.Instance.Get(key);
                message += $"<color=#{QualityConfigHelper.GetQualityColor(metalConfig.Quality)}>[{metalConfig.Name}]</color>" + kv.Value + "¸ö";
            }


            //Debug.Log(message);
            GameProcessor.Inst.EventCenter.Raise(new MineMsgEvent() { Message = message });
        }

        //foreach (var miner in user.MinerList)
        //{
        //    if (miner.BirthDay == 0)
        //    {
        //        miner.BirthDay = nt;
        //    }
        //    else if (nt - miner.BirthDay >= ConfigHelper.Mine_Time)
        //    {
        //        //MineConfig config = miner.InlineBuild(nt);

        //        if (config != null)
        //        {

        //        }
        //    }
        //}
    }
}
