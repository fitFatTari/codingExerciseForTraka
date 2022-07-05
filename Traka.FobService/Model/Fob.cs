namespace Traka.FobService.Model
{
    public class Fob
    {
        public int Position { get; set; }

        public string SerialNumber { get; set; }

        public FobStatus CurrentStatus { get; set; }

        public int? CurrentUser { get; set; }
    }
}
