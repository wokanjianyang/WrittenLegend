using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    
//    public class MapDrawHelper2 : MaskableGraphic
//    {
//        [LabelText("列数")]
//        public int ColCount = 1;

//        [LabelText("行数")]
//        public int RowCount = 1;

//        [LabelText("线宽")]
//        public float LineWidth = 1f;

//        public Vector3 MapStartPos { get; private set; } = Vector3.zero;
        
//        public Vector3 CellSize { get; private set; } = Vector3.zero;

//        public List<Vector3Int> AllCells { get; private set; }

//        /// <summary>
//        /// 寻路网格
//        /// </summary>
//        private Game.Grid AStarBattleGrid { get; set; }

//        protected override void Awake()
//        {
//            base.Awake();

//            //创建AStar
//            this.CreateAStar();
//        }

//        #region Draw Map

//        //可以自定义网格线颜色、如渐变色等，这里我是直接使用基类的颜色
//        protected override void OnPopulateMesh(VertexHelper vh)
//        {
//            if (ColCount * RowCount * rectTransform.rect.width * rectTransform.rect.height == 0) return;
//            vh.Clear();

//            //取整数
//            float mapWidth = rectTransform.rect.width;
//            float mapHeight = rectTransform.rect.height;
//            var gridWidth = mapWidth / ColCount;
//            var gridHeight = mapHeight / RowCount;
//            var startX = mapWidth * -0.5f;
//            var startY = mapHeight * -0.5f;
//            MapStartPos = new Vector3(startX, startY);
//            this.CellSize = new Vector3(gridWidth, gridHeight);

//            //先画水平方向上的线，从左到右绘制垂直线段
//            for (float i = 0; i <= mapWidth; i += gridWidth)
//            {
//                //四个点可以绘制一个矩形面片
//                var horizontal_A = new Vector2(i + startX - LineWidth * 0.5f, startY - LineWidth * 0.5f);
//                var horizontal_B = new Vector2(i + startX - LineWidth * 0.5f, (startY - LineWidth * 0.5f) * -1);
//                var horizontal_C = new Vector2(i + startX + LineWidth * 0.5f, (startY - LineWidth * 0.5f) * -1);
//                var horizontal_D = new Vector2(i + startX + LineWidth * 0.5f, startY - LineWidth * 0.5f);
//                vh.AddUIVertexQuad(GetRectangleQuad(color, horizontal_A, horizontal_B, horizontal_C, horizontal_D));
//            }

//            //最后画垂直方向上的线，从下到上绘制水平线段
//            for (float i = 0; i <= mapHeight; i += gridHeight)
//            {
//                var vertical_A = new Vector2(startX - LineWidth * 0.5f, i + startY - LineWidth * 0.5f);
//                var vertical_B = new Vector2(startX - LineWidth * 0.5f, i + startY + LineWidth * 0.5f);
//                var vertical_C = new Vector2((startX - LineWidth * 0.5f) * -1, i + startY + LineWidth * 0.5f);
//                var vertical_D = new Vector2((startX - LineWidth * 0.5f) * -1, i + startY - LineWidth * 0.5f);
//                vh.AddUIVertexQuad(GetRectangleQuad(color, vertical_A, vertical_B, vertical_C, vertical_D));
//            }
//        }

//        //得到一个矩形面片
//        private UIVertex[] GetRectangleQuad(Color color, params Vector2[] points)
//        {
//            UIVertex[] vertexs = new UIVertex[points.Length];
//            for (int i = 0; i < vertexs.Length; i++)
//            {
//                vertexs[i] = GetUIVertex(points[i], color);
//            }

//            return vertexs;
//        }

//        //得到一个顶点信息
//        private UIVertex GetUIVertex(Vector2 point, Color color)
//        {
//            UIVertex vertex = new UIVertex
//            {
//                position = point,
//                color = color,
//                uv0 = Vector2.zero
//            };
//            return vertex;
//        }

//        #endregion

//        #region pos <--> cell

//        public Vector3 GetWorldPosition(Vector3Int cell)
//        {
//            return new Vector3(cell.x*this.CellSize.x,cell.y*this.CellSize.y) + MapStartPos + this.CellSize*0.5f;
//        }

//        public Vector3Int GetLocalCell(Vector3 pos)
//        {
//            var cell = (pos - this.CellSize*0.5f);
//            cell = new Vector3(cell.x / this.CellSize.x, cell.y / this.CellSize.y);
//            Vector3Int local = new Vector3Int(Mathf.RoundToInt(cell.x), Mathf.RoundToInt(cell.y));
//            return local;
//        }

//        #endregion

//        #region AStar

//        private void CreateAStar()
//        {
//            AStarBattleGrid = new Grid(this.ColCount, this.RowCount, 1f);
//            AllCells = new List<Vector3Int>();
//            for (var i = 0; i < this.ColCount; i++)
//            {
//                for (var j = 0; j < this.RowCount; j++)
//                {
//                    AllCells.Add(new Vector3Int(i, j));
//                }
//            }
//        }

//        /// <summary>
//        /// 获取路径
//        /// </summary>
//        /// <param name="curPos"></param>
//        /// <param name="endPos"></param>
//        /// <returns></returns>
//        private List<Vector3Int> GetPathWithAStar(Vector3Int curPos, Vector3Int endPos)
//        {
//            var start = new Position(curPos.x, curPos.y);
//            var end = new Position(endPos.x, endPos.y);
//            //A*
//            var paths = AStarBattleGrid.GetPath(start, end);
//            var list = new List<Vector3Int>();
//            //排除出发点
//            if (paths.Length > 0)
//            {
//                for (int i = 1; i < paths.Length; i++)
//                {
//                    var p = paths[i];
//                    list.Add(new Vector3Int(p.X, p.Y, 0));
//                }
//            }
//            else
//            {
//                Debug.LogError("计算出错!");
//            }
//#if UNITY_EDITOR

//            // DrawLine(list);
//#endif
//            return list;
//        }

//        public Vector3Int GetPath(Vector3Int startPos, Vector3Int endPos)
//        {
//            var allPlayersCells = GameProcessor.Inst.PlayerManager.GetAllPlayers(true).Select(p => p.Cell).ToList();
//            var costDict = new Dictionary<Position, float>();

//            float highCost = 9999;
//            float lowCost = 1;

//            //临时将敌人的cell的cost设置为可通过但是代价高
//            float tempCost = highCost;
//            foreach (var cell in allPlayersCells)
//            {
//                var pos = new Position(cell.x, cell.y);
//                costDict[pos] = AStarBattleGrid.GetCellCost(pos);
//                AStarBattleGrid.SetCellCost(pos, tempCost);
//            }

//            //寻路
//            Action getPath = null;
//            //解析路径，判断最后几格是否能行走
//            Action<List<Vector3Int>> isLastPosHasSelfCampPlayer = null;

//            getPath = () =>
//            {
//                var pathList = new List<Vector3Int>();

//                var path = this.GetPathWithAStar(startPos, endPos);
//                //排除出发点
//                if (path.Count > 0)
//                {
//                    foreach (var p in path)
//                    {
//                        pathList.Add(p);
//                    }

//                    if (pathList.Count > 0)
//                    {
//                        isLastPosHasSelfCampPlayer.Invoke(pathList);
//                    }
//                    else
//                    {
//                        Debug.LogError("寻路失败：计算出错!");
//                    }
//                }
//                else
//                {
//                    Debug.LogError("寻路失败：计算出错!");
//                }
//            };


//            //由于无视障碍下，所有的格子都可以行走，第一条路可能不是最好的路，先尝试多次寻路
//            var otherList = new List<List<Vector3Int>>();

//            isLastPosHasSelfCampPlayer = (path) =>
//            {
//                var minCount = Mathf.Min(1, path.Count);
//                path = path.GetRange(0, minCount);

//                //找不到路，不需要再尝试
//                if (path.Count == 0) return;


//                //在无视阻挡的情况下，最后一个格子可能是队友、敌人和建筑，但因为位置不能重叠，所以最后一个格子也不能走
//                var last = path.Last();
//                if (allPlayersCells.Contains(last))
//                {
//                    path.RemoveAt(path.Count - 1);

//                    if (path.Count > 0)
//                    {
//                        //如果最后一个不能站立，最后一格周围4格格子可以站立，则路径没问题，否则要继续移除
//                        last = path.Last();
//                        if (allPlayersCells.Contains(last))
//                        {
//                            var pos = new Position(last.x, last.y);
//                            costDict[pos] = AStarBattleGrid.GetCellCost(pos);
//                            AStarBattleGrid.SetCellCost(pos, float.PositiveInfinity);
//                            path.RemoveAt(path.Count - 1);

//                            for (var i = path.Count - 1; i >= 0; i--)
//                            {
//                                if (allPlayersCells.Contains(last))
//                                {
//                                    path.RemoveAt(path.Count - 1);
//                                }
//                                else
//                                {
//                                    break;
//                                }
//                            }

//                            if (path.Count > 0)
//                            {
//                                //缓存找到的路径
//                                otherList.Add(path.ToArray().ToList());
//                            }

//                            //因为自己和周围4格都不能行走，可能不是最佳路径，将倒数第二格设为不可行走，尝试重新寻路
//                            //A-2-5-8-B 由于258不能站立，导致A实际上不能行走
//                            //A-2-3-[6]-B 由于6可以站立，在无视阻挡的情况下，可以穿过23或25来到6
//                            //****
//                            // A23
//                            //  5 
//                            //  8B
//                            //****
//                            getPath?.Invoke();
//                        }
//                        else
//                        {
//                            //缓存找到的路径
//                            otherList.Add(path.ToArray().ToList());
//                        }
//                    }
//                    else
//                    {
//                        //只有一格且不能站立说明就在目标四周，已是最近距离，不需要再寻路
//                        //这种情况理论上不会出现，如果是UI操作，目标点无法点击，不存在寻路，如果是AI，目标点不会是技能格
//                        //Debug.Log("不用寻路，当前点在目标点四周，已是最近位置！");
//                    }
//                }
//                else
//                {
//                    //缓存找到的路径
//                    otherList.Add(path.ToArray().ToList());
//                }
//            };

//            getPath.Invoke();

//            // 恢复cell的cost
//            foreach (var cell in allPlayersCells)
//            {
//                var pos = new Position(cell.x, cell.y);
//                if (costDict.ContainsKey(pos))
//                {
//                    AStarBattleGrid.SetCellCost(pos, costDict[pos]);
//                }
//            }
//#if UNITY_EDITOR

//            // DrawLine(list);
//#endif
//            //优先走拐点少的路
//            if (otherList != null && otherList.Count > 0)
//            {
//                var list = otherList[0];
//                foreach (var other in otherList)
//                {
//                    if (other.Count > list.Count)
//                    {
//                        list = other;
//                    }
//                }

//                return list[0];
//            }

//            return startPos;
//        }

//        #endregion

//        #region AOE Range

//        public List<Vector3Int> GetAttackRangeCell(Vector3Int selfCell, Vector3Int dir, int attackRange, AttackGeometryType geometryType)
//        {
//            List<Vector3Int> rangeCells = new List<Vector3Int>();
//            Vector3Int targetCell = Vector3Int.zero;
//            switch (geometryType)
//            {
//                case AttackGeometryType.FrontRow:
//                {
//                    for (var i = 1; i <= attackRange; i++)
//                    {
//                        targetCell = selfCell + dir * i;

//                        rangeCells.Add(targetCell);
//                    }
//                }
//                    break;
//                case AttackGeometryType.Cross:
//                {
//                    for (var i = 1; i <= attackRange; i++)
//                    {
//                        targetCell = selfCell + Vector3Int.up * i;
//                        rangeCells.Add(targetCell);
//                        targetCell = selfCell + Vector3Int.down * i;
//                        rangeCells.Add(targetCell);
//                        targetCell = selfCell + Vector3Int.left * i;
//                        rangeCells.Add(targetCell);
//                        targetCell = selfCell + Vector3Int.right * i;
//                        rangeCells.Add(targetCell);
//                    }
//                }
//                    break;
//                case AttackGeometryType.Square:
//                {
//                    for (var i = attackRange*-1; i <= attackRange; i++)
//                    {
//                        for (var j = attackRange * -1; j <= attackRange; j++)
//                        {
//                            targetCell = selfCell + new Vector3Int(j, i); 
//                            rangeCells.Add(targetCell);
//                        }
//                    }
//                }
//                    break;
//                case AttackGeometryType.Diamond:
//                    for (var i = attackRange*-1; i <= attackRange; i++)
//                    {
//                        var rowCount = attackRange - Mathf.Abs(i);
//                        for (var j = rowCount * -1; j <= rowCount; j++)
//                        {
//                            targetCell = selfCell + new Vector3Int(j, i);
//                            rangeCells.Add(targetCell);
//                        }
//                    }
//                    break;
//                case AttackGeometryType.FullBox:
//                    rangeCells.AddRange(this.AllCells);
//                    break;
//            }

//            rangeCells = rangeCells.Distinct().ToList();
//            return rangeCells;
//        }

//        #endregion
//    }
}

