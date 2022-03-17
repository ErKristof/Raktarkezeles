using System;
using System.Collections.Generic;
using System.Text;
using Raktarkezeles.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using System.Windows.Input;
using Raktarkezeles.Views;
using Raktarkezeles.DAL;

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

        public List<Occurrence> Occurrences
        {
            get
            {
                return (List<Occurrence>)part.Occurrences;
            }
        }

        public ICommand EditPartCommand { protected set; get; }
        public ICommand DeletePartCommand { protected set; get; }
        public DetailsViewModel(INavigation navigation, Part _part) : base(navigation)
        {
            part = _part;
            EditPartCommand = new Command(EditPartCommandExecute);
            DeletePartCommand = new Command(DeletePartCommandExecute);
        }

        public async void EditPartCommandExecute()
        {
            await Navigation.PushAsync(new NewPartPage(part), true);
        }
        public async void DeletePartCommandExecute()
        {
            PartContext.DeletePart(part);
            await Navigation.PopAsync();
        }
        public override void OnAppearing()
        {
            base.OnAppearing();
            OnPropertyChanged(nameof(Part));
        }
    }
}
