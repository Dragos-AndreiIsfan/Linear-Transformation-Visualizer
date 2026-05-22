using System;
using SkiaSharp;
using LinearTransformationVisualizer.CoordinateSystem;
using LinearTransformationVisualizer.Mathematics;
using System.Windows.Controls;
using CSharpMath;
using CSharpMath.SkiaSharp;


namespace LinearTransformationVisualizer.Rendering
{
    public class Render
    {   
        readonly CoordinateScreenSystem CoordinateSystem;
        public float XAxisLowerBound = -10;
        public float XAxisUpperBound = 10;
        public float YAxisLowerBound = -10;
        public float YAxisUpperBound = 10;

        public Render(CoordinateScreenSystem CS)
        {
            CoordinateSystem = CS;
        }
        public Render(CoordinateScreenSystem CS, float WorldLeft=-10f, float WorldRight=10f, float WorldDown=-10f, float WorldUp = 10f)
        {
            CoordinateSystem = CS;
            XAxisLowerBound = WorldLeft;
            XAxisUpperBound = WorldRight;
            YAxisLowerBound = WorldDown;
            YAxisUpperBound = WorldUp;
        }

        public void DrawGrid(SKCanvas canvas, SKPaint paint)
        {
            //a grid is just a set of lines drawn from left to right and up to low
            //first of all,draw the grid lines passing through x
            float start = YAxisLowerBound;
            float end = YAxisUpperBound;
            for(float x = (float)Math.Floor(XAxisLowerBound); x <= (float)Math.Floor(XAxisUpperBound); x++)
            {   if(x == 0f) continue;
                //convert coordinates from the world to screen coordinates using WorldToScreen()
                SKPoint p1 = CoordinateSystem.WorldToScreen(x,start);
                SKPoint p2 = CoordinateSystem.WorldToScreen(x,end);
                canvas.DrawLine(p1,p2,paint);
            }

            start = XAxisLowerBound;
            end = XAxisUpperBound;

            for(float y = (float)Math.Floor(YAxisLowerBound); y <= (float)Math.Floor(YAxisUpperBound); y++)
            {
                if(y == 0f) continue;
                SKPoint p1 = CoordinateSystem.WorldToScreen(start,y);
                SKPoint p2 = CoordinateSystem.WorldToScreen(end,y);
                canvas.DrawLine(p1,p2,paint);
            }

        }

        public void DrawAxes(SKCanvas canvas, SKPaint axesPaint,SKFont font)
        {
            //We use this function to draw the axis, using the drawLine function
            //from SkiaSharp
            SKPoint xLeft = CoordinateSystem.WorldToScreen(XAxisLowerBound,0f);
            SKPoint xRight = CoordinateSystem.WorldToScreen(XAxisUpperBound,0f);
            SKPoint yLeft = CoordinateSystem.WorldToScreen(0f,YAxisLowerBound);
            SKPoint yRight = CoordinateSystem.WorldToScreen(0f,YAxisUpperBound);
            
            float epsilon = 0.5f;
            float x = XAxisUpperBound - epsilon;
            //y axis has a smaller interval then x axis when drawn, therefore
            //a small amount epsilon will be removed
            epsilon = 0.3f;
            float y = YAxisUpperBound - epsilon;
            
            SKPoint fontXPosition = CoordinateSystem.WorldToScreen(x,0.2f);
            SKPoint fontYPosition = CoordinateSystem.WorldToScreen(0.1f,y);

            canvas.DrawLine(xLeft,xRight,axesPaint);
            canvas.DrawLine(yLeft,yRight,axesPaint);
            canvas.DrawText("x",fontXPosition,font,axesPaint);
            canvas.DrawText("y",fontYPosition,font,axesPaint);
        }

        protected void DrawTip(SKCanvas canvas, Mathematics.Vector2 v,SKPaint vectorColor)
        {

            //this function draws the tip of a vector
            double baseSize = 1.2f;
            double scale = 0.2f * CoordinateSystem.Scale;
            double arrowLength = 2*baseSize/scale;
            double arrowWidth = 0.08f;

            Vector2 directionV = v.Normalize();
            Vector2 orthogonalToV = new Vector2(-directionV.Y,directionV.X);
            
            directionV.ScalarMultiply(arrowLength);
            orthogonalToV.ScalarMultiply(arrowWidth);

            Vector2 baseCenter = Vector2.Vector2Subtract(v,directionV);
            Vector2 left = Vector2.Vector2Add(baseCenter,orthogonalToV);
            Vector2 right = Vector2.Vector2Subtract(baseCenter,orthogonalToV);
            
            var vertex1 = CoordinateSystem.WorldToScreen(v);
            var vertex2 = CoordinateSystem.WorldToScreen(left);
            var vertex3 = CoordinateSystem.WorldToScreen(right);


            SKPaint fillPaint = new SKPaint()
            {
                Style = SKPaintStyle.Fill,
                Color = vectorColor.Color
            };

            SKPath path = new SKPath();

            path.MoveTo(vertex1);
            path.LineTo(vertex2);
            path.LineTo(vertex3);
            path.Close();
            
            canvas.DrawPath(path,fillPaint);
        }

        public void DrawVector(SKCanvas canvas, Vector2 v,SKPaint vectorColor)
        {
            //A vector is drawn from the origin, in world coordinates (0,0)
            SKPoint tail = CoordinateSystem.WorldToScreen(0f,0f);
            SKPoint tip = CoordinateSystem.WorldToScreen(v);
            canvas.DrawLine(tail,tip,vectorColor);
            DrawTip(canvas,v,vectorColor);
            
        }

        public void DrawVector(SKCanvas canvas, Vector2 v,SKPaint vectorColor, string LaTeXName)
        {
            //A vector is drawn from the origin, in world coordinates (0,0)
            SKPoint tail = CoordinateSystem.WorldToScreen(0f,0f);
            SKPoint tip = CoordinateSystem.WorldToScreen(v);
            canvas.DrawLine(tail,tip,vectorColor);
            DrawTip(canvas,v,vectorColor);
            DrawVectorName(canvas,tip,vectorColor,LaTeXName);
        }

        void DrawVectorName(SKCanvas canvas, SKPoint v, SKPaint vectorColor, string LaTeXName)
        {
            v.X -= 50f;
            v.Y -= 25f;

            var notation = new MathPainter
            {
                LaTeX = LaTeXName,
                FontSize = 24,
                TextColor = vectorColor.Color,
                AntiAlias = true
            };
            notation.Draw(canvas,v);
        }
        public void DrawBasisVectors(SKCanvas canvas,bool shouldDrawNames,SKFont font)
        {
            
            SKPaint ihatclr = new SKPaint()
            {   
                Color = SKColors.DarkRed,
                StrokeWidth = 5,
                IsAntialias = true
            };

            
            SKPaint jhatclr = new SKPaint()
            {   
                Color = SKColors.ForestGreen,
                StrokeWidth = 5,
                IsAntialias = true
            };

            Vector2 ihat = new Vector2(1.0,0.0);
            Vector2 jhat = new Vector2(0.0,1.0);

            DrawVector(canvas,ihat,ihatclr);
            DrawVector(canvas,jhat,jhatclr);

            if (shouldDrawNames)
            {
                DrawBasisVectorsNames(canvas,font,ihatclr,jhatclr);
            }
        }

        public void DrawBasisVectorsNames(SKCanvas canvas, SKFont font, SKPaint colorI,SKPaint colorJ)
        {
            //first draw i hat text then j hat
            float x = 0.85f;
            float y = -0.5f;
            SKPoint position = CoordinateSystem.WorldToScreen(x,y);
            canvas.DrawText("î",position,font,colorI);

            x = 0.3f;
            y = 0.90f;
            position = CoordinateSystem.WorldToScreen(x,y);
            canvas.DrawText("ĵ",position,font,colorJ);
        } 
    }
}