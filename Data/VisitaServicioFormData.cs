using cash_server.Interfaces;
using cash_server.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace cash_server.Data
{
    public class VisitaServicioFormData : IRepository<VisitaServicioForm>
    {
        public void Delete(int id)
        {
            var db = new ApiDbContext();
            VisitaServicioForm VisitaServicioForm = db.VisitaServicioForms.Find(id);
            db.VisitaServicioForms.Remove(VisitaServicioForm);
            db.SaveChanges();

        }

        public VisitaServicioForm GetById(int id)
        {
            var db = new ApiDbContext();
            return db.VisitaServicioForms.Find(id);

        }

        public void Insert(VisitaServicioForm entity)
        {
            var db = new ApiDbContext();
            db.VisitaServicioForms.Add(entity);
            db.SaveChanges();
        }

        public IEnumerable<VisitaServicioForm> List()
        {

            var db = new ApiDbContext();
            var VisitaServicioForms = db.VisitaServicioForms.ToList();
            return VisitaServicioForms;


        }

        public void Update(VisitaServicioForm entity)
        {
            var db = new ApiDbContext();
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}