using System;
using System.Drawing;

namespace lab4
{
    public class PixelImageEditor : IImageEditor
    {
        public Bitmap ChangeSaturation(Bitmap bmp, int saturation)
        {
            if (saturation < 0 || saturation > 200)
            {
                Console.Error.WriteLine($"Saturation level should from 0 to 200, but have `{saturation}`");
                Environment.Exit(1);
            }
            float factor = saturation / 100f;
            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color color = bmp.GetPixel(j, i);
                    int R = color.R;
                    int G = color.G;
                    int B = color.B;
                    double h = 0;
                    double l = 0;
                    double s = 0;
                    RgbToHls(R, G, B, out h, out l, out s);
                    s *= factor;
                    if (s > 1)
                    {
                        s = 1;
                    }
                    else if (s < -1)
                    {
                        s = 0;
                    }
                    HlsToRgb(h, l, s, out R, out G, out B);

                    Color newColor = Color.FromArgb(255, R, G, B);
                    bmp.SetPixel(j, i, newColor);
                }
            }
            return bmp;
        }

        public Bitmap Crop(Bitmap bmp, int width, int height, int left, int top)
        {
            // Console.WriteLine(width);
            // Console.WriteLine(height);
            // Console.WriteLine(left);
            // Console.WriteLine(top);
            if (left < 1 || top < 1 || width < 0 || height < 0)
            {
                Console.Error.WriteLine("Only positive integers in crop params!");
                Environment.Exit(1);
            }
            if (width > bmp.Width || height > bmp.Height)
            {
                Console.Error.WriteLine("Your starting crop point is outside the bounds of the image!");
                Environment.Exit(1);
            }
            if (left + (bmp.Width - width) > bmp.Width || top + (bmp.Height - height) > bmp.Height)
            {
                Console.Error.WriteLine("Part of the image you want to crop is outside the bounds of this image!");
                Environment.Exit(1);
            }

            Bitmap croppedBitmap = new Bitmap(width - (width - left), height - (height - top));
            // Console.WriteLine($"{croppedBitmap.Width}x{croppedBitmap.Height}");
            
            int y = croppedBitmap.Height - 1;
            for (int i = height - 1; i >= height - top + 1; i--)
            {
                int x = croppedBitmap.Width - 1;
                for (int j = width - 1; j >= width - left + 1; j--)
                {
                    Color color = bmp.GetPixel(j, i);
                    croppedBitmap.SetPixel(x, y, color);
                    x--;
                }
                y--;
            }
            return croppedBitmap;
        }

        public Bitmap ExtractGreen(Bitmap bmp)
        {
            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color color = bmp.GetPixel(j, i);
                    Color newColor = Color.FromArgb(255, 0, color.G, 0);
                    bmp.SetPixel(j, i, newColor);
                }
            }
            return bmp;
        }

        public Bitmap RotateLeft90(Bitmap bmp)
        {
            Bitmap bitmapCopy = new Bitmap(bmp.Height, bmp.Width);
            for (int i = 0; i < bmp.Height; i++)
            {
                int heightPixel = bitmapCopy.Height;
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color color = bmp.GetPixel(j, i);
                    bitmapCopy.SetPixel(i, heightPixel-1, color);
                    heightPixel--;
                }
            }
            return bitmapCopy;
        }

        public Bitmap Sepia(Bitmap bmp)
        {
            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color color = bmp.GetPixel(j, i);
                    int R = color.R;
                    int G = color.G;
                    int B = color.B;
                    int newRed = (int)Math.Min(255, 0.393*R + 0.769*G + 0.189*B);
                    int newGreen = (int)Math.Min(255, 0.349*R + 0.686*G + 0.168*B);
                    int newBlue = (int)Math.Min(255, 0.272*R + 0.534*G + 0.131*B);
                    
                    Color newColor = Color.FromArgb(255, newRed, newGreen, newBlue);
                    bmp.SetPixel(j, i, newColor);
                }
            }
            return bmp;
        }

        public static void RgbToHls(int r, int g, int b, out double h, out double l, out double s)
        {
            // Convert RGB to a 0.0 to 1.0 range.
            double double_r = r / 255.0;
            double double_g = g / 255.0;
            double double_b = b / 255.0;
        
            // Get the maximum and minimum RGB components.
            double max = double_r;
            if (max < double_g) max = double_g;
            if (max < double_b) max = double_b;
        
            double min = double_r;
            if (min > double_g) min = double_g;
            if (min > double_b) min = double_b;
        
            double diff = max - min;
            l = (max + min) / 2;
            if (Math.Abs(diff) < 0.00001)
            {
                s = 0;
                h = 0;  // H is really undefined.
            }
            else
            {
                if (l <= 0.5) s = diff / (max + min);
                else s = diff / (2 - max - min);
        
                double r_dist = (max - double_r) / diff;
                double g_dist = (max - double_g) / diff;
                double b_dist = (max - double_b) / diff;
        
                if (double_r == max) h = b_dist - g_dist;
                else if (double_g == max) h = 2 + r_dist - b_dist;
                else h = 4 + g_dist - r_dist;
        
                h = h * 60;
                if (h < 0) h += 360;
            }
        }

        // Convert an HLS value into an RGB value.
        public static void HlsToRgb(double h, double l, double s, out int r, out int g, out int b)
        {
            double p2;
            if (l <= 0.5) p2 = l * (1 + s);
            else p2 = l + s - l * s;
        
            double p1 = 2 * l - p2;
            double double_r, double_g, double_b;
            if (s == 0)
            {
                double_r = l;
                double_g = l;
                double_b = l;
            }
            else
            {
                double_r = QqhToRgb(p1, p2, h + 120);
                double_g = QqhToRgb(p1, p2, h);
                double_b = QqhToRgb(p1, p2, h - 120);
            }
        
            // Convert RGB to the 0 to 255 range.
            r = (int)(double_r * 255.0);
            g = (int)(double_g * 255.0);
            b = (int)(double_b * 255.0);
        }

        private static double QqhToRgb(double q1, double q2, double hue)
        {
            if (hue > 360) hue -= 360;
            else if (hue < 0) hue += 360;
        
            if (hue < 60) return q1 + (q2 - q1) * hue / 60;
            if (hue < 180) return q2;
            if (hue < 240) return q1 + (q2 - q1) * (240 - hue) / 60;
            return q1;
        }
    }
}
