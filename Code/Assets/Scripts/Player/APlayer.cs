using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using TMPro;

public enum PlayerType
{
    Hero = 0,
    Enemy
}

abstract public class APlayer
{
    public int ID { get; set; }

    public string Name { get; set; }
    
    public PlayerType Camp { get; set; }
    
    public Vector3Int Cell { get; set; }
    
    public Transform Transform { get; set; }

    public bool IsSurvice { get; set; } = true;
    
    public APlayer()
    {
        this.Load();
    }

    abstract public void Load();

    public void DoEvent()
    {
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
            PlayerManager.Inst.StartCoroutine(IE_Attack(nearestEnemy));
        }
        else
        {
            fourSide.Sort((a, b) =>
            {
                var l0 = Vector3.Distance(a, nearestEnemy.Cell);

                var l1 = Vector3.Distance(b, nearestEnemy.Cell);

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

            foreach (var side in fourSide)
            {
                if (PlayerManager.Inst.IsCellCanMove(side))
                {
                    this.Move(side);
                    break;
                }
            }
        }
    }

    public void Move(Vector3Int cell)
    {
        this.Cell = cell;
        this.Transform.DOMove(cell, 1f);
    }

    IEnumerator IE_Attack(APlayer enemy)
    {
        var distance = enemy.Cell - this.Cell;
        Vector3 offset = new Vector3(distance.x * 0.5f, distance.y * 0.5f);
        this.Transform.DOMove(this.Cell+offset, 0.5f);
        yield return new WaitForSeconds(0.5f);
        this.Transform.DOMove(this.Cell, 0.5f);
        enemy.OnHit(new List<float>(){10f});
    }

    public void SetPosition(Vector3 pos)
    {
        this.Cell = new Vector3Int((int)pos.x, (int)pos.y);
        this.Transform.position = Cell;
    }

    public APlayer FindNearestEnemy()
    {
        var enemys = PlayerManager.Inst.GetPlayersByCamp(this.Camp == PlayerType.Hero ? PlayerType.Enemy : PlayerType.Hero);
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
            var text = damage.GetComponent<TextMeshPro>();
            text.text = d.ToString();
            damage.transform.position = this.Transform.position;
            damage.transform.DOMoveY(this.Transform.position.y + 1f,0.5f).OnComplete(() =>
            {
                GameObject.Destroy(damage);
            });
        }
    }
}
