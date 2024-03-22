using cash_server.Interfaces;
using cash_server.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace cash_server.Data
{
    public class UsuarioData : IRepository<Usuario>
    {
        public void Delete(int id)
        {
            var db = new ApiDbContext();
            Usuario usuario = db.Users.Find(id);
            db.Users.Remove(usuario);
            db.SaveChanges();

        }

        public Usuario GetById(int id)
        {
            var db = new ApiDbContext();
            return db.Users.Find(id);

        }

        public void Insert(Usuario entity)
        {
            var db = new ApiDbContext();
            db.Users.Add(entity);
            db.SaveChanges();
        }

        public IEnumerable<Usuario> List()
        {

            var db = new ApiDbContext();
            var usuarios = db.Users.ToList();
            return usuarios;


        }

        public void Update(Usuario entity)
        {
            var db = new ApiDbContext();
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}