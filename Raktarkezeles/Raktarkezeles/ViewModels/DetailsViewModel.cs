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
using Raktarkezeles.MVVM;
using System.Linq;
using Raktarkezeles.Services;

namespace Raktarkezeles.ViewModels
{
    public class DetailsViewModel : BindableBase
    {
        private RaktarkezelesService service = new RaktarkezelesService();
        private Alkatresz part;
        private byte[] image;
        public byte[] Image
        {
            get
            {
                return image;
            }
            set
            {
                if (image != value)
                {
                    image = value;
                    OnPropertyChanged();
                }
            }
        }
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }
        private Gyarto manufacturer;
        public Gyarto Manufacturer
        {
            get
            {
                return manufacturer;
            }
            set
            {
                if (manufacturer != value)
                {
                    manufacturer = value;
                    OnPropertyChanged();
                }
            }
        }
        private string itemNumber;
        public string ItemNumber
        {
            get
            {
                return itemNumber;
            }
            set
            {
                if (itemNumber != value)
                {
                    itemNumber = value;
                    OnPropertyChanged();
                }
            }
        }
        private string typeNumber;
        public string TypeNumber
        {
            get
            {
                return typeNumber;
            }
            set
            {
                if (typeNumber != value)
                {
                    typeNumber = value;
                    OnPropertyChanged();
                }
            }
        }
        private string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<AlkatreszElofordulas> Occurrences
        {
            get
            {
                return part != null ? part.AlkatreszElofordulasok : null;
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
        public DetailsViewModel(Alkatresz part)
        {
            this.part = part;
            UpdatePage();
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
            MessagingCenter.Send(this, "Deleted", part.Id);
            await service.DeleteAlkatresz(part.Id);
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        private async void NewOccurrenceCommandExecute()
        {
            MessagingCenter.Subscribe<NewOccurrenceViewModel, AlkatreszElofordulas>(this, "NewOccurrence", (vm, newOccurrence) =>
            {
                Occurrences.Add(newOccurrence);
                MessagingCenter.Unsubscribe<NewOccurrenceViewModel, AlkatreszElofordulas>(this, "NewOccurrence");
            });
            NewOccurrenceViewModel newOccurrenceVM = new NewOccurrenceViewModel(part);
            NewOccurrencePage newOccurrencePage = new NewOccurrencePage();
            newOccurrencePage.BindingContext = newOccurrenceVM;
            await Application.Current.MainPage.Navigation.PushModalAsync(newOccurrencePage);
        }
        private async void TransferQuantityCommandExecute(int id)
        {
            TransferQuantityViewModel transferQuantityVM = new TransferQuantityViewModel(Occurrences.FirstOrDefault(o => o.Id == id).Id, part);
            TransferQuantityPage transferQuantityPage = new TransferQuantityPage();
            transferQuantityPage.BindingContext = transferQuantityVM;
            await Application.Current.MainPage.Navigation.PushModalAsync(transferQuantityPage);
        }
        private async void ChangeQuantityCommandExecute(int id)
        {
            QuantityChangeViewModel quantityChangeVM = new QuantityChangeViewModel(Occurrences.FirstOrDefault(o => o.Id == id));
            QuantityChangePage quantityChangePage = new QuantityChangePage();
            quantityChangePage.BindingContext = quantityChangeVM;
            await Application.Current.MainPage.Navigation.PushModalAsync(quantityChangePage);
        }
        private async void MinusOneCommandExecute(int id)
        {
            var changedElofordulas = Occurrences.First(o => o.Id == id);
            if (changedElofordulas.Mennyiseg > 0)
            {
                if (await service.ChangeQuantity(id, changedElofordulas.Mennyiseg - 1))
                {
                    changedElofordulas.Mennyiseg--;
                }
            }
        }
        private async void PlusOneCommandExecute(int id)
        {
            var changedElofordulas = Occurrences.First(o => o.Id == id);
            if (await service.ChangeQuantity(id, changedElofordulas.Mennyiseg + 1))
            {
                changedElofordulas.Mennyiseg++;
            }
        }
        private async void DeleteOccurrenceCommandExecute(int id)
        {
            await service.DeleteElofordulas(id);
            Occurrences.Remove(Occurrences.First(x => x.Id == id));
        }
        public override void OnAppearing()
        {
            base.OnAppearing();
            UpdatePage();
        }
        public void OnDisappearing()
        {
            MessagingCenter.Send(this, "Updated", part);
        }
        private void UpdatePage()
        {
            Image = part.Foto;
            Name = part.Nev;
            Manufacturer = part.Gyarto;
            ItemNumber = part.Cikkszam;
            TypeNumber = part.Tipus;
            Description = part.Leiras;
        }
    }
}
