using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using SDD.Events;
using TMPro;

namespace Game
{
    abstract public class APlayer
    {
        public int ID { get; set; }

        public string Name { get; set; }
        
        public PlayerType Camp { get; set; }
        
        public Vector3Int Cell { get; set; }
        
        public Transform Transform { get; private set; }
        
        public Logic Logic { get; private set; }
        
        public int RoundCounter { get; set; }

        public EventManager EventCenter { get; private set; }
        
        public bool IsSurvice
        {
            get
            {
                return this.Logic.IsSurvice;
            }
        }
        
        public APlayer()
        {
            this.EventCenter = new EventManager();
            this.Load();
        }

        virtual public void Load()
        {
            var prefab = Resources.Load<GameObject>("Prefab/Char/Model");
            this.Transform = GameObject.Instantiate(prefab).transform;
            this.Transform.SetParent(GameProcessor.Inst.MapRoot);
            this.Logic = this.Transform.GetComponent<Logic>();
            var coms = this.Transform.GetComponents<MonoBehaviour>();
            foreach (var com in coms)
            {
                if (com is IPlayer _com)
                {
                    _com.SetParent(this);
                }
            }
        }

        public void DoEvent()
        {
            this.RoundCounter++;
            var up = this.Cell + Vector3Int.up;
            var down = this.Cell + Vector3Int.down;
            var right = this.Cell + Vector3Int.right;
            var left = this.Cell + Vector3Int.left;

            var fourSide = new List<Vector3Int>()
            {
                up, down, right, left
            };
            var nearestEnemy = this.FindNearestEnemy();
            if (fourSide.Contains(nearestEnemy.Cell))
            {
                GameProcessor.Inst.PlayerManager.StartCoroutine(IE_Attack(nearestEnemy));
            }
            else
            {
                var endPos = GameProcessor.Inst.MapProcessor.GetPath(this.Cell, nearestEnemy.Cell);
                if (GameProcessor.Inst.PlayerManager.IsCellCanMove(endPos))
                {
                    this.Move(endPos);
                }
            }
        }

        public void Move(Vector3Int cell)
        {
            this.SetPosition(cell);
            var targetPos = GameProcessor.Inst.MapProcessor.GetWorldPosition(cell);
            this.Transform.DOLocalMove(targetPos, 1f);
        }

        IEnumerator IE_Attack(APlayer enemy)
        {
            this.GetComponent<SkillProcessor>().UseSkill();

            var distance = enemy.Cell - this.Cell;
            Vector3 offset = new Vector3(distance.x * 0.5f, distance.y * 0.5f)*120;
            var targetPos = GameProcessor.Inst.MapProcessor.GetWorldPosition(this.Cell);
            this.Transform.DOLocalMove(targetPos+offset, 0.5f);
            yield return new WaitForSeconds(0.5f);
            this.Transform.DOLocalMove(targetPos, 0.5f);
            enemy.OnHit(new List<float>(){this.Logic.GetAttributeFloat(AttributeEnum.Atk)});
        }

        public void SetPosition(Vector3 pos,bool isGraphic = false)
        {
            this.Cell = new Vector3Int((int)pos.x, (int)pos.y,0);
            if (isGraphic)
            {
                this.Transform.localPosition = GameProcessor.Inst.MapProcessor.GetWorldPosition(this.Cell);
            }
        }
        
        public APlayer FindNearestEnemy()
        {
            var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(this.Camp == PlayerType.Hero ? PlayerType.Enemy : PlayerType.Hero);
            var distLen = 0;
            enemys.Sort((a, b) =>
            {
                var distance = a.Cell - this.Cell;
                var l0 = Math.Abs(distance.x) + Math.Abs(distance.y);

                distance = b.Cell - this.Cell;
                var l1 = Math.Abs(distance.x) + Math.Abs(distance.y);

                if (l0 < l1)
                {
                    return -1;
                }
                else if (l0 > l1)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            });

            return enemys[0];
        }

        public void OnHit(List<float> damages)
        {
            foreach (var d in damages)
            {
                var prefab = Resources.Load<GameObject>("Prefab/Effect/Damage");
                var damage = GameObject.Instantiate(prefab);
                damage.transform.SetParent(GameObject.Find("Canvas").transform,false);
                var text = damage.GetComponent<TextMeshProUGUI>();
                text.text = d.ToString();
                damage.transform.position = this.Transform.position;
                damage.transform.DOMoveY(this.Transform.position.y + 80f,1f).OnComplete(() =>
                {
                    GameObject.Destroy(damage);
                });
                this.Logic.OnDamage(d);
            }

            GameProcessor.Inst.StartCoroutine(CheckPlayerDead());
        }

        private IEnumerator CheckPlayerDead()
        {
            yield return new WaitForSeconds(1f);
            if (!this.IsSurvice)
            {
                this.Transform.gameObject.SetActive(false);
            }
        }
        
        public T GetComponent<T>()
        {
            return this.Transform.GetComponent<T>();
        }
    }
}
