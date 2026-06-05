using System;
using System.Collections.Generic;
using System.Linq;
using SpaceBattle.Lib;

namespace SpaceBattle.Lib.Command
{
    public class SpatialGrid
    {
        private readonly double _cellSize;
        private readonly Dictionary<(int x, int y), HashSet<Guid>> _grid = new();

        public SpatialGrid(double cellSize)
        {
            _cellSize = cellSize;
        }

        private (int x, int y) GetCell(Vector pos)
        {
            int x = (int)Math.Floor(pos.Coordinates[0] / _cellSize);
            int y = (int)Math.Floor(pos.Coordinates[1] / _cellSize);
            return (x, y);
        }

        public void Add(Guid id, Vector pos)
        {
            var cell = GetCell(pos);
            if (!_grid.ContainsKey(cell))
                _grid[cell] = new HashSet<Guid>();
            _grid[cell].Add(id);
        }

        public void Remove(Guid id, Vector pos)
        {
            var cell = GetCell(pos);
            if (_grid.TryGetValue(cell, out var set))
                set.Remove(id);
        }

        public void Update(Guid id, Vector oldPos, Vector newPos)
        {
            var oldCell = GetCell(oldPos);
            var newCell = GetCell(newPos);
            if (oldCell == newCell) return;
            Remove(id, oldPos);
            Add(id, newPos);
        }

        public IEnumerable<Guid> GetNearby(Guid id, Vector pos, double radius)
        {
            var center = GetCell(pos);
            int offset = (int)Math.Ceiling(radius / _cellSize);
            for (int dx = -offset; dx <= offset; dx++)
            for (int dy = -offset; dy <= offset; dy++)
            {
                var cell = (center.x + dx, center.y + dy);
                if (_grid.TryGetValue(cell, out var set))
                    foreach (var other in set)
                        if (other != id)
                            yield return other;
            }
        }
    }
}