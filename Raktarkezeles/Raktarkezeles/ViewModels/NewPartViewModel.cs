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
using ZXing.Mobile;
using ZXing;

namespace Raktarkezeles.ViewModels
{
    public class NewPartViewModel : BindableBase
    {
        private Part part;
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
        private string itemNumber;
        public string ItemNumber
        {
            get
            {
                return itemNumber;
            }
            set
            {
                if(itemNumber != value)
                {
                    itemNumber = value;
                    OnPropertyChanged();
                }
            }
        }
        private Unit unit;
        public Unit Unit
        {
            get
            {
                return unit;
            }
            set
            {
                if(unit != value)
                {
                    unit = value;
                    OnPropertyChanged();
                }
            }
        }
        private Category category;
        public Category Category
        {
            get
            {
                return category;
            }
            set
            {
                if(category != value)
                {
                    category = value;
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
                if(description != value)
                {
                    description = value;
                    OnPropertyChanged();
                }
            }
        }
        private byte[] image;
        public byte[] Image
        {
            get
            {
                return image;
            }
            set
            {
                if(image != value)
                {
                    image = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand AddPartCommand { protected set; get; }
        public ICommand TakePictureCommand { private set; get; }
        public ICommand ScanBarcodeCommand { private set; get; }

        private bool invalidName = false;
        public bool InvalidName { get { return invalidName; } set { invalidName = value; OnPropertyChanged(); } }
        private bool invalidManufacturer = false;
        public bool InvalidManufacturer { get { return invalidManufacturer; } set { invalidManufacturer = value; OnPropertyChanged(); } }
        private bool invalidTypeNumber = false;
        public bool InvalidTypeNumber { get { return invalidTypeNumber; } set { invalidTypeNumber = value; OnPropertyChanged(); } }
        private bool invalidItemNumber = false;
        public bool InvalidItemNumber { get { return invalidItemNumber; } set { invalidItemNumber = value; OnPropertyChanged(); } }
        private bool invalidUnit = false;
        public bool InvalidUnit { get { return invalidUnit; } set { invalidUnit = value; OnPropertyChanged(); } }
        private bool invalidCategory = false;
        public bool InvalidCategory { get { return invalidCategory; } set { invalidCategory = value; OnPropertyChanged(); } }
        private bool invalidImage = false;
        public bool InvalidImage { get { return invalidImage; } set { invalidImage = value; OnPropertyChanged(); } }
 
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
            manufacturers = (List<Manufacturer>)PartContext.GetManufacturers();
            categories = (List<Category>)PartContext.GetCategories();
            units = (List<Unit>)PartContext.GetUnits();
            if (newPart != null)
            {
                part = newPart;
                Image = newPart.Image;
                Name = newPart.Name;
                Manufacturer = newPart.Manufacturer;
                TypeNumber = newPart.TypeNumber;
                ItemNumber = newPart.ItemNumber;
                Unit = newPart.Unit;
                Category = newPart.Category;
                Description = newPart.Description;
                AddPartCommand = new Command(SavePartCommandExecute);
            }
            else
            {
                AddPartCommand = new Command(AddPartCommandExecute);
            }
            TakePictureCommand = new Command(TakePictureComandExecute);
            ScanBarcodeCommand = new Command(ScanBarcodeCommandExecute);
        }

        private async void AddPartCommandExecute()
        {
            if (!CheckValidation())
            {
                Part newPart = new Part
                {
                    Image = Image,
                    Name = Name,
                    Manufacturer = Manufacturer,
                    ManufacturerId = Manufacturer.Id,
                    TypeNumber = TypeNumber,
                    ItemNumber = ItemNumber,
                    Unit = Unit,
                    UnitId = Unit.Id,
                    Category = Category,
                    CategoryId = Category.Id,
                    Description = Description,
                    Occurrences = new ObservableCollection<Occurrence>()
                };
                newPart = PartContext.AddPart(newPart);
                MessagingCenter.Send(this, "New", newPart);
                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }

        private async void SavePartCommandExecute()
        {
            if (!CheckValidation())
            {
                part.Image = Image;
                part.Name = Name;
                part.Manufacturer = Manufacturer;
                part.ManufacturerId = Manufacturer.Id;
                part.TypeNumber = TypeNumber;
                part.ItemNumber = ItemNumber;
                part.Unit = Unit;
                part.UnitId = Unit.Id;
                part.Category = Category;
                part.CategoryId = Category.Id;
                part.Description = Description;
                PartContext.EditPart(part);
                await Application.Current.MainPage.Navigation.PopAsync();
            }
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
                    var rotatedImage = 
                    Image = ms.ToArray();
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
        private async void ScanBarcodeCommandExecute()
        {
            MobileBarcodeScanner scanner = new MobileBarcodeScanner();
            Result result = await scanner.Scan();
            if (result != null)
            {
                ItemNumber = result.Text;
            }
        }
        private bool CheckValidation()
        {
            //InvalidImage = Image == null;
            InvalidName = string.IsNullOrWhiteSpace(Name);
            InvalidManufacturer = Manufacturer == null;
            InvalidTypeNumber = string.IsNullOrWhiteSpace(TypeNumber);
            InvalidItemNumber = string.IsNullOrWhiteSpace(ItemNumber);
            InvalidUnit = Unit == null;
            InvalidCategory = Category == null;
            return InvalidImage || InvalidName || InvalidManufacturer || InvalidTypeNumber || InvalidItemNumber || InvalidUnit || InvalidCategory;
        }
    }
}
