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
using Raktarkezeles.MVVM;
using System.Linq;

namespace Raktarkezeles.ViewModels
{
    public class DetailsViewModel : BindableBase
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
                return part != null ? (ObservableCollection<Occurrence>)part.Occurrences : null;
            }
        }

        public ICommand EditPartCommand { protected set; get; }
        public ICommand DeletePartCommand { protected set; get; }
        public ICommand NewOccurrenceCommand { protected set; get; }
        public ICommand TransferQuantityCommand { protected set; get; }
        public ICommand MinusOneCommand { protected set; get; }
        public ICommand PlusOneCommand { protected set; get; }
        public ICommand ChangeQuantityCommand { private set; get; }
        public ICommand DeleteOccurrenceCommnad { protected set; get; }
        public DetailsViewModel(int partId)
        {
            Part = PartContext.GetPart(partId);
            EditPartCommand = new Command(EditPartCommandExecute);
            DeletePartCommand = new Command(DeletePartCommandExecute);
            NewOccurrenceCommand = new Command(NewOccurrenceCommandExecute);
            TransferQuantityCommand = new Command<int>(TransferQuantityCommandExecute);
            MinusOneCommand = new Command<int>(MinusOneCommandExecute);
            PlusOneCommand = new Command<int>(PlusOneCommandExecute);
            DeleteOccurrenceCommnad = new Command<int>(DeleteOccurrenceCommandExecute);
            ChangeQuantityCommand = new Command<int>(ChangeQuantityCommandExecute);
        }
        private async void EditPartCommandExecute()
        {
            NewPartViewModel newPartVM = new NewPartViewModel(part);
            NewPartPage newPartPage = new NewPartPage();
            newPartPage.BindingContext = newPartVM;
            await Application.Current.MainPage.Navigation.PushAsync(newPartPage);
        }
        private async void DeletePartCommandExecute()
        {
            PartContext.DeletePart(part);
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        private async void NewOccurrenceCommandExecute()
        {
            NewOccurrenceViewModel newOccurrenceVM = new NewOccurrenceViewModel(part.Id);
            NewOccurrencePage newOccurrencePage = new NewOccurrencePage();
            newOccurrencePage.BindingContext = newOccurrenceVM;
            await Application.Current.MainPage.Navigation.PushModalAsync(newOccurrencePage);
        }
        private async void TransferQuantityCommandExecute(int id)
        {
            TransferQuantityViewModel transferQuantityVM = new TransferQuantityViewModel(Occurrences.FirstOrDefault(o => o.Id == id).Id);
            TransferQuantityPage transferQuantityPage = new TransferQuantityPage();
            transferQuantityPage.BindingContext = transferQuantityVM;
            await Application.Current.MainPage.Navigation.PushModalAsync(transferQuantityPage);
        }
        private async void ChangeQuantityCommandExecute(int id)
        {
            QuantityChangeViewModel quantityChangeVM = new QuantityChangeViewModel(Occurrences.FirstOrDefault(o => o.Id == id).Id);
            QuantityChangePage quantityChangePage = new QuantityChangePage();
            quantityChangePage.BindingContext = quantityChangeVM;
            await Application.Current.MainPage.Navigation.PushModalAsync(quantityChangePage);
        }
        private void MinusOneCommandExecute(int id)
        {
            PartContext.ChangeQuantity(id, -1);
        }
        private void PlusOneCommandExecute(int id)
        {
            PartContext.ChangeQuantity(id, 1);
        }
        private void DeleteOccurrenceCommandExecute(int id)
        {
            PartContext.DeleteOccurrence(id);
        }
        public override void OnAppearing()
        {
            base.OnAppearing();
            Part = PartContext.GetPart(part.Id);
        }
    }
}
