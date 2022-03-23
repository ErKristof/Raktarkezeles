using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Raktarkezeles.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using System.Windows.Input;
using Raktarkezeles.Views;
using Raktarkezeles.DAL;
using System.Linq;

namespace Raktarkezeles.ViewModels
{
    public class DetailsViewModel : ViewModelBase
    {
        private Part part;
        public Part Part
        {
            get 
            { 
                return part;
            }
            set
            {
                part = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Occurrence> Occurrences
        {
            get
            {
                if (part != null)
                { 
                    return (ObservableCollection<Occurrence>)part.Occurrences;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                part.Occurrences = value;
                OnPropertyChanged(nameof(Occurrences));
            }
        }

        public ICommand EditPartCommand { protected set; get; }
        public ICommand DeletePartCommand { protected set; get; }
        public ICommand NewOccurrenceCommand { protected set; get; }
        public ICommand TransferQuantityCommand { protected set; get; }
        public ICommand MinusOneCommand { protected set; get; }
        public ICommand PlusOneCommand { protected set; get; }
        public DetailsViewModel()
        {
            EditPartCommand = new Command(EditPartCommandExecute);
            DeletePartCommand = new Command(DeletePartCommandExecute);
            NewOccurrenceCommand = new Command(NewOccurrenceCommandExecute);
            TransferQuantityCommand = new Command<int>(TransferQuantityCommandExecute);
            MinusOneCommand = new Command<int>(MinusOneCommandExecute);
            PlusOneCommand = new Command<int>(PlusOneCommandExecute);
        }
        private async void EditPartCommandExecute()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new NewPartPage(part), true);
        }
        private async void DeletePartCommandExecute()
        {
            PartContext.DeletePart(part);
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        private async void NewOccurrenceCommandExecute()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new NewOccurrencePage(part));
        }
        private async void TransferQuantityCommandExecute(int id)
        {
            TransferQuantityViewModel transferQuantityVM = new TransferQuantityViewModel();
            transferQuantityVM.FromOccurrence = Occurrences.Where(o => o.Id == id).FirstOrDefault();
            transferQuantityVM.Occurrences = Occurrences;
            TransferQuantityPage transferQuantityPage = new TransferQuantityPage();
            transferQuantityPage.BindingContext = transferQuantityVM;
            await Application.Current.MainPage.Navigation.PushModalAsync(transferQuantityPage);
            OnPropertyChanged(nameof(Occurrences));
        }
        private void MinusOneCommandExecute(int id)
        {
            PartContext.ChangeQuantity(id, -1);
            //Occurrences.Where(o => o.Id == id).FirstOrDefault().Quantity--;
        }
        private void PlusOneCommandExecute(int id)
        {
            PartContext.ChangeQuantity(id, 1);
        }
        public override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
