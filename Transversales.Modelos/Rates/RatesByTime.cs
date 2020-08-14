namespace Transversales.Modelos
{
    public class RatesByTime
    {
        public byte RateId { get; set; }
        public string Description { get; set; }
        public RateType RateType { get; set; }
        public double Value { get; set; }
        public double Time { get; set; }
        public string Name { get; set; }
    }

    public enum RateType : byte
    {
        Hora = 1,
        Fraccion = 2
    }
}
