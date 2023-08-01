using System;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dialog_Power : MonoBehaviour, IBattleLife,IPointerDownHandler,IPointerMoveHandler,IPointerUpHandler,IPointerClickHandler
{
    public Transform Content;

    public Slider Slider;

    public Text Time;
    
    public Text Date;

    public Text Map;

    public Transform Button;

    private int minute;
    private int day;

    private DragEnum dragType;
    
    public enum DragEnum
    {
        None,
        Down,
        Drag,
        Up
    }
    
    // Start is called before the first frame update
    void Start()
    {
        this.Slider.onValueChanged.AddListener((progress) =>
        {
            if (progress >= 1f)
            {
                this.Content.gameObject.SetActive(false);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        var min = DateTime.Now.Minute;
        var d = DateTime.Now.Day;

        if (min != minute)
        {
            minute = DateTime.Now.Minute;
            
            Time.text = $"{DateTime.Now.Hour}:{DateTime.Now.Minute}";
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

    public void OnClick_Open()
    {
        this.Content.gameObject.SetActive(true);
        this.Slider.value = 0;
        
        User user = GameProcessor.Inst.User;
        if (user != null)
        {
            MapConfig config = MapConfigCategory.Instance.Get(user.MapId);
            Map.text = config.Name;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(this.Button.GetComponent<RectTransform>(), eventData.position))
        {
            this.dragType = DragEnum.Down;
        }
        else
        {
            this.dragType = DragEnum.None;
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (this.dragType == DragEnum.Down || this.dragType == DragEnum.Drag)
        {
            this.dragType = DragEnum.Drag;
            this.Button.position = eventData.position;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (this.dragType == DragEnum.Drag)
        {
            this.dragType = DragEnum.Up;
        }
        else
        {
            this.dragType = DragEnum.None;
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.dragType == DragEnum.None && RectTransformUtility.RectangleContainsScreenPoint(this.Button.GetComponent<RectTransform>(), eventData.position))
        {
            this.OnClick_Open();
        }
    }
}
