using cash_server.Interfaces;
using cash_server.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace cash_server.Data
{
    public class ItemData : IRepository<Item>
    {
        public void Delete(int id)
        {
            var db = new ApiDbContext();
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
            db.SaveChanges();

        }

        public Item GetById(int id)
        {
            var db = new ApiDbContext();
            return db.Items.Find(id);

        }

        public void Insert(Item entity)
        {
            var db = new ApiDbContext();
            db.Items.Add(entity);
            db.SaveChanges();
        }

        public IEnumerable<Item> List()
        {

            var db = new ApiDbContext();
            var items = db.Items.ToList();
            return items;


        }

        public void Update(Item entity)
        {
            var db = new ApiDbContext();
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}