using System;
using System.Numerics;


namespace LinearTransformationVisualizer.Mathematics
{
    public class Vector2
    {
        private double x;
        private double y;

        public Vector2(double x,double y)
        {
            this.x = x;
            this.y = y;
        }

        public double X
        {
            get{return this.x;}
        }

        public double Y
        {
            get{return this.y;}
        }

        public static double DotProductVec2(Vector2 v1, Vector2 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        //Computes the Euclidean norm of a vector
        public double Norm()
        {
            
            double sqX = x*x;
            double sqY = y*y;
            return Math.Sqrt(sqX + sqY);
        }

        //Returns a normalized form of calling vector
        public Vector2 Normalize()
        {
            
            double norm = this.Norm();
            return new Vector2(x/norm,y/norm);
        }

        public void ScalarMultiply(double scalar)
        {
            x *= scalar;
            y *= scalar;
        }

        public Vector2 ScalarMultiplyReturnVector(double scalar)
        {
            return new Vector2(x * scalar,y * scalar);
        }

        public static Vector2 Vector2Add(Vector2 v, Vector2 u)
        {
            return new Vector2(v.X+u.X,v.Y+u.Y);
        }

        public static Vector2 Vector2Subtract(Vector2 v, Vector2 u)
        {
            return new Vector2(v.X - u.X, v.Y - u.Y);
        }
        public static bool IsZeroVector(Vector2 v)
        {
            return v.X == 0 && v.Y == 0; 
        }

        public void AddToVector(double valueToAdd)
        {
            this.x += valueToAdd;
            this.y += valueToAdd;
        }

        
        public void AddToVectorOtherVector(Vector2 v)
        {
            this.x += v.x;
            this.y += v.y;
        }

        public void SubtractFromVector(double valueToSubtract)
        {
            this.x -= valueToSubtract;
            this.y -= valueToSubtract;
        }
        
        public void SubtractFromVectorOtherVector(Vector2 v)
        {
            this.x -= v.x;
            this.y -= v.y;
        }

        public static bool SameVectors(Vector2 v, Vector2 w)
        {
            return (v.X == w.X )&&(v.Y == w.Y);
        }
        
        public static Vector2 Interpolate(Vector2 v, Vector2 u, double t)
        {
            return new Vector2((1-t)*v.X + t * u.X,(1-t)*v.Y + t * u.Y);
        }

    }

    
}