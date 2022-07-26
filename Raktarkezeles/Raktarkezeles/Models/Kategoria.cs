namespace Raktarkezeles.Models
{
    public class Kategoria
    {
        public int Id { get; set; }
        public string Nev { get; set; }
        public Kategoria()
        {
            Id = -1;
            Nev = "";
        }
    }
}
