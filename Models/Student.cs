using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jmedinaS3.Models
{
    public class Student
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public double Seguimiento1 { get; set; }
        public double Examen1 { get; set; }
        public double Seguimiento2 { get; set; }
        public double Examen2 { get; set; }

        public double NotaParcial1 => (Seguimiento1 * 0.3) + (Examen1 * 0.2);
        public double NotaParcial2 => (Seguimiento2 * 0.3) + (Examen2 * 0.2);
        public double NotaFinal => NotaParcial1 + NotaParcial2;

        public string Estado
        {
            get
            {
                if (NotaFinal >= 7) return "APROBADO";
                if (NotaFinal >= 5 && NotaFinal <= 6.9) return "COMPLEMENTARIO";
                return "REPROBADO";
            }
        }
    }
}
