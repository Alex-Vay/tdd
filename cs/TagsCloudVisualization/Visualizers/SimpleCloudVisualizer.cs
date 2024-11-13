using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization.Visualizers;

public class SimpleCloudVisualizer
{
    public void CreateBitmap(
        IEnumerable<Rectangle> rectangles, 
        Size bitmapSize,
        string directory = Constans.ImagesDirectory,
        string currentPath = null
        )
    {
        var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height);
        var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.White);
        foreach (var rectangle in rectangles)
        {
            var pen = new Pen(GetRandomColor());
            graphics.DrawRectangle(pen, rectangle);
        }
        Directory.CreateDirectory(directory);
        var path = currentPath == null ? GetPathToImages() : currentPath;
        bitmap.Save(path, ImageFormat.Jpeg);
    }

    private Color GetRandomColor()
    {
        var random = new Random();
        return Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
    }

    private static string GetPathToImages()
    {
        var filename = $"{Constans.RectanglesNumber}_{Constans.LayoutStep}_{Constans.LayoutAngleOffset}_TagCloud.jpg";
        return Path.Combine(Constans.ImagesDirectory, filename);
    }
}