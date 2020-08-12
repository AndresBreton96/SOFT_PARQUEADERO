using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace Transversales.Modelos
{
    public class Rates
    {
        public byte RateId { get; set; }
        public string Description { get; set; }
        public RateType RateType { get; set; }
        public double Value { get; set; }
        public double Time { get; set; }
        public string Name
        {
            get
            {
                return RateType.ToString();
            }
        }
    }

    public enum RateType : byte
    {
        Hora = 1,
        Fraccion = 2
    }
}
