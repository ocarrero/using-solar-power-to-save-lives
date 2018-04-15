﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace using_solar_power_to_save_lives.Models
{
    public interface IParabola{

        int Diameter { get; }
        int Depth { get; }
        int LineSegments { get; }
        double LinearDiameter { get; }
        int FocalLength { get; }
        double Volume { get; }
        double FocalLengthDiameter { get; }
        double Area { get; }

        double[,] SegmentCoordinates { get; }




    }

}