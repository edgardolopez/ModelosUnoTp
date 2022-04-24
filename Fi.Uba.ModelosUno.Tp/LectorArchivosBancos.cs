using Fi.Uba.ModelosUno.Tp.Entidades;

namespace Fi.Uba.ModelosUno.Tp
{
    public class LectorArchivosBancos
    {
        public BancoArchivoEntrada Leer(string archivoEntrada)
        {
            string[] lines = System.IO.File.ReadAllLines(archivoEntrada);

            return new BancoArchivoEntrada()
            {
                CapacidadMaximaDeTranporte = Convert.ToDouble(lines[0].Split(' ')[1]),
                Dimension = Convert.ToInt32(lines[1].Split(' ')[1]),
                Bancos = MapearBancos(lines)
            };
        }

        private List<BancoDemanda> MapearBancos(string[] lines)
        {
            var bancos = new List<BancoDemanda>();

            var posicionDemandasInicial = lines.ToList().IndexOf("DEMANDAS");
            var posicionDemandasFinal = lines.ToList().IndexOf("FIN DEMANDAS");
            var posicionCoordenadasInicial = lines.ToList().IndexOf("NODE_COORD_SECTION");
            var posicionCoordenadasFinal = lines.ToList().IndexOf("EOF");
            var coordenadas = new Dictionary<string, string>();

            for (int i = posicionCoordenadasInicial + 1; i < posicionCoordenadasFinal; i++)
            {
                coordenadas.Add(lines[i].Split(' ')[0].Trim(), lines[i]);
            }

            for (int i = posicionDemandasInicial + 1; i < posicionDemandasFinal; i++)
            {
                var nombre = lines[i].Split(' ')[0].Trim();
                var monto = Convert.ToInt32(lines[i].Split(' ')[1].Trim());

                bancos.Add(new BancoDemanda()
                {
                    Nombre = nombre,
                    Monto = monto,
                    CoordenadaX = Convert.ToDouble(coordenadas.FirstOrDefault(x => x.Key == nombre).Value.Split(' ')[1].Trim()),
                    CoordenadaY = Convert.ToDouble(coordenadas.FirstOrDefault(x => x.Key == nombre).Value.Split(' ')[2].Trim())
                });
            }

            return bancos;
        }
    }
}
