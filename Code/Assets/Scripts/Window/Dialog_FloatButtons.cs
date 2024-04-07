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

    public Dialog_Festive dialog_Festive;

    public Transform Menu;

    public Image btn_Power;

    public Image btn_Exit;

    public Image btn_AD;

    private DragEnum dragType;

    public Text Txt_Version;

    public Toggle Tg_Expend;

    public Button Btn_Festive;

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

        if (!ConfigHelper.ShowFestive)
        {
            this.Btn_Festive.gameObject.SetActive(false);
        }
        else
        {
            this.Btn_Festive.onClick.AddListener(OnClick_Festive);
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

    private void OnClick_Power()
    {
        this.com_Power.Open();
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
            else if (RectTransformUtility.RectangleContainsScreenPoint(this.btn_AD.rectTransform, eventData.position))
            {
                this.OnClick_AD();
            }
        }
    }
}
