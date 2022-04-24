using Fi.Uba.ModelosUno.Tp;
using Fi.Uba.ModelosUno.Tp.Entidades;

var archivoEntrada = @".\problema_uno.txt";
var archivoResultado = @".\resultado.txt";

BancoArchivoEntrada entrada = new LectorArchivosBancos().Leer(archivoEntrada);
List<BancoDemanda> bancosRecorridos = new RecorridoBancos().ObtenerRecorridoMasCorto(entrada.Bancos, entrada.CapacidadMaximaDeTranporte);

if (bancosRecorridos.Count != entrada.Bancos.Count)
{
    Console.WriteLine("No es posible realizar el recorrido.");
}
else
{
    using (TextWriter tw = new StreamWriter(archivoResultado))
    {
        foreach (var banco in bancosRecorridos)
        {
            Console.Write($"{banco.Nombre} ");
            tw.Write($"{banco.Nombre} ");
        }
    };
}


