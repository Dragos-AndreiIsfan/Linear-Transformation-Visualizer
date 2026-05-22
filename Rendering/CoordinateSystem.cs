using System;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Data;
using LinearTransformationVisualizer.Mathematics;
using SkiaSharp;

namespace LinearTransformationVisualizer.CoordinateSystem
{
    public class CoordinateScreenSystem
    {
        private float Width;
        private float Height;

        public float Scale{get; set;} = 1f; //by default 1.0


        public float GWidth{get {return Width;}}
        public float GHeight{get {return Height;}}

        public CoordinateScreenSystem(float w,float h)
        {
            Width = w;
            Height = h;
           
        }
        public CoordinateScreenSystem(float w,float h,float scale)
        {
            Width = w;
            Height = h;
            Scale = scale;
        }

        public SKPoint WorldToScreen(Vector2 v)
        {   

            float x = Width/2 + Scale * (float)v.X;
            float y = Height/2 - Scale * (float)v.Y;
            //use height/2 - vector.y * scale because on the screen things move down
            //while mathematically they move up

            return new SKPoint(x,y);
        }

        public SKPoint WorldToScreen(float vx, float vy)
        {
            
            float x = Width/2 + Scale * vx;
            float y = Height/2 - Scale * vy;

            return new SKPoint(x,y);    
        }

    }
}