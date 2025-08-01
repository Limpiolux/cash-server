﻿using cash_server.Interfaces;
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
        //verifica si existe un empleado (supervisor) con email y si esta activo, si existe, retorna el empleado de limpiolux
        public Empleado GetByEmailAndActivoSupervisor(string email)
        {
            var db = new ApiDbContext();
            return db.Empleados.FirstOrDefault(e => e.Email == email && e.Activo && e.Rol == RolEmpleado.Supervisor && e.UnidadNegocio_id == 1); //es de limpio
        }
        public Empleado GetByEmailAndActivoSupervisorCeiling(string email)
        {
            var db = new ApiDbContext();
            return db.Empleados.FirstOrDefault(e => e.Email == email && e.Activo && e.Rol == RolEmpleado.Supervisor && e.UnidadNegocio_id == 6); //es de ceiling
        }

        public Empleado GetByEmailAndActivoPreventor(string email)
        {
            var db = new ApiDbContext();
            return db.Empleados.FirstOrDefault(e => e.Email == email && e.Activo && e.Rol == RolEmpleado.Supervisor);
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