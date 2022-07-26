namespace Raktarkezeles.Models
{
    public class Gyarto
    {
        public int Id { get; set; }
        public string RovidNev { get; set; }
        public string TeljesNev { get; set; }
        public Gyarto()
        {
            Id = -1;
            RovidNev = "";
            TeljesNev = "";
        }
    }
}
