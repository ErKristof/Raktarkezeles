using System;
using System.Linq;
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
using Raktarkezeles.Services;

namespace Raktarkezeles.ViewModels
{
    public class NewPartViewModel : BindableBase
    {
        private RaktarkezelesService service = new RaktarkezelesService();
        private Alkatresz part;
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
        private MennyisegiEgyseg unit;
        public MennyisegiEgyseg Unit
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
        private Kategoria category;
        public Kategoria Category
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
 
        private ObservableCollection<Gyarto> manufacturers = new ObservableCollection<Gyarto>();
        private ObservableCollection<Kategoria> categories = new ObservableCollection<Kategoria>();
        private ObservableCollection<MennyisegiEgyseg> units = new ObservableCollection<MennyisegiEgyseg>();

        public ObservableCollection<Gyarto> Manufacturers
        {
            get { return manufacturers; }
            set { manufacturers = value; }
        }
        public ObservableCollection<Kategoria> Categories
        {
            get { return categories; }
            set { categories = value; }
        }
        public ObservableCollection<MennyisegiEgyseg> Units
        {
            get { return units; }
            set { units = value; }
        }

        public NewPartViewModel(Alkatresz newPart = null)
        {
            Setup(newPart);
            if(newPart != null)
            {
                AddPartCommand = new Command(SavePartCommandExecute);
            }
            else
            {
                AddPartCommand = new Command(AddPartCommandExecute);
            }
            TakePictureCommand = new Command(TakePictureComandExecute);
            ScanBarcodeCommand = new Command(ScanBarcodeCommandExecute);
        }
        private async void Setup(Alkatresz newPart = null)
        {
            await GetLists();
            if (newPart != null)
            {
                part = newPart;
                //Image = newPart.Foto;
                Name = newPart.Nev;
                Manufacturer = Manufacturers.First(x => x.Id == newPart.GyartoId);
                TypeNumber = newPart.Tipus;
                ItemNumber = newPart.Cikkszam;
                Unit = Units.First(x => x.Id == newPart.MennyisegiEgysegId);
                Category = Categories.First(x => x.Id == newPart.KategoriaId);
                Description = newPart.Leiras;
            }
        }
        private async Task GetLists()
        {
            foreach (var m in await service.GetGyartok())
            {
                Manufacturers.Add(m);
            }
            foreach (var c in await service.GetKategoria())
            {
                Categories.Add(c);
            }
            foreach (var u in await service.GetMennyisegiEgysegek())
            {
                Units.Add(u);
            }
        }

        private async void AddPartCommandExecute()
        {
            if (!CheckValidation())
            {
                Alkatresz newPart = new Alkatresz
                {
                    //Foto = Image,
                    Nev = Name,
                    Gyarto = Manufacturer,
                    GyartoId = Manufacturer.Id,
                    Tipus = TypeNumber,
                    Cikkszam = ItemNumber,
                    MennyisegiEgyseg = Unit,
                    MennyisegiEgysegId = Unit.Id,
                    Kategoria = Category,
                    KategoriaId = Category.Id,
                    Leiras = Description,
                    AlkatreszElofordulasok = new ObservableCollection<AlkatreszElofordulas>()
                };
                newPart = await service.PostAlkatresz(newPart);
                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }

        private async void SavePartCommandExecute()
        {
            if (!CheckValidation())
            {
                //part.Foto = Image;
                part.Nev = Name;
                part.Gyarto = Manufacturer;
                part.GyartoId = Manufacturer.Id;
                part.Tipus = TypeNumber;
                part.Cikkszam = ItemNumber;
                part.MennyisegiEgyseg = Unit;
                part.MennyisegiEgysegId = Unit.Id;
                part.Kategoria = Category;
                part.KategoriaId = Category.Id;
                part.Leiras = Description;
                await service.PutAlkatresz(part);
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
