namespace cash_server.Migrations
{
    using cash_server.Models;
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Web.Services.Description;
    using System.Web.UI;

    internal sealed class Configuration : DbMigrationsConfiguration<cash_server.Data.ApiDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(cash_server.Data.ApiDbContext context)
        {

            IList<Formulario> formularios = new List<Formulario>();
            //carga de formularios
            formularios.Add(new Formulario()
            {
                Numero = 1,
                Nombre = "Documentación del Servicio"
            });

            formularios.Add(new Formulario()
            {
                Numero = 2,
                Nombre = "Cartelería del Servicio"
            });

            formularios.Add(new Formulario()
            {
                Numero = 3,
                Nombre = "Almacenamiento de Productos"
            });
            formularios.Add(new Formulario()
            {
                Numero = 4,
                Nombre = "Máquinas de Limpieza"
            });
            formularios.Add(new Formulario()
            {
                Numero = 5,
                Nombre = "Vestuarios y/o Area de Descanso o para Cambiarse"
            });
            formularios.Add(new Formulario()
            {
                Numero = 6,
                Nombre = "Básico de Seguridad"
            });
            formularios.Add(new Formulario()
            {
                Numero = 7,
                Nombre = "Control del Vehículo"
            });
           

            foreach (Formulario formulario in formularios)
            {
                context.Formularios.AddOrUpdate(f => f.Numero, formulario);
            }

            base.Seed(context);

            //Creo los items que se relacionan con los formularios
            var items = new List<Item>()
            {
                new Item { FormId = 1, Descripcion = "Indicar el estado de los siguientes documentos que deben estar en la carpeta del Servicio" },
                new Item { FormId = 1, Descripcion = "Indicar el estado de los siguientes Formularios" },
                new Item { FormId = 1, Descripcion = "Registros" },

            };

            foreach (var item in items)
            {
                context.Items.AddOrUpdate(i => new { i.FormId, i.Descripcion }, item);
            }

            context.SaveChanges();

            //carga de subItems
            
            var subItems = new List<SubItem>()
                {
                    new SubItem { ItemId = 1, Descripcion = "Política Integrada del SGI (Extracción MA-01)", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Manual del Supervisor (MA 6.2-01)", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Gestión de Elementos de Protección Personal (PG 4.3.1-02)", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Control Operacional (PG 4.4.6-01)", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Prevención y Respuesta Ante Emergencias (PG 4.4.7-01)", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Comunicación (PG 5.5.3-01)", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Gestión de Sugerencias, Quejas y Reclamos (PG 5.5.4-01)", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Identificación y Trazabilidad (PG 7.5.3-01)", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Control e Inspección de los Servicios al Cliente (Limpiolux y Ceiling ) PG 8.2.4-01  / (FBM y Distmaster) PG.CO.08", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Gestión de Hallazgos , NC y ACPM (PG 8.5-01)", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Instructivo de Trabajo segun actividad desarrollada en el servicio (IT 8.2.4)", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Gestión Interna de Residuos en Servicio (IT 4.4.6-03)", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Gestión de ATS (PE 4.4.6-01)", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Reglas de Almacenamiento Pañol (IT 4.4.6-02)", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Medidas de Prevención Ergonómicas (IT 4.3.1-01)", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Gestión de Incidentes/Accidentes (PE 4.4.7-02)", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Consulta y participación (PE 5.5.3-01)", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Identificación y Evaluación de RO y AA y su Significancia (PG.CO.01) (FBM y Distmaster)", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Procedimiento FBM Operaciones (PG.CO.26)", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Gestión de Residuos (PG 4.4.6-02) FBM", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Gestión de Sustancias Peligrosas (PG.CO.13) FBM", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Trabajos con Riesgos Especiales (PG.CO.18) FBM", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Trabajos en Altura (IT.CASH.01) FBM", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Trabajos en Espacios Confinados (IT.CASH.02) FBM", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Trabajos en Caliente  (IT.CASH.03)  FBM", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Trabajos con Tensión (IT.CASH.04) FBM", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Bloqueo de Instalaciones Intervenidas(IT.CASH.05) FBM", Comentario = null },
                    new SubItem { ItemId = 1, Descripcion = "Afiche ART", Comentario = null },
                    

            };

            foreach (var subItem in subItems)
            {
                context.SubItems.AddOrUpdate(si => new { si.ItemId, si.Descripcion }, subItem);
            }

            // Guardar cambios
            context.SaveChanges();


        }



    }
}
