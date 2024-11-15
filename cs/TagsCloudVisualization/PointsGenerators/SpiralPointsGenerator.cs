using System.Drawing;

namespace TagsCloudVisualization.PointsGenerators;

public class SpiralPointsGenerator : IPointsGenerator
{
    private readonly double step = 0.1;
    private readonly double angleOffset = 0.1;
    private readonly Point center;
    private double angle = 0;

    public SpiralPointsGenerator(Point center) => this.center = center;

    public SpiralPointsGenerator(Point center, double step, double angleOffset)
    {
        if (step == 0 || angleOffset == 0)
            throw new ArgumentException($"Step and angleOffset must not be zero");
        this.center = center;
        this.step = step;
        this.angleOffset = angleOffset;
    }

    public Point GetNextPointPosition()
    {
        var radius = step * angle;
        var x = (int)(center.X + radius * Math.Cos(angle));
        var y = (int)(center.Y + radius * Math.Sin(angle));
        angle += angleOffset;
        return new(x, y);
    }
}