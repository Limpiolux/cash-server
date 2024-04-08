using cash_server.Interfaces;
using cash_server.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace cash_server.Data
{
    public class UnidadNegocioData : IRepository<UnidadNegocio>
    {
        public void Delete(int id)
        {
            var db = new ApiDbContext();
            UnidadNegocio unidadNegocio = db.UnidadesNegocios.Find(id);
            db.UnidadesNegocios.Remove(unidadNegocio);
            db.SaveChanges();

        }

        public UnidadNegocio GetById(int id)
        {
            var db = new ApiDbContext();
            return db.UnidadesNegocios.Find(id);

        }

        public void Insert(UnidadNegocio entity)
        {
            var db = new ApiDbContext();
            db.UnidadesNegocios.Add(entity);
            db.SaveChanges();
        }

        public IEnumerable<UnidadNegocio> List()
        {

            var db = new ApiDbContext();
            var unidadesNegocios = db.UnidadesNegocios.ToList();
            return unidadesNegocios;

        }

        public void Update(UnidadNegocio entity)
        {
            var db = new ApiDbContext();
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}