using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;


//using ImageMagick;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace CESAPSCOREWEBAPP.Helpers
{
    public class ImageResizeHelper
    {


  


        public static void Image_resize(string input_Image_Path, string output_Image_Path, int new_Width)

        {
            // Individual pixels
            using (Image<Rgba32> image = Image.Load(input_Image_Path))
            {

                image.Mutate(x => x
                     .Resize(new ResizeOptions
                     {
                         Size = new Size(new_Width, new_Width),
                         Mode = ResizeMode.Pad
                     })
                     .BackgroundColor(new Rgba32(255, 252, 255)));
                     //.Grayscale());

                
                image.Save(output_Image_Path); // Automatic encoder selected based on extension.
            }

        }
    }
}
