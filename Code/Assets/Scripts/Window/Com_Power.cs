using System;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Com_Power : MonoBehaviour, IBattleLife
{
    public Slider Slider;

    public Text Time;

    public Text Date;

    public Text Map;

    private int minute;
    private int day;

    // Start is called before the first frame update
    void Start()
    {
        this.Slider.onValueChanged.AddListener((progress) =>
        {
            if (progress >= 1f)
            {
                this.gameObject.SetActive(false);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.gameObject.activeInHierarchy)
        {
            return;
        }
        var min = DateTime.Now.Minute;
        var d = DateTime.Now.Day;

        if (min != minute)
        {
            minute = DateTime.Now.Minute;

            Time.text = $"{DateTime.Now.Hour.ToString().PadLeft(2, '0')}:{DateTime.Now.Minute.ToString().PadLeft(2, '0')}";
        }

        if (d != day)
        {
            day = DateTime.Now.Day;
            Date.text = $"{DateTime.Now.Month}月{DateTime.Now.Day}日 {GetWeekStr(DateTime.Now.DayOfWeek)}";
        }

    }

    private string GetWeekStr(DayOfWeek week)
    {
        string weekStr = "";
        switch (week)
        {
            case DayOfWeek.Monday:
                weekStr = "星期一";
                break;
            case DayOfWeek.Tuesday:
                weekStr = "星期二";
                break;
            case DayOfWeek.Wednesday:
                weekStr = "星期三";
                break;
            case DayOfWeek.Thursday:
                weekStr = "星期四";
                break;
            case DayOfWeek.Friday:
                weekStr = "星期五";
                break;
            case DayOfWeek.Saturday:
                weekStr = "星期六";
                break;
            case DayOfWeek.Sunday:
                weekStr = "星期日";
                break;
        }

        return weekStr;
    }

    public void OnBattleStart()
    {

        GameProcessor.Inst.EventCenter.AddListener<ChangeMapEvent>(this.OnChangeMapEvent);

    }

    public int Order => (int)ComponentOrder.Dialog;

    private void OnChangeMapEvent(ChangeMapEvent e)
    {
        MapConfig config = MapConfigCategory.Instance.Get(e.MapId);
        Map.text = config.Name;
    }

    public void Open()
    {
        this.gameObject.SetActive(true);
        this.Slider.value = 0;

        User user = GameProcessor.Inst.User;
        if (user != null)
        {
            int MapNo = Math.Max(user.MapId - 1, ConfigHelper.MapStartId);

            MapConfig config = MapConfigCategory.Instance.Get(MapNo);
            Map.text = config.Name;
        }
    }

}
