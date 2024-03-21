using cash_server.Interfaces;
using cash_server.Models;
using cash_server.SharedKernel;
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
        //verifica si existe un empleado (supervisor) con email y si esta activo, si existe, retorna el empleado
        public Empleado GetByEmailAndActivoTrue(string email)
        {
            var db = new ApiDbContext();
            return db.Empleados.FirstOrDefault(e => e.Email == email && e.Activo && e.Rol == RolEmpleado.Supervisor);
        }

        public void Insert(Empleado entity)
        {
            var db = new ApiDbContext();
            var nuevoEmpleado = new Empleado();
            //si la entidad viene con Id (porque viene de un endpoint de datos, en este caso del endpoint de supervisores
            //armo un nueva entidad con solo las propiedades que necesito, el Id no lo necesito
            if (entity.Id != 0){
                nuevoEmpleado.Nombre = entity.Nombre;
                nuevoEmpleado.Email = entity.Email;
                nuevoEmpleado.Rol = entity.Rol;
                nuevoEmpleado.Activo = entity.Activo;
                nuevoEmpleado.Usuario = null;
                db.Empleados.Add(nuevoEmpleado);
                db.SaveChanges();
            }
            else
            {
                //si no sigue el flujo normal de inserción
                db.Empleados.Add(entity);
                db.SaveChanges();
            }

        }

        public IEnumerable<Empleado> List()
        {

            var db = new ApiDbContext();
            var empleados = db.Empleados.ToList();
            return empleados;


        }
        /*dejo por las dudas si se necesita tener dos listas separadas
         pero en realidad se debe llamar a List() y ahi en el resultado de los
        datos se filtra por preventor o supervisor
        public IEnumerable<Empleado> ListPreventores()
        {
            var db = new ApiDbContext();
            var preventores = db.Empleados.Where(e => e.Rol == RolEmpleado.Preventor).ToList();
            return preventores;
        }

        public IEnumerable<Empleado> ListSupervisores()
        {
            var db = new ApiDbContext();
            var supervisores = db.Empleados.Where(e => e.Rol == RolEmpleado.Supervisor).ToList();
            return supervisores;
        }
         */

        public void Update(Empleado entity)
        {
            var db = new ApiDbContext();
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}