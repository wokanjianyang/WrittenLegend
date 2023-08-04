using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Monster_Phantom : APlayer
{
    bool Real = true;

    PhantomConfig config;
    int Layer = 0;
    int Percent = 10;

    public Monster_Phantom(int id, int layer, bool real, int percent)
    {
        this.Init();

        config = PhantomConfigCategory.Instance.Get(id);
        Real = real;
        this.Layer = layer;
        this.Percent = percent;
    }

    private void Init()
    {

        this.Camp = PlayerType.Enemy;
        this.Name = config.Name;
        this.Level = Layer;

        this.SetAttr();  //设置属性值
        this.SetSkill(); //设置技能

        base.Load();
        this.Logic.SetData(null); //设置UI
    }

    private void SetSkill()
    {
        //加载技能
        List<SkillData> list = new List<SkillData>();
        //if (config.SkillIdList.Length > 0)
        //{
        //    int i = this.Index % config.SkillIdList.Length;
        //    SkillData skill = new SkillData(config.SkillIdList[i], 1);
        //    SkillData skill = new SkillData(10004, i); //测试技能代码
        //    skill.SkillConfig.CD = 2;
        //    skill.Status = SkillStatus.Equip;
        //    skill.Position = 1;
        //    skill.Level = 1;
        //    SkillList.Add(skill);
        //}
        list.Add(new SkillData(9001, (int)SkillPosition.Default)); //增加默认技能

        foreach (SkillData skillData in list)
        {
            List<SkillRune> runeList = new List<SkillRune>();
            List<SkillSuit> suitList = new List<SkillSuit>();

            SkillPanel skillPanel = new SkillPanel(skillData, runeList, suitList);

            SkillState skill = new SkillState(this, skillPanel, skillData.Position, 0);
            SelectSkillList.Add(skill);
        }
    }

    private void SetAttr()
    {

        long attr = config.Attr * (Level * config.Increa);
        long hp = config.Hp * (Level * config.Increa);
        long def = config.Def * (Level * config.Increa);

        AttributeBonus.SetAttr(AttributeEnum.HP, AttributeFrom.HeroBase, hp);
        AttributeBonus.SetAttr(AttributeEnum.PhyAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.MagicAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.SpiritAtt, AttributeFrom.HeroBase, attr);
        AttributeBonus.SetAttr(AttributeEnum.Def, AttributeFrom.HeroBase, def);

        long MaxHP = AttributeBonus.GetTotalAttr(AttributeEnum.HP);
        long CurrentHp = Percent * MaxHP / 10;

        SetHP(CurrentHp);
    }

    public override void DoEvent()
    {
        base.DoEvent();
    }

    public override void OnHit(int fromId, params long[] damages)
    {
        long maxHp = this.AttributeBonus.GetTotalAttr(AttributeEnum.HP);
        long pp = this.HP * 10 / maxHp;

        base.OnHit(fromId, damages);

        int nowPercent = (int)(this.HP * 10 / maxHp);

        if (HP > 0 && pp < 10 && pp > nowPercent && Real)  //只有本体，从90%开始,过了每10%的界限
        {
            //sepcial logic
            var enemy = new Monster_Phantom(config.Id, Layer, false, nowPercent);
            GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
        }
    }
}
