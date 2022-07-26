using Android.App;
using Android.Widget;
using Raktarkezeles.Services;

[assembly: Xamarin.Forms.Dependency(typeof(Raktarkezeles.Droid.Services.AlertService))]
namespace Raktarkezeles.Droid.Services
{
    public class AlertService : IAlertService
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}