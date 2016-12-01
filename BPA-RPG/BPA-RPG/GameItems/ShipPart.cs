﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA_RPG.GameItems
{
    public abstract class ShipPart : GameItem
    {
        public ShipPart(string name, Texture2D texture, string info = "")
            : base(name, texture, info)
        {
        }
    }
}
