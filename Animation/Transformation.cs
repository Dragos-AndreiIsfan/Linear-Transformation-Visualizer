using System;
using System.Drawing;
using LinearTransformationVisualizer;
using LinearTransformationVisualizer.Mathematics;
using LinearTransformationVisualizer.Rendering;
using SkiaSharp;
using SkiaSharp.Views.WPF;

namespace LinearTransformationVisualizer.Animation
{
    class Transformation
    {
        private static double tVec = 0.0;
        private static double tMat = 0.0;

        public static double AnimateTransformForward(Render r,SKCanvas canvas, Vector2 v_initial, Vector2 v_final, SKPaint vColor, string name)
        {   
            //if t == 1, stop calculating the interpolation vector
            //draw the final vector immediately
            if(tVec == 1)
            {
                r.DrawVector(canvas,v_final,vColor,name);
                return tVec;
            }
           //this function uses interpolation of the form
           // v_intermediate = (1-t)*v_start + t*v_end
           //this way it draws and draws and draws, incrementing 1 until t=1.
            Vector2 v_intermediate = Vector2.Vector2Add(v_initial.ScalarMultiplyReturnVector(1-tVec),v_final.ScalarMultiplyReturnVector(tVec));
            r.DrawVector(canvas,v_intermediate,vColor,name);
            if(tVec < 1.0){

                tVec += 0.01;

            }
            return tVec;
        }

        public static double AnimateTransformBackwards(Render r,SKCanvas canvas, Vector2 v_initial, Vector2 v_final, SKPaint vColor, string name)
        {   
            //if t == 1, stop calculating the interpolation vector
            //draw the final vector immediately
            //this function will draw backwards
            //i.e., what we do is simply draw inversely
            //what we do is simple
            //we animate backwards
            if(tVec == 1)
            {
                r.DrawVector(canvas,v_final,vColor,name);
                return tVec;
            }
           //this function uses interpolation of the form
           // v_intermediate = (1-t)*v_start + t*v_end
           //this way it draws and draws and draws, decrementing 1 until t=0.
            Vector2 v_intermediate = Vector2.Vector2Add(v_initial.ScalarMultiplyReturnVector(1-tVec),v_final.ScalarMultiplyReturnVector(tVec));
            r.DrawVector(canvas,v_intermediate,vColor,name);
            if(tVec > 0.0){

                tVec -= 0.01;

            }
            return tVec;
        }

        public static void ResetT(){tVec = 0.0;}

        public static double TransformSpaceForward(SKElement skel,
            Render r, CoordinateSystem.CoordinateScreenSystem coords, 
            SKCanvas canvas,SKPaint axisColors, SKPaint gridLinesColors,
            Matrix2 A)
        {

            float startX = r.XAxisLowerBound; //take the left-most part of x axis
            float endX =   r.XAxisUpperBound; //take the right-most part of x axis
            float startY = r.YAxisLowerBound; //take the down-most part of y axis
            float endY =   r.YAxisUpperBound; //take the upper-most part of y axis

            SKPaint ihatColor = new SKPaint(){
                Color = SKColors.DarkRed,
                StrokeWidth = 5,
                IsAntialias = true
            };

            SKPaint jhatColor = new SKPaint()
            {
                Color = SKColors.ForestGreen,
                StrokeWidth = 5,
                IsAntialias = true
            };

            //Algorithm -> take p1 (x,start), p2(x,end)
            //p1' = Ap1
            //p2' = Ap2
            //drawLine(p1',p2')
            SKPoint p1,p2;
            Vector2 ihat = new Vector2(1.0,0.0);
            Vector2 jhat = new Vector2(0.0,1.0);
            for(float x = (float)Math.Floor(r.XAxisLowerBound); x <= (float)Math.Floor(r.XAxisUpperBound); x++)
            {   
                Vector2 v1 = new Vector2(x,startY);
                Vector2 v2 = new Vector2(x,endY);
                Vector2 v1Prime = A.Mat2Vec2Mult(v1);
                Vector2 v2Prime = A.Mat2Vec2Mult(v2);
                Vector2 Aihat   = A.Mat2Vec2Mult(ihat);
                if(tMat < 1.0)
                {
                    Vector2 v1Int = Vector2.Interpolate(v1,v1Prime,tMat);
                    Vector2 v2Int = Vector2.Interpolate(v2,v2Prime,tMat);
                    Vector2 ihatInt = Vector2.Interpolate(ihat,Aihat,tMat);
                    p1 = coords.WorldToScreen(v1Int);
                    p2 = coords.WorldToScreen(v2Int);
                    r.DrawVector(canvas,ihatInt,ihatColor,@"A\hat{\imath}");    
                    
                }
                else
                {   
                    p1 = coords.WorldToScreen(v1Prime);
                    p2 = coords.WorldToScreen(v2Prime);  
                    r.DrawVector(canvas,Aihat,ihatColor,@"A\hat{\imath}");  
                }
                
                if(x == 0)
                {
                    canvas.DrawLine(p1,p2,axisColors);
                }
                canvas.DrawLine(p1,p2,gridLinesColors);
                
            }

            for(float y = (float)Math.Floor(r.XAxisLowerBound); y <= (float)Math.Floor(r.XAxisUpperBound); y++)
            {   
                Vector2 v1 = new Vector2(startX,y);
                Vector2 v2 = new Vector2(endX,y);
                Vector2 v1Prime = A.Mat2Vec2Mult(v1);
                Vector2 v2Prime = A.Mat2Vec2Mult(v2);
                Vector2 Ajhat   = A.Mat2Vec2Mult(jhat);
                if(tMat < 1.0)
                {
                    Vector2 jhatInt = Vector2.Interpolate(jhat,Ajhat,tMat);
                    Vector2 v1Int = Vector2.Interpolate(v1,v1Prime,tMat);
                    Vector2 v2Int = Vector2.Interpolate(v2,v2Prime,tMat);

                    p1 = coords.WorldToScreen(v1Int);
                    p2 = coords.WorldToScreen(v2Int);
                    r.DrawVector(canvas,jhatInt,jhatColor,@"A\hat{\jmath}");
                }
                else
                {
                    p1 = coords.WorldToScreen(v1Prime);
                    p2 = coords.WorldToScreen(v2Prime);    
                    r.DrawVector(canvas,Ajhat,jhatColor,@"A\hat{\jmath}");
                }
                
                if(y == 0)
                {
                    canvas.DrawLine(p1,p2,axisColors);
                }
                canvas.DrawLine(p1,p2,gridLinesColors);
            }
            if(tMat < 1.0)
            {
                tMat += 0.01;
            }
            return tMat;
        }
        public static double TransformSpaceBackward(SKFont font,
            Render r, CoordinateSystem.CoordinateScreenSystem coords, 
            SKCanvas canvas,SKPaint axisColors, SKPaint gridLinesColors,
            Matrix2 A)
        {

            float startX = r.XAxisLowerBound; //take the left-most part of x axis
            float endX =   r.XAxisUpperBound; //take the right-most part of x axis
            float startY = r.YAxisLowerBound; //take the down-most part of y axis
            float endY =   r.YAxisUpperBound; //take the upper-most part of y axis

            //Algorithm -> take p1 (x,start), p2(x,end)
            //p1' = Ap1
            //p2' = Ap2
            //drawLine(p1',p2')
            SKPoint p1,p2;
            for(float x = (float)Math.Floor(r.XAxisLowerBound); x <= (float)Math.Floor(r.XAxisUpperBound); x++)
            {   
                Vector2 v1 = new Vector2(x,startY);
                Vector2 v2 = new Vector2(x,endY);
                Vector2 v1Prime = A.Mat2Vec2Mult(v1);
                Vector2 v2Prime = A.Mat2Vec2Mult(v2);
                if(tMat < 1.0)
                {
                    Vector2 v1Int = Vector2.Interpolate(v1,v1Prime,tMat);
                    Vector2 v2Int = Vector2.Interpolate(v2,v2Prime,tMat);
                    
                    p1 = coords.WorldToScreen(v1Int);
                    p2 = coords.WorldToScreen(v2Int);
                }
                else
                {
                    p1 = coords.WorldToScreen(v1Prime);
                    p2 = coords.WorldToScreen(v2Prime);    
                }
                
                if(x == 0)
                {
                    canvas.DrawLine(p1,p2,axisColors);
                }
                canvas.DrawLine(p1,p2,gridLinesColors);
            }

            for(float y = (float)Math.Floor(r.XAxisLowerBound); y <= (float)Math.Floor(r.XAxisUpperBound); y++)
            {   
                Vector2 v1 = new Vector2(startX,y);
                Vector2 v2 = new Vector2(endX,y);
                Vector2 v1Prime = A.Mat2Vec2Mult(v1);
                Vector2 v2Prime = A.Mat2Vec2Mult(v2);
                if(tMat < 1.0)
                {
                    Vector2 v1Int = Vector2.Interpolate(v1,v1Prime,tMat);
                    Vector2 v2Int = Vector2.Interpolate(v2,v2Prime,tMat);
                    p1 = coords.WorldToScreen(v1Int);
                    p2 = coords.WorldToScreen(v2Int);
                }
                else
                {
                    p1 = coords.WorldToScreen(v1Prime);
                    p2 = coords.WorldToScreen(v2Prime);    
                }
                
                if(y == 0)
                {
                    canvas.DrawLine(p1,p2,axisColors);
                }
                canvas.DrawLine(p1,p2,gridLinesColors);
            }
            if(tMat > 0.0)
            {
                tMat -= 0.01;
            }

            

            float epsilon = 0.5f;
            float xx = r.XAxisUpperBound - epsilon;
            //y axis has a smaller interval then x axis when drawn, therefore
            //a small amount epsilon will be removed
            epsilon = 0.3f;
            float yx = r.YAxisUpperBound - epsilon;
            
            SKPoint fontXPosition = coords.WorldToScreen(xx,0.2f);
            SKPoint fontYPosition = coords.WorldToScreen(0.1f,yx);

            //canvas.DrawLine(xLeft,xRight,axisColors);
            //canvas.DrawLine(yLeft,yRight,axisColors);
            canvas.DrawText("x",fontXPosition,font,axisColors);
            canvas.DrawText("y",fontYPosition,font,axisColors);

            return tMat;
        }
    }
}