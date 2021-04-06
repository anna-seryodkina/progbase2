using System;
using System.Drawing;
using System.Drawing.Imaging;
using ProgbaseLab.ImageEditor.Common;

namespace ProgbaseLab.ImageEditor.Fast
{
    public class Fast : IImageEditor
    {
        public Bitmap Crop(Bitmap bmp, Rectangle rect)
        {
            Bitmap newBitmap = new Bitmap(rect.Width, rect.Height);
            Graphics g = Graphics.FromImage(newBitmap);
            g.DrawImage(bmp, 0, 0, rect, GraphicsUnit.Pixel);
            return newBitmap;
        }
        public Bitmap RotateRight90(Bitmap bmp)
        {
            bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            return bmp;
        }
        public Bitmap ExtractRed(Bitmap bmp)
        {
            Bitmap newBitmap = new Bitmap(bmp.Width, bmp.Height);

            Graphics g = Graphics.FromImage(newBitmap);

            ColorMatrix colorMatrix = new ColorMatrix(
                new float[][]
                {
                    new float[] {1,  0,  0,  0, 0},
                    new float[] {0,  0,  0,  0, 0},
                    new float[] {0,  0,  0,  0, 0},
                    new float[] {0,  0,  0,  1, 0},
                    new float[] {0,  0,  0,  0, 1}
                });
            ImageAttributes attributes = new ImageAttributes();
            
            attributes.SetColorMatrix(colorMatrix);
            g.DrawImage(
                bmp,
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
            
            attributes.Dispose();
            g.Dispose();
            return newBitmap;
        }
        public Bitmap Grayscale(Bitmap bmp)
        {
            Bitmap newBitmap = new Bitmap(bmp.Width, bmp.Height);

            Graphics g = Graphics.FromImage(newBitmap);

            ColorMatrix colorMatrix = new ColorMatrix(
                new float[][]
                {
                    new float[] {.3f, .3f, .3f, 0, 0},
                    new float[] {.59f, .59f, .59f, 0, 0},
                    new float[] {.11f, .11f, .11f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                });
            ImageAttributes attributes = new ImageAttributes();
            
            attributes.SetColorMatrix(colorMatrix);
            g.DrawImage(
                bmp,
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
            
            attributes.Dispose();
            g.Dispose();
            return newBitmap;
        }
        public Bitmap ChangeHue(Bitmap bmp, int hue)
        {
            Bitmap newBitmap = new Bitmap(bmp.Width, bmp.Height);
            Graphics g = Graphics.FromImage(newBitmap);
            ColorMatrix colorMatrix = new ColorMatrix(CreateHueMatrix(hue));
            ImageAttributes attributes = new ImageAttributes();
            
            attributes.SetColorMatrix(colorMatrix);
            g.DrawImage(
                bmp,
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
            
            attributes.Dispose();
            g.Dispose();
            return newBitmap;
        }
        private static float[][] CreateHueMatrix(float hueShiftDegrees)
        {
            float theta = (float)(hueShiftDegrees / 360 * 2 * Math.PI);
            float c = (float)Math.Cos(theta);
            float s = (float)Math.Sin(theta);
            float A00 = 0.213f + 0.787f * c - 0.213f * s;
            float A01 = 0.213f - 0.213f * c + 0.413f * s;
            float A02 = 0.213f - 0.213f * c - 0.787f * s;
            float A10 = 0.715f - 0.715f * c - 0.715f * s;
            float A11 = 0.715f + 0.285f * c + 0.140f * s;
            float A12 = 0.715f - 0.715f * c + 0.715f * s;
            float A20 = 0.072f - 0.072f * c + 0.928f * s;
            float A21 = 0.072f - 0.072f * c - 0.283f * s;
            float A22 = 0.072f + 0.928f * c + 0.072f * s;
            return new float[][] {
                new float[]{A00, A01, A02, 0, 0},
                new float[]{A10, A11, A12, 0, 0},
                new float[]{A20, A21, A22, 0, 0},
                new float[]{0, 0, 0, 1, 0},
                new float[]{0, 0, 0, 0, 1}
            };
        }

    }
}
