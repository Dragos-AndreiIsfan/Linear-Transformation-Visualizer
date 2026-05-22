using System;
using System.Windows;

namespace LinearTransformationVisualizer{

    public partial class App : Application{
        [System.STAThread]
        public static void Main(){
            var app = new App();
            var window = new MainWindow(){
                Title = "Linear Transformation Visualizer",
                Width = 800,
                Height = 600
            };

            app.Run(window);
        }
    }

}