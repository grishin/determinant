using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Determinant.Domain.Models.Matrix
{
    public class Matrix3x3 
    {
        private int?[,] _matrix = new int?[Columns, Rows];
        private int[] _allValues = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        public const int Columns = 3;
        public const int Rows = 3;

        public const int MinValue = 1;
        public const int MaxValue = 9;

        public Matrix3x3()
        {
            InitMatrix(); 
        }

        private Matrix3x3(int?[,] matrixData)
        {
            _matrix = matrixData;
        }

        public Matrix3x3 Clone()
        {
            var copy = new int?[Columns, Rows];

            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    copy[x, y] = _matrix[x, y];
                }
            }


            return new Matrix3x3(copy);
        }

        private bool ValidateCellPosition(int x, int y)
        {
            if (x > 0 && x < Columns) return true;
            if (y > 0 && y < Rows) return true;

            return false;
        }

        private bool ValidateCellValue(int value)
        {
            if (value >= MinValue && value <= MaxValue) return true;

            return false;
        }

        private void InitMatrix()
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    _matrix[x, y] = null;
                }
            }
        }

        public IEnumerable<int> GetUsedValues()
        {
            var usedValues = new List<int>();

            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
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
            return _allValues.Except(GetUsedValues());
        }

        public bool IsCellEmpty(MatrixCell cell)
        {
            return GetValue(cell) == null;
        }

        public IEnumerable<MatrixCell> GetAvailableCells()
        {
            var availableCells = new List<MatrixCell>();

            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
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
        public int? GetValue(MatrixCell cell)
        {
            return _matrix[cell.Column, cell.Row];
        }


        /// <summary>
        /// Set matrix cell value
        /// </summary>
        /// <param name="x">Zero-based horizontal cell position</param>
        /// <param name="y">Zero-based vertical cell position</param>
        /// <param name="value">Cell value</param>
        public void SetValue(MatrixCell cell, int value)
        {
           if (GetValue(cell) == null && cell.Column < Columns && cell.Row < Rows && value > 0 && value <= MaxValue)
           {
               _matrix[cell.Column, cell.Row] = value;     
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
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    if (_matrix[x, y] == null) { return false; };
                }
            }

            return true;
        }
    }
}
