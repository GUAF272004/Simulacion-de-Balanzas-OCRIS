using System;
using System.Collections.Generic;
using System.Globalization; // Necesario para arreglar lo de la coma
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion_de_Balanzas_OCRIS
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
        // ACTUALIZADO: Ahora pide el SKU del producto también
        bool EnviarTransaccion(int idBalanza, decimal peso, string usuarioRfid, string skuProducto);
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

        public bool EnviarTransaccion(int idBalanza, decimal peso, string usuarioRfid, string skuProducto)
        {
            System.Diagnostics.Debug.WriteLine($"[MOCK] Balanza {idBalanza}: {peso}kg ({skuProducto})");
            return true;
        }
    }

    public class RealServer : IServerAPI
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string _baseUrl;
        private List<Producto> _catalogoLocal;

        public RealServer(string baseUrl = "https://ocris.stellarbanana.com")
        {
            _baseUrl = baseUrl;
            _catalogoLocal = new List<Producto>
            {
                new Producto { SKU = "111", Nombre = "Maíz Palomero", PesoUnitario = 1.0m },
                new Producto { SKU = "222", Nombre = "Vasos Jumbo", PesoUnitario = 0.5m },
                new Producto { SKU = "333", Nombre = "Refresco Cola", PesoUnitario = 1.5m }
            };
        }

        public Producto ObtenerProductoPorSku(string sku)
        {
            return _catalogoLocal.FirstOrDefault(p => p.SKU == sku);
        }

        public bool EnviarTransaccion(int idBalanza, decimal peso, string usuarioRfid, string skuProducto)
        {
            try
            {
                string url = $"{_baseUrl}/api/update-weight";
                string scaleIdReal = $"RACK-A-{idBalanza.ToString("D2")}";

                // ARREGLO IMPORTANTE: CultureInfo.InvariantCulture asegura que use PUNTO (.) y no coma
                string pesoString = peso.ToString(CultureInfo.InvariantCulture);

                // Construimos el JSON incluyendo el PRODUCTO y el USUARIO
                string json = $"{{" +
                              $"\"scaleId\": \"{scaleIdReal}\", " +
                              $"\"weight\": {pesoString}, " +
                              $"\"productSku\": \"{skuProducto ?? ""}\", " +
                              $"\"userRfid\": \"{usuarioRfid ?? ""}\"" +
                              $"}}";

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"[NUBE] Éxito: {json}");
                    return true;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[NUBE] Error: {response.StatusCode} - {response.ReasonPhrase}");
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