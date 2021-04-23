using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace lab4
{
    public class FastImageEditor : IImageEditor
    {
        public Bitmap ChangeSaturation(Bitmap bmp, int saturation)
        {
            if (saturation < 0 || saturation > 200)
            {
                Console.Error.WriteLine($"Saturation level should from 0 to 200, but have `{saturation}`");
                Environment.Exit(1);
            }

            float s = 0;
            if (saturation != 100 && saturation != 200)
            {
                s = saturation % 100f;
                if (saturation <= 100)
                {

                }
                else
                {
                    s += 100;
                }
            }
            else
            {
                s = saturation;
            }
            s = s * 0.5f / 100f;

            Bitmap newBitmap = new Bitmap(bmp.Width, bmp.Height);
            Graphics g = Graphics.FromImage(newBitmap);
            float[][] matrix = CreateSaturationMatrix(s);
            ColorMatrix colorMatrix = new ColorMatrix(matrix);
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(colorMatrix);
            g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
            attributes.Dispose();
            g.Dispose();
            return newBitmap;
        }

        public Bitmap Crop(Bitmap bmp, int width, int height, int left, int top)
        {
            int x = width - left;
            int y = height - top;
            int rectWidth = left;
            int rectHeight = top;
            Rectangle rectangle = new Rectangle(x, y, rectWidth, rectHeight);
            IsCorrectCropRectangle(bmp, rectangle);
            Bitmap result = new Bitmap(rectangle.Width, rectangle.Height);
            Graphics graphics = Graphics.FromImage(result);
            graphics.DrawImage(bmp, 0, 0, rectangle, GraphicsUnit.Pixel);
            return result;
        }

        public Bitmap ExtractGreen(Bitmap bmp)
        {
            Bitmap result = new Bitmap(bmp.Width, bmp.Height);
            Graphics graphics = Graphics.FromImage(result);
            ImageAttributes attributes = new ImageAttributes();

            float[][] colorMatrixElements = {
                    new float[] {0,  0,  0,  0, 0},
                    new float[] {0,  1,  0,  0, 0},
                    new float[] {0,  0,  0,  0, 0},
                    new float[] {0,  0,  0,  1, 0},
                    new float[] {.2f, .2f, .2f, 0, 1}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
            attributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
            attributes.Dispose();
            graphics.Dispose();
            return result;
        }

        public Bitmap RotateLeft90(Bitmap bmp)
        {
            bmp.RotateFlip(RotateFlipType.Rotate90FlipX);
            return bmp;
        }

        public Bitmap Sepia(Bitmap bmp)
        {
            throw new System.NotImplementedException();
        }

        private static float[][] CreateSaturationMatrix(float saturation)
        {
            const float lumR = 0.3086f;
            const float lumG = 0.6094f;
            const float lumB = 0.0820f;
            float satCompl = 1.0f - saturation;
            float satComplR = lumR * satCompl;
            float satComplG = lumG * satCompl;
            float satComplB = lumB * satCompl;
            
            return new float[][]
            {
                new float[]{satComplR + saturation, satComplR,  satComplR,  0.0f, 0.0f,},
                new float[]{satComplG,  satComplG + saturation, satComplG,  0.0f, 0.0f,},
                new float[]{satComplB,  satComplB,  satComplB + saturation, 0.0f, 0.0f,},
                new float[]{0.0f,   0.0f,   0.0f,   1.0f,   0.0f,},
                new float[]{0.0f,   0.0f,   0.0f,   0.0f,   1.0f,},
            };
        }

        private void IsCorrectCropRectangle(Bitmap bmp, Rectangle rect)
        {
            if (rect.Left < 0 || rect.Left >= bmp.Width)
            {
                throw new Exception($"Invalid left: {rect.Left}");
            }
            else if (rect.Right >= bmp.Width)
            {
                throw new Exception($"Invalid right: {rect.Right}");

            }
            else if (rect.Top < 0 || rect.Top >= bmp.Height)
            {
                throw new Exception($"Invalid top : {rect.Top}");

            }
            else if (rect.Bottom >= bmp.Height)
            {
                throw new Exception($"Invalid bottom :{rect.Bottom}");

            }
        }
    }
}
