namespace Fi.Uba.ModelosUno.Tp.Entidades
{
    public class BancoDemanda
    {
        public string Nombre { get; set; }
        public int Monto { get; set; }

        public double CoordenadaX { get; set; }
        public double CoordenadaY { get; set; }


        //La distancia entre las sucursales i y j es sqrt((Xi-Xj)^2+(Yi-Yj)^2)
        public double Distancia(BancoDemanda bancoDestino)
        {
            var inicial = Math.Pow(this.CoordenadaX - bancoDestino.CoordenadaX, 2);
            var final = Math.Pow(this.CoordenadaY - bancoDestino.CoordenadaY, 2);
            return Math.Sqrt(inicial + final);
        }

    }
}
