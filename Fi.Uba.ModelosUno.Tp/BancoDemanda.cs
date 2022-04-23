using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fi.Uba.ModelosUno.Tp
{
    public class BancoDemanda
    {
        public string Nombre { get; set; }
        public int Monto { get; set; }

        public double CoordenadaX { get; set; }
        public double CoordenadaY { get; set; }


        public double Distancia(BancoDemanda bancoDestino) {
            //La distancia entre las sucursales i y j es sqrt((Xi-Xj)^2+(Yi-Yj)^2)
            var inicial = Math.Pow(this.CoordenadaX - bancoDestino.CoordenadaX, 2);
            var final = Math.Pow(this.CoordenadaY - bancoDestino.CoordenadaY, 2);
            return Math.Sqrt(inicial + final);
        }

    }
}
