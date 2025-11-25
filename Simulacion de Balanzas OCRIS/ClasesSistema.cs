using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Globalization;

namespace Simulacion_de_Balanzas_OCRIS
{
    // --- ENTIDADES ---

    public class Producto
    {
        public string SKU { get; set; }
        public string Nombre { get; set; }
        // Nota: PesoUnitario eliminado para soportar peso variable/dinámico
        public decimal UmbralMin { get; set; }
        public decimal UmbralMax { get; set; }
    }

    public class BalanzaFisica
    {
        public int IdHardware { get; set; }      // Puerto físico (0-7)
        public int IndiceLogico { get; set; }    // Número asignado con Pinpad
        public string SkuProducto { get; set; }
        public decimal PesoActual { get; set; }
        public bool LedEncendido { get; set; }
        public bool ProductoAsignado => !string.IsNullOrEmpty(SkuProducto);

    }

    // --- INTERFAZ DEL SERVIDOR ---

    public interface IServerAPI
    {
        Producto ObtenerProductoPorSku(string sku);
        bool EnviarTransaccion(int idBalanza, decimal peso, string usuarioRfid, string skuProducto);

        // Métodos para Estatus y Monitoreo
        bool EnviarHeartbeat(string rackId, string status);
        bool EnviarEstatusBalanza(int idBalanza, string estatus);
    }

    // --- IMPLEMENTACIÓN REAL (HTTP) ---

    public class RealServer : IServerAPI
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string _baseUrl;
        private List<Producto> _catalogoLocal;

        public RealServer(string baseUrl)
        {
            _baseUrl = baseUrl;
            // Catálogo base (siempre cargan estos por defecto)
            _catalogoLocal = new List<Producto>
            {
                new Producto { SKU = "111", Nombre = "Maíz Palomero", UmbralMin=2, UmbralMax=10 },
                new Producto { SKU = "222", Nombre = "Vasos Jumbo", UmbralMin=1, UmbralMax=20 },
                new Producto { SKU = "333", Nombre = "Refresco Cola", UmbralMin=3, UmbralMax=15 }
            };
        }

        public Producto ObtenerProductoPorSku(string sku)
        {
            // Busca primero en memoria local
            return _catalogoLocal.FirstOrDefault(p => p.SKU == sku);
        }

        // Método para registrar productos nuevos creados dinámicamente en el simulador
        public void RegistrarProductoLocal(string sku, string nombre)
        {
            if (!_catalogoLocal.Any(p => p.SKU == sku))
            {
                // Asignamos umbrales por defecto a los productos nuevos
                _catalogoLocal.Add(new Producto { SKU = sku, Nombre = nombre, UmbralMin = 0, UmbralMax = 100 });
            }
        }

        // 1. Envío de Latido (Heartbeat) del Rack completo
        // MÉTODO ACTUALIZADO
        public bool EnviarHeartbeat(string rackId, string status)
        {
            try
            {
                // Endpoint: /api/rack-status
                string url = $"{_baseUrl}/api/rack-status";
                // Inyectamos el estatus dinámicamente (ACTIVE o SHUTDOWN)
                string json = $"{{\"rackId\": \"{rackId}\", \"status\": \"{status}\", \"timestamp\": \"{DateTime.Now:O}\"}}";

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Enviamos (bloqueante para la simulación, idealmente async)
                var response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"[RACK] {rackId} -> {status}");
                    return true;
                }
                return false;
            }
            catch (Exception) { return false; }
        }

        // 2. Envío de Estatus Individual (para Apagar balanza o Keep-Alive individual)
        public bool EnviarEstatusBalanza(int idBalanza, string estatus)
        {
            try
            {
                string url = $"{_baseUrl}/api/scale-status";
                string scaleIdReal = $"RACK-A-{idBalanza.ToString("D2")}";

                string json = $"{{\"scaleId\": \"{scaleIdReal}\", \"status\": \"{estatus}\", \"timestamp\": \"{DateTime.Now:O}\"}}";
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine($"[PING] {scaleIdReal} -> {estatus}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[PING ERROR] {ex.Message}");
                return false;
            }
        }

        // 3. Envío de Transacción de Peso
        public bool EnviarTransaccion(int idBalanza, decimal peso, string usuarioRfid, string skuProducto)
        {
            try
            {
                string url = $"{_baseUrl}/api/update-weight";
                string scaleIdReal = $"RACK-A-{idBalanza.ToString("D2")}";
                // Usamos InvariantCulture para asegurar que el decimal use punto (.)
                string pesoString = peso.ToString(CultureInfo.InvariantCulture);

                string json = $"{{" +
                              $"\"scaleId\": \"{scaleIdReal}\", " +
                              $"\"weight\": {pesoString}, " +
                              $"\"productSku\": \"{skuProducto ?? ""}\", " +
                              $"\"userRfid\": \"{usuarioRfid ?? ""}\"" +
                              $"}}";

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode) return true;

                System.Diagnostics.Debug.WriteLine($"[NUBE] Error: {response.StatusCode}");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NUBE] Excepción: {ex.Message}");
                return false;
            }
        }
    }
}