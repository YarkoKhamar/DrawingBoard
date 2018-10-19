﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace DrawingBoard
{
    public static class BitmapFilters
    {

        public static void DrawTriangle(this Graphics g, Pen pen, Rectangle rect)
        {
            var lines = new Point[]
            {
                new Point(rect.Left, rect.Bottom),
                new Point(rect.Left + rect.Width/2, rect.Top),
                new Point(rect.Right, rect.Bottom)
                //new Point(rect.Left, rect.Bottom),
            };
            g.DrawPolygon(pen, lines);
        }
        public static void FillTriangle(this Graphics g, Brush brush, Rectangle rect)
        {
            var lines = new Point[]
            {
                new Point(rect.Left, rect.Bottom),
                new Point(rect.Left + rect.Width/2, rect.Top),
                new Point(rect.Right, rect.Bottom)
            };
            g.FillPolygon(brush, lines);
        }

        private static Bitmap GetArgbCopy(Image sourceImage)
        {
            Bitmap bmpNew = new Bitmap(sourceImage.Width, sourceImage.Height, PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(bmpNew))
            {
                graphics.DrawImage(sourceImage, new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), GraphicsUnit.Pixel);
                graphics.Flush();
            }

            return bmpNew;
        }
        private static Bitmap ApplyColorMatrix(Image sourceImage, ColorMatrix colorMatrix)
        {
            Bitmap bmp32BppSource = GetArgbCopy(sourceImage);
            Bitmap bmp32BppDest = new Bitmap(bmp32BppSource.Width, bmp32BppSource.Height, PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(bmp32BppDest))
            {
                ImageAttributes bmpAttributes = new ImageAttributes();
                bmpAttributes.SetColorMatrix(colorMatrix);

                graphics.DrawImage(bmp32BppSource, new Rectangle(0, 0, bmp32BppSource.Width, bmp32BppSource.Height),
                                 0, 0, bmp32BppSource.Width, bmp32BppSource.Height, GraphicsUnit.Pixel, bmpAttributes);

            }

            bmp32BppSource.Dispose();

            return bmp32BppDest;
        }
        public static Bitmap CopyWithTransparency(this Image sourceImage, byte alphaComponent = 100)
        {
            Bitmap bmpNew = GetArgbCopy(sourceImage);
            BitmapData bmpData = bmpNew.LockBits(new Rectangle(0, 0, sourceImage.Width, sourceImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            IntPtr ptr = bmpData.Scan0;

            byte[] byteBuffer = new byte[bmpData.Stride * bmpNew.Height];

            Marshal.Copy(ptr, byteBuffer, 0, byteBuffer.Length);

            for (int k = 3; k < byteBuffer.Length; k += 4)
            {
                byteBuffer[k] = alphaComponent;
            }

            Marshal.Copy(byteBuffer, 0, ptr, byteBuffer.Length);

            bmpNew.UnlockBits(bmpData);

            bmpData = null;
            byteBuffer = null;

            return bmpNew;
        }
        public static Bitmap DrawWithTransparency(this Image sourceImage)
        {
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                                                {
                                                    new float[] {1, 0, 0, 0, 0},
                                                    new float[] {0, 1, 0, 0, 0},
                                                    new float[] {0, 0, 1, 0, 0},
                                                    new float[] {0, 0, 0, 0.3f, 0},
                                                    new float[] {0, 0, 0, 0, 1}
                                                });

            return ApplyColorMatrix(sourceImage, colorMatrix);
        }
        public static Bitmap GBC(this Image sourceImage, float brightness, float contrast)
        {
            //brightness = -1.0f; // no change in brightness
            //contrast = 3.0f; // twice the contrast

            //float adjustedBrightness = brightness - 1.0f;

            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                                                {
                                                    new float[] {contrast, 0, 0, 0, 0}, // scale red
                                                    new float[] {0, contrast, 0, 0, 0}, // scale green
                                                    new float[] {0, 0, contrast, 0, 0}, // scale blue
                                                    new float[] {0, 0, 0, 1.0f, 0}, // don't scale alpha
                                                    new float[] { brightness, brightness, brightness, 0, 1}
                                                });

            return ApplyColorMatrix(sourceImage, colorMatrix);
        }
        public static Bitmap CopyAsNegative(this Image sourceImage)
        {
            Bitmap bmpNew = GetArgbCopy(sourceImage);
            BitmapData bmpData = bmpNew.LockBits(new Rectangle(0, 0, sourceImage.Width, sourceImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            IntPtr ptr = bmpData.Scan0;

            byte[] byteBuffer = new byte[bmpData.Stride * bmpNew.Height];

            Marshal.Copy(ptr, byteBuffer, 0, byteBuffer.Length);
            byte[] pixelBuffer = null;

            int pixel = 0;

            for (int k = 0; k < byteBuffer.Length; k += 4)
            {
                pixel = ~BitConverter.ToInt32(byteBuffer, k);
                pixelBuffer = BitConverter.GetBytes(pixel);

                byteBuffer[k] = pixelBuffer[0];
                byteBuffer[k + 1] = pixelBuffer[1];
                byteBuffer[k + 2] = pixelBuffer[2];
            }

            Marshal.Copy(byteBuffer, 0, ptr, byteBuffer.Length);

            bmpNew.UnlockBits(bmpData);

            bmpData = null;
            byteBuffer = null;

            return bmpNew;
        }
        public static Bitmap DrawAsNegative(this Image sourceImage)
        {
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                                                {
                                                    new float[] {-1, 0, 0, 0, 0},
                                                    new float[] {0, -1, 0, 0, 0},
                                                    new float[] {0, 0, -1, 0, 0},
                                                    new float[] {0, 0, 0, 1, 0},
                                                    new float[] {1, 1, 1, 1, 1}
                                                });

            return ApplyColorMatrix(sourceImage, colorMatrix);
        }
        public static Bitmap PolaroidColor(this Image sourceImage)
        {
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                                                {
                                                    new float[] {1.438f, -0.062f, -0.062f, 0, 0},
                                                    new float[] {-0.122f, 1.378f, -0.122f, 0, 0},
                                                    new float[] {-0.016f, -0.016f, 1.483f, 0, 0},
                                                    new float[] {0, 0, 0, 1, 0},
                                                    new float[] {-0.03f, 0.05f, -0.02f, 0, 1}
                                                });

            return ApplyColorMatrix(sourceImage, colorMatrix);
        }
        public static Bitmap SwapRGB(this Image sourceImage)
        {
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                                                {
                                                    new float[] {0, 0, 1, 0, 0},
                                                    new float[] {0, 1, 0, 0, 0},
                                                    new float[] {1, 0, 0, 0, 0},
                                                    new float[] {0, 0, 0, 1, 0},
                                                    new float[] {0, 0, 0, 0, 1}
                                                });

            return ApplyColorMatrix(sourceImage, colorMatrix);
        }
        public static Bitmap WhiteToAplha(this Image sourceImage)
        {
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                                                {
                                                    new float[] {1, 0, 0, -1, 0},
                                                    new float[] {0, 1, 0, -1, 0},
                                                    new float[] {0, 0, 1, -1, 0},
                                                    new float[] {0, 0, 0, 1, 0},
                                                    new float[] {0, 0, 0, 0, 1}
                                                });

            return ApplyColorMatrix(sourceImage, colorMatrix);
        }
        public static Bitmap BW(this Image sourceImage)
        {
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                                                {
                                                    new float[] {1.5f, 1.5f, 1.5f, 0, 0},
                                                    new float[] {1.5f, 1.5f, 1.5f, 0, 0},
                                                    new float[] {1.5f, 1.5f, 1.5f, 0, 0},
                                                    new float[] {0, 0, 0, 1, 0},
                                                    new float[] {-1, -1, -1, 0, 1}
                                                });

            return ApplyColorMatrix(sourceImage, colorMatrix);
        }
        public static Bitmap DrawAsSheared(this Image sourceImage)
        {
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                                                {
                                                    new float[] {1, 0, 0, 0, 0},
                                                    new float[] {0, 1, 0, 0, 0},
                                                    new float[] {0.50f, 0, 1, 0, 0},
                                                    new float[] {0, 0, 0, 1, 0},
                                                    new float[] {0, 0, 0, 0, 1}
                                                });

            return ApplyColorMatrix(sourceImage, colorMatrix);
        }
        public static Bitmap CopyAsGrayscale(this Image sourceImage)
        {
            Bitmap bmpNew = GetArgbCopy(sourceImage);
            BitmapData bmpData = bmpNew.LockBits(new Rectangle(0, 0, sourceImage.Width, sourceImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            IntPtr ptr = bmpData.Scan0;

            byte[] byteBuffer = new byte[bmpData.Stride * bmpNew.Height];

            System.Runtime.InteropServices.Marshal.Copy(ptr, byteBuffer, 0, byteBuffer.Length);

            float rgb = 0;

            for (int k = 0; k < byteBuffer.Length; k += 4)
            {
                rgb = byteBuffer[k] * 0.11f;
                rgb += byteBuffer[k + 1] * 0.59f;
                rgb += byteBuffer[k + 2] * 0.3f;

                byteBuffer[k] = (byte)rgb;
                byteBuffer[k + 1] = byteBuffer[k];
                byteBuffer[k + 2] = byteBuffer[k];

                byteBuffer[k + 3] = 255;
            }

            Marshal.Copy(byteBuffer, 0, ptr, byteBuffer.Length);

            bmpNew.UnlockBits(bmpData);

            bmpData = null;
            byteBuffer = null;

            return bmpNew;
        }
        public static Bitmap DrawAsGrayscale(this Image sourceImage)
        {
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                                                {
                                                    new float[] {.3f, .3f, .3f, 0, 0},
                                                    new float[] {.59f, .59f, .59f, 0, 0},
                                                    new float[] {.11f, .11f, .11f, 0, 0},
                                                    new float[] {0, 0, 0, 1, 0},
                                                    new float[] {0, 0, 0, 0, 1}
                                                });

            return ApplyColorMatrix(sourceImage, colorMatrix);
        }
        public static Bitmap CopyAsSepiaTone(this Image sourceImage)
        {
            Bitmap bmpNew = GetArgbCopy(sourceImage);
            BitmapData bmpData = bmpNew.LockBits(new Rectangle(0, 0, sourceImage.Width, sourceImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            IntPtr ptr = bmpData.Scan0;

            byte[] byteBuffer = new byte[bmpData.Stride * bmpNew.Height];

            Marshal.Copy(ptr, byteBuffer, 0, byteBuffer.Length);

            byte maxValue = 255;
            float r = 0;
            float g = 0;
            float b = 0;

            for (int k = 0; k < byteBuffer.Length; k += 4)
            {
                r = byteBuffer[k] * 0.189f + byteBuffer[k + 1] * 0.769f + byteBuffer[k + 2] * 0.393f;
                g = byteBuffer[k] * 0.168f + byteBuffer[k + 1] * 0.686f + byteBuffer[k + 2] * 0.349f;
                b = byteBuffer[k] * 0.131f + byteBuffer[k + 1] * 0.534f + byteBuffer[k + 2] * 0.272f;

                byteBuffer[k + 2] = (r > maxValue ? maxValue : (byte)r);
                byteBuffer[k + 1] = (g > maxValue ? maxValue : (byte)g);
                byteBuffer[k] = (b > maxValue ? maxValue : (byte)b);
            }

            Marshal.Copy(byteBuffer, 0, ptr, byteBuffer.Length);

            bmpNew.UnlockBits(bmpData);

            bmpData = null;
            byteBuffer = null;

            return bmpNew;
        }

        public static Bitmap DrawAsSepiaTone(this Image sourceImage)
        {           
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                                                {
                                                    new float[] {.393f, .349f, .272f, 0, 0},
                                                    new float[] {.769f, .686f, .534f, 0, 0},
                                                    new float[] {.189f, .168f, .131f, 0, 0},
                                                    new float[] {0, 0, 0, 1, 0},
                                                    new float[] {0, 0, 0, 0, 1}
                                                });

            return ApplyColorMatrix(sourceImage, colorMatrix);
        }
    }
}
