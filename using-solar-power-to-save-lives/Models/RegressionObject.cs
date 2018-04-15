using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;

namespace using_solar_power_to_save_lives.Models
{
    public class RegressionObject
    {

        private const int maxO = 25;
        //"Ordnung" = degree of the polynom expected
        private long degreeOfExpectedPolynomial;

        private bool finished;
        private double[] sumOfX = new double[2 * maxO + 1];
        private double[] sumOfYandX = new double[maxO + 1];
        private double[,] M = new double[maxO + 1, maxO + 2];
        //coefficients in: Y = C(0)*X^0 + C(1)*X^1 + C(2)*X^2 + ...
        private double[] coefficientsIn = new double[maxO + 1];


        public RegressionObject()
        {
            Init();
            degreeOfExpectedPolynomial = 2;
        }
        private void GaussSolve(long O)
        {
            //gauss algorithm implementation,
            // following R.Sedgewick's "Algorithms in C",
            // Addison-Wesley, with minor modifications
            long i = 0;
            long j = 0;
            long k = 0;
            long iMax = 0;
            double T = 0;
            double O1 = 0;
            O1 = O + 1;
            //first triangulize the matrix
            for (i = 0; i <= O; i++)
            {
                iMax = i;
                T = System.Math.Abs(M[iMax, i]);
                for (j = i + 1; j <= O; j++)
                {
                    //find the line with the largest absvalue in this row
                    if (T < System.Math.Abs(M[j, i]))
                    {
                        iMax = j;
                        T = System.Math.Abs(M[iMax, i]);
                    }
                }
                if (i < iMax)
                {
                    //exchange the two lines
                    for (k = i; k <= O1; k++)
                    {
                        T = M[i, k];
                        M[i, k] = M[iMax, k];
                        M[iMax, k] = T;
                    }
                }
                //scale all following lines to have a leading zero
                for (j = i + 1; j <= O; j++)
                {
                    T = M[j, i] / M[i, i];
                    M[j, i] = 0.0;
                    for (k = i + 1; k <= O1; k++)
                    {
                        M[j, k] = M[j, k] - M[i, k] * T;
                    }
                }
            }
            //then substitute the coefficients
            for (j = O; j >= 0; j += -1)
            {
                T = M[j, (int)O1];
                for (k = j + 1; k <= O; k++)
                {
                    T = T - M[j, k] * coefficientsIn[k];
                }
                coefficientsIn[j] = T / M[j, j];
            }
            finished = true;
        }

        private void BuildMatrix(long O)
        {
            long i = 0;
            long k = 0;
            long O1 = 0;
            O1 = O + 1;
            for (i = 0; i <= O; i++)
            {
                for (k = 0; k <= O; k++)
                {
                    M[i, k] = sumOfX[i + k];
                }
                M[i, O1] = sumOfYandX[i];
            }
        }

        private void FinalizeMatrix(long O)
        {
            long i = 0;
            long O1 = 0;
            O1 = O + 1;
            for (i = 0; i <= O; i++)
            {
                M[i, O1] = sumOfYandX[i];
            }
        }

        private void Solve()
        {
            long O = 0;
            O = degreeOfExpectedPolynomial;
            if (XYCount() <= O)
                O = XYCount() - 1;
            if (O >= 0)
            {
   
                BuildMatrix(O);
                GaussSolve(O);

                while ((1 < O))
                {
                    coefficientsIn[0] = 0;
                    O = O - 1;
                    FinalizeMatrix(O);
                }

            }

        }

        private void Class_Initialize()
        {
            Init();
            degreeOfExpectedPolynomial = 2;
        }

        public void Init()
        {
            long i = 0;
            finished = false;
            for (i = 0; i <= maxO; i++)
            {
                sumOfX[i] = 0;
                sumOfX[i + maxO] = 0;
                sumOfYandX[i] = 0;
                coefficientsIn[i] = 0;
            }
        }

        public double Coefficient(long Exponent)
        {
            long Ex = 0;
            long O = 0;
            if (!finished)
                Solve();
            Ex = System.Math.Abs(Exponent);
            O = degreeOfExpectedPolynomial;
            if (XYCount() <= O)
            {
                O = XYCount() - 1;
            }
            if (O < Ex)
            {
                return 0;
            }
            else
            {
                return coefficientsIn[Ex];
            }
        }

        public long DegreeOfExpectedPoly
        {
            get { return degreeOfExpectedPolynomial; }
            set { degreeOfExpectedPolynomial = value; }
        }

        public long XYCount()
        {
            return Convert.ToInt64(sumOfX[0]);
        }

        public void XYAdd(double NewX, double NewY)
        {
            long i = 0;
            double TX = 0;
            long Max2O = 0;
            finished = false;
            Max2O = 2 * degreeOfExpectedPolynomial;
            TX = 1.0;
            sumOfX[0] = sumOfX[0] + 1;
            sumOfYandX[0] = sumOfYandX[0] + NewY;
            for (i = 1; i <= degreeOfExpectedPolynomial; i++)
            {
                TX = TX * NewX;
                sumOfX[i] = sumOfX[i] + TX;
                sumOfYandX[i] = sumOfYandX[i] + NewY * TX;
            }
            for (i = degreeOfExpectedPolynomial + 1; i <= Max2O; i++)
            {
                TX = TX * NewX;
                sumOfX[i] = sumOfX[i] + TX;
            }
        }

        public double RegVal(double X)
        {
            long i = 0;
            long O = 0;
            double result = 0;
            if (!finished)
            {
                Solve();
            }
            result = 0;
            O = degreeOfExpectedPolynomial;
            if (XYCount() <= O)
            {
                O = XYCount() - 1;
            }
            for (i = 0; i <= O; i++)
            {
                result = result + coefficientsIn[i] * Math.Pow(X, i);
            }
            return result;
        }
    }


}
