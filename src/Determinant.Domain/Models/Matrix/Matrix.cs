using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Determinant.Domain.Models.Matrix
{
    public class Matrix3x3 : IMatrix
    {
        private int?[,] _matrix = new int?[SizeX, SizeY];
        private int[] _allValues = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        public const int SizeX = 3;
        public const int SizeY = 3;

        public const int MinValue = 1;
        public const int MaxValue = 9;

        public Matrix3x3()
        {
            InitMatrix(); 
        }

        private bool ValidateCellPosition(int x, int y)
        {
            if (x > 0 && x < SizeX) return true;
            if (y > 0 && y < SizeY) return true;

            return false;
        }

        private bool ValidateCellValue(int value)
        {
            if (value >= MinValue && value <= MaxValue) return true;

            return false;
        }

        private void InitMatrix()
        {
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    _matrix[x, y] = null;
                }
            }
        }

        public IEnumerable<int> GetUsedValues()
        {
            var usedValues = new List<int>();

            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    if (_matrix[x, y] != null)
                    {
                        usedValues.Add(_matrix[x, y].Value);
                    };
                }
            }

            return usedValues.ToArray();
        }

        public IEnumerable<int> GetAvailableValues()
        {
            int[] allValues = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            return allValues.Except(GetUsedValues());
        }

        public bool IsCellEmpty(int x, int y)
        {
            return GetValue(x, y) == null;
        }

        public IEnumerable<MatrixCell> GetAvailableCells()
        {
            var availableCells = new List<MatrixCell>();

            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    if (_matrix[x, y] == null)
                    {
                        availableCells.Add(new MatrixCell { Column = x, Row = y});
                    };
                }
            }

            return availableCells;
        }

        /// <summary>
        /// Set matrix cell value
        /// </summary>
        /// <param name="x">Zero-based horizontal cell position</param>
        /// <param name="y">Zero-based vertical cell position</param>
        /// <returns>Cell value</returns>
        public int? GetValue(int x, int y)
        {
            return _matrix[x, y];
        }


        /// <summary>
        /// Set matrix cell value
        /// </summary>
        /// <param name="x">Zero-based horizontal cell position</param>
        /// <param name="y">Zero-based vertical cell position</param>
        /// <param name="value">Cell value</param>
        public void SetValue(int x, int y, int value)
        {
           if (GetValue(x, y) == null && x < SizeX && y < SizeY && value <= MaxValue)
           {
               _matrix[x, y] = value;     
           }            
        }
  
        /// <summary>
        /// Get matrix determinant
        /// </summary>
        /// <returns>Determinant</returns>
        public int GetDeterminant()
        {
            int positive = (_matrix[0, 0] ?? 0) * (_matrix[1, 1] ?? 0) * (_matrix[2, 2] ?? 0) +
                (_matrix[0, 1] ?? 0) * (_matrix[1, 2] ?? 0) * (_matrix[2, 0] ?? 0) +
                (_matrix[1, 0] ?? 0) * (_matrix[2, 1] ?? 0) * (_matrix[0, 2] ?? 0);

            int negative =  (_matrix[0, 2] ?? 0) * (_matrix[1, 1] ?? 0) * (_matrix[2, 0] ?? 0) +
                (_matrix[0, 1] ?? 0) * (_matrix[1, 0] ?? 0) * (_matrix[2, 2] ?? 0) +
                (_matrix[1, 2] ?? 0) * (_matrix[2, 1] ?? 0) * (_matrix[0, 0] ?? 0);

            return positive - negative;
        }

        public bool IsFull()
        {
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {
                    if (_matrix[x, y] == null) { return false; };
                }
            }

            return true;
        }
    }
}
