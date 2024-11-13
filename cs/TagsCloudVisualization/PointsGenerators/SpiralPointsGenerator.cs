using System.Drawing;

namespace TagsCloudVisualization.PointsGenerators;

public class SpiralPointsGenerator
{
    private double angleOffset;
    private readonly Point center;
    private readonly double step;
    private double angle = 0;

    public SpiralPointsGenerator(Point center, double step, double angleOffset)
    {
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