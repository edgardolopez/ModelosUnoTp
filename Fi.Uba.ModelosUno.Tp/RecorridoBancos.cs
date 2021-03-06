using Fi.Uba.ModelosUno.Tp.Entidades;

namespace Fi.Uba.ModelosUno.Tp
{
    public class RecorridoBancos
    {
        public List<BancoDemanda> ObtenerRecorridoMasCorto(List<BancoDemanda> bancos, double capacidadMaxima)
        {
            var cargaActual = 0;
            var bancosRecorridos = new List<BancoDemanda>();
            var bancosBloqueados = new List<BancoDemanda>();
            var bancosPendientes = bancos;

            //Tomo el primer Banco que tenga que entregar dinero.
            var bancosConMontoMayorACero = bancosPendientes.Where(x => x.Monto >= 0).ToList();

            //// Pondero la distancia de los bancos, le doy mas importancia a los que tienen mas bancos cerca, tomando el que tiene min de la suma de las distancia al resto.
            //// calculo la distancia entre ellos.
            //var bancoPoderacionDistancias = new Dictionary<BancoDemanda, double>();
            //bancosConMontoMayorACero.ForEach(bancoPendiente =>
            //{
            //    var distancias = bancosPendientes.Select(x => new KeyValuePair<string, double>(x.Nombre, bancoPendiente.Distancia(x))).Sum(y => y.Value);
            //    bancoPoderacionDistancias.Add(bancoPendiente, distancias);
            //});
            //var minDistanciaPonderada = bancoPoderacionDistancias.Min(x => x.Value);
            //var bancoSeleccionado = bancoPoderacionDistancias.First(x=> x.Value == minDistanciaPonderada);
            //var primerBanco = bancosConMontoMayorACero.First( x=> x.Nombre == bancoSeleccionado.Key.Nombre);

            // Tomo como primer banco el que tenga mas cerca un banco.
            // calculo la distancia entre ellos.
            var bancoPoderacionDistancias = new Dictionary<BancoDemanda, double>();
            bancosConMontoMayorACero.ForEach(bancoPendiente =>
            {
                var distancias = bancosPendientes.Select(x => new KeyValuePair<string, double>(x.Nombre, bancoPendiente.Distancia(x))).Min(x=> x.Value);
                bancoPoderacionDistancias.Add(bancoPendiente, distancias);
            });

            var minDistanciaPonderada = bancoPoderacionDistancias.Min(x => x.Value);
            var bancoSeleccionado = bancoPoderacionDistancias.First(x => x.Value == minDistanciaPonderada);
            var primerBanco = bancosConMontoMayorACero.First(x => x.Nombre == bancoSeleccionado.Key.Nombre);

            cargaActual += primerBanco.Monto;
            bancosRecorridos.Add(primerBanco);
             
            //Saco el banco que ya procese de la lista
            bancosPendientes = bancosPendientes.Where(x => x != primerBanco).ToList();

            //Supongo que desde la sede central a todos los bancos la distancia no importa.
            //Cual es la sucursal mas cerca.
            //Tengo capacidad para atender a esa sucursal? 
            //- SI: atiendo la sucursal y paso a la que sigue.
            //- NO: bloqueo esa sucursal, busco la siguiente mas cerca y vuelvo a ver si la puedo atender.
            while (bancosPendientes.Count > 0)
            {
                // Tomo el ultimo banco recorrido, este es el banco donde esta el camion actualmente
                var bancoActual = bancosRecorridos.LastOrDefault();

                // Busco la sucursal a menor distancia
                var distancias = bancosPendientes.Select(x => new KeyValuePair<string, double>(x.Nombre, bancoActual.Distancia(x)));
                var minDistancia = distancias.Min(x => x.Value);
                var bancoMinDistancia = distancias.FirstOrDefault(x => x.Value == minDistancia);
                var bancoDemandaMinDistancia = bancosPendientes.FirstOrDefault(x => x.Nombre == bancoMinDistancia.Key);

                //Reviso si puedo cumplir con el pedido de la sucursal.
                if (cargaActual + bancoDemandaMinDistancia.Monto >= 0 && cargaActual + bancoDemandaMinDistancia.Monto <= capacidadMaxima)
                {
                    bancosRecorridos.Add(bancoDemandaMinDistancia);
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

            return bancosRecorridos;
        }
    }
}
