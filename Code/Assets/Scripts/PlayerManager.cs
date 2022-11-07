
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class PlayerManager : MonoBehaviour
    {

        private List<APlayer> AllPlayers =  new List<APlayer>();

        public void AddPlayer(APlayer player)
        {
            player.ID = this.AllPlayers.Count;
            this.AllPlayers.Add(player);
        }

        public List<APlayer> GetAllPlayers()
        {
            return this.AllPlayers.FindAll(p => p.IsSurvice);
        }

        public List<APlayer> GetPlayersByCamp(PlayerType camp)
        {
            return this.AllPlayers.FindAll(p => p.Camp == camp && p.IsSurvice);
        }

        public bool IsCellCanMove(Vector3Int cell)
        {
            var allCells = this.AllPlayers.Where(p => p.IsSurvice).Select(p => p.Cell).ToList();
            return !allCells.Contains(cell);
        }
    }
}
