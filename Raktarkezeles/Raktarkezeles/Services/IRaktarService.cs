using Raktarkezeles.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Raktarkezeles.Services
{
    public interface IRaktarService
    {
        Task<List<int>> GetAlkatreszek(string kereses = "");
        Task<Alkatresz> GetAlkatresz(int id);
        Task<Alkatresz> PostAlkatresz(Alkatresz alkatresz);
        Task<Alkatresz> UpdateAlkatresz(Alkatresz alkatresz);
        Task DeleteAlkatresz(int id);
        Task<byte[]> GetFoto(int id);
        Task PostFoto(int id, byte[] foto);
        Task<AlkatreszElofordulas> PostElofodulas(AlkatreszElofordulas elofordulas);
        Task UpdateElofordulasQuantity(int id, int quantity);
        Task DeleteElofordulas(int id);
        Task<ObservableCollection<Gyarto>> GetGyartok();
        Task<ObservableCollection<Kategoria>> GetKategoriak();
        Task<ObservableCollection<RaktarozasiHely>> GetRaktarozasiHelyek();
        Task<ObservableCollection<MennyisegiEgyseg>> GetMennyisegiEgysegek();
    }
}
