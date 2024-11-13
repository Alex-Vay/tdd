using System.Drawing;
using TagsCloudVisualization.Visualizers;
using TagsCloudVisualizationTests;

namespace TagsCloudVisualization;

public static class Program
{
    public static void Main()
    {
        var cloudLayouter = CloudGenerator.GenerateCloud();
        var rectangles = cloudLayouter.GetRectangles();
        var visualizer = new SimpleCloudVisualizer();
        visualizer.CreateBitmap(rectangles, new Size(Constans.ImageWidth, Constans.ImageHeight));
    }
}