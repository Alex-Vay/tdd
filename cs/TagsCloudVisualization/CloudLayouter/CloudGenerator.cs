using System.Drawing;
using TagsCloudVisualization;
using TagsCloudVisualization.CloudLayouter;

namespace TagsCloudVisualizationTests;

public static class CloudGenerator
{
    public static CircularCloudLayouter GenerateCloud(int rectanglesNumber = Constans.RectanglesNumber)
    {
        var center = new Point(Constans.ImageWidth / 2, Constans.ImageHeight / 2);
        var cloudLayouter = new CircularCloudLayouter(center, Constans.LayoutStep, Constans.LayoutAngleOffset);
        var random = new Random();
        var rectangles = new Rectangle[rectanglesNumber];
        rectangles = rectangles
            .Select(x => cloudLayouter.PutNextRectangle(new Size(
                random.Next(Constans.MinRectangleSize, Constans.MaxRectangleSize),
                random.Next(Constans.MinRectangleSize, Constans.MaxRectangleSize))))
            .ToArray();
        return cloudLayouter;
    }
}
