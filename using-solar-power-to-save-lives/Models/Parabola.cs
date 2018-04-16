using System;
using System.ComponentModel.DataAnnotations;


namespace using_solar_power_to_save_lives.Models
{


    public class Parabola : IParabola

    {
        [Display(Name = "Diameter")]
        [Required]
        [Range(48, 1500, ErrorMessage = "Please enter a diameter value.")]
        private int diameter;
        [Display(Name = "Depth")]
        [Required]
        [Range(12, 900, ErrorMessage = "Please enter a depth value.")]
        private int depth;

        [Display(Name = "Line Segments")]
        [Required]
        [Range(4, 16, ErrorMessage = "Please enter a depth value.")]
        private int lineSegments;

        private double linearDiameter;
        private int focalLength;
        private double volume;
        private double focalLengthDiameter;
        private double area;
        private double[,] segmentCoordinatesFinal = new double[3, 200001];

        public struct PointType
        {
            public double x;
            public double y;
        }

        private RegressionObject regression;

        public int Diameter { get { return diameter; } set { diameter = value; } }

        public int Depth { get { return depth; } set { depth = value; } }
        public int LineSegments { get { return lineSegments; } set { lineSegments = value; } }

        public double LinearDiameter { get { return linearDiameter; } }

        public int FocalLength { get { return focalLength; } }

        public double Volume { get { return volume; } }

        public double FocalLengthDiameter { get { return linearDiameter; } }

        public double Area { get { return linearDiameter; } }

        public double[,] SegmentCoordinates { get { return segmentCoordinatesFinal; } }

        public Parabola()
        {
            // set defaults
            diameter = 48;
            depth = 12;
            lineSegments = 16;
  
        }

        public Parabola(int dia, int dep, int lineSeg)
        {
            this.diameter = dia;
            this.depth = dep;
            this.lineSegments = lineSeg;

        }

        public void Refresh()
        {
            SetParabolaProperties();
        }

        private void SetParabolaProperties()
        {
            int segments = 0;
            PointType[] pointTypes = new PointType[4];
            int xPoint = 0;
            double yPoint = 0;

            float parabolaPoints = 0;
            int segmentPointsCtr = 0;
            double[,] segmentCoordinates = new double[2, 200001];

            area = System.Math.PI * (Math.Pow((diameter / 2), 2));
            // todo focal length calculation is wrong
            focalLength = (diameter * diameter) / (16 * depth);
            focalLengthDiameter = focalLength / diameter;
            volume = (System.Math.PI * Math.Pow((diameter / 2), 2) * depth) / 2;
            linearDiameter = 0;
            segments = (int)(Math.Round((decimal)(lineSegments / 2)));
            regression = new RegressionObject();

            var _with1 = regression;
            _with1.DegreeOfExpectedPoly = 3;
            _with1.XYAdd(-1 * diameter / 2, depth);
            pointTypes[0].x = (-1 * diameter / Convert.ToDouble(2.0));
            //add points for graphing
            pointTypes[0].y = depth;
            _with1.XYAdd(0, 0);
            //add bottom midpoint
            pointTypes[1].x = 0;
            pointTypes[1].y = 0;
            _with1.XYAdd(diameter / 2, depth);
            //add top right point
            pointTypes[2].x = diameter / Convert.ToDouble(2.0);
            pointTypes[2].y = depth;

            parabolaPoints = (float)((pointTypes[2].x - pointTypes[0].x) / (segments * 2));

            // Calculate all line segment coordinates
            for (xPoint = (int)pointTypes[0].x; xPoint <= 1.002 * pointTypes[2].x; xPoint += (int)parabolaPoints)
            {
                yPoint = regression.RegVal(Convert.ToDouble(xPoint));
                segmentCoordinates[0, segmentPointsCtr] = xPoint;
                //x point
                segmentCoordinates[1, segmentPointsCtr] = yPoint;
                //y point
                segmentPointsCtr = segmentPointsCtr + 1;
            }

            // Clean up any segment coordinates not needed
            segmentCoordinates = (double[,])Microsoft.VisualBasic.CompilerServices.Utils.CopyArray(segmentCoordinates, new double[2, segmentPointsCtr]);

            //=========================================================
            //calculate linear distance of parabola = unfolded diameter
            //Distance between 2 points In 2D
            //Point 1 at (x1, y1) and Point 2 at (x2, y2).
            //    xd = X2 - X1
            //    yd = Y2 - Y1
            //    Distance = SquareRoot(xd * xd + yd * yd)

            double x1 = 0;
            double x2 = 0;
            double y1 = 0;
            double y2 = 0;


            int MaxGraphPoint = segmentCoordinates.GetUpperBound(1); // UBound(segmentCoordinates, 2);
            for (int n = segmentCoordinates.GetLowerBound(1); n <= segmentCoordinates.GetUpperBound(1) - 1; n++)
            {
                x1 = segmentCoordinates[0, n];
                if (MaxGraphPoint == (n + 1))
                {
                    x2 = x1;
                }
                else
                {
                    x2 = segmentCoordinates[0, n + 1];
                }
                y1 = segmentCoordinates[1, n];
                if (MaxGraphPoint == (n + 1))
                {
                    y2 = y1;
                }
                else
                {
                    y2 = segmentCoordinates[1, n + 1];
                }

                linearDiameter = linearDiameter + System.Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
            }

            // for all segment x coordinates, get the absolute value of first x coordinate and add that two every other x coordinate in the sequence
            double absValFirstX = System.Math.Abs(segmentCoordinates[0, 0]);
            double xCoorTemp = 0;

            segmentCoordinatesFinal = new double[2, segmentPointsCtr];


            for (int i = 0; i <= segmentPointsCtr - 1; i++)
            {
                xCoorTemp = segmentCoordinates[0, i];
                segmentCoordinatesFinal[0, i] = xCoorTemp + absValFirstX;
                segmentCoordinatesFinal[1, i] = System.Math.Abs(segmentCoordinates[1, i] - depth);

            }


        }


    }

}
