using System;
using Raktarkezeles.Models;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Windows.Input;
using Raktarkezeles.MVVM;
using Xamarin.Essentials;
using System.IO;
using ZXing.Mobile;
using ZXing;
using Raktarkezeles.Services;
using System.Net.Http;

namespace Raktarkezeles.ViewModels
{
    public class NewPartViewModel : BindableBase
    {
        private IRaktarService service = new LocalRaktarkezelesService();
        private Alkatresz alkatresz;
        private string nev;
        public string Nev
        {
            get
            {
                return nev;
            }
            set
            {
                if (nev != value)
                {
                    nev = value;
                    OnPropertyChanged();
                }
            }
        }
        private Gyarto gyarto;
        public Gyarto Gyarto
        {
            get
            {
                return gyarto;
            }
            set
            {
                if (gyarto != value)
                {
                    gyarto = value;
                    OnPropertyChanged();
                }
            }
        }
        private string tipus;
        public string Tipus
        {
            get
            {
                return tipus;
            }
            set
            {
                if (tipus != value)
                {
                    tipus = value;
                    OnPropertyChanged();
                }
            }
        }
        private string cikkszam;
        public string Cikkszam
        {
            get
            {
                return cikkszam;
            }
            set
            {
                if (cikkszam != value)
                {
                    cikkszam = value;
                    OnPropertyChanged();
                }
            }
        }
        private MennyisegiEgyseg mennyisegiEgyseg;
        public MennyisegiEgyseg MennyisegiEgyseg
        {
            get
            {
                return mennyisegiEgyseg;
            }
            set
            {
                if (mennyisegiEgyseg != value)
                {
                    mennyisegiEgyseg = value;
                    OnPropertyChanged();
                }
            }
        }
        private Kategoria kategoria;
        public Kategoria Kategoria
        {
            get
            {
                return kategoria;
            }
            set
            {
                if (kategoria != value)
                {
                    kategoria = value;
                    OnPropertyChanged();
                }
            }
        }
        private string leiras;
        public string Leiras
        {
            get
            {
                return leiras;
            }
            set
            {
                if (leiras != value)
                {
                    leiras = value;
                    OnPropertyChanged();
                }
            }
        }
        private byte[] eredetiFoto;
        private byte[] foto;
        public byte[] Foto
        {
            get
            {
                return foto;
            }
            set
            {
                if (foto != value)
                {
                    foto = value;
                    OnPropertyChanged();
                }
            }
        }
        public ICommand AddPartCommand { private set; get; }
        public ICommand TakePictureCommand { private set; get; }
        public ICommand ScanBarcodeCommand { private set; get; }
        public ICommand RotateImageCommand { private set; get; }

        private bool invalidNev = false;
        public bool InvalidNev { get { return invalidNev; } set { invalidNev = value; OnPropertyChanged(); } }
        private bool invalidGyarto = false;
        public bool InvalidGyarto { get { return invalidGyarto; } set { invalidGyarto = value; OnPropertyChanged(); } }
        private bool invalidTipus = false;
        public bool InvalidTipus { get { return invalidTipus; } set { invalidTipus = value; OnPropertyChanged(); } }
        private bool invalidCikkszam = false;
        public bool InvalidCikkszam { get { return invalidCikkszam; } set { invalidCikkszam = value; OnPropertyChanged(); } }
        private bool invalidMennyisegiEgyseg = false;
        public bool InvalidMennyisegiEgyseg { get { return invalidMennyisegiEgyseg; } set { invalidMennyisegiEgyseg = value; OnPropertyChanged(); } }
        private bool invalidKategoria = false;
        public bool InvalidKategoria { get { return invalidKategoria; } set { invalidKategoria = value; OnPropertyChanged(); } }
        private bool invalidFoto = false;
        public bool InvalidFoto { get { return invalidFoto; } set { invalidFoto = value; OnPropertyChanged(); } }

        public ObservableCollection<Gyarto> Gyartok { get; set; } = new ObservableCollection<Gyarto>();
        public ObservableCollection<Kategoria> Kategoriak { get; set; } = new ObservableCollection<Kategoria>();
        public ObservableCollection<MennyisegiEgyseg> MennyisegiEgysegek { get; set; } = new ObservableCollection<MennyisegiEgyseg>();

        public NewPartViewModel(Alkatresz newPart = null)
        {
            Setup(newPart);
            AddPartCommand = newPart == null ? new Command(AddPartCommandExecute) : new Command(SavePartCommandExecute);
            TakePictureCommand = new Command(TakePictureComandExecute);
            ScanBarcodeCommand = new Command(ScanBarcodeCommandExecute);
            RotateImageCommand = new Command(RotateImageCommandExecute);
        }
        private async void Setup(Alkatresz newPart)
        {
            try
            {
                Gyartok = await service.GetGyartok();
                Kategoriak = await service.GetKategoriak();
                MennyisegiEgysegek = await service.GetMennyisegiEgysegek();
            }
            catch (HttpRequestException HREx) { DependencyService.Get<IAlertService>().LongAlert(HREx.Message); }
            catch (TimeoutException TEx) { DependencyService.Get<IAlertService>().LongAlert(TEx.Message); }
            if (newPart != null)
            {
                alkatresz = newPart;
                Foto = newPart.Foto;
                Nev = newPart.Nev;
                Gyarto = newPart.Gyarto;
                Tipus = newPart.Tipus;
                Cikkszam = newPart.Cikkszam;
                MennyisegiEgyseg = newPart.MennyisegiEgyseg;
                Kategoria = newPart.Kategoria;
                Leiras = newPart.Leiras;
            }
        }
        private async void AddPartCommandExecute()
        {
            if (CheckValidation())
            {
                Alkatresz newAlkatresz = new Alkatresz
                {
                    Foto = Foto,
                    Nev = Nev,
                    Gyarto = Gyarto,
                    GyartoId = Gyarto.Id,
                    Tipus = Tipus,
                    Cikkszam = Cikkszam,
                    MennyisegiEgyseg = MennyisegiEgyseg,
                    MennyisegiEgysegId = MennyisegiEgyseg.Id,
                    Kategoria = Kategoria,
                    KategoriaId = Kategoria.Id,
                    Leiras = Leiras
                };
                try
                {
                    Alkatresz resultAlkatresz = await service.PostAlkatresz(newAlkatresz);
                    newAlkatresz.Id = resultAlkatresz.Id;
                    if (Foto != null)
                    {
                        await service.PostFoto(newAlkatresz.Id, Foto);
                    }
                    MessagingCenter.Send(this, "Added", newAlkatresz);
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                catch (HttpRequestException HREx) { DependencyService.Get<IAlertService>().LongAlert(HREx.Message); }
                catch (TimeoutException TEx) { DependencyService.Get<IAlertService>().LongAlert(TEx.Message); }
            }
        }
        private async void SavePartCommandExecute()
        {
            if (CheckValidation())
            {
                Alkatresz newAlkatresz = new Alkatresz
                {
                    Id = alkatresz.Id,
                    Foto = Foto,
                    Nev = Nev,
                    Gyarto = Gyarto,
                    GyartoId = Gyarto.Id,
                    Tipus = Tipus,
                    Cikkszam = Cikkszam,
                    MennyisegiEgyseg = MennyisegiEgyseg,
                    MennyisegiEgysegId = MennyisegiEgyseg.Id,
                    Kategoria = Kategoria,
                    KategoriaId = Kategoria.Id,
                    Leiras = Leiras
                };
                try
                {
                    Alkatresz resultAlkatresz = await service.UpdateAlkatresz(newAlkatresz);
                    if (alkatresz.Foto != Foto)
                    {
                        await service.PostFoto(alkatresz.Id, Foto);
                        alkatresz.Foto = Foto;
                    }
                    alkatresz.Nev = Nev;
                    alkatresz.Gyarto = Gyarto;
                    alkatresz.GyartoId = Gyarto.Id;
                    alkatresz.Tipus = Tipus;
                    alkatresz.Cikkszam = Cikkszam;
                    alkatresz.MennyisegiEgyseg = MennyisegiEgyseg;
                    alkatresz.MennyisegiEgysegId = MennyisegiEgyseg.Id;
                    alkatresz.Kategoria = Kategoria;
                    alkatresz.KategoriaId = Kategoria.Id;
                    alkatresz.Leiras = Leiras;
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                catch (HttpRequestException HREx) { DependencyService.Get<IAlertService>().LongAlert(HREx.Message); }
                catch (TimeoutException TEx) { DependencyService.Get<IAlertService>().LongAlert(TEx.Message); }
            }
        }

        private async void TakePictureComandExecute()
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                if (photo == null)
                {
                    return;
                }
                var stream = await photo.OpenReadAsync();
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    eredetiFoto = ms.ToArray();
                    Foto = DependencyService.Get<IMediaService>().ResizeImageByte(eredetiFoto, 500, 500);
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                DependencyService.Get<IAlertService>().LongAlert(fnsEx.Message);
            }
            catch (PermissionException pEx)
            {
                DependencyService.Get<IAlertService>().LongAlert(pEx.Message);
            }
            catch (Exception ex)
            {
                DependencyService.Get<IAlertService>().LongAlert(ex.Message);
            }
        }
        private async void ScanBarcodeCommandExecute()
        {
            MobileBarcodeScanner scanner = new MobileBarcodeScanner();
            Result result = await scanner.Scan();
            if (result != null)
            {
                Cikkszam = result.Text;
            }
        }
        private void RotateImageCommandExecute()
        {
            if (eredetiFoto != null)
            {
                eredetiFoto = DependencyService.Get<IRotationService>().RotateImage(eredetiFoto, 90);
                Foto = DependencyService.Get<IMediaService>().ResizeImageByte(eredetiFoto, 500, 500);
            }
        }
        private bool CheckValidation()
        {
            InvalidNev = string.IsNullOrWhiteSpace(Nev);
            InvalidGyarto = Gyarto.Id == -1;
            InvalidTipus = string.IsNullOrWhiteSpace(Tipus);
            InvalidCikkszam = string.IsNullOrWhiteSpace(Cikkszam);
            InvalidMennyisegiEgyseg = MennyisegiEgyseg.Id == -1;
            InvalidKategoria = Kategoria.Id == -1;
            return !(InvalidFoto || InvalidNev || InvalidGyarto || InvalidTipus || InvalidCikkszam || InvalidMennyisegiEgyseg || InvalidKategoria);
        }
    }
}
