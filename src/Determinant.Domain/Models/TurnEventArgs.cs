using Determinant.Domain.Models.Matrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Determinant.Models
{
    public class TurnEventArgs : EventArgs
    {
        public MatrixCell Cell { get; set; }
        public int Value { get; set; }
    }
}
