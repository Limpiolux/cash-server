using cash_server.Interfaces;
using cash_server.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace cash_server.Data
{
    public class EmpleadoData : IRepository<Empleado>
    {
        public void Delete(int id)
        {
            var db = new ApiDbContext();
            Empleado empleado = db.Empleados.Find(id);
            db.Empleados.Remove(empleado);
            db.SaveChanges();

        }

        public Empleado GetById(int id)
        {
            var db = new ApiDbContext();
            return db.Empleados.Find(id);

        }

        public void Insert(Empleado entity)
        {
            var db = new ApiDbContext();
            db.Empleados.Add(entity);
            db.SaveChanges();
        }

        public IEnumerable<Empleado> List()
        {

            var db = new ApiDbContext();
            var empleados = db.Empleados.ToList();
            return empleados;


        }

        public void Update(Empleado entity)
        {
            var db = new ApiDbContext();
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}