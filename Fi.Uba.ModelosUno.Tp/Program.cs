using Fi.Uba.ModelosUno.Tp;

string[] lines = System.IO.File.ReadAllLines(@".\problema_uno.txt");
var capacidadMaxima = Convert.ToDecimal(lines[0].Split(' ')[1]);
var dimension = Convert.ToDecimal(lines[1].Split(' ')[1]);
var posicionDemandasInicial = lines.ToList().IndexOf("DEMANDAS");
var posicionDemandasFinal = lines.ToList().IndexOf("FIN DEMANDAS");
var posicionCoordenadasInicial = lines.ToList().IndexOf("NODE_COORD_SECTION");
var posicionCoordenadasFinal = lines.ToList().IndexOf("EOF");
var coordenadas = new Dictionary<string, string>();

for (int i = posicionCoordenadasInicial + 1; i < posicionCoordenadasFinal; i++)
{
    coordenadas.Add(lines[i].Split(' ')[0].Trim(), lines[i]);
}

var bancoDemandas = new List<BancoDemanda>();

for (int i = posicionDemandasInicial + 1; i < posicionDemandasFinal; i++)
{
    var nombre = lines[i].Split(' ')[0].Trim();
    var monto = Convert.ToInt32(lines[i].Split(' ')[1].Trim());

    bancoDemandas.Add(new BancoDemanda()
    {
        Nombre = nombre,
        Monto = monto,
        CoordenadaX = Convert.ToDouble(coordenadas.FirstOrDefault(x => x.Key == nombre).Value.Split(' ')[1].Trim()),
        CoordenadaY = Convert.ToDouble(coordenadas.FirstOrDefault(x => x.Key == nombre).Value.Split(' ')[2].Trim())
    });
}

var cargaActual = 0;
List<BancoDemanda> bancosRecorrido = new List<BancoDemanda>();

// Por ahora agrego el primer banco de los que tengo demanda
var bancosPendientes = bancoDemandas;
bancosRecorrido.Add(bancoDemandas.First());

//Cual es la sucursal mas cerca.
//Tengo capacidad para atender a esa sucursal? 
//- SI: atiendo y paso a la que sigue.
//- NO: busco la siguiente sucursal cerca y vuelvo a ver si la puedo atender.
while (bancosPendientes.Count > 1)
{
    // Tomo el ultimo banco recorrido que es en donde esta el camion actualmente
    var bancoOrigen = bancosRecorrido.LastOrDefault();
    var bancosDestinosPendientes = bancosPendientes.Where(x => x != bancoOrigen);

    // Busco la sucursal a menor distancia
    var distancias = bancosDestinosPendientes.Select(x => new KeyValuePair<string, double>(x.Nombre, bancoOrigen.Distancia(x)));
    var minDistancia = distancias.Min(x => x.Value);
    var bancoMinDistancia = distancias.FirstOrDefault(x => x.Value == minDistancia);
    var bancoDemandaMinDistancia = bancosPendientes.FirstOrDefault(x => x.Nombre == bancoMinDistancia.Key);
    // Por ahora no tengo en cuenta ninguna restriccion, solo busco los bancos que tienen menor distancia
    bancosPendientes = bancosPendientes.Where(x => x != bancoOrigen).ToList();
    bancosRecorrido.Add(bancoDemandaMinDistancia);
}

foreach (var item in bancosRecorrido)
{
    Console.WriteLine(item.Nombre);
}
