using Determinant.Domain.Models.Matrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Determinant.Domain.Models.Player
{
    public class TurnResult
    {
        public MatrixCell Cell { get; set; }
        public int Value { get; set; }
    }
}
