﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Determinant.Domain.Services.Game
{
    using Determinant.Domain.Models.AI;
    using Game = Determinant.Domain.Models.Game;

    public interface IGameBuilder
    {
        Game CreateGame();
    }
}
