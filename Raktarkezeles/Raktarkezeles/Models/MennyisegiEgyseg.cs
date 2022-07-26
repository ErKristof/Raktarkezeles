namespace Raktarkezeles.Models
{
    public class MennyisegiEgyseg
    {
        public int Id { get; set; }
        public string RovidNev { get; set; }
        public string TeljesNev { get; set; }
        public MennyisegiEgyseg()
        {
            Id = -1;
            RovidNev = "";
            TeljesNev = "";
        }
    }
}
