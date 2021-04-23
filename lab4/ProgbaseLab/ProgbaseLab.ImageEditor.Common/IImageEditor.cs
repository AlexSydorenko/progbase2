using System.Drawing;

namespace lab4
{
    public interface IImageEditor
    {
        Bitmap Crop(Bitmap bmp, int width, int height, int left, int top);

        Bitmap RotateLeft90(Bitmap bmp);
        
        Bitmap ExtractGreen(Bitmap bmp);

        Bitmap Sepia(Bitmap bmp);

        Bitmap ChangeSaturation(Bitmap bmp, int saturation);
    }
}
