using System;
using System.Collections.Generic;
using UnityEngine;

namespace __Game.Scripts
{

    [Serializable]
    public class Map
    {
        public int Radius { get; set; }

        public List<Vent> Vents { get; set; }

        public List<Nest> Nests { get; set; }
    }
}