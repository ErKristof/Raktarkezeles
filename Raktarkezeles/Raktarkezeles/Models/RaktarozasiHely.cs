namespace Raktarkezeles.Models
{
    public class RaktarozasiHely
    {
        public int Id { get; set; }
        public string Nev { get; set; }
        public RaktarozasiHely()
        {
            Id = -1;
            Nev = "";
        }
    }
}
