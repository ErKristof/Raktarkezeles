using System;
using System.Collections.Generic;
using System.Text;
using Raktarkezeles.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Raktarkezeles.DAL;
using Xamarin.Forms;
using System.Windows.Input;
using Raktarkezeles.MVVM;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.IO;

namespace Raktarkezeles.ViewModels
{
    public class NewPartViewModel : BindableBase
    {
        private Part newPart = new Part();
        public Part NewPart
        {
            get
            {
                return newPart;
            }
            set
            {
                newPart = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddPartCommand { protected set; get; }
        public ICommand TakePictureCommand { private set; get; }
 
        private List<Manufacturer> manufacturers;
        private List<Category> categories;
        private List<Unit> units;

        public List<Manufacturer> Manufacturers
        {
            get { return manufacturers; }
        }
        public List<Category> Categories
        {
            get { return categories; }
        }
        public List<Unit> Units
        {
            get { return units; }
        }

        public NewPartViewModel(Part newPart = null)
        {
            manufacturers = PartContext.GetManufacturers();
            categories = PartContext.GetCategories();
            units = PartContext.GetUnits();
            if (newPart != null)
            {
                this.newPart = newPart;
                AddPartCommand = new Command(SavePartCommandExecute);
            }
            else
            {
                AddPartCommand = new Command(AddPartCommandExecute);
            }
            TakePictureCommand = new Command(TakePictureComandExecute);
        }

        private async void AddPartCommandExecute()
        {
            newPart.CategoryId = newPart.Category.Id;
            newPart.ManufacturerId = newPart.Manufacturer.Id;
            newPart.UnitId = newPart.Unit.Id;
            newPart.Occurrences = new ObservableCollection<Occurrence>();
            newPart.Quantity = 0;
            PartContext.AddPart(newPart);
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void SavePartCommandExecute()
        {
            newPart.CategoryId = newPart.Category.Id;
            newPart.ManufacturerId = newPart.Manufacturer.Id;
            newPart.UnitId = newPart.Unit.Id;
            PartContext.EditPart(newPart);
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void TakePictureComandExecute()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                if(photo == null)
                {
                    return;
                }
                var stream = await photo.OpenReadAsync();
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    newPart.Image = ms.ToArray();
                    OnPropertyChanged(nameof(NewPart));
                }
                
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await Application.Current.MainPage.DisplayAlert("Exception", fnsEx.Message,"OK");
            }
            catch (PermissionException pEx)
            {
                await Application.Current.MainPage.DisplayAlert("Exception", pEx.Message, "OK");
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Exception", ex.Message, "OK");
            }
        }
    }
}
