﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Game
{
    /// <summary>
    ///     Representation of your world for the pathfinding algorithm.
    ///     Use SetCellCost to change the cost of traversing a cell.
    ///     Use BlockCell to make a cell completely intraversable.
    /// </summary>
    public sealed class Grid
    {
        private readonly float   DefaultCost;
        private readonly float[] Weights;

        /// <summary>
        ///     Creates a grid
        /// </summary>
        /// <param name="dimX">The x-dimension of your world</param>
        /// <param name="dimY">The y-dimesion of your world</param>
        /// <param name="defaultCost">The default cost every cell is initialized with</param>
        public Grid(int dimX, int dimY, float defaultCost = 1.0f)
        {
            if (defaultCost < 1) throw new ArgumentOutOfRangeException($"Argument {nameof(defaultCost)} with value {defaultCost} is invalid. The cost of traversing a cell cannot be less than one");

            DefaultCost = defaultCost;
            Weights     = new float[dimX * dimY];
            DimX        = dimX;
            DimY        = dimY;

            for (var n = 0; n < Weights.Length; n++) Weights[n] = defaultCost;
        }

        /// <summary>
        ///     X-dimension of the grid
        /// </summary>
        public int DimX { get; }

        /// <summary>
        ///     Y-dimension of the grid
        /// </summary>
        public int DimY { get; }

        /// <summary>
        ///     Sets the cost for traversing a cell
        /// </summary>
        /// <param name="position">A position inside the grid</param>
        /// <param name="cost">The cost of traversing the cell, cannot be less than one</param>
        public void SetCellCost(Position position, float cost)
        {
            if (cost < 1) throw new ArgumentOutOfRangeException($"Argument {nameof(cost)} with value {cost} is invalid. The cost of traversing a cell cannot be less than one");
            int index = GetIndex(position.X, position.Y);
            if (index != -1)
            {
                Weights[index] = cost;
            }
        }

        public void ClearCellCost()
        {
            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i] = DefaultCost;
            }
        }

        /// <summary>
        ///     Makes the cell intraversable
        /// </summary>
        /// <param name="position">A position inside the grid</param>
        public void BlockCell(Position position)
        {
            SetCellCost(position, float.PositiveInfinity);
        }

        /// <summary>
        ///     Makes the cell traversable, gives it the default traversal cost as provided in the constructor
        /// </summary>
        /// <param name="position">A position inside the grid</param>
        public void UnblockCell(Position position)
        {
            SetCellCost(position, DefaultCost);
        }

        /// <summary>
        ///     Looks-up the cost for traversing a given cell, if a cell is blocked (<see cref="BlockCell" />)
        ///     +infinity is returned
        /// </summary>
        /// <param name="position">A position inside the grid</param>
        /// <returns>The cost</returns>
        public float GetCellCost(Position position)
        {
            var index = GetIndex(position.X, position.Y);
            if (index == -1)
            {
                return float.PositiveInfinity;
            }
            return Weights[index];
        }

        /// <summary>
        /// 判断格子是否阻挡
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public bool IsCellNotBlock(int x,int y)
        {
            var idx = GetIndex(x, y);
            if (idx >= 0)
            {
                return Weights[idx] == DefaultCost;
            }
            else
            {
                return false;
            }
        }

        
        /// <summary>
        ///     Looks-up the cost for traversing a given cell, does not check
        ///     if the position is inside the grid
        /// </summary>
        /// <param name="position">A position inside the grid</param>
        /// <returns>The cost</returns>
        internal float GetCellCostUnchecked(Position position)
        {
            return Weights[GetIndexUnchecked(position.X, position.Y)];
        }

        /// <summary>
        ///     Computes the lowest-cost path from start to end inside the grid for an agent that can
        ///     move both diagonal and lateral
        /// </summary>
        /// <param name="start">The start position</param>
        /// <param name="end">The end position</param>
        /// <returns>Positions along the shortest path from start to end, or an empty array if no path could be found</returns>
        public Position[] GetPath(Position start, Position end)
        {
            return GetPath(start, end, MovementPatterns.LateralOnly);
        }

        /// <summary>
        ///     Computes the lowest-cost path from start to end inside the grid for an agent with a custom
        ///     movement pattern
        /// </summary>
        /// <param name="start">The start position</param>
        /// <param name="end">The end position</param>
        /// <param name="movementPattern">
        ///     The movement pattern of the agent, <see cref="MovementPatterns" /> for several built-in
        ///     options
        /// </param>
        /// <returns>Positions along the shortest path from start to end, or an empty array if no path could be found</returns>
        public Position[] GetPath(Position start, Position end, Offset[] movementPattern)
        {
            var current = PathFinder.FindPath(this, start, end, movementPattern);

            if (current == null) return new Position[0];

            // The Pathfinder returns the positions that found the end. If we want
            // to list positions from start to end we need reverse the traversal.
            var steps = new Stack<Position>();

            foreach (var step in current) steps.Push(step);

            return steps.ToArray();
        }

        /// <summary>
        ///     Computes the lowest-cost path from start to end inside the grid for an agent with a custom
        ///     movement pattern. Instructs the path finder to give up if the path is not found after a number of iterations.
        /// </summary>
        /// <param name="start">The start position</param>
        /// <param name="end">The end position</param>
        /// <param name="movementPattern">
        ///     The movement pattern of the agent, <see cref="MovementPatterns" /> for several built-in
        ///     options
        /// </param>
        /// <param name="iterationLimit">Maximum number of nodes to check before the path finder gives up</param>
        /// <returns>Positions along the shortest path from start to end, or an empty array if no path could be found</returns>
        public Position[] GetPath(Position start, Position end, Offset[] movementPattern, int iterationLimit)
        {
            var current = PathFinder.FindPath(this, start, end, movementPattern, iterationLimit);

            if (current == null) return new Position[0];

            // The Pathfinder returns the positions that found the end. If we want
            // to list positions from start to end we need reverse the traversal.
            var steps = new Stack<Position>();

            foreach (var step in current) steps.Push(step);

            return steps.ToArray();
        }

        /// <summary>
        ///     Converts a 2d index to a 1d index and performs bounds checking
        /// </summary>
        private int GetIndex(int x, int y)
        {
            if (x < 0 || x >= DimX)
            {
                return -1;
            };

            if (y < 0 || y >= DimY)
            {
                return -1;
            };

            return GetIndexUnchecked(x, y);
        }

        /// <summary>
        ///     Converts a 2d index to a 1d index without any bounds checking
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int GetIndexUnchecked(int x, int y)
        {
            return DimX * y + x;
        }
    }
}