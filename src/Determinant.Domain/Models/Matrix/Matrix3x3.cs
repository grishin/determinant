using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Determinant.Domain.Models.Matrix
{
    public class Matrix3x3 : IMatrix
    {
        int?[,] _matrix = new int?[SizeX, SizeY];				
        List<int> _deletedNumbers = new List<int>(9);

        public const int SizeX = 3;
        public const int SizeY = 3;

        public const int MaxValue = 9;

        public Matrix3x3()
        {
            InitMatrix(); 
            InitDeletedNumbersList();
        }

        private void InitDeletedNumbersList()
        {
            _deletedNumbers.Clear();
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
               _deletedNumbers.Add(value);
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
    }
}
