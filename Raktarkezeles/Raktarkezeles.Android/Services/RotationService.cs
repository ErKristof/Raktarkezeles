using Android.Graphics;
using Raktarkezeles.Services;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(Raktarkezeles.Droid.Services.RotationService))]
namespace Raktarkezeles.Droid.Services
{
    public class RotationService : IRotationService
    {
        public byte[] RotateImage(byte[] image, int degree)
        {
            Bitmap original = BitmapFactory.DecodeByteArray(image, 0, image.Length);
            Matrix matrix = new Matrix();
            matrix.PostRotate(degree);
            Bitmap rotatedBitmap = Bitmap.CreateBitmap(original, 0, 0, original.Width, original.Height, matrix, true);
            original.Recycle();
            using (MemoryStream ms = new MemoryStream())
            {
                rotatedBitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                rotatedBitmap.Recycle();
                return ms.ToArray();
            }
        }
    }
}