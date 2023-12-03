using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;

namespace Game
{

    public class MapData : MonoBehaviour
    {
        [LabelText("列数")]
        public int ColCount = 1;

        [LabelText("行数")]
        public int RowCount = 1;

        private const int minX = 0;
        private const int minY = 0;
        private const int maxX = 6;
        private const int maxY = 7;

        public Vector3 MapStartPos { get; private set; } = Vector3.zero;

        public Vector3 CellSize = Vector3.zero;

        public List<Vector3Int> AllCells { get; private set; }

        public List<MapCell> MapCells { get; private set; }

        /// <summary>
        /// 寻路网格
        /// </summary>
        private Game.Grid AStarBattleGrid { get; set; }

        void Awake()
        {
            //创建AStar
            this.CreateAStar();
        }

        void Start()
        {
            //取整数
            var rectTransform = this.transform as RectTransform;
            float mapWidth = rectTransform.rect.width;
            float mapHeight = rectTransform.rect.height;
            var gridWidth = mapWidth / ColCount;
            var gridHeight = mapHeight / RowCount;
            var startX = mapWidth * -0.5f;
            var startY = mapHeight * -0.5f;
            this.MapStartPos = new Vector3(startX, startY);
            //this.CellSize = new Vector3(gridWidth, gridHeight);
        }


        #region pos <--> cell

        public Vector3 GetWorldPosition(Vector3Int cell)
        {
            return new Vector3(cell.x * this.CellSize.x, cell.y * this.CellSize.y) + MapStartPos + this.CellSize * 0.5f;
        }

        public Vector3Int GetLocalCell(Vector3 pos)
        {
            var cell = (pos - this.CellSize * 0.5f);
            cell = new Vector3(cell.x / this.CellSize.x, cell.y / this.CellSize.y);
            Vector3Int local = new Vector3Int(Mathf.RoundToInt(cell.x), Mathf.RoundToInt(cell.y));
            return local;
        }

        public Vector3 GetCenterPosition(List<Vector3Int> list)
        {
            float x = list.Select(m => m.x).Sum() * 1.0f / list.Count;
            float y = list.Select(m => m.y).Sum() * 1.0f / list.Count;
            float z = list.Select(m => m.z).Sum() * 1.0f / list.Count;
            float offset = list.Count % 2 == 0 ? 0.5f : 0;

            return new Vector3(x * this.CellSize.x, y * this.CellSize.y, z) + MapStartPos + this.CellSize * offset;
        }

        #endregion

        #region AStar

        private void CreateAStar()
        {
            AStarBattleGrid = new Grid(this.ColCount, this.RowCount, 1f);
            AllCells = new List<Vector3Int>();
            MapCells = new List<MapCell>();
            for (var i = 0; i < this.ColCount; i++)
            {
                for (var j = 0; j < this.RowCount; j++)
                {
                    Vector3Int cell = new Vector3Int(i, j);
                    AllCells.Add(cell);
                    MapCells.Add(new MapCell(cell));
                }
            }
        }

        /// <summary>
        /// 获取路径
        /// </summary>
        /// <param name="curPos"></param>
        /// <param name="endPos"></param>
        /// <returns></returns>
        private List<Vector3Int> GetPathWithAStar(Vector3Int curPos, Vector3Int endPos)
        {
            var start = new Position(curPos.x, curPos.y);
            var end = new Position(endPos.x, endPos.y);
            //A*
            var paths = AStarBattleGrid.GetPath(start, end);
            var list = new List<Vector3Int>();
            //排除出发点
            if (paths.Length > 0)
            {
                for (int i = 1; i < paths.Length; i++)
                {
                    var p = paths[i];
                    list.Add(new Vector3Int(p.X, p.Y, 0));
                }
            }
            else
            {
                Debug.LogError("计算出错!");
            }
#if UNITY_EDITOR

            // DrawLine(list);
#endif
            return list;
        }

        public Vector3Int GetPath(Vector3Int startPos, Vector3Int endPos)
        {
            AStarBattleGrid.ClearCellCost();

            var allPlayersCells = GameProcessor.Inst.PlayerManager.GetAllPlayers(true).Select(p => p.Cell).ToList();

            float highCost = 9999;
            foreach (var cell in allPlayersCells)
            {
                var pos = new Position(cell.x, cell.y);
                if (cell != startPos && cell != endPos)
                {
                    AStarBattleGrid.SetCellCost(pos, highCost);
                }
            }

            var path = this.GetPathWithAStar(startPos, endPos);

            if (path.Count > 0)
            {
                return path[0];
            }

            return startPos;
        }

        public Vector3Int GetPath1(Vector3Int startPos, Vector3Int endPos)
        {
            var allPlayersCells = GameProcessor.Inst.PlayerManager.GetAllPlayers(true).Select(p => p.Cell).ToList();
            var costDict = new Dictionary<Position, float>();

            float highCost = 9999999;
            //float lowCost = 1;

            //临时将敌人的cell的cost设置为可通过但是代价高
            float tempCost = highCost;
            foreach (var cell in allPlayersCells)
            {
                var pos = new Position(cell.x, cell.y);
                costDict[pos] = AStarBattleGrid.GetCellCost(pos);
                if (cell != startPos && cell != endPos)
                {
                    AStarBattleGrid.SetCellCost(pos, tempCost);
                    //AStarBattleGrid.BlockCell(pos);
                }
            }

            //寻路
            Action getPath = null;
            //解析路径，判断最后几格是否能行走
            Action<List<Vector3Int>> isLastPosHasSelfCampPlayer = null;

            getPath = () =>
            {
                var pathList = new List<Vector3Int>();

                var path = this.GetPathWithAStar(startPos, endPos);
                //排除出发点
                if (path.Count > 0)
                {
                    foreach (var p in path)
                    {
                        pathList.Add(p);
                    }

                    if (pathList.Count > 0)
                    {
                        isLastPosHasSelfCampPlayer.Invoke(pathList);
                    }
                    else
                    {
                        Debug.LogError("寻路失败：计算出错!");
                    }
                }
                else
                {
                    Debug.LogError("寻路失败：计算出错!");
                }
            };


            //由于无视障碍下，所有的格子都可以行走，第一条路可能不是最好的路，先尝试多次寻路
            var otherList = new List<List<Vector3Int>>();

            isLastPosHasSelfCampPlayer = (path) =>
            {
                var minCount = Mathf.Min(1, path.Count);
                path = path.GetRange(0, minCount);

                //找不到路，不需要再尝试
                if (path.Count == 0) return;


                //在无视阻挡的情况下，最后一个格子可能是队友、敌人和建筑，但因为位置不能重叠，所以最后一个格子也不能走
                var last = path.Last();
                if (allPlayersCells.Contains(last))
                {
                    path.RemoveAt(path.Count - 1);

                    if (path.Count > 0)
                    {
                        //如果最后一个不能站立，最后一格周围4格格子可以站立，则路径没问题，否则要继续移除
                        last = path.Last();
                        if (allPlayersCells.Contains(last))
                        {
                            var pos = new Position(last.x, last.y);
                            costDict[pos] = AStarBattleGrid.GetCellCost(pos);
                            AStarBattleGrid.SetCellCost(pos, float.PositiveInfinity);
                            path.RemoveAt(path.Count - 1);

                            for (var i = path.Count - 1; i >= 0; i--)
                            {
                                if (allPlayersCells.Contains(last))
                                {
                                    path.RemoveAt(path.Count - 1);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            if (path.Count > 0)
                            {
                                //缓存找到的路径
                                otherList.Add(path.ToArray().ToList());
                            }

                            //因为自己和周围4格都不能行走，可能不是最佳路径，将倒数第二格设为不可行走，尝试重新寻路
                            //A-2-5-8-B 由于258不能站立，导致A实际上不能行走
                            //A-2-3-[6]-B 由于6可以站立，在无视阻挡的情况下，可以穿过23或25来到6
                            //****
                            // A23
                            //  5 
                            //  8B
                            //****
                            getPath?.Invoke();
                        }
                        else
                        {
                            //缓存找到的路径
                            otherList.Add(path.ToArray().ToList());
                        }
                    }
                    else
                    {
                        //只有一格且不能站立说明就在目标四周，已是最近距离，不需要再寻路
                        //这种情况理论上不会出现，如果是UI操作，目标点无法点击，不存在寻路，如果是AI，目标点不会是技能格
                        //Debug.Log("不用寻路，当前点在目标点四周，已是最近位置！");
                    }
                }
                else
                {
                    //缓存找到的路径
                    otherList.Add(path.ToArray().ToList());
                }
            };

            getPath.Invoke();

            // 恢复cell的cost
            foreach (var cell in allPlayersCells)
            {
                var pos = new Position(cell.x, cell.y);
                if (costDict.ContainsKey(pos))
                {
                    if (cell != startPos && cell != endPos)
                    {
                        AStarBattleGrid.SetCellCost(pos, costDict[pos]);
                        //AStarBattleGrid.UnblockCell(pos);
                    }
                }
            }
#if UNITY_EDITOR

            // DrawLine(list);
#endif
            //优先走拐点少的路
            if (otherList != null && otherList.Count > 0)
            {
                var list = otherList[0];
                foreach (var other in otherList)
                {
                    if (other.Count > list.Count)
                    {
                        list = other;
                    }
                }

                return list[0];
            }

            return startPos;
        }

        public void Clear()
        {
            foreach (var mc in MapCells)
            {
                mc.Clear();
            }
        }

        #endregion

        #region AOE Range

        public Vector3Int GetDir(Vector3Int selfCell, Vector3Int enemyCell)
        {
            var dir = enemyCell - selfCell;
            int x = dir.x == 0 ? 0 : dir.x / Math.Abs(dir.x);
            int y = dir.y == 0 ? 0 : dir.y / Math.Abs(dir.y);
            dir = new Vector3Int(x, y, dir.z);
            return dir;
        }

        public List<Vector3Int> GetAttackRangeCell(Vector3Int selfCell, Vector3Int enemyCell, SkillPanel skill)
        {
            List<Vector3Int> rangeCells = new List<Vector3Int>();
            Vector3Int targetCell = Vector3Int.zero;
            int distance = skill.Dis;

            switch (skill.Area)
            {
                case AttackGeometryType.FrontRow: //直线
                    {
                        var dir = GetDir(selfCell, enemyCell);
                        for (var i = 1; i <= distance; i++)
                        {
                            targetCell = selfCell + dir * i;
                            rangeCells.Add(targetCell);
                        }
                    }
                    break;
                case AttackGeometryType.Cross: //十字
                    {
                        for (var i = 1; i <= distance; i++)
                        {
                            targetCell = selfCell + Vector3Int.up * i;
                            rangeCells.Add(targetCell);
                            targetCell = selfCell + Vector3Int.down * i;
                            rangeCells.Add(targetCell);
                            targetCell = selfCell + Vector3Int.left * i;
                            rangeCells.Add(targetCell);
                            targetCell = selfCell + Vector3Int.right * i;
                            rangeCells.Add(targetCell);
                        }
                    }
                    break;
                case AttackGeometryType.Chase://追击
                    {
                        targetCell = enemyCell + Vector3Int.up;
                        rangeCells.Add(targetCell);
                        targetCell = enemyCell + Vector3Int.down;
                        rangeCells.Add(targetCell);
                        targetCell = enemyCell + Vector3Int.left;
                        rangeCells.Add(targetCell);
                        targetCell = enemyCell + Vector3Int.right;
                        rangeCells.Add(targetCell);
                    }
                    break;
                case AttackGeometryType.Circle: //圆
                    {
                        for (var i = distance * -1; i <= distance; i++)
                        {
                            for (var j = distance * -1; j <= distance; j++)
                            {
                                targetCell = selfCell + new Vector3Int(j, i);
                                rangeCells.Add(targetCell);
                            }
                        }
                    }
                    break;
                case AttackGeometryType.Square: //矩形
                    {
                        var dir = enemyCell - selfCell;

                        int xdir = dir.x > 0 ? -1 : 1;
                        int ydir = dir.y > 0 ? -1 : 1;

                        for (var x = 0; x < skill.Column; x++)
                        {
                            for (var y = 0; y < skill.Row; y++)
                            {
                                int px = enemyCell.x + x * xdir;
                                int py = enemyCell.y + y * ydir;

                                if (px > maxX)
                                {
                                    px = enemyCell.x - (px - maxX);
                                }
                                if (px < minX)
                                {
                                    px = enemyCell.x + Math.Abs(px);
                                }

                                if (py > maxY)
                                {
                                    py = enemyCell.y - (py - maxY);
                                }
                                if (py < minY)
                                {
                                    py = enemyCell.y + Math.Abs(py);

                                }

                                rangeCells.Add(new Vector3Int(px, py));
                            }
                        }
                    }
                    break;
                case AttackGeometryType.Arc: //逆时针弧线
                    {
                        Vector3Int[] clockwise = new Vector3Int[] {
                            new Vector3Int(0,1,0), //北
                            new Vector3Int(1,1,0), //东北
                            new Vector3Int(1,0,0), //东
                            new Vector3Int(1,-1,0), //东南
                            new Vector3Int(0,-1,0), //南
                            new Vector3Int(-1,-1,0), //西南
                            new Vector3Int(-1,0,0), //西
                            new Vector3Int(-1,1,0), //西北
                        };
                        var dir = enemyCell - selfCell;

                        //得到弧线起点
                        int startPos = 0;
                        for (startPos = 0; startPos < clockwise.Length; startPos++)
                        {
                            if (dir == clockwise[startPos])
                            {
                                break;
                            }
                        }

                        startPos += skill.EnemyMax / 2;
                        for (var i = 0; i < skill.EnemyMax; i++)
                        {
                            int p = (startPos - i + 8) % 8;
                            targetCell = selfCell + clockwise[p];
                            rangeCells.Add(targetCell);
                        }
                    }
                    break;
                case AttackGeometryType.Diamond: //菱形
                    for (var i = distance * -1; i <= distance; i++)
                    {
                        var rowCount = distance - Mathf.Abs(i);
                        for (var j = rowCount * -1; j <= rowCount; j++)
                        {
                            targetCell = enemyCell + new Vector3Int(j, i);
                            rangeCells.Add(targetCell);
                        }
                    }
                    break;
                case AttackGeometryType.FullBox: //全图
                    rangeCells.AddRange(this.AllCells);
                    break;
            }

            //移除范围内的
            rangeCells.RemoveAll(m => m.x > maxX || m.x < minX || m.y > maxY || m.y < minY);
            rangeCells = rangeCells.Distinct().ToList();

            return rangeCells;
        }

        public List<Vector3Int> GetAttackRangeCell(Vector3Int selfCell, int attackRange, AttackGeometryType geometryType)
        {
            List<Vector3Int> rangeCells = new List<Vector3Int>();
            Vector3Int targetCell = Vector3Int.zero;
            switch (geometryType)
            {
                //case AttackGeometryType.FrontRow:
                //{
                //    for (var i = 1; i <= attackRange; i++)
                //    {
                //        targetCell = selfCell + dir * i;

                //        rangeCells.Add(targetCell);
                //    }
                //}
                //    break;
                case AttackGeometryType.FrontRow:
                case AttackGeometryType.Cross:
                    {
                        for (var i = 1; i <= attackRange; i++)
                        {
                            targetCell = selfCell + Vector3Int.up * i;
                            rangeCells.Add(targetCell);
                            targetCell = selfCell + Vector3Int.down * i;
                            rangeCells.Add(targetCell);
                            targetCell = selfCell + Vector3Int.left * i;
                            rangeCells.Add(targetCell);
                            targetCell = selfCell + Vector3Int.right * i;
                            rangeCells.Add(targetCell);
                        }
                    }
                    break;
                case AttackGeometryType.Square:
                    {
                        for (var i = attackRange * -1; i <= attackRange; i++)
                        {
                            for (var j = attackRange * -1; j <= attackRange; j++)
                            {
                                targetCell = selfCell + new Vector3Int(j, i);
                                rangeCells.Add(targetCell);
                            }
                        }
                    }
                    break;
                case AttackGeometryType.Diamond:
                    for (var i = attackRange * -1; i <= attackRange; i++)
                    {
                        var rowCount = attackRange - Mathf.Abs(i);
                        for (var j = rowCount * -1; j <= rowCount; j++)
                        {
                            targetCell = selfCell + new Vector3Int(j, i);
                            rangeCells.Add(targetCell);
                        }
                    }
                    break;
                case AttackGeometryType.FullBox:
                    rangeCells.AddRange(this.AllCells);
                    break;
            }

            rangeCells = rangeCells.Distinct().ToList();
            return rangeCells;
        }

        public MapCell GetMapCell(Vector3Int cell)
        {
            return MapCells.Find(m => m.cell == cell);
        }

        #endregion
    }
}

