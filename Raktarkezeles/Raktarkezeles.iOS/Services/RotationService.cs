using CoreGraphics;
using Foundation;
using Raktarkezeles.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(Raktarkezeles.iOS.Services.RotationService))]
namespace Raktarkezeles.iOS.Services
{
    public class RotationService : IRotationService 
    {
        public byte[] RotateImage(byte[] image, int degree)
        {
            UIImage original = new UIImage(NSData.FromArray(image));
            float radian = degree * (float)Math.PI / 180;

            UIView view = new UIView(frame: new CGRect(0, 0, original.Size.Width, original.Size.Height));
            CGAffineTransform t = CGAffineTransform.MakeRotation(radian);
            view.Transform = t;
            CGSize size = view.Frame.Size;
            UIGraphics.BeginImageContext(size);
            CGContext context = UIGraphics.GetCurrentContext();

            context.TranslateCTM(size.Width / 2, size.Height / 2);
            context.RotateCTM(radian);
            context.ScaleCTM(1, -1);

            context.DrawImage(new CGRect(-original.Size.Width / 2, -original.Size.Height / 2, original.Size.Width, original.Size.Height), original.CGImage);

            UIImage imageCopy = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return imageCopy.AsJPEG().ToArray();
        }
    }
}