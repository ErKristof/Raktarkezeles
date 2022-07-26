using Android.Graphics;
using Raktarkezeles.Services;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(Raktarkezeles.Droid.Services.MediaService))]
namespace Raktarkezeles.Droid.Services
{
    public class MediaService : IMediaService
    {
        public byte[] ResizeImageByte(byte[] image, float width, float height)
        {
            Bitmap original = BitmapFactory.DecodeByteArray(image, 0, image.Length);
            
            float newHeight = 0;
            float newWidth = 0;

            int originalHeight = original.Height;
            int originalWidth = original.Width;

            if(originalHeight > originalWidth)
            {
                newHeight = height;
                float ratio = originalHeight / height;
                newWidth = originalWidth / ratio;
            }
            else
            {
                newWidth = width;
                float ratio = originalWidth / width;
                newHeight = originalHeight / ratio;
            }

            Bitmap resized = Bitmap.CreateScaledBitmap(original, (int)newWidth, (int)newHeight, true);
            original.Recycle();
            using(MemoryStream ms = new MemoryStream())
            {
                resized.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                resized.Recycle();
                return ms.ToArray();
            }
        }
    }
}