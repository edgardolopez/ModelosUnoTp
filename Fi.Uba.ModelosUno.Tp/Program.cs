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
List<BancoDemanda> bancosBloqueados = new List<BancoDemanda>();

// Por ahora agrego el primer banco de los que tengo demanda
var bancosPendientes = bancoDemandas;

//Tomo el primer Banco que tenga que entregar dinero.
var primerBanco = bancoDemandas.First(x => x.Monto >= 0);
cargaActual += primerBanco.Monto;
bancosRecorrido.Add(primerBanco);
//saco el banco que ya procese de la lista
bancosPendientes = bancosPendientes.Where(x => x != primerBanco).ToList();

//Supongo que desde la sede central a todos los bancos la distancia no importa.
//Cual es la sucursal mas cerca.
//Tengo capacidad para atender a esa sucursal? 
//- SI: atiendo y paso a la que sigue.
//- NO: bloqueo esa sucursal, busco la siguiente mas cerca y vuelvo a ver si la puedo atender.
while (bancosPendientes.Count > 0)
{
    // Tomo el ultimo banco recorrido, este es el banco donde esta el camion actualmente
    var bancoActual = bancosRecorrido.LastOrDefault();

    // Busco la sucursal a menor distancia
    var distancias = bancosPendientes.Select(x => new KeyValuePair<string, double>(x.Nombre, bancoActual.Distancia(x)));
    var minDistancia = distancias.Min(x => x.Value);
    var bancoMinDistancia = distancias.FirstOrDefault(x => x.Value == minDistancia);
    var bancoDemandaMinDistancia = bancosPendientes.FirstOrDefault(x => x.Nombre == bancoMinDistancia.Key);

    //Reviso si puedo cumplir con el pedido de la sucursal.
    if (cargaActual + bancoDemandaMinDistancia.Monto >= 0 && cargaActual + bancoDemandaMinDistancia.Monto <= capacidadMaxima)
    {
        //bancosPendientes = bancosPendientes.Where(x => x != bancoActual).ToList();
        bancosRecorrido.Add(bancoDemandaMinDistancia);
        cargaActual += bancoDemandaMinDistancia.Monto;

        //Ya pude procesar el siguiente banco, vuelvo a agregar a los bloqueados.
        bancosPendientes = bancosPendientes.Concat(bancosBloqueados).ToList();
        bancosBloqueados.Clear();
    }
    else
    {
        bancosBloqueados.Add(bancoDemandaMinDistancia);
    }
    bancosPendientes = bancosPendientes.Where(x => x != bancoDemandaMinDistancia).ToList();
}

if (bancosPendientes.Count > 0)
{
    Console.WriteLine("No es posible realizar el recorrido.");
}
else
{
    using (TextWriter tw = new StreamWriter(@".\resultado.txt"))
    {
        foreach (var banco in bancosRecorrido)
        {
            Console.Write($"{banco.Nombre} ");
            tw.Write($"{banco.Nombre} ");
        }
    };
}


