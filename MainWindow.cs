using System.Windows;
using SkiaSharp;
using SkiaSharp.Views.WPF;
using SkiaSharp.Views.Desktop;
using System.Windows.Media; 
using LinearTransformationVisualizer;
using LinearTransformationVisualizer.Rendering;
using LinearTransformationVisualizer.CoordinateSystem;
using LinearTransformationVisualizer.Mathematics;
using LinearTransformationVisualizer.Animation;
using System.Windows.Input;
using System.Windows.Controls;
using CSharpMath.Atom.Atoms;
using CSharpMath.SkiaSharp;
using CSharpMath.Rendering.FrontEnd;
using OpenTK.Graphics.ES20;

namespace LinearTransformationVisualizer{

    public class MainWindow : Window
    {
        private SKElement skElement;
        bool pressedD = false;
        bool pressedT = false;
        bool pressedS = false;

        bool start = false;
        bool transformVector = false;
        private void MouseFocus(object sender, MouseEventArgs e)
        {
            skElement.Focus();
        }
        private void HandleDKeyPress(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.D)
            {
                pressedD = !pressedD;
                skElement.InvalidateVisual();
            }
        }

        private void HandleTKeyPress(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.T)
            {
                pressedT = !pressedT;
                transformVector = true;
                skElement.InvalidateVisual();
            }
        }
        
        private void HandleSKeyPress(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.S)
            {
                pressedS = !pressedS;
                start = true;
                skElement.InvalidateVisual();
            }
        }

        public MainWindow(){
            Title = "LinearTransformationVisualizer";
            Width = 800;
            Height = 600;

            skElement = new SKElement();
            Content = skElement;
            
            

            this.KeyDown += HandleDKeyPress; //D -> Draw Baxis Vector Names
            this.KeyDown += HandleTKeyPress; //T -> Apply Linear Transformation
            this.KeyDown += HandleSKeyPress; //S -> Transforms ALL of Space
            
            skElement.PaintSurface += OnPaintSurface;

            CompositionTarget.Rendering += (s, e) =>
            {
                skElement.InvalidateVisual();
            };

        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.Black);
            var paintAxis = new SKPaint(){
                Color = SKColors.White,
                StrokeWidth = 2,
                IsAntialias = true
            };
            var paintGrid = new SKPaint()
            {
                Color = SKColors.GhostWhite,
                StrokeWidth = 0.5f,
                IsAntialias = true
            };
            int w = e.Info.Width;
            int h = e.Info.Height;
            float AspectRatio = ((float)w)/h;
            float WorldDown = -4f;
            float WorldUp =    4f;
            float WorldHeight = WorldUp - WorldDown;
            float WorldWidth = WorldHeight * AspectRatio;
            float WorldLeft = -WorldWidth/2;
            float WorldRight = WorldWidth/2;
            
            float scale = Math.Min(
                (float)w/WorldWidth,(float)h/WorldHeight
            );
            SKTypeface typeface = SKTypeface.FromFile("Assets/latin-modern-roman.mroman10-italic.otf");
            SKFont font = new SKFont(typeface,40);
            CoordinateScreenSystem screen = new CoordinateScreenSystem((float)w,(float)h,scale);
            Render r = new Render(screen,WorldLeft,WorldRight,WorldDown,WorldUp);
            //canvas.DrawLine(0,h/2,w,h/2,paintAxis); //X axis
            //canvas.DrawLine(w/2,0,w/2,h,paintAxis); //Y axis


            Matrix2 M = new Matrix2(1f,-2f,1f,4f);

            if (pressedS == true && start == true)
            {   
                double t = Transformation.TransformSpaceForward(skElement,r,screen,canvas,paintAxis,paintGrid,M);
                if(t < 1.0)
                {
                    skElement.InvalidateVisual();
                }
            }
            else if(pressedS == false && start == true){
                double t = Transformation.TransformSpaceBackward(font,r,screen,canvas,paintAxis,paintGrid,M);
                if(t > 0.0)
                {
                    skElement.InvalidateVisual();
                }
                /*r.DrawAxes(canvas,paintAxis,font);
                r.DrawGrid(canvas,paintGrid);
                r.DrawBasisVectors(canvas,pressedD,font);*/
            }
            else if(start == false)
            {
                r.DrawAxes(canvas,paintAxis,font);
                r.DrawGrid(canvas,paintGrid);
                r.DrawBasisVectors(canvas,pressedD,font);    
            }
            /*var eq = new MathPainter
            {
                LaTeX = @"A\vec{v} = \lambda \vec{v} \to p(\lambda) = \det(A - \lambda I)",
                FontSize = 24,
                TextColor = SKColors.SkyBlue
            };
            eq.Draw(canvas,new SKPoint(w/2+100f,h/2+100f));*/
            //the code above draws LaTeX equationd directly onto the SkiaSharp canvas
            Vector2 v = new Vector2(1f,1f);
            
            Vector2 Mv = M.Mat2Vec2Mult(v);
            Vector2 intermediateMV = v;
            
            SKPaint vCol = new SKPaint()
            {
                Color = SKColors.LightGreen,
                StrokeWidth = 5,
                IsAntialias = true
            };
            /*SKPaint MvCol = new SKPaint()
            {
                Color = SKColors.Orange,
                StrokeWidth = 5,
                IsAntialias = true
            };*/
            
            if (pressedT == true && transformVector)
            {
                double t = Transformation.AnimateTransformForward(r,canvas,v,Mv,vCol,@"M\vec{v}");
                if(t < 1.0)
                {
                    skElement.InvalidateVisual();
                }
                
            }   
            else if(pressedT == false && transformVector)
            {   

                double t = Transformation.AnimateTransformBackwards(r,canvas,v,Mv,vCol,@"\vec{v}");
                if(t > 0.0)
                {
                    skElement.InvalidateVisual();
                }
                
            }
            else
            {
                r.DrawVector(canvas,v,vCol);
            }
            
        }
    }
}

