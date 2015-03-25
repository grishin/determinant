using Determinant.Domain.Models.Matrix;
using Determinant.Domain.Models.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Determinant.Domain.Models.AI
{
    public class ComputerPlayerStrategySmart : IComputerPlayerStrategy
    {
        class Cross
        {
            public MatrixCell Cell { get; set; }
            public int Value { get; set; }
            public int Determinant { get; set; }
        }

        private Random _random;

        public ComputerPlayerStrategySmart()
        {
            _random = new Random();
        }

        protected int GetRandomValue(int[] values)
        {
            int position = _random.Next(0, values.Length - 1);
            return values[position];
        }

        protected MatrixCell GetRandomCell(MatrixCell[] cells)
        {
            int position = _random.Next(0, cells.Length - 1);
            return cells[position];
        }

        public TurnResult MakeTurn(Matrix3x3 matrix, PlayerGoal goal)
        {
            int[] availableValues = matrix.GetAvailableValues().ToArray();
            MatrixCell[] availableCells = matrix.GetAvailableCells().ToArray();

            // first value we set randomly (just for optimization)
            if (IsFirstTurn(availableValues))
            {
                return new TurnResult
                {
                    Cell = GetRandomCell(availableCells),
                    Value = GetRandomValue(availableValues)
                };
            }

            // get all cells crossed by all values
            var cross = (from value in availableValues
                        from cell in availableCells
                        select new Cross
                        {
                            Value = value,
                            Cell = cell
                        }).ToArray();

            // counting determinants for each cross item
            foreach (var item in cross)
            {
                var m = matrix.Clone();
                m.SetValue(item.Cell, item.Value);
                item.Determinant = m.GetDeterminant();
            }

            var effectiveItems = goal == PlayerGoal.Positive ? cross.Where(x => x.Determinant > 0) : cross.Where(x => x.Determinant < 0);
            if (!effectiveItems.Any())
            {
                // if all determinants are 0 we set random value in random cell
                return new TurnResult
                {
                    Cell = GetRandomCell(availableCells),
                    Value = GetRandomValue(availableValues)
                };
            }

            // we try to get cells & values where determinant is larger for positive goal
            // or smaller for negative goal
            var crossOrdered = goal == PlayerGoal.Positive ? effectiveItems.OrderByDescending(x => x.Determinant) : effectiveItems.OrderBy(x => x.Determinant);
            
            var result = crossOrdered.First();

            return new TurnResult
            {
                Cell = result.Cell,
                Value = result.Value
            };
        }

        private static bool IsFirstTurn(int[] availableValues)
        {
            return availableValues.Count() == Matrix3x3.Columns * Matrix3x3.Rows;
        }

        public string Name
        {
            get { return "Smart"; }
        }


    }
}
