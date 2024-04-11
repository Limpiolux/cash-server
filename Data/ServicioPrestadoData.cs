using cash_server.Interfaces;
using cash_server.Models;
using cash_server.SharedKernel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace cash_server.Data
{
    public class ServicioPrestadoData : IRepository<ServicioPrestado>
    {
        public void Delete(int id)
        {
            var db = new ApiDbContext();
            ServicioPrestado servicioPrestado = db.ServiciosPrestados.Find(id);
            db.ServiciosPrestados.Remove(servicioPrestado);
            db.SaveChanges();

        }

        public ServicioPrestado GetById(int id)
        {
            var db = new ApiDbContext();
            return db.ServiciosPrestados.Find(id);

        }
        //verifica si existe existe una casa (servicio prestado) con el mismo nombre y nrodecasa y que este activo
        public ServicioPrestado GetByCasaNroyNombre(ServicioPrestado casa)
        {
            var db = new ApiDbContext();
            return db.ServiciosPrestados.
                FirstOrDefault(c => c.CasaNro == casa.CasaNro && c.Activo && casa.CasaNombre == c.CasaNombre );
        }
        public void Insert(ServicioPrestado entity)
        {
            var db = new ApiDbContext();
            db.ServiciosPrestados.Add(entity);
            db.SaveChanges();
        }

        public IEnumerable<ServicioPrestado> List()
        {

            var db = new ApiDbContext();
            var serviciosPrestados = db.ServiciosPrestados.Include(sp => sp.UnidadNegocio).ToList();
            return serviciosPrestados;


        }

        public void Update(ServicioPrestado entity)
        {
            var db = new ApiDbContext();
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}