using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ViewBattleProcessor : AViewPage
    {
        [Title("掉落")]
        [LabelText("掉落信息")]
        public ScrollRect sr_BattleMsg;
        
        [Title("世界地图")]
        [LabelText("地图信息")]
        public ScrollRect sr_WorldMap;

        [LabelText("BOSS信息")]
        public Button btn_BossInfo;

        [LabelText("地图名称")]
        public Text txt_MapName;

        [LabelText("新手指引")]
        public TaskNav PlayerGuide;

        private bool isViewMapShowing = false;

        private GameObject msgPrefab;

        private Dictionary<int,Com_MapName> mapNameDict = new Dictionary<int,Com_MapName>();
        private Vector2 scrollHalfSize; //列表尺寸
        void Start()
        {
            this.btn_BossInfo.onClick.AddListener(this.OnClick_BossInfo);
            
            scrollHalfSize = sr_WorldMap.viewport.rect.size * 0.5f;
        }

        protected override bool CheckPageType(ViewPageType page)
        {
            var ret = page == ViewPageType.View_Battle;
            //if (ret)
            //{
            //    GameProcessor.Inst.Resume();
            //}
            //else if (this.isViewMapShowing)
            //{
            //    GameProcessor.Inst.Pause();
            //}
            //this.isViewMapShowing = ret;
            return true;
        }

        public override void OnBattleStart()
        {
            base.OnBattleStart();

            this.msgPrefab = Resources.Load<GameObject>("Prefab/Window/Item/Item_DropMsg");

            GameProcessor.Inst.EventCenter.AddListener<BattleMsgEvent>(this.OnBattleMsgEvent);
 
            GameProcessor.Inst.EventCenter.AddListener<ChangeFloorEvent>(this.OnChangeFloorEvent);

            GameProcessor.Inst.EventCenter.AddListener<TaskChangeEvent>(this.OnTaskChangeEvent);

            //加载世界地图
            //this.LoadWroldMap();

            this.PlayerGuide.Init();

            ShowName();
        }

        private List<Text> msgPool = new List<Text>();
        private int msgId = 0;
        private void OnBattleMsgEvent(BattleMsgEvent e)
        {
            if (e.Type != RuleType.Normal)
            {
                return;
            }

            msgId++;
            Text txt_msg = null;
            if (this.sr_BattleMsg.content.childCount > 50)
            {
                txt_msg = msgPool[0];
                msgPool.RemoveAt(0);
                txt_msg.transform.SetAsLastSibling();
            }
            else
            {
                var msg = GameObject.Instantiate(this.msgPrefab);
                msg.transform.SetParent(this.sr_BattleMsg.content);
                msg.transform.localScale = Vector3.one;

                var m = msg.GetComponent<Text>();
                

                txt_msg = m;
            }
            msgPool.Add(txt_msg);

            txt_msg.gameObject.name = $"msg_{msgId}";
            txt_msg.text = e.Message;
            this.sr_BattleMsg.normalizedPosition = new Vector2(0, 0);

            //if (e.MsgType == MsgType.SecondExp)
            //{
            //    msg.GetComponent<TextMeshProUGUI>().text = $"增加泡点经验{e.Exp}";
                
            //}
            //else if (e.BattleType == BattleType.Normal)
            //{
            //    MonsterBase config = MonsterBaseCategory.Instance.Get(e.MonsterId);
            //    string drops = "";
            //    if (e.Drops != null && e.Drops.Count > 0)
            //    {
            //        drops = ",����";
            //        foreach (var drop in e.Drops)
            //        {
            //            drops += $"<color=#{ItemHelper.GetColor(drop.GetQuality())}>[{drop.Name}]";
            //        }
            //    }
            //    msg.GetComponent<TextMeshProUGUI>().text = $"<color=#{ItemHelper.GetColor(4)}>[{config.Name}]<color=white>死亡,经验增加:{e.Exp},金币增加:{e.Gold}{drops}";
            //    msg.name = $"msg_{e.RoundNum}";

            //    this.sr_BattleMsg.normalizedPosition = new Vector2(0, 0);
            //}
        }

        private void OnChangeFloorEvent(ChangeFloorEvent e)
        {
            ShowName();
        }

        private void OnTaskChangeEvent(TaskChangeEvent e)
        {
            PlayerGuide.Init();
        }

        private void ShowName()
        {
            User user = GameProcessor.Inst.User;

            if (user != null)
            {
                txt_MapName.text = user.MagicTowerFloor.Data + "层";
            }

        }

        private void OnShowMapIcon (int mapId)
        {
            foreach (var kvp in mapNameDict)
            {
                kvp.Value.ShowIcon(mapId);
            }
            
            var pos = this.sr_WorldMap.content.anchoredPosition;
            pos.y -= scrollHalfSize.y + mapNameDict[mapId].transform.localPosition.y;
            this.sr_WorldMap.content.anchoredPosition = pos;
        }
        public class MapNameData
        {
            public int Id;
            public string Name;
            public Vector3 Position = new Vector3(-1, -1, 0);
            public bool HasRight;
            public bool HasDown;
        }

        private Dictionary<int,MapNameData> MapDatas = new Dictionary<int,MapNameData>();
        private Dictionary<int,MapConfig> MapConfigs = new Dictionary<int,MapConfig>();

        private readonly Vector3 NameSize = new Vector3(150, 44);
        private readonly Vector3 LeftArrowSize = new Vector3(40, 11);
        private readonly Vector3 UpArrowSize = new Vector3(12, 25);
        private readonly float ArrowOffset = 5f;

        private readonly float margin = 20f;
        //地图名 150x44
        //左右箭头 40x11
        //上下箭头 12x25
        //箭头间距 5
        /**
         *      1
         *   2  0  4
         *      3
         * 上左下右
         */
        private void LoadWroldMap()
        {
            var mapNamePrefab = this.sr_WorldMap.content.GetChild(0);
            mapNamePrefab.gameObject.SetActive(false);

            var leftArrow = this.sr_WorldMap.content.GetChild(1);
            leftArrow.gameObject.SetActive(false);

            var upArrow = this.sr_WorldMap.content.GetChild(2);
            upArrow.gameObject.SetActive(false);

            MapConfigs = MapConfigCategory.Instance.GetAll();
            foreach (var config in MapConfigs.Values)
            {
                MapDatas[config.Id] = new MapNameData()
                {
                    Id = config.Id,
                    Name = config.Name,
                };
            }
            var keys = MapConfigs.Keys.ToList();
            keys.Sort();
            MapDatas[keys[0]].Position = Vector3.zero;
            
            var minX = 0f;
            var maxX = 0f;
            var minY = 0f;
            var maxY = 0f;
            
            foreach (var key in keys)
            {
                var config = MapConfigs[key];
                var area = config.MapAfter;
                var data = MapDatas[config.Id];
                MapDatas.TryGetValue(area[0], out var up);
                MapDatas.TryGetValue(area[1], out var left);
                MapDatas.TryGetValue(area[2], out var down);
                MapDatas.TryGetValue(area[3], out var right);
                if (up != null)
                {
                    data.Position.y = up.Position.y - (NameSize.y + UpArrowSize.y + ArrowOffset * 2);
                    data.Position.x = up.Position.x;
                }
                
                if (left != null)
                {
                    if (left.Id < data.Id)
                    {
                        data.Position.x = left.Position.x + (NameSize.x + LeftArrowSize.x + ArrowOffset * 2);
                        
                        if (up == null)
                        {
                            data.Position.y = left.Position.y;
                        }
                    }
                    else
                    {
                        left.Position.x = data.Position.x - (NameSize.x + LeftArrowSize.x + ArrowOffset * 2);
                        left.Position.y = data.Position.y;
                    }

                }
                if (down != null)
                {
                    if (down.Id < data.Id)
                    {
                        data.Position.y = down.Position.y + (NameSize.y + UpArrowSize.y + ArrowOffset * 2);
                    }
                    else
                    {
                        down.Position.y = data.Position.y - (NameSize.y + UpArrowSize.y + ArrowOffset * 2);
                    }
                }
                
                if (right != null)
                {
                    if (right.Id < data.Id)
                    {
                        data.Position.x = right.Position.x - (NameSize.x + LeftArrowSize.x + ArrowOffset * 2);
                    }
                    else
                    {
                        right.Position.x = data.Position.x + (NameSize.x + LeftArrowSize.x + ArrowOffset * 2);
                    }
                }
                
                data.HasDown = down != null;
                data.HasRight = right != null;

                if (minX > data.Position.x)
                {
                    minX = data.Position.x;
                }
                if (maxX < data.Position.x)
                {
                    maxX = data.Position.x;
                }
                if (minY > data.Position.y)
                {
                    minY = data.Position.y;
                }
                if (maxY < data.Position.y)
                {
                    maxY = data.Position.y;
                }
            }

            var width = margin * 2 + Mathf.Max(Mathf.Abs(minX), Mathf.Abs(maxX)) * 2 + NameSize.x;
            width = Mathf.Max(960, width);
            var height = margin * 2 + Mathf.Max(Mathf.Abs(minY), Mathf.Abs(maxX)) + NameSize.y;

            var rect = this.sr_WorldMap.content;
            rect.sizeDelta = new Vector2(width, height);
            rect.localPosition = new Vector3((960-width)*0.5f,0);
            var startY = height * 0.5f - margin - NameSize.y * 0.5f;
            foreach (var data in MapDatas.Values)
            {
                var pos = data.Position;
                pos.y += startY;
                var nameGo = GameObject.Instantiate(mapNamePrefab);
                nameGo.SetParent(this.sr_WorldMap.content);
                nameGo.GetComponent<RectTransform>().anchoredPosition = pos;
                nameGo.GetComponentInChildren<Text>().text = data.Name;
                nameGo.name = data.Name;
                nameGo.gameObject.SetActive(true);
                nameGo.GetComponent<Com_MapName>().SetData(data);
                mapNameDict[data.Id] = nameGo.GetComponent<Com_MapName>();
                
                if (data.HasDown)
                {
                    var upGo = GameObject.Instantiate(upArrow);
                    upGo.SetParent(this.sr_WorldMap.content);
                    var p = pos;
                    p.y = pos.y - (NameSize.y*0.5f + UpArrowSize.y*0.5f + ArrowOffset);
                    upGo.GetComponent<RectTransform>().anchoredPosition = p;
                    upGo.gameObject.SetActive(true);

                }
                if (data.HasRight)
                {
                    var leftGo = GameObject.Instantiate(leftArrow);
                    leftGo.SetParent(this.sr_WorldMap.content);
                    var p = pos;
                    p.x = pos.x + NameSize.x*0.5f + LeftArrowSize.x*0.5f + ArrowOffset;
                    leftGo.GetComponent<RectTransform>().anchoredPosition = p;
                    leftGo.gameObject.SetActive(true);
                }
            }
        }
        
        private void OnClick_BossInfo()
        {
            GameProcessor.Inst.EventCenter.Raise(new BossInfoEvent());
        }
    }
}
