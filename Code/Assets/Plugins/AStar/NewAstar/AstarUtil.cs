using System;
using System.Collections.Generic;
using System.Linq;
using Astar;

namespace Game.NewAstar
{
    public enum CorrectType
    {
        Rightangle = 1,
        Hypotenuse = 2
    }

    public class AstarUtil
    {
        public static List<Position> CorrectAstar(List<Position> posList, Grid grid)
        {
            var corners = CollectCorners(posList);
            if (corners.Count <= 2) return posList;
            var first = posList.First();
            var last  = posList.Last();

            var resultList = corners;

            while (true)
            {
                var correctList = new List<Position>();
                var rCount      = resultList.Count;
                var endCorrect  = true;
                for (var i = 1; i < rCount - 1; i++)
                {
                    var fi             = resultList[i - 1];
                    var se             = resultList[i];
                    var th             = resultList[i + 1];
                    var curCorrectList = CorrectAstar(fi, se, th, posList, grid);
                    if (!CheckSame(curCorrectList, fi, se, th))
                    {
                        if (correctList.Count > 0 && correctList.Last() == curCorrectList[0])
                        {
                            correctList.Remove(correctList.Last());
                        }

                        correctList.AddRange(curCorrectList);
                        var start = resultList.IndexOf(th);
                        correctList.AddRange(resultList.GetRange(start + 1, rCount - start - 1));
                        if (correctList.First() != first)
                        {
                            correctList.Insert(0, first);
                        }

                        if (correctList.Last() != last)
                        {
                            correctList.Add(last);
                        }

                        endCorrect = false;
                        resultList = correctList;
                        break;
                    }

                    correctList.Add(se);
                }

                if (endCorrect)
                {
                    if (resultList.First() != first)
                    {
                        resultList.Insert(0, first);
                    }

                    if (resultList.Last() != last)
                    {
                        resultList.Add(last);
                    }

                    break;
                }
            }

            return resultList;
        }

        public static bool CheckSame(List<Position> curCorrectList, Position fi, Position se, Position th)
        {
            return curCorrectList.Contains(fi) && curCorrectList.Contains(se) && curCorrectList.Contains(th);
        }

        public static List<Position> CorrectAstar(Position fi, Position se, Position th, List<Position> posList, Grid grid)
        {
            var type  = GetCorrectType(fi, se);
            var type2 = GetCorrectType(se, th);

            if (type == type2)
            {
                return new List<Position> {fi, se, th};
            }

            var result    = new List<Position>();
            var isCorrect = false;
            var i         = 0;
            if (type == CorrectType.Rightangle)
            {
                //fi->se
                var start = posList.IndexOf(fi);
                var end   = posList.IndexOf(se);
                for (i = start; i < end; i++)
                {
                    if (CanCorrect(posList[i], th, grid))
                    {
                        isCorrect = true;
                        result.Add(posList[i]);
                        break;
                    }
                }

                if (!isCorrect)
                {
                    result.Add(fi);
                    result.Add(se);
                }

                result.Add(th);
            }
            else
            {
                //th->se
                var start = posList.IndexOf(th);
                var end   = posList.IndexOf(se);
                for (i = start; i > end + 1; i--)
                {
                    if (CanCorrect(fi, posList[i], grid))
                    {
                        isCorrect = true;
                        result.Add(posList[i]);
                        break;
                    }
                }

                if (!isCorrect)
                {
                    result.Add(se);
                    result.Add(th);
                }

                result.Insert(0, fi);
            }

            return result;
        }

        public static bool CanCorrect(Position start, Position end, Grid grid)
        {
            var equation   = new CorrectEquation(start, end);
            var canCorrect = true;
            if (Math.Abs(end.X - start.X) > Math.Abs(end.Y - start.Y))
            {
                if (end.X - start.X > 0)
                {
                    for (var x = start.X; x < end.X; x++)
                    {
                        var y        = equation.GetY(x);
                        var walkable = grid.IsWalkableAt(x, y);
                        if (!walkable)
                        {
                            canCorrect = false;
                            break;
                        }
                    }
                }
                else
                {
                    for (var x = start.X; x >= end.X; x--)
                    {
                        var y        = equation.GetY(x);
                        var walkable = grid.IsWalkableAt(x, y);
                        if (!walkable)
                        {
                            canCorrect = false;
                            break;
                        }
                    }
                }
            }
            else
            {
                if (end.Y - start.Y > 0)
                {
                    for (var y = start.Y; y < end.Y; y++)
                    {
                        var x        = equation.GetX(y);
                        var walkable = grid.IsWalkableAt(x, y);
                        if (!walkable)
                        {
                            canCorrect = false;
                            break;
                        }
                    }
                }
                else
                {
                    for (var y = start.Y; y >= end.Y; y--)
                    {
                        var x        = equation.GetX(y);
                        var walkable = grid.IsWalkableAt(x, y);
                        if (!walkable)
                        {
                            canCorrect = false;
                            break;
                        }
                    }
                }
            }

            return canCorrect;
        }


        public static CorrectType GetCorrectType(Position pos1, Position pos2)
        {
            if (pos1.X != pos2.X && pos1.Y != pos2.Y)
            {
                return CorrectType.Hypotenuse;
            }

            return CorrectType.Rightangle;
        }


        /// <summary>
        ///     计算拐点的集合
        /// </summary>
        /// <param name="resultList"></param>
        public static List<Position> CollectCorners(List<Position> resultList)
        {
            var length = resultList.Count;
            var result = new List<Position>();
            result.Add(resultList[0]);
            for (var i = 1; i < length - 1; i++)
            {
                var first  = resultList[i - 1];
                var second = resultList[i];
                var third  = resultList[i + 1];
                var sf     = second - first;
                var ts     = third  - second;
                if (sf != ts)
                {
                    result.Add(second);
                }
            }

            result.Add(resultList[length - 1]);
            return result;
        }
    }
}