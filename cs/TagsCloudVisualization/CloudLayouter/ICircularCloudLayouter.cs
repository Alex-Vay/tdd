using System.Drawing;

namespace TagsCloudVisualization.CloudLayouter;

public interface ICircularCloudLayouter
{
    List<Rectangle> GetRectangles();
    Rectangle PutNextRectangle(Size rectangleSize);
}