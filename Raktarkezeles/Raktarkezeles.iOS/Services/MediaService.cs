using Foundation;
using Raktarkezeles.Services;
using System;
using UIKit;
using CoreGraphics;
using Xamarin.Forms;

[assembly: Dependency(typeof(Raktarkezeles.iOS.Services.MediaService))]
namespace Raktarkezeles.iOS.Services
{
    public class MediaService : IMediaService
    {
        public byte[] ResizeImageByte(byte[] image, float width, float height)
        {
            UIImage original = new UIImage(NSData.FromArray(image));
            nfloat originalHeight = original.Size.Height;
            nfloat originalWidth = original.Size.Width;
            nfloat newHeight = 0;
            nfloat newWidth = 0;

            if (originalHeight > originalWidth)
            {
                newHeight = height;
                nfloat ratio = originalHeight / height;
                newWidth = originalWidth / ratio;
            }
            else
            {
                newWidth = width;
                nfloat ratio = originalWidth / width;
                newHeight = originalHeight / ratio;
            }

            UIGraphics.BeginImageContext(new CGSize(newWidth, newHeight));
            original.Draw(new CGRect(0, 0, newWidth, newHeight));
            var resized = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            var bytes = resized.AsJPEG().ToArray();
            resized.Dispose();
            return bytes;
        }
    }
}