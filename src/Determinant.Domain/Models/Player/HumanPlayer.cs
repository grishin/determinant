﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Determinant.Domain.Models.Player
{
    public class HumanPlayer : IPlayer
    {
        public string GetName()
        {
            return "Human";
        }
    }
}
