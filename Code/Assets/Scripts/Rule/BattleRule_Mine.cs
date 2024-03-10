using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleRule_Mine
{
    protected float currentRoundTime = 0f;

    public void OnUpdate()
    {
        this.currentRoundTime += Time.unscaledDeltaTime;

        if (this.currentRoundTime >= 5)
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
        Debug.Log("Mine Build Reward ");

        User user = GameProcessor.Inst.User;

        long nt = TimeHelper.ClientNowSeconds();

        foreach (var miner in user.MinerList)
        {
            if (miner.BirthDay == 0)
            {
                miner.BirthDay = nt;
            }
            else if (nt - miner.BirthDay >= 60)
            {
                miner.BirthDay += RandomHelper.RandomNumber(50, 70);

                MineConfig config = miner.InlineBuild();

                string message = DateTime.Now.ToString("yyyy��MM��dd�� HH:mm:ss") + " ��" + miner.Name + " �ڵ���" + config.Name;

                var md = user.MetalData;
                int key = config.Id;
                if (!md.ContainsKey(key))
                {
                    md[key] = new Game.Data.MagicData();
                }

                md[key].Data += 1;

                message += ",����ȼ�Ϊ:" + md[key].Data;

                //Debug.Log(message);

                GameProcessor.Inst.EventCenter.Raise(new MineMsgEvent() { Message = message });
            }
        }
    }
}
