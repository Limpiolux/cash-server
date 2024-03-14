using cash_server.Interfaces;
using cash_server.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace cash_server.Data
{
    public class SubItemData : IRepository<SubItem>
    {
        public void Delete(int id)
        {
            var db = new ApiDbContext();
            SubItem subItem = db.SubItems.Find(id);
            db.SubItems.Remove(subItem);
            db.SaveChanges();

        }

        public SubItem GetById(int id)
        {
            var db = new ApiDbContext();
            return db.SubItems.Find(id);

        }

        public void Insert(SubItem entity)
        {
            var db = new ApiDbContext();
            db.SubItems.Add(entity);
            db.SaveChanges();
        }

        public IEnumerable<SubItem> List()
        {

            var db = new ApiDbContext();
            var subItems = db.SubItems.ToList();
            return subItems;


        }

        public void Update(SubItem entity)
        {
            var db = new ApiDbContext();
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}