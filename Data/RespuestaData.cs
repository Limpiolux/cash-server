using cash_server.Interfaces;
using cash_server.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace cash_server.Data
{
    public class RespuestaData : IRepository<Respuesta>
    {

        public void Delete(int id)
        {
            var db = new ApiDbContext();
            Respuesta respuesta = db.Respuestas.Find(id);
            db.Respuestas.Remove(respuesta);
            db.SaveChanges();

        }

        public Respuesta GetById(int id)
        {
            var db = new ApiDbContext();
            return db.Respuestas.Find(id);

        }

        public void Insert(Respuesta entity)
        {
            var db = new ApiDbContext();
            db.Respuestas.Add(entity);
            db.SaveChanges();
        }

        public IEnumerable<Respuesta> List()
        {

            var db = new ApiDbContext();
            var respuestas = db.Respuestas.ToList();
            return respuestas;


        }

        public void Update(Respuesta entity)
        {
            var db = new ApiDbContext();
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}