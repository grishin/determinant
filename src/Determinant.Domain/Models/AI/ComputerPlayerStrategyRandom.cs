using Determinant.Domain.Models.Matrix;
using Determinant.Domain.Models.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Determinant.Domain.Models.AI
{
    public class ComputerPlayerStrategyRandom : IComputerPlayerStrategy
    {
        private Random _random;

        public ComputerPlayerStrategyRandom()
        {
            _random = new Random();
        }

        public string Name
        {
            get { return "Dummy"; }
        }

        public TurnResult MakeTurn(IMatrix matrix)
        {
            int[] availableValues = matrix.GetAvailableValues().ToArray();
            MatrixCell[] availableCells = matrix.GetAvailableCells().ToArray();

            return new TurnResult
            {
                Cell = GetRandomCell(availableCells),
                Value = GetRandomValue(availableValues)
            };
        }

        private int GetRandomValue(int[] values)
        {
            int position = _random.Next(0, values.Length - 1);
            return values[position];
        }

        private MatrixCell GetRandomCell(MatrixCell[] cells)
        {
            int position = _random.Next(0, cells.Length - 1);
            return cells[position];
        }

    }
}
