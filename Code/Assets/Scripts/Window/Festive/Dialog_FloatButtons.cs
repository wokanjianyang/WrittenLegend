using System;
using Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Dialog_FloatButtons : MonoBehaviour, IBattleLife, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler, IPointerClickHandler
{
    public Com_Power com_Power;

    public Com_AD com_AD;



    public Transform Menu;

    public Image btn_Power;

    public Image btn_Exit;

    public Button btn_AD;

    private DragEnum dragType;

    public Text Txt_Version;

    public Toggle Tg_Expend;

    public Button Btn_Festive;
    public Text Txt_Festive;
    public Dialog_Festive dialog_Festive;

    public Button Btn_Seven_Day;
    public Dialog_Seven_Day DialogSevenDay;

    public Button Btn_Festive_Week;
    public Dialog_Festive_Week DialogFestiveWeek;

    public enum DragEnum
    {
        None,
        Down,
        Drag,
        Up
    }

    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        this.Tg_Expend.onValueChanged.AddListener((isOn) =>
        {
            Expend(isOn);
        });

        this.Txt_Version.text = "V" + ConfigHelper.Version + "";

        DropLimitConfig dropLimit = DropLimitConfigCategory.Instance.Get(1);
        long nt = DateTime.Now.Ticks;

        this.btn_AD.onClick.AddListener(OnClick_AD);

        if (nt < DateTime.Parse(dropLimit.EndDate).AddDays(1).Ticks)
        {
            this.Txt_Festive.text = dropLimit.Name;
            this.Btn_Festive.onClick.AddListener(OnClick_Festive);
        }
        else
        {
            this.Btn_Festive.gameObject.SetActive(false);
        }

        User user = GameProcessor.Inst.User;
        long day = (TimeHelper.ClientNowSeconds() - user.First_Create_Time) / 86400 + 1;

        if (day > 30)
        {
            this.Btn_Seven_Day.gameObject.SetActive(false);
        }
        else
        {
            this.Btn_Seven_Day.gameObject.SetActive(true);
            this.Btn_Seven_Day.onClick.AddListener(OnClick_SevenDay);
        }

        if (DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
        {
            this.Btn_Festive_Week.gameObject.SetActive(false);
        }
        else
        {
            this.Btn_Festive_Week.gameObject.SetActive(true);
            this.Btn_Festive_Week.onClick.AddListener(OnClick_Week);
        }


    }

    public void OnBattleStart()
    {
        this.gameObject.SetActive(true);
    }

    private void Expend(bool isOn)
    {
        if (isOn)
        {
            Menu.gameObject.SetActive(true);
        }
        else
        {
            Menu.gameObject.SetActive(false);
        }
    }

    private void OnClick_Festive()
    {
        this.dialog_Festive.Open();
    }

    private void OnClick_SevenDay()
    {
        this.DialogSevenDay.Open();
    }

    private void OnClick_Power()
    {
        this.com_Power.Open();
    }

    private void OnClick_Week()
    {

        if (DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
        {
            this.Btn_Festive_Week.gameObject.SetActive(false);
        }
        else
        {
            this.DialogFestiveWeek.Open();
        }

    }


    private void OnClick_Exit()
    {
        GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("是否确认退出？", true, () =>
        {
            UserData.Save();
            Application.Quit();
        }, () =>
        {
            UserData.Save();
        });
    }

    private void OnClick_AD()
    {
        this.com_AD.Open();
    }
    private Vector2 dragStartPosition = Vector2.zero;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(this.Menu.GetComponent<RectTransform>(), eventData.position))
        {
            this.dragType = DragEnum.Down;
            this.dragStartPosition = eventData.position;
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
            var pos = this.Menu.position;
            var offset = eventData.position - this.dragStartPosition;
            this.dragStartPosition = eventData.position;
            this.Menu.position = pos + new Vector3(offset.x, offset.y);
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
        if (this.dragType == DragEnum.None)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(this.btn_Power.rectTransform, eventData.position))
            {
                this.OnClick_Power();
            }
            else if (RectTransformUtility.RectangleContainsScreenPoint(this.btn_Exit.rectTransform, eventData.position))
            {
                this.OnClick_Exit();
            }
        }
    }
}
