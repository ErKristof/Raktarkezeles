namespace Raktarkezeles.Services
{
    public interface IAlertService
    {
        public void LongAlert(string message);
        public void ShortAlert(string message);
    }
}
