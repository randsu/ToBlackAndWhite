using System.Drawing;
using System.Drawing.Imaging;

namespace ToBlackAndWhite
{
    public class Program
    {
        /// <summary>
        /// Converts color png image to black and white
        /// by converting to greyscale and using a threshold for
        /// "how dark to convert to black", if below, will be white.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var colorImgPath = @"C:\Git\ToBlackAndWhite\ToBlackAndWhite\color.png";
            var blackAndWhiteImgPath = @"C:\Git\ToBlackAndWhite\ToBlackAndWhite\blackandwhite.png";
            var threshold = 0.8f; // TODO: get as argument

            if (args.Length > 1)
                blackAndWhiteImgPath = args[1];
            if (args.Length > 0)
                colorImgPath = args[0];

            var image1 = (Bitmap)Image.FromFile(colorImgPath, true);

            var conv = Helper.Transform(image1, threshold);

            conv.Save(blackAndWhiteImgPath);
        }
        
        internal static class Helper
        {
            public static Bitmap Transform(Bitmap colorImage, float threshold = 0.4f)
            {
                var transformedImage = new Bitmap(colorImage.Width, colorImage.Height);

                var newGfx = Graphics.FromImage(transformedImage);

                var colorMatrix = new ColorMatrix(
                    new[]
                    {
                        new[] {.3f, .3f, .3f, 0, 0},
                        new[] {.59f, .59f, .59f, 0, 0},
                        new[] {.11f, .11f, .11f, 0, 0},
                        new float[] {0, 0, 0, 1, 0},
                        new float[] {0, 0, 0, 0, 1}
                    });

                var imgAttr = new ImageAttributes();

                imgAttr.SetThreshold(threshold); // This threshold separates black from white
                
                imgAttr.SetColorMatrix(colorMatrix);

                newGfx.DrawImage(
                    colorImage, 
                    new Rectangle(0, 0, colorImage.Width, colorImage.Height),
                    0, 
                    0, 
                    colorImage.Width, 
                    colorImage.Height, 
                    GraphicsUnit.Pixel, 
                    imgAttr);

                newGfx.Dispose();
                return transformedImage;
            }
        }
    }
}
