using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Artifact : MonoBehaviour
{
    public ScrollRect sr_Boss;
    private GameObject ItemPrefab;

    public Button btn_Close;

    List<Item_Artifact> items = new List<Item_Artifact>();
    public int Order => (int)ComponentOrder.Dialog;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("artifact start");

        this.btn_Close.onClick.AddListener(OnClick_Close);

        ItemPrefab = Resources.Load<GameObject>("Prefab/Window/Item/Item_Artifact");

        Init();
    }

    private void Init()
    {
        List<ArtifactConfig> configs = ArtifactConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        User user = GameProcessor.Inst.User;

        for (int i = 0; i < configs.Count; i++)
        {
            var item = GameObject.Instantiate(ItemPrefab);
            Item_Artifact com = item.GetComponentInChildren<Item_Artifact>();

            int key = configs[i].Id;

            com.SetContent(configs[i], 0);
            com.gameObject.SetActive(false);

            item.transform.SetParent(this.sr_Boss.content);
            item.transform.localScale = Vector3.one;

            items.Add(com);
        }

        this.Show();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);

        if (this.items.Count <= 0)
        {
            return;
        }

        List<ArtifactConfig> configs = ArtifactConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();

        User user = GameProcessor.Inst.User;

        for (int i = 0; i < configs.Count; i++)
        {
            Item_Artifact com = items.Where(m => m.Config.Id == configs[i].Id).FirstOrDefault();

            if (com != null)
            {
                int level = user.GetArtifactLevel(configs[i].Id);

                if (level > 0)
                {
                    com.gameObject.SetActive(true);
                    com.SetContent(configs[i], level);
                }
                else
                {
                    com.gameObject.SetActive(false);
                }
            }
        }
    }


    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }
}
