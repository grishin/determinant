﻿using Determinant.Domain.Models.Matrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Determinant.Domain.Models.Player
{
    public interface IPlayer
    {
        string Name { get; }

        PlayerGoal Goal { get; }
    }
}
