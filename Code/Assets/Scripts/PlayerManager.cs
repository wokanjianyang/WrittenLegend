
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Inst { get; private set; } = null;

    private List<APlayer> allPlayers = null;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        allPlayers = new List<APlayer>();
        Inst = this;
    }


    public void AddPlayer(APlayer player)
    {
        player.ID = this.allPlayers.Count;
        this.allPlayers.Add(player);
    }

    public List<APlayer> GetPlayersByCamp(PlayerType camp)
    {
        return this.allPlayers.FindAll(p => p.Camp == camp && p.IsSurvice);
    }

    public bool IsCellCanMove(Vector3Int cell)
    {
        var allCells = this.allPlayers.Where(p => p.IsSurvice).Select(p => p.Cell).ToList();
        return !allCells.Contains(cell);
    }
}