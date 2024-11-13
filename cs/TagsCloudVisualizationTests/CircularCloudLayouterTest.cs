using NUnit.Framework.Interfaces;
using FluentAssertions;
using System.Drawing;
using TagsCloudVisualization.CloudLayouter;
using TagsCloudVisualization.Visualizers;
using TagsCloudVisualizationTests;

namespace TagsCloudVisualization;

[TestFixture]
public class CircularCloudLayouterTests
{
    private List<Rectangle> rectanglesInTest;

    [TearDown]
    public void TearDown()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
            return;
        var directory = "FailedVisualisations";
        var path = Path.Combine(directory, $"{TestContext.CurrentContext.Test.Name}_visualisation.png");
        var visuliser = new SimpleCloudVisualizer();
        visuliser.CreateBitmap(rectanglesInTest, new(Constans.ImageWidth, Constans.ImageHeight), directory, path);
        Console.WriteLine($"Tag cloud visualization saved to file {path}");
    }

    [TestCase(0, 1, TestName = "WhenWidthIsZero")]
    [TestCase(1, 0, TestName = "WhenHeightIsZero")]
    [TestCase(-1, 1, TestName = "WhenWidthIsNegative")]
    [TestCase(1, -1, TestName = "WhenHeightIsNegative")]
    public void Layouter_ShouldThrowArgumentExceptionn(int width, int height)
    {
        var layouter = new CircularCloudLayouter(new(0, 0), 0.1, 0.1);
        var size = new Size(width, height);

        Action action = () => layouter.PutNextRectangle(size);

        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void FirstRectang_ShouldBeInCenter()
    {
        var layouter = new CircularCloudLayouter(new(0, 0), 0.1, 0.1);
        var rectangleSize = new Size(10, 10);

        var actualRectangle = layouter.PutNextRectangle(rectangleSize);
        var expectedRectangle = new Rectangle(
            -rectangleSize.Width / 2,
            -rectangleSize.Height / 2,
            rectangleSize.Width,
            rectangleSize.Height
        );
        rectanglesInTest = layouter.GetRectangles();

        actualRectangle.Should().BeEquivalentTo(expectedRectangle);
    }

    [Test]
    [Repeat(10)]
    public void Rectangles_ShouldNotHaveIntersects()
    {
        var layouter = CloudGenerator.GenerateCloud(100);
        rectanglesInTest = layouter.GetRectangles();

        var expectedResult = AreRectanglesHaveIntersects(rectanglesInTest);

        expectedResult.Should().BeFalse();
    }

    [Test]
    [Repeat(10)]
    public void AllRectanglesCenter_ShoulBeLikeInitCenter()
    {
        var center = new Point(Constans.ImageWidth / 2, Constans.ImageHeight / 2);
        var treshold = Constans.MaxRectangleSize / 2;
        var layouter = CloudGenerator.GenerateCloud(100);
        rectanglesInTest = layouter.GetRectangles();

        var actualCenter = GetCenterOfAllRectangles(rectanglesInTest);

        actualCenter.X.Should().BeInRange(center.X - treshold, center.X + treshold);
        actualCenter.Y.Should().BeInRange(center.Y - treshold, center.Y + treshold);
    }

    [Test]
    [Repeat(10)]
    public void RectanglesDensity_ShouldBeMax()
    {
        var expectedDensity = 0.5;
        var center = new Point(Constans.ImageWidth / 2, Constans.ImageHeight / 2);
        var layouter = CloudGenerator.GenerateCloud(100);
        rectanglesInTest = layouter.GetRectangles();

        var rectanglesArea = rectanglesInTest.Sum(rect => rect.Width * rect.Height);
        var radius = GetMaxDistanceBetweenRectangleAndCenter(rectanglesInTest);
        var circleArea = Math.PI * radius * radius;
        var density = rectanglesArea / circleArea;

        density.Should().BeGreaterThanOrEqualTo(expectedDensity);
    }

    private Point GetCenterOfAllRectangles(List<Rectangle> rectangles)
    {
        var top = rectangles.Max(r => r.Top);
        var right = rectangles.Max(r => r.Right);
        var bottom = rectangles.Min(r => r.Bottom);
        var left = rectangles.Min(r => r.Left);
        var x = left + (right - left) / 2;
        var y = bottom + (top - bottom) / 2;
        return new(x, y);
    }

    private double GetMaxDistanceBetweenRectangleAndCenter(List<Rectangle> rectangles)
    {
        var center = GetCenterOfAllRectangles(rectangles);
        double maxDistance = -1;
        foreach (var rectangle in rectangles)
        {
            var corners = new Point[4]
            {
                new(rectangle.Top, rectangle.Left),
                new(rectangle.Bottom, rectangle.Left),
                new(rectangle.Top, rectangle.Right),
                new(rectangle.Bottom, rectangle.Right)
            };
            var distance = corners.Max(p => GetDistanceBetweenPoints(p, center));
            maxDistance = Math.Max(maxDistance, distance);
        }
        return maxDistance;
    }

    

    private bool AreRectanglesHaveIntersects(List<Rectangle> rectangles)
    {
        for (var i = 0; i < rectangles.Count; i++)
            for (var j = i + 1; j < rectangles.Count; j++)
                if (rectangles[i].IntersectsWith(rectangles[j]))
                    return true;
        return false;
    }

    private double GetDistanceBetweenPoints(Point point1, Point point2)
        => Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
}