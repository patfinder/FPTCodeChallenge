using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherDTO
{
    public class AdviceResultDTO
    {
        public int Precip { get; set; }
        public bool GoOutside { get; set; }
        public int UvIndex { get; set; }
        public bool WearSunscreen { get; set; }
        public int WindSpeed { get; set; }
        public bool FlyKite { get; set; }
    }
}
