namespace Raktarkezeles.Services
{
    public interface IRotationService
    {
        public byte[] RotateImage(byte[] image, int degree);
    }
}
