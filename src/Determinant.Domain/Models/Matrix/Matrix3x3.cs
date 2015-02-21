using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Determinant.Domain.Models.Matrix
{
    public class Matrix3x3 : IMatrix
    {
        private int[,] _data = new int[3, 3];				
        private List<int> _deletedNumbers = new List<int>(9);	

        public Matrix3x3()
        {

        }


    }
}
