﻿using System;
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
        private Manufacturer manufacturer;
        public Manufacturer Manufacturer
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
            part = PartContext.GetPart(partId);
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
            if (Occurrences.Where(o => o.Id == id).First().Quantity > 0)
            {
                PartContext.ChangeQuantity(id, -1);
            }
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
            part = PartContext.GetPart(part.Id);
            UpdatePage();
        }
        private void UpdatePage()
        {
            Image = part.Image;
            Name = part.Name;
            Manufacturer = part.Manufacturer;
            ItemNumber = part.ItemNumber;
            TypeNumber = part.TypeNumber;
            Description = part.Description;
        }
    }
}
