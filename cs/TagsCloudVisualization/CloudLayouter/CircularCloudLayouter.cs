using System.Drawing;
using TagsCloudVisualization.PointsGenerator;

namespace TagsCloudVisualization.CloudLayouter;

public class CircularCloudLayouter : ICircularCloudLayouter
{
    private Point center;
    private List<Rectangle> rectangles = [];
    private SpiralPointsGenerator spiral;

    public CircularCloudLayouter(Point center, double step, double angleOffset)
    {
        this.center = center;
        spiral = new SpiralPointsGenerator(center, step, angleOffset);
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException($"{nameof(rectangleSize)} height and width must be greater than zero");
        var rectangle = GetNextRectangle(rectangleSize);
        while (rectangles.Any(rectangle.IntersectsWith)) //можно использовать do while
            rectangle = GetNextRectangle(rectangleSize);
        rectangles.Add(rectangle);
        return rectangle;
    }

    private Rectangle GetNextRectangle(Size rectagleSize)
    {
        var rectanglePosition = spiral.GetNextPointPosition();
        var centerOfRectangle = CreateRectangleWithCenter(rectanglePosition, rectagleSize);
        return centerOfRectangle;
    }

    private Rectangle CreateRectangleWithCenter(Point center, Size rectangleSize)
    {
        var x = center.X - rectangleSize.Width / 2;
        var y = center.Y - rectangleSize.Height / 2;
        return new Rectangle(x, y, rectangleSize.Width, rectangleSize.Height);
    }

    public List<Rectangle> GetRectangles() => rectangles;
}