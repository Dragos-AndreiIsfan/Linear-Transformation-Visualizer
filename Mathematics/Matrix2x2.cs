using System;
using System.Drawing.Drawing2D;

namespace LinearTransformationVisualizer.Mathematics
{
    public class Matrix2
    {
        private double[,] MatrixValue;
        public Matrix2(double a, double b, double c, double d)
        {
            MatrixValue = new double[2,2];
            MatrixValue[0,0] = a;
            MatrixValue[0,1] = b;
            MatrixValue[1,0] = c;
            MatrixValue[1,1] = d;
        }
        
        public double[,] MatrixVal{get {return MatrixValue;}}
        public double Matrix2Determinant()
        {
            return MatrixValue[0,0] * MatrixValue[1,1] - 
                   MatrixValue[0,1] * MatrixValue[1,0];
        }

        public Vector2 Mat2Vec2Mult(Vector2 v)
        {
            double x = MatrixValue[0,0] * v.X + MatrixValue[0,1] * v.Y;
            double y = MatrixValue[1,0] * v.X + MatrixValue[1,1] * v.Y;
            return new Vector2(x,y);
        }

        public Matrix2 MatMul(Matrix2 M)
        {

            /*

               0 1      0 1    
            0 [a b]  0 [x y] = [(ax + by) (ay + bw)] = [X(0,0)Y(0,0) + X(0,1)Y(1,0) X(0,0)Y(0,1) + X(0,1)Y(1,1)]
            1 [c d]  1 [z w] = [(cx + dz) (cy + dw)] = [X(1,0)Y(0,0) + X(1,1)Y(1,0) X(1,0)Y(0,1) + X(1,1)Y(1,1)]
                ^        ^               ^
                |        |               |
                X        Y               Z

                
            */


            double[,] MVals = M.MatrixVal;
            double a = MatrixValue[0,0] * MVals[0,0] + 
                       MatrixValue[0,1] * MVals[1,0];

            double b = MatrixValue[0,0] * MVals[0,1] + 
                       MatrixValue[0,1] * MVals[1,1];

            double c = MatrixValue[1,0] * MVals[0,0] + 
                       MatrixValue[1,1] * MVals[1,0];

            double d = MatrixValue[1,0] * MVals[0,1] + 
                       MatrixValue[1,1] * MVals[1,1];
            
            return new Matrix2(a,b,c,d);
        }

    }
}