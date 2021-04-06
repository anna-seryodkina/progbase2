using System;
using System.Drawing;

namespace ProgbaseLab.ImageEditor.Common
{
    public interface IImageEditor
    {
        Bitmap Crop(Bitmap bmp, Rectangle rect);
        Bitmap RotateRight90(Bitmap bmp);
        Bitmap ExtractRed(Bitmap bmp);
        Bitmap Grayscale(Bitmap bmp);
        Bitmap ChangeHue(Bitmap bmp, int hue);
    }
}
