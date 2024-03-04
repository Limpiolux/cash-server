using cash_server.Data;
using Microsoft.Extensions.DependencyInjection;

namespace cash_server
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Registrar FormularioData como un servicio de ámbito
            services.AddScoped<FormularioData>();

            // Otros servicios y configuraciones
        }

        // Otros métodos de configuración
    }
}