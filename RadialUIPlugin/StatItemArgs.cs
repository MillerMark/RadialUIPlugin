using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bounce.Unmanaged;
using UnityEngine;

namespace RadialUI
{
    public class StatItemArgs
    {
        public Color Color;
        public string Title;
        public float Max;
        public float Current;
        public Action<MapMenuStatItem, NGuid> Callback;
    }
}
