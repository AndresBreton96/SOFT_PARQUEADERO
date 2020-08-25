using System;

namespace Transversales.Modelos.VehiclesRegistration
{
    public class Bills
    {
        public int BillId { get; set; }
        public int Consecutive { get; set; }
        public string LicensePlate { get; set; }
        public double ParkingCharged { get; set; }
        public double ParkingTime { get; set; }
        public int EntryTicketId { get; set; }
        public int DepartureTicketId { get; set; }
        public double Rate { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime BillDate { get; set; }
    }
}
