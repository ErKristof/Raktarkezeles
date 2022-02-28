using System;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace Raktarkezeles.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        string name;
        ObservableCollection<TempPart> parts = new ObservableCollection<TempPart>();

        public ObservableCollection<TempPart> Parts
        {
            get
            {
                return parts;
            }
        }

        public string Name
        {
            get => name;
            set
            {
                if (name == value)
                    return;
                name = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainPageViewModel()
        {
            name = "test";
            parts.Add(new TempPart() { Name = "Sorkapocs 4es valami nagyon hosszú szövegvgggggggg", Gyarto = "Siemens", Kategoria = "Sorkapocs", Mennyiseg = 16245 });
            parts.Add(new TempPart() { Name = "Tápegység", Gyarto = "WAGO", Kategoria = "Tápegység", Mennyiseg = 345 });
            parts.Add(new TempPart() { Name = "Sorkapocs véglap", Gyarto = "Siemens", Kategoria = "Véglap", Mennyiseg = 1 });
            parts.Add(new TempPart() { Name = "Tápegység", Gyarto = "Phoenix Contact", Kategoria = "Tápegység", Mennyiseg = 1210 });
            parts.Add(new TempPart() { Name = "Sorkapocs 4es", Gyarto = "Phoenix Contac", Kategoria = "Sorkapocs", Mennyiseg = 343 });
            parts.Add(new TempPart() { Name = "Sorkapocs", Gyarto = "Siemens", Kategoria = "Sorkapocs", Mennyiseg = 56 });
            parts.Add(new TempPart() { Name = "Sorkapocs véglap", Gyarto = "Phoenix Contact", Kategoria = "Véglap", Mennyiseg = 98 });
            parts.Add(new TempPart() { Name = "Véglap", Gyarto = "WAGO", Kategoria = "Véglap", Mennyiseg = 6 });
            parts.Add(new TempPart() { Name = "Sorkapocs véglap", Gyarto = "Siemens", Kategoria = "Véglap", Mennyiseg = 657 });
            parts.Add(new TempPart() { Name = "Sorkapocs", Gyarto = "Phoenix Contact", Kategoria = "Sorkapocs", Mennyiseg = 37 });
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class TempPart
    {
        public string Name { get; set; }
        public string Gyarto { get; set; }
        public string Kategoria { get; set; }
        public int Mennyiseg { get; set; }
    }
}
