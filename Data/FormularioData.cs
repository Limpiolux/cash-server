using cash_server.Interfaces;
using cash_server.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace cash_server.Data
{
    public class FormularioData : IRepository<Formulario>
    {
        public void Delete(int id)
        {
            var db = new ApiDbContext();
            Formulario formulario = db.Formularios.Find(id);
            db.Formularios.Remove(formulario);
            db.SaveChanges();

        }

        public Formulario GetById(int id)
        {
            var db = new ApiDbContext();
            return db.Formularios.Find(id);

        }

        public void Insert(Formulario entity)
        {
            var db = new ApiDbContext();
            db.Formularios.Add(entity);
            db.SaveChanges();
        }

        public IEnumerable<Formulario> List()
        {

            var db = new ApiDbContext();
            var formularios = db.Formularios.ToList();
            return formularios;


        }

        public void Update(Formulario entity)
        {
            var db = new ApiDbContext();
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }
    
    }
}