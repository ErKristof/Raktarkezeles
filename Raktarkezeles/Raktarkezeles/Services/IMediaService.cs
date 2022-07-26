namespace Raktarkezeles.Services
{
    public interface IMediaService
    {
        public byte[] ResizeImageByte(byte[] image, float width, float height);
    }
}
