using System;
using System.Collections.Generic;
using System.Linq;

namespace Simulacion_de_Balanzas_OCRIS // IMPORTANTE: Cambia esto por el nombre real de tu proyecto
{
    public class Producto
    {
        public string SKU { get; set; }
        public string Nombre { get; set; }
        public decimal PesoUnitario { get; set; }
        public string RutaImagen { get; set; }
    }

    public class BalanzaFisica
    {
        public int IdHardware { get; set; } // Puerto físico del multiplexor
        public int IndiceLogico { get; set; } // Coordenada X,Y asignada manualmente [cite: 1464]
        public string SkuProducto { get; set; }
        public decimal PesoActual { get; set; }
        public bool LedEncendido { get; set; }
        public bool ProductoAsignado => !string.IsNullOrEmpty(SkuProducto);
    }

    public interface IServerAPI
    {
        Producto ObtenerProductoPorSku(string sku);
        bool EnviarTransaccion(int idBalanza, decimal peso, string usuarioRfid);
    }

    public class MockServer : IServerAPI
    {
        private List<Producto> _catalogo;

        public MockServer()
        {
            _catalogo = new List<Producto>
            {
                new Producto { SKU = "111", Nombre = "Maíz Palomero", PesoUnitario = 1.0m },
                new Producto { SKU = "222", Nombre = "Vasos Jumbo", PesoUnitario = 0.5m },
                new Producto { SKU = "333", Nombre = "Refresco Cola", PesoUnitario = 1.5m }
            };
        }

        public Producto ObtenerProductoPorSku(string sku)
        {
            return _catalogo.FirstOrDefault(p => p.SKU == sku);
        }

        public bool EnviarTransaccion(int idBalanza, decimal peso, string usuarioRfid)
        {
            // Simulación de envío de datos al backend
            System.Diagnostics.Debug.WriteLine($"[SERVER] Recibido: Balanza {idBalanza}, Peso {peso}, User {usuarioRfid} -> OK");
            return true;
        }
    }
}