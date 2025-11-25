using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Globalization;

namespace Simulacion_de_Balanzas_OCRIS
{
    public class Producto
    {
        public string SKU { get; set; }
        public string Nombre { get; set; }
        // public decimal PesoUnitario { get; set; } <--- ELIMINADO para forzar peso dinámico
        public decimal UmbralMin { get; set; }
        public decimal UmbralMax { get; set; }
    }

    public class BalanzaFisica
    {
        public int IdHardware { get; set; }
        public int IndiceLogico { get; set; }
        public string SkuProducto { get; set; }
        public decimal PesoActual { get; set; }
        public bool LedEncendido { get; set; }
        public bool ProductoAsignado => !string.IsNullOrEmpty(SkuProducto);
    }

    public interface IServerAPI
    {
        Producto ObtenerProductoPorSku(string sku);
        // Actualizamos la firma para incluir el SKU
        bool EnviarTransaccion(int idBalanza, decimal peso, string usuarioRfid, string skuProducto);
    }

    // Tu MockServer actualizado para que no de error si quieres usarlo luego
    public class MockServer : IServerAPI
    {
        private List<Producto> _catalogo;
        public MockServer() { /* ... tu código de lista dummy ... */ }

        public Producto ObtenerProductoPorSku(string sku) => _catalogo.FirstOrDefault(p => p.SKU == sku);

        public bool EnviarTransaccion(int idBalanza, decimal peso, string usuarioRfid, string skuProducto)
        {
            System.Diagnostics.Debug.WriteLine($"[MOCK] ID:{idBalanza} Peso:{peso} SKU:{skuProducto} User:{usuarioRfid}");
            return true;
        }
    }

    // Tu RealServer corregido y limpio
    public class RealServer : IServerAPI
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string _baseUrl;
        private List<Producto> _catalogoLocal; // Mantenemos catalogo local para info rápida (nombres, pesos)

        public RealServer(string baseUrl)
        {
            _baseUrl = baseUrl;
            // Datos semilla locales necesarios para la simulación visual (arrastrar y soltar)
            // NOTA: Se eliminó PesoUnitario porque ahora el peso se pide dinámicamente al usuario.
            _catalogoLocal = new List<Producto>
    {
        new Producto { SKU = "111", Nombre = "Maíz Palomero", UmbralMin=2, UmbralMax=10 },
        new Producto { SKU = "222", Nombre = "Vasos Jumbo", UmbralMin=1, UmbralMax=20 },
        new Producto { SKU = "333", Nombre = "Refresco Cola", UmbralMin=3, UmbralMax=15 }
    };
        }

        public Producto ObtenerProductoPorSku(string sku)
        {
            // En una app real, esto también podría venir de una API GET /products/{sku}
            return _catalogoLocal.FirstOrDefault(p => p.SKU == sku);
        }

        public bool EnviarTransaccion(int idBalanza, decimal peso, string usuarioRfid, string skuProducto)
        {
            try
            {
                string url = $"{_baseUrl}/api/update-weight";
                // Generamos ID tipo "RACK-A-01" basado en el ID físico
                string scaleIdReal = $"RACK-A-{idBalanza.ToString("D2")}";
                string pesoString = peso.ToString(CultureInfo.InvariantCulture);

                string json = $"{{" +
                              $"\"scaleId\": \"{scaleIdReal}\", " +
                              $"\"weight\": {pesoString}, " +
                              $"\"productSku\": \"{skuProducto ?? ""}\", " +
                              $"\"userRfid\": \"{usuarioRfid ?? ""}\"" +
                              $"}}";

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Usamos .Result para mantenerlo síncrono por ahora (en simulación está bien)
                var response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"[NUBE] Éxito: {json}");
                    return true;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[NUBE] Error: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NUBE] Excepción: {ex.Message}");
                return false;
            }
        }
    }
}