using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Raktarkezeles.MVVM
{
    class TextChangedBehavior : Behavior<SearchBar>
    {
        private CancellationTokenSource tokenSource;
        protected override void OnAttachedTo(SearchBar bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += Bindable_TextChanged;
        }
        protected override void OnDetachingFrom(SearchBar bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= Bindable_TextChanged;
        }
        private void Bindable_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(tokenSource != null)
            {
                tokenSource.Cancel();
            }
            tokenSource = new CancellationTokenSource();
            ExecuteSearchCommand(sender as SearchBar, e, tokenSource.Token);
        }

        private async void ExecuteSearchCommand(SearchBar sender, TextChangedEventArgs e, CancellationToken token)
        {
            await Task.Delay(1000);

            if (token.IsCancellationRequested)
            {
                return;
            }
            sender.SearchCommand.Execute(e.NewTextValue);
        }
    }
}
