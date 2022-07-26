using Foundation;
using Raktarkezeles.Services;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(Raktarkezeles.iOS.Services.AlertService))]
namespace Raktarkezeles.iOS.Services
{
    public class AlertService : IAlertService
    {
        private const double LONG_DELAY = 3.5;
        private const double SHORT_DELAY = 2.0;

        NSTimer alertDelay;
        UIAlertController alert;
        public void LongAlert(string message)
        {
            ShowAlert(message, LONG_DELAY);
        }

        public void ShortAlert(string message)
        {
            ShowAlert(message, SHORT_DELAY);
        }

        private void ShowAlert(string message, double seconds)
        {
            alertDelay = NSTimer.CreateScheduledTimer(seconds, (obj) =>
            {
                DismissMessage();
            });
            alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }
        private void DismissMessage()
        {
            if(alert != null)
            {
                alert.DismissViewController(true, null);
            }
            if(alertDelay != null)
            {
                alert.Dispose();
            }
        }
    }
}