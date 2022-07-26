using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Raktarkezeles.Models;
using Raktarkezeles.DTOModels;

namespace Raktarkezeles.Services
{
    public class RaktarkezelesService : IRaktarService
    { 
        private static readonly Uri serverUrl = new Uri("https://10.0.2.2:5001/");
        private static readonly HttpClient httpClient;
        static RaktarkezelesService()
        {
            HttpClientHandler handler = GetInsecureHandler();
            httpClient = new HttpClient(handler);
            httpClient.BaseAddress = serverUrl;
        }
        public RaktarkezelesService() { }
        public async Task<List<int>> GetAlkatreszek(string kereses = "")
        {
            HttpResponseMessage result = kereses == "" ? await httpClient.GetAsync("alkatreszek") : await httpClient.GetAsync("alkatreszek?kereses=" + kereses);
            result.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<List<int>>(await result.Content.ReadAsStringAsync());
        }
        public async Task<Alkatresz> GetAlkatresz(int id)
        {
            var result = await httpClient.GetAsync("alkatreszek/" + id);
            result.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<Alkatresz>(await result.Content.ReadAsStringAsync());
        }
        public async Task<Alkatresz> PostAlkatresz(Alkatresz alkatresz)
        {
            var dto = new AlkatreszDTO(alkatresz);
            var content = JsonConvert.SerializeObject(dto);
            var result = await httpClient.PostAsync("alkatreszek", new StringContent(content, Encoding.Default, "application/json"));
            result.EnsureSuccessStatusCode();
            return new Alkatresz(JsonConvert.DeserializeObject<AlkatreszDTO>(await result.Content.ReadAsStringAsync()));
        }
        public async Task<Alkatresz> UpdateAlkatresz(Alkatresz alkatresz)
        {
            var dto = new AlkatreszDTO(alkatresz);
            var content = JsonConvert.SerializeObject(dto);
            var result = await httpClient.PutAsync("alkatreszek/" + alkatresz.Id, new StringContent(content, Encoding.Default, "application/json"));
            result.EnsureSuccessStatusCode();
            alkatresz.Id = JsonConvert.DeserializeObject<AlkatreszDTO>(await result.Content.ReadAsStringAsync()).Id;
            return alkatresz;
        }
        public async Task DeleteAlkatresz(int id)
        {
            var result = await httpClient.DeleteAsync("alkatreszek/" + id);
            result.EnsureSuccessStatusCode();
        }
        public async Task<AlkatreszElofordulas> PostElofodulas(AlkatreszElofordulas elofordulas)
        {
            var dto = new AlkatreszElofordulasDTO(elofordulas);
            var content = JsonConvert.SerializeObject(dto);
            var result = await httpClient.PostAsync("elofordulasok", new StringContent(content, Encoding.Default, "application/json"));
            result.EnsureSuccessStatusCode();
            elofordulas.Id = JsonConvert.DeserializeObject<AlkatreszElofordulasDTO>(await result.Content.ReadAsStringAsync()).Id;
            return elofordulas;
        }
        public async Task UpdateElofordulasQuantity(int id, int quantity)
        {
            string patchDoc = "[{ \"op\": \"add\", \"path\": \"/mennyiseg\", \"value\": \"" + quantity + "\"}]";
            var result = await httpClient.PatchAsync("elofordulasok/" + id, new StringContent(patchDoc, Encoding.Default, "application/json-patch+json"));
            result.EnsureSuccessStatusCode();
        }
        public async Task DeleteElofordulas(int id)
        {
            var result = await httpClient.DeleteAsync("elofordulasok/" + id);
            result.EnsureSuccessStatusCode();
        }
        public async Task<ObservableCollection<Gyarto>> GetGyartok()
        {
            var result = await httpClient.GetAsync("gyartok");
            result.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<ObservableCollection<Gyarto>>(await result.Content.ReadAsStringAsync());
        }
        public async Task<ObservableCollection<Kategoria>> GetKategoriak()
        {
            var result = await httpClient.GetAsync("kategoriak");
            result.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<ObservableCollection<Kategoria>>(await result.Content.ReadAsStringAsync());
        }
        public async Task<ObservableCollection<RaktarozasiHely>> GetRaktarozasiHelyek()
        {
            var result = await httpClient.GetAsync("raktarak");
            result.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<ObservableCollection<RaktarozasiHely>>(await result.Content.ReadAsStringAsync());
        }
        public async Task<ObservableCollection<MennyisegiEgyseg>> GetMennyisegiEgysegek()
        {
            var result = await httpClient.GetAsync("egysegek");
            result.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<ObservableCollection<MennyisegiEgyseg>>(await result.Content.ReadAsStringAsync());
        }
        public async Task<byte[]> GetFoto(int alkatreszId)
        {
            var result = await httpClient.GetAsync($"alkatreszek/{alkatreszId}/foto");
            result.EnsureSuccessStatusCode();
            return await result.Content.ReadAsByteArrayAsync();
            
        }
        public async Task PostFoto(int alkatreszId, byte[] foto)
        {
            var result = await httpClient.PostAsync($"alkatreszek/{alkatreszId}/foto", new StreamContent(new MemoryStream(foto)));
            result.EnsureSuccessStatusCode();
        }

        //Ideiglenes a teszteléshez
        private static HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };
            return handler;
        }
    }
}
