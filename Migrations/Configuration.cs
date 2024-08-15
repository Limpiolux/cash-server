namespace cash_server.Migrations
{
    using Antlr.Runtime.Misc;
    using cash_server.Models;
    using cash_server.SharedKernel;
    using Microsoft.Win32;
    using Org.BouncyCastle.Crypto.Macs;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Web.Helpers;
    using System.Web.Services.Description;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using WebGrease.Css.Ast.MediaQuery;

    internal sealed class Configuration : DbMigrationsConfiguration<cash_server.Data.ApiDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }
        //En el metodo seed se deben cargar los datos parametricos que van en las tablas parametricas de la bd
        //si se necesita modificar algun dato (texto) se debe hacer desde acá, crear un a migración y correr la migración para que se modifique dicha tabla
        protected override void Seed(cash_server.Data.ApiDbContext context)
        {

            IList<Formulario> formularios = new List<Formulario>();
            //carga de formularios
            formularios.Add(new Formulario()
            {
                Numero = 1,
                Nombre = "Documentación del Servicio",
                version = 16
            });

            formularios.Add(new Formulario()
            {
                Numero = 2,
                Nombre = "Cartelería del Servicio",
                version = 16
            });

            formularios.Add(new Formulario()
            {
                Numero = 3,
                Nombre = "Almacenamiento de Productos",
                version = 16
            });
            formularios.Add(new Formulario()
            {
                Numero = 4,
                Nombre = "Máquinas de Limpieza",
                version = 16
            });
            formularios.Add(new Formulario()
            {
                Numero = 5,
                Nombre = "Vestuarios y/o Area de Descanso o para Cambiarse",
                version = 16
            });
            formularios.Add(new Formulario()
            {
                Numero = 6,
                Nombre = "Básico de Seguridad",
                version = 16
            });
            formularios.Add(new Formulario()
            {
                Numero = 7,
                Nombre = "Control del Vehículo",
                version = 16
            });


            foreach (Formulario formulario in formularios)
            {
                context.Formularios.AddOrUpdate(f => f.Numero, formulario);
            }

            base.Seed(context);

            //carga de Items
            var items = new List<Item>()
            {
                //formulario 1 Documentacion de servicio
                new Item { FormId = 1, Descripcion = "Indicar el estado de los siguientes documentos que deben estar en la carpeta del Servicio" }, //itemid=1
                new Item { FormId = 1, Descripcion = "Indicar el estado de los siguientes Formularios" }, //itemid=2
                new Item { FormId = 1, Descripcion = "Registros" }, //itemid=3
                new Item { FormId = 1, Descripcion = "Registros del cliente" }, //itemid=4 

                //formulario 2 item1
                new Item { FormId = 2, Descripcion = "Completar el formulario de Cartelería de Servicio" }, //itemid=5
               
                //formulario 3 item1
                new Item { FormId = 3, Descripcion = "Por favor indique las condiciones de Almacenamiento de los Productos" }, //itemid=6
                //item2
                new Item { FormId = 3, Descripcion = "Por favor indique las condiciones Generales del Pañol" }, //itemid=7

                //formulario 4
                new Item { FormId = 4, Descripcion = "Indique el Estado de las Máquinas" }, //itemid=8
                new Item { FormId = 4, Descripcion = "Indique la Cantidad de las Máquinas (LIMPIOLUX / CEILING / Distmaster)" }, //itemid=9
                new Item { FormId = 4, Descripcion = "Indique la Cantidad de Máquinas (Fbm)" }, //itemid=10
                new Item { FormId = 4, Descripcion = "Indique los siguientes aspectos del Sector de Carga de Baterias" }, //itemid=11
                new Item { FormId = 4, Descripcion = "Habilitación de manejo de máquinas (Si el servicio no posee este tipo de maquinas, indicar No Aplica" }, //itemid=12

                //formulario 5
                new Item { FormId = 5, Descripcion = "Indique el Estado de vestuarios y Areas de Descanso" }, //itemid=13
                
                //formulario 6
                new Item { FormId = 6, Descripcion = "Personal" }, //itemid=14
                new Item { FormId = 6, Descripcion = "Ambiente de Trabajo" }, //itemid=15
                new Item { FormId = 6, Descripcion = "Señalizacion" }, //itemid=16
                new Item { FormId = 6, Descripcion = "Productos" }, //itemid=17
                new Item { FormId = 6, Descripcion = "Elementos de proteccion personal"}, //itemid=18

                //formulario 7 - control de vehiculo
                new Item { FormId = 7, Descripcion = "Completar el formulario de Control del Vehículo" }, //itemid=19
                new Item { FormId = 7, Descripcion = "Completar el formulario de Elementos de Seguridad" }, //itemid=20
              

            };

            //se utiliza FormId y Descripcion como elementos para comparar contra otro elemento en la bd, para q no se guarden repetidos
            foreach (var item in items)
            {
                context.Items.AddOrUpdate(i => new { i.FormId, i.Descripcion }, item);
            }

            context.SaveChanges();


            //carga de subItems

            var subItems = new List<SubItem>()
            {
                    //formulario1
                    //subitems que pertenecen al Item 1
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
                   

                    //SubItems que pertenecen al Item 2
                    new SubItem { ItemId = 2, Descripcion = "Recepción de Documentos (RG 4.2-02)", Comentario = null },
                    new SubItem { ItemId = 2, Descripcion = "Registro de Incidente Personal (RG 4.4.7-01 )", Comentario = null },
                    new SubItem { ItemId = 2, Descripcion = "Registro de Accidente Personal (RG 4.4.7-02)", Comentario = null },
                    new SubItem { ItemId = 2, Descripcion = "Registro de Accidente con Daño material (RG 4.4.7-03)", Comentario = null },
                    new SubItem { ItemId = 2, Descripcion = "Registro de Accidente Ambiental. (RG 4.4.7-10)", Comentario = null },
                    new SubItem { ItemId = 2, Descripcion = "Registro de Incidente Ambiental (RG 4.4.7-11)", Comentario = null },
                    new SubItem { ItemId = 2, Descripcion = "Informe de Simulacro (RG 4.4.7-04)", Comentario = null },
                    new SubItem { ItemId = 2, Descripcion = "Minuta Reunión (RG 5.5.3-01)", Comentario = null },
                    new SubItem { ItemId = 2, Descripcion = "Constancia de Capacitación (RG 6.2-01)", Comentario = null },
                    new SubItem { ItemId = 2, Descripcion = "Solicitud de Materiales y Máquinas (RG 7.4.1-01)", Comentario = null },
                    new SubItem { ItemId = 2, Descripcion = "Solicitud de Uniformes y Elementos de Seguridad (RG 7.4.1-02)", Comentario = null },
                    new SubItem { ItemId = 2, Descripcion = "Constancia de Entrega Ropa de Trabajo y EPP (RG 7.4.1-12)", Comentario = null },
                    new SubItem { ItemId = 2, Descripcion = "Planilla de Frecuencia de Tareas (RG 7.5.3-01)", Comentario = null },
                    new SubItem { ItemId = 2, Descripcion = "Diagrama del Servicio (RG 7.5.3-02)", Comentario = null },
                    new SubItem { ItemId = 2, Descripcion = "Tareas Diarias (RG 7.5.3-03)", Comentario = null },
                    new SubItem { ItemId = 2, Descripcion = "Tareas Diarias por Sectores (RG 7.5.3-04)", Comentario = null },
                    new SubItem { ItemId = 2, Descripcion = "Verificación y liberación del servicio (RG 7.5.3-06)", Comentario = null },
                    new SubItem { ItemId = 2, Descripcion = "Registro de Inventario de Máquinas (RG 7.5.3-07)", Comentario = null },
                    new SubItem { ItemId = 2, Descripcion = "Control de Máquinas (RG 7.5.3-08)", Comentario = null },
                    new SubItem { ItemId = 2, Descripcion = "Ingresos y Salidas (RG 7.5.3-09)", Comentario = null },
                    new SubItem { ItemId = 2, Descripcion = "Estado de Máquinas (RG 7.5.3-10)", Comentario = null },
                    new SubItem { ItemId = 2, Descripcion = "Planilla de Control de Baños (RG 7.5.3-11)", Comentario = null },
                    new SubItem { ItemId = 2, Descripcion = "Conformidad de Servicio (RG 7.5.3-25)", Comentario = null },
                    new SubItem { ItemId = 2, Descripcion = "Inspección de Servicio Supervisor (Limpiolux y Ceiling) RG 8.2.4-01 / (FBM y Distmaster) RG.PG.CO.08.01", Comentario = null },
                    new SubItem { ItemId = 2, Descripcion = "Inspección de Servicio Gerente Zonal (RG 8.2.4-02)", Comentario = null },
                    new SubItem { ItemId = 2, Descripcion = "Participación y Consulta (RG 5.5.3-05)", Comentario = null },
                    new SubItem { ItemId = 2, Descripcion = "Permisos de Trabajo (RG.PG.CO.18.01)", Comentario = null },

                    //SubItems que pertenecen al Item 3
                    new SubItem { ItemId = 3, Descripcion = "Matriz de Aspectos e Impactos (RG 4.3.1-01 ) (Limpiolux y Ceiling)", Comentario = null },
                    new SubItem { ItemId = 3, Descripcion = "Matriz de Riesgos y su Significancia (RG 4.3.1-02 ) (Limpiolux y Ceiling)", Comentario = null },
                    new SubItem { ItemId = 3, Descripcion = "Listado, identificación y evaluación de AA y RO( RG.PG.CO.01.01) (FBM y Distmaster)", Comentario = null },
                    new SubItem { ItemId = 3, Descripcion = "ATS (RG 4.4.6-01) (Limpiolux y Ceiling)", Comentario = null },
                    new SubItem { ItemId = 3, Descripcion = "Cuaderno de Novedades (Limpiolux / Ceiling)\r\n", Comentario = null },
                    new SubItem { ItemId = 3, Descripcion = "Planilla de Frecuencia de Tareas (RG 7.5.3-01) (Limpiolux y Ceiling)", Comentario = null },
                    new SubItem { ItemId = 3, Descripcion = "Listado de Sustancias (RG.PG.CO.13.01) FBM", Comentario = null },
                    new SubItem { ItemId = 3, Descripcion = "Diagrama del Servicio (RG 7.5.3-02 ) (Limpiolux y Ceiling)", Comentario = null },
                    new SubItem { ItemId = 3, Descripcion = "Tareas Diarias (RG 7.5.3-03 ) (Limpiolux y Ceiling)", Comentario = null },
                    new SubItem { ItemId = 3, Descripcion = "Tareas Diarias por Sectores (RG 7.5.3-04 ) (Limpiolux y Ceiling)", Comentario = null },
                    new SubItem { ItemId = 3, Descripcion = "Constancia de Capacitacion (RG 6.2-01)", Comentario = null },
                    new SubItem { ItemId = 3, Descripcion = "Constancia de Entrega Ropa de Trabajo y EPP (RG 7.4.1-12)", Comentario = null },
                    new SubItem { ItemId = 3, Descripcion = "Matriz de Uso de EPP por Puesto y Tarea (RG 7.4.1-13 ) FBM", Comentario = null },
                    new SubItem { ItemId = 3, Descripcion = "Permisos de Trabajo (RG.PG.CO.18.01) FBM", Comentario = null },
                    new SubItem { ItemId = 3, Descripcion = "Registro de Incidente Personal (RG 4.4.7-01)\r\n", Comentario = null },
                    new SubItem { ItemId = 3, Descripcion = "Registro de Accidente Personal (RG 4.4.7-02)", Comentario = null },
                    new SubItem { ItemId = 3, Descripcion = "Registro de Accidente con Daño material (RG 4.4.7-03)", Comentario = null },
                    new SubItem { ItemId = 3, Descripcion = "Registro de Accidente Ambiental. (RG 4.4.7-10)", Comentario = null },
                    new SubItem { ItemId = 3, Descripcion = "Registro de Incidente Ambiental (RG 4.4.7-11)", Comentario = null },
                    new SubItem { ItemId = 3, Descripcion = "Participación y Consulta (RG 5.5.3-05)", Comentario = null },
                    new SubItem { ItemId = 3, Descripcion = "Informe de Simulacro (RG 4.4.7-04 )", Comentario = null },
                    new SubItem { ItemId = 3, Descripcion = "Ingresos y Salidas (RG 7.5.3-09 ) (Limpiolux y Ceiling)", Comentario = null },
                    new SubItem { ItemId = 3, Descripcion = "Registro de Inventario de Máquinas (RG 7.5.3-07 ) (Limpiolux y Ceiling)", Comentario = null },
                    new SubItem { ItemId = 3, Descripcion = "Planilla de Control de Baños (RG 7.5.3-11 ) (Limpiolux y Ceiling)", Comentario = null },
                    new SubItem { ItemId = 3, Descripcion = "Hojas de Seguridad", Comentario = null },
                    new SubItem { ItemId = 3, Descripcion = "Plan Anual de Capacitación (RG 6.2-04)", Comentario = null },

                    //SubItems que pertenecen al Item 4
                    new SubItem { ItemId = 4, Descripcion = "El servicio posee documentos propios del clientes, indique cuales", Comentario = null },

                     /*fin formulario 1*/

                    //formulario 2 - tiene un solo itemid = 5
                    //SubItems que pertenecen al Item 1 id=5 formulario 2 
                    new SubItem { ItemId = 5, Descripcion = "Cartel de la Política del SGI", Comentario = null },
                    new SubItem { ItemId = 5, Descripcion = "Cartel Objetivos/Metas del SGI", Comentario = null },
                    new SubItem { ItemId = 5, Descripcion = "Cartel indicador del Punto de Encuentro (RG 4.4.7-05)", Comentario = null },
                    new SubItem { ItemId = 5, Descripcion = "Cartel de la ART", Comentario = null },
                    new SubItem { ItemId = 5, Descripcion = "Cartel con los Nros. de Emergencia (RG 4.4.7-06)", Comentario = null },
                    new SubItem { ItemId = 5, Descripcion = "Cartel de la Tabla de Dilución (RG 7.5.3-12) (Limpiolux / Ceiling)", Comentario = null },
                    new SubItem { ItemId = 5, Descripcion = "Cartel Listado de Sustancias (RG.PG.CO.13.01) (Fbm)", Comentario = null },
                    new SubItem { ItemId = 5, Descripcion = "Cartel de la Matriz de Uso de EPP por puesto y tarea (RG 7.4.1-13) (Fbm)", Comentario = null },
                    new SubItem { ItemId = 5, Descripcion = "Carteles Indicadores de Piso Húmedo", Comentario = null },
                    new SubItem { ItemId = 5, Descripcion = "Cartel de Ingreso al Pañol", Comentario = null },
                    new SubItem { ItemId = 5, Descripcion = "Carteles de Baños Femenino y Masculino", Comentario = null },
                    new SubItem { ItemId = 5, Descripcion = "Carteles de Riesgo Electrico", Comentario = null },

                    //formulario 3
                    //subitems que pertenecen al item1 id=6 formulario 3
                    new SubItem { ItemId = 6, Descripcion = "Se encuentran adecuadamente rotulados y tapados", Comentario = null },
                    new SubItem { ItemId = 6, Descripcion = "Se encuentran almacenados en sus adecuadas bateas de contención", Comentario = null },
                    new SubItem { ItemId = 6, Descripcion = "Se distinguen mediante etiqueta los Productos Puros de los Diluídos", Comentario = null },
                    new SubItem { ItemId = 6, Descripcion = "Se utilizan únicamente recipientes autorizados", Comentario = null },
                    new SubItem { ItemId = 6, Descripcion = "Todos los productos cuentan con hojas de seguridad", Comentario = null },
                    new SubItem { ItemId = 6, Descripcion = "Los aerosoles están almacenados por separado de los demás productos", Comentario = null },
                    new SubItem { ItemId = 6, Descripcion = "Se encuentra el Kit Anti-Derrame", Comentario = null },

                    //subitems que pertenecen al item2 id=7 formulario 3
                    new SubItem { ItemId = 7, Descripcion = "Existe una apropiada contención de los productos en el sector", Comentario = null },
                    new SubItem { ItemId = 7, Descripcion = "Se utiliza algún producto Especial", Comentario = null },
                    new SubItem { ItemId = 7, Descripcion = "La Tabla de dilución está en el sector, visible y en buenas condiciones (Limpiolux / Ceiling)", Comentario = null },
                    new SubItem { ItemId = 7, Descripcion = "El Listado de Sustancias (RG.PG.CO.13.01) está en el sector, visible y en buenas condiciones (Fbm)", Comentario = null },
                    new SubItem { ItemId = 7, Descripcion = "Están en el sector las hojas de seguridad de los productos que se utilizan", Comentario = null },
                    
                    /*fin formulario 3*/
                    
                    //formulario 4 - items van del itemid=8 al itemid =12
                    //subitems que pertenecen al item1 id=8 indique estado de las maquinas - formulario 4

                    new SubItem { ItemId = 8, Descripcion = "Todas las Máquinas se Encuentran Rotuladas con su Nro. de Máquina", Comentario = null },
                    new SubItem { ItemId = 8, Descripcion = "Todas las Máquinas tienen Visible la Etiqueta de Puesta a Tierra", Comentario = null },
                    new SubItem { ItemId = 8, Descripcion = "Todas las Máquinas se encuentran en Buen Estado de Funcionamiento", Comentario = null },
                    new SubItem { ItemId = 8, Descripcion = "En alguna máquina se visualizan manchas de aceite o de algún fluido", Comentario = null },
                    new SubItem { ItemId = 8, Descripcion = "Existen en el servicio Escaleras (En caso de ser SI. Efectue el RG.PG.CO.18.02 CHECK LIST ESCALERAS Y ANDAMIOS)", Comentario = null },
                    new SubItem { ItemId = 8, Descripcion = "Existen en el servicio Andamios (En caso de ser SI. Efectue el RG.PG.CO.18.02 CHECK LIST ESCALERAS Y ANDAMIOS)", Comentario = null },
                    new SubItem { ItemId = 8, Descripcion = "Existen en el servicio autoelevadores (En caso de ser SI. Efectue el RG.PG.CASH 03.03 CHECK LIST AUTOELEVADOR)", Comentario = null },

                    //subitems que pertenecen al item2 id=9 formulario 4
                    new SubItem { ItemId = 9, Descripcion = "Lustradoras", Comentario = null },
                    new SubItem { ItemId = 9, Descripcion = "Aspiradoras", Comentario = null },
                    new SubItem { ItemId = 9, Descripcion = "Sopladoras", Comentario = null },
                    new SubItem { ItemId = 9, Descripcion = "Hidrolavadoras", Comentario = null },
                    new SubItem { ItemId = 9, Descripcion = "Lava Fregadora", Comentario = null },
                    new SubItem { ItemId = 9, Descripcion = "Lavadora", Comentario = null },
                    new SubItem { ItemId = 9, Descripcion = "Lava Alfombra Rotativa", Comentario = null },
                    new SubItem { ItemId = 9, Descripcion = "Desmalezadoras", Comentario = null },
                    new SubItem { ItemId = 9, Descripcion = "Desmalezadoras De tractor", Comentario = null },
                    new SubItem { ItemId = 9, Descripcion = "Barredoras Hombre a Bordo", Comentario = null },
                    new SubItem { ItemId = 9, Descripcion = "Lava Alfombra Inyección Extracción", Comentario = null },
                    new SubItem { ItemId = 9, Descripcion = "Motosierras", Comentario = null },
                    new SubItem { ItemId = 9, Descripcion = "Autoelevador", Comentario = null },
                    new SubItem { ItemId = 9, Descripcion = "tractor", Comentario = null },
                    
                    //subitems que pertenecen al item3 id=10 formulario 4
                    new SubItem { ItemId = 10, Descripcion = "Se deben indicar la Cantidad de Máquinas Presentes en el Servicio", Comentario = null },
                    
                    //subitems que pertenecen al item4 id=11 formulario 4
                    new SubItem { ItemId = 11, Descripcion = "Cada cargador tiene su propio conector a la red electrica", Comentario = null },
                    new SubItem { ItemId = 11, Descripcion = "Esta delimitado o demarcado el lugar de carga", Comentario = null },
                    new SubItem { ItemId = 11, Descripcion = "Se encuentran visibles los carteles de Riesgo Electrico", Comentario = null },
                    
                    //subitems que pertenecen al item5 id=12 formulario 4
                    new SubItem { ItemId = 12, Descripcion = "El personal se encuentra capacitado para el uso de maquinas hombre a bordo", Comentario = null },
                    new SubItem { ItemId = 12, Descripcion = "Posee las credenciales vigentes", Comentario = null },

                    /*fin formulario 4*/

                    /*formulario 5 - unico item itemid=13*/
                    /*subitem*/
                     new SubItem { ItemId =13, Descripcion = "El espacio para cambiarse se encuentra en buenas condiciones de conservación", Comentario = null },
                     new SubItem { ItemId =13, Descripcion = "Cuenta con duchas", Comentario = null },
                     new SubItem { ItemId =13, Descripcion = "Las duchas tienen agua caliente", Comentario = null },
                     new SubItem { ItemId =13, Descripcion = "Las duchas tienen cortina", Comentario = null },
                     new SubItem { ItemId =13, Descripcion = "Cuenta con Calefacción", Comentario = null },
                     new SubItem { ItemId =13, Descripcion = "Cuenta con Aire Acondicionado", Comentario = null },
                     new SubItem { ItemId =13, Descripcion = "Cuenta con Espejo y Bachas", Comentario = null },
                     new SubItem { ItemId =13, Descripcion = "Las condiciones de limpieza del lugar son buenas", Comentario = null },
                     new SubItem { ItemId =13, Descripcion = "La iluminación es suficiente", Comentario = null },

                     /*formulario 6 - itemid=14 personal*/
                    /*subitem*/
                     new SubItem { ItemId =14, Descripcion = "Cumple con las instrucciones de limpieza y seguridad establecidas", Comentario = null },
                     new SubItem { ItemId =14, Descripcion = "Posee conocimientos sobre el Analisis de Tarea Segura", Comentario = null },
                     new SubItem { ItemId =14, Descripcion = "Sabe como proceder en caso de accidente", Comentario = null },
                     new SubItem { ItemId =14, Descripcion = "Conoce los impactos ambientales que se generan durante su trabajo", Comentario = null },
                     new SubItem { ItemId =14, Descripcion = "Utiliza los elementos de protección personal para realizar las tareas que lo requieren", Comentario = null },

                     /*formulario 6 - itemid=15 Ambiente de trabajo*/
                     new SubItem { ItemId =15, Descripcion = "Las condiciones de luminosidad son adecuadas", Comentario = null },
                     new SubItem { ItemId =15, Descripcion = "El estado de los lockers es adecuado", Comentario = null },
                     new SubItem { ItemId =15, Descripcion = "El estado del pañol / Taller / Deposito u oficina para el personal  se encuentra en buenas condiciones", Comentario = null },
                     new SubItem { ItemId =15, Descripcion = "Todos los peligros se encuentran identificados", Comentario = null },
                     new SubItem { ItemId =15, Descripcion = "Todo impacto ambiental se encuentra identificado", Comentario = null },
                     new SubItem { ItemId =15, Descripcion = "Los extintores del  servicio se encuentran correctamente", Comentario = null },
                     new SubItem { ItemId =15, Descripcion = "Extintor es propio?", Comentario = null },


                     /*formulario 6 - itemid=16 señalizacion*/
                     new SubItem { ItemId =16, Descripcion = "Se cuenta con conos o letreros de piso humedo", Comentario = null },
                     new SubItem { ItemId =16, Descripcion = "Las zonas de trabajo se encuentran debidamente señalizadas o demarcadas", Comentario = null },
                     new SubItem { ItemId =16, Descripcion = "Se encuentra el Cartel de la ART presente en el pañol", Comentario = null },
                     new SubItem { ItemId =16, Descripcion = "Se encuentra el Cartel con los Nros. Telefónicos de Emergencias", Comentario = null },
                     new SubItem { ItemId =16, Descripcion = "Se encuentra el Cartel de la Política del SGI", Comentario = null },
                     new SubItem { ItemId =16, Descripcion = "Se encuentra el Cartel Indicador de Punto de Encuentro", Comentario = null },
                     new SubItem { ItemId =16, Descripcion = "Se encuentra el Cartel de Metas/Objetivos del SGI", Comentario = null },
                     new SubItem { ItemId =16, Descripcion = "Los peligros del servicio se encuentran señalizados y/o protegidos (ej: tableros, cables, etc.)", Comentario = null },


                     /*formulario 6 - itemid=17 productos*/
                     new SubItem { ItemId =17, Descripcion = "Los productos se encuentran rotulados, tapados y dentro de su apropiado medio de contencion", Comentario = null },
                     new SubItem { ItemId =17, Descripcion = "Se utilizan únicamente recipientes autorizados", Comentario = null },
                     new SubItem { ItemId =17, Descripcion = "Los productos cuentan con su hoja de seguridad correspondiente", Comentario = null },
                     new SubItem { ItemId =17, Descripcion = "Los productos se encuentran almacenados en sus adecuadas bateas de contención", Comentario = null },
                     new SubItem { ItemId =17, Descripcion = "Se encuentra en el servicio el Kit Antiderrame", Comentario = null },

                     /*formulario 6 - itemid=18 Proteccion personal*/
                     new SubItem { ItemId =18, Descripcion = "Todo el personal del servicio cuenta con el uniforme y ropa de trabajo adecuada", Comentario = null },
                     new SubItem { ItemId =18, Descripcion = "Se encuentran en el servicio los Elementos de Protección necesarios para realizar las tareas", Comentario = null },
                     new SubItem { ItemId =18, Descripcion = "Los Elementos de Proteccion se encuentran en buenas condiciones de uso", Comentario = null },
                     new SubItem { ItemId =18, Descripcion = "Existe suficiente cantidad de los Elementos de Proteccion para todo el personal que lo requiere", Comentario = null },
                     new SubItem { ItemId =18, Descripcion = "El Botiquín de primeros auxilios en el servicio se encuentra en condiciones", Comentario = null },
                     new SubItem { ItemId =18, Descripcion = "Botiquín es propio?", Comentario = null },
                     /*fin formulario 6*/

                     /*formulario 7 - itemid 19 -control de vehiculo*/
                     new SubItem { ItemId =19, Descripcion = "Registro de conducir", Comentario = null },
                     new SubItem { ItemId =19, Descripcion = "Seguro de vehículo", Comentario = null },
                     new SubItem { ItemId =19, Descripcion = "Cédula Azul", Comentario = null },
                     new SubItem { ItemId =19, Descripcion = "VTV", Comentario = null },
                     new SubItem { ItemId =19, Descripcion = "Ruta (Certificado) ", Comentario = null },
                     new SubItem { ItemId =19, Descripcion = "Matafuegos", Comentario = null },
                     new SubItem { ItemId =19, Descripcion = "Botiquín", Comentario = null },

                     /*formulario 7 - itemid 20 - elementos seguridad*/
                     new SubItem { ItemId =20, Descripcion = "Balizas", Comentario = null },
                     new SubItem { ItemId =20, Descripcion = "Chaleco Reflectivo", Comentario = null },
                     new SubItem { ItemId =20, Descripcion = "Bocina", Comentario = null },
                     new SubItem { ItemId =20, Descripcion = "Rueda de Auxilio", Comentario = null },
                     new SubItem { ItemId =20, Descripcion = "Luces", Comentario = null },
                     new SubItem { ItemId =20, Descripcion = "Cinturón de Seguridad", Comentario = null },


            };

            foreach (var subItem in subItems)
            {
                context.SubItems.AddOrUpdate(si => new { si.ItemId, si.Descripcion }, subItem);
            }


            // Guardar cambios
            context.SaveChanges();


            //identifico los items a borrar - se ha borrado el siguiente
            var itemAntiguo = context.SubItems.FirstOrDefault(i => i.Id == 153 && i.Descripcion == "Los extintores del  servicio se encuentran OK (Extintor propio / Extintor cliente)");
            if (itemAntiguo != null)
            {
                context.SubItems.Remove(itemAntiguo);
                context.SaveChanges();
            }

            var itemAntiguo2 = context.SubItems.FirstOrDefault(i => i.Id == 171 && i.Descripcion == "El Botiquín de primeros auxilios en el servicio se encuentra OK (Botiquín propio / Botiquín cliente)");
            if (itemAntiguo2 != null)
            {
                context.SubItems.Remove(itemAntiguo2);
                context.SaveChanges();
            }

            //Cargar las respuestas que pertenecen a un item
            var respuestas = new List<Respuesta>()
            {   
                /*formulario 1*/
                new Respuesta { ItemId = 1, Descripcion = "Se encuentra en buenas condiciones" },
                new Respuesta { ItemId = 1, Descripcion = "Se encuentra deteriorado" },
                new Respuesta { ItemId = 1, Descripcion = "No se encuentra en el servicio" },
                new Respuesta { ItemId = 1, Descripcion = "Necesita modificaciones" },
                new Respuesta { ItemId = 1, Descripcion = "No aplica" },

                new Respuesta { ItemId = 2, Descripcion = "Se encuentra en buenas condiciones" },
                new Respuesta { ItemId = 2, Descripcion = "Se encuentra deteriorado" },
                new Respuesta { ItemId = 2, Descripcion = "No se encuentra en el servicio" },
                new Respuesta { ItemId = 2, Descripcion = "Necesita modificaciones" },
                new Respuesta { ItemId = 2, Descripcion = "No aplica" },

                new Respuesta { ItemId = 3, Descripcion = "Está completo y concuerda con las tareas que se realizan en la práctica" },
                new Respuesta { ItemId = 3, Descripcion = "Está completo y no concuerda con las tareas que se realizan en la práctica" },
                new Respuesta { ItemId = 3, Descripcion = "No está en el servicio" },
                new Respuesta { ItemId = 3, Descripcion = "Está deteriorado" },
                new Respuesta { ItemId = 3, Descripcion = "No aplica" },

                new Respuesta { ItemId = 4, Descripcion = "Está completo y concuerda con las tareas que se realizan en la práctica" },
                new Respuesta { ItemId = 4, Descripcion = "Está completo y no concuerda con las tareas que se realizan en la práctica" },
                new Respuesta { ItemId = 4, Descripcion = "No está en el servicio" },
                new Respuesta { ItemId = 4, Descripcion = "Está deteriorado" },
                new Respuesta { ItemId = 4, Descripcion = "No aplica" },

                /*formulario 2*/
                new Respuesta { ItemId = 5, Descripcion = "Está en buenas condiciones" },
                new Respuesta { ItemId = 5, Descripcion = "Requiere cambio por deterioro" },
                new Respuesta { ItemId = 5, Descripcion = "No está presente en el servicio" },
                new Respuesta { ItemId = 5, Descripcion = "No aplica" },

                /*formulario 3*/
                new Respuesta { ItemId = 6, Descripcion = "Si" },
                new Respuesta { ItemId = 6, Descripcion = "No" },
                new Respuesta { ItemId = 6, Descripcion = "No aplica" },

                new Respuesta { ItemId = 7, Descripcion = "Si" },
                new Respuesta { ItemId = 7, Descripcion = "No" },
                new Respuesta { ItemId = 7, Descripcion = "No aplica" },

                /*formulario 4 itemid 8 al 12*/
                new Respuesta { ItemId = 8, Descripcion = "Si" },
                new Respuesta { ItemId = 8, Descripcion = "No" },
                new Respuesta { ItemId = 8, Descripcion = "No aplica" },

                /*indique cantidad de las maquinas*/
                new Respuesta { ItemId = 9, Descripcion = "Cantidad: 1" },
                new Respuesta { ItemId = 9, Descripcion = "Cantidad: 2" },
                new Respuesta { ItemId = 9, Descripcion = "Cantidad: 3" },
                new Respuesta { ItemId = 9, Descripcion = "Cantidad: 4" },
                new Respuesta { ItemId = 9, Descripcion = "Cantidad: 5" },
                new Respuesta { ItemId = 9, Descripcion = "Cantidad: 6" },
                new Respuesta { ItemId = 9, Descripcion = "Cantidad: 7" },
                new Respuesta { ItemId = 9, Descripcion = "Cantidad: 8" },
                new Respuesta { ItemId = 9, Descripcion = "Cantidad: 10" },
                new Respuesta { ItemId = 9, Descripcion = "Más de 10" },
                
                //cantidad de maquinas Fbm
                new Respuesta { ItemId = 10, Descripcion = "Cantidad: 1" },
                new Respuesta { ItemId = 10, Descripcion = "Cantidad: 2" },
                new Respuesta { ItemId = 10, Descripcion = "Cantidad: 3" },
                new Respuesta { ItemId = 10, Descripcion = "Cantidad: 4" },
                new Respuesta { ItemId = 10, Descripcion = "Cantidad: 5" },
                new Respuesta { ItemId = 10, Descripcion = "Cantidad: 6" },
                new Respuesta { ItemId = 10, Descripcion = "Cantidad: 7" },
                new Respuesta { ItemId = 10, Descripcion = "Cantidad: 8" },
                new Respuesta { ItemId = 10, Descripcion = "Cantidad: 10" },
                new Respuesta { ItemId = 10, Descripcion = "Más de 10" },

                /*aspectos del Sector de Carga de Baterias*/
                new Respuesta { ItemId = 11, Descripcion = "Si" },
                new Respuesta { ItemId = 11, Descripcion = "No" },
                new Respuesta { ItemId = 11, Descripcion = "No aplica" },

                /*habilitacion de manejo de maquinas*/
                new Respuesta { ItemId = 12, Descripcion = "Si" },
                new Respuesta { ItemId = 12, Descripcion = "No" },
                new Respuesta { ItemId = 12, Descripcion = "No aplica" },
                /*fin formulario 4*/

                /*formulario5*/
                new Respuesta { ItemId = 13, Descripcion = "Si" },
                new Respuesta { ItemId = 13, Descripcion = "No" },
                new Respuesta { ItemId = 13, Descripcion = "No aplica" },

                /*formulario6 del 14 al 18 itemId*/
                new Respuesta { ItemId = 14, Descripcion = "Si" },
                new Respuesta { ItemId = 14, Descripcion = "No" },
                new Respuesta { ItemId = 14, Descripcion = "No se pudo verificar" },
                new Respuesta { ItemId = 14, Descripcion = "No aplica" },

                new Respuesta { ItemId = 15, Descripcion = "Si" },
                new Respuesta { ItemId = 15, Descripcion = "No" },
                new Respuesta { ItemId = 15, Descripcion = "No se pudo verificar" },
                new Respuesta { ItemId = 15, Descripcion = "No aplica" },

                new Respuesta { ItemId = 16, Descripcion = "Si" },
                new Respuesta { ItemId = 16, Descripcion = "No" },
                new Respuesta { ItemId = 16, Descripcion = "No se pudo verificar" },
                new Respuesta { ItemId = 16, Descripcion = "No aplica" },

                new Respuesta { ItemId = 17, Descripcion = "Si" },
                new Respuesta { ItemId = 17, Descripcion = "No" },
                new Respuesta { ItemId = 17, Descripcion = "No se pudo verificar" },
                new Respuesta { ItemId = 17, Descripcion = "No aplica" },

                new Respuesta { ItemId = 18, Descripcion = "Si" },
                new Respuesta { ItemId = 18, Descripcion = "No" },
                new Respuesta { ItemId = 18, Descripcion = "No se pudo verificar" },
                new Respuesta { ItemId = 18, Descripcion = "No aplica" },


                /*formulario 7*/
                new Respuesta { ItemId = 19, Descripcion = "Si" },
                new Respuesta { ItemId = 19, Descripcion = "No" },

                new Respuesta { ItemId = 20, Descripcion = "Bueno" },
                new Respuesta { ItemId = 20, Descripcion = "Regular" },
                new Respuesta { ItemId = 20, Descripcion = "Malo" }


            };

            foreach (var respuesta in respuestas)
            {   //busca en la db itemID y descripcion que no se repiran
                context.Respuestas.AddOrUpdate(r => new { r.ItemId, r.Descripcion }, respuesta);
            }

            // Guardar cambios
            context.SaveChanges();

            //voy a cargar los preventores (que son empleados)
            var empleados = new List<Empleado>()
            {
                new Empleado { Nombre = "Juárez María De los Ángeles", Email = "maar_juarez@hotmail.com.ar", Rol = RolEmpleado.Preventor, Activo = true },
                new Empleado { Nombre = "Miriam Betancourt", Email = "miriam.betancourt@limpiolux.com.ar", Rol = RolEmpleado.Preventor, Activo = true },
                new Empleado { Nombre = "Gastón Storani", Email = "gastonstorani@hotmail.com", Rol = RolEmpleado.Preventor, Activo = true },
                new Empleado { Nombre = "Cynthia Dayana Alcaraz Blanco", Email = "Alcaraz105@gmail.com", Rol = RolEmpleado.Preventor, Activo = true },
                new Empleado { Nombre = "Hernán Ingrassia", Email = "heringrassia@gmail.com", Rol = RolEmpleado.Preventor, Activo = true },
                new Empleado { Nombre = "Maria Laura Barreto", Email = "mlauri126@gmail.com", Rol = RolEmpleado.Preventor, Activo = true },
                new Empleado { Nombre = "Mariela Alegre", Email = "mariela.alegre@limpiolux.com.ar", Rol = RolEmpleado.Preventor, Activo = true },
                new Empleado { Nombre = "Julio Delgado", Email = "juliodelgado1@gmail.com", Rol = RolEmpleado.Preventor, Activo = true },
                new Empleado { Nombre = "Jesica Sanabria", Email = "jesica.sanabria@limpiolux.com.ar", Rol = RolEmpleado.Preventor, Activo = true },
                new Empleado { Nombre = "Vanesa Gomez", Email = "vanesagtorres24@gmail.com", Rol = RolEmpleado.Preventor, Activo = true },
                new Empleado { Nombre = "Adolfo Rafael Ariza Lucena", Email = "arizalucena.ar@gmail.com", Rol = RolEmpleado.Preventor, Activo = true },
                new Empleado { Nombre = "Julieta Perales", Email = "jperales-ext@limpiolux.com.ar", Rol = RolEmpleado.Preventor, Activo = true },
                new Empleado { Nombre = "Eduardo Ibiza", Email = "h.eduardoibiza@gmail.com", Rol = RolEmpleado.Preventor, Activo = true },
                new Empleado { Nombre = "Maximiliano Siñeriz", Email = "delfinfiel@gmail.com", Rol = RolEmpleado.Preventor, Activo = true },
                new Empleado { Nombre = "María Luz Irassar", Email = "luz_irassar@hotmail.com", Rol = RolEmpleado.Preventor, Activo = true },
                new Empleado { Nombre = "Yamila Guerrero", Email = "yamiguerrero2008@hotmail.com", Rol = RolEmpleado.Preventor, Activo = true },
                new Empleado { Nombre = "Sebastian Lescano", Email = "sebastianlesscano28@gmail.com", Rol = RolEmpleado.Preventor, Activo = true },
                new Empleado { Nombre = "Juan Carlos Daniel Garay", Email = "juancarlosdanielgaray@gmail.com", Rol = RolEmpleado.Preventor, Activo = true },
                /*ultimos agregados*/
                new Empleado { Nombre = "Daiana Fabre", Email = "daianaelifabre@gmail.com", Rol = RolEmpleado.Preventor, Activo = true },
                new Empleado { Nombre = "Tomas Rodriguez", Email = "Hystomasrodriguez@gmail.com", Rol = RolEmpleado.Preventor, Activo = true },
                new Empleado { Nombre = "Karla Briceño", Email = "karlabriceo53@gmail.com", Rol = RolEmpleado.Preventor, Activo = true },
                
            };

            //se usa email para comparar si existe otro registro en la db con ese mail
            foreach (var empleado in empleados)
            {
                context.Empleados.AddOrUpdate(e => e.Email, empleado);
            }

            context.SaveChanges();

            //cargo las unidades de negocio 

            IList<UnidadNegocio> unidadesNegocio = new List<UnidadNegocio>();
            //ID 1 en tabla
            unidadesNegocio.Add(new UnidadNegocio()
            {
                Nombre = "LIMPIOLUX S.A.",
                Cuit = "30540984626",
                Activo = true,

            });
            //ID 2
            unidadesNegocio.Add(new UnidadNegocio()
            {
                Nombre = "FBM S.A.",
                Cuit = null,
                Activo = true,

            });
            //ID 3
            unidadesNegocio.Add(new UnidadNegocio()
            {
                Nombre = "T&T",
                Cuit = null,
                Activo = true,

            });
            //ID 4
            unidadesNegocio.Add(new UnidadNegocio()
            {
                Nombre = "Dist Master",
                Cuit = null,
                Activo = true,

            });
            //ID 5
            unidadesNegocio.Add(new UnidadNegocio()
            {
                Nombre = "Otro Servicio",
                Cuit = null,
                Activo = true,

            });

            foreach (var unidadNegocio in unidadesNegocio)
            {
                context.UnidadesNegocios.AddOrUpdate(un => un.Nombre, unidadNegocio);
            }
           
            context.SaveChanges();


            //cargo las dos casas (servicio prestado) ficticias para T&T y DistMaster
            //estas no vienen de ninguna base de produccion asi que las cargo acá para que se carguen en la tabla de servicioPrestad

            IList<ServicioPrestado> ServiciosPrestados = new List<ServicioPrestado>();

            ServiciosPrestados.Add(new ServicioPrestado()
            {   //T&T
                ClienteNro = 0000, //generico
                ClienteNombre = "T&T",
                CasaNro = "0000", //generico
                CasaNombre = "T&T",
                UnidadNegocioId = 3,
                Localidad = null,
                Activo = true

            });


            ServiciosPrestados.Add(new ServicioPrestado()
            {   //T&T
                ClienteNro = 0000, //generico
                ClienteNombre = "Dist master",
                CasaNro = "0000", //generico
                CasaNombre = "DistMaster",
                UnidadNegocioId = 4,
                Localidad = null,
                Activo = true

            });

            ServiciosPrestados.Add(new ServicioPrestado()
            {   //T&T
                ClienteNro = 0000, //generico
                ClienteNombre = "Otro Servicio",
                CasaNro = "0000", //generico
                CasaNombre = "Otro Servicio",
                UnidadNegocioId = 5, //Unidad de negocio otro servicio
                Localidad = null,
                Activo = true

            });

            foreach (var Servicio  in ServiciosPrestados)
            {
                context.ServiciosPrestados.AddOrUpdate(un => un.CasaNombre, Servicio);
            }

            context.SaveChanges();

            //identifico las casas a borrar - se ha borrado el siguiente
            var casaAntigua = context.ServiciosPrestados.FirstOrDefault(c => c.Id == 2144 && c.ClienteNombre == "T&T" && c.CasaNombre == "Casa T&T");
            if (casaAntigua != null)
            {
                context.ServiciosPrestados.Remove(casaAntigua);
                context.SaveChanges();
            }

            var casaAntigua2 = context.ServiciosPrestados.FirstOrDefault(c => c.Id == 2145 && c.ClienteNombre == "Dist master" && c.CasaNombre == "Casa DistMaster");
            if (casaAntigua2 != null)
            {
                context.ServiciosPrestados.Remove(casaAntigua2);
                context.SaveChanges();
            }
        }

    }
}
