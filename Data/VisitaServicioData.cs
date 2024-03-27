﻿using cash_server.Interfaces;
using cash_server.Models;
using cash_server.SharedKernel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace cash_server.Data
{
    public class VisitaServicioData : IRepository<VisitaServicio>
    {
        public void Delete(int id)
        {
            var db = new ApiDbContext();
            VisitaServicio VisitaServicio = db.VisitaServicios.Find(id);
            db.VisitaServicios.Remove(VisitaServicio);
            db.SaveChanges();

        }

        public VisitaServicio GetById(int id)
        {
            var db = new ApiDbContext();
            return db.VisitaServicios.Find(id);

        }

        public void Insert(VisitaServicio entity)
        {
            using (var db = new ApiDbContext())
            {
                //devuelve el primer elemento de una secuencia que cumple con una condición específica.
                var supervisor = db.Empleados.FirstOrDefault(e => e.Id == entity.SupervisorId && e.Rol == RolEmpleado.Supervisor);
                if (supervisor != null)
                {
                    entity.Supervisor = supervisor;
                }
                else
                {
                    throw new Exception($"El Supervisor con Id {entity.SupervisorId} no fue encontrado en la base de datos o no tiene el rol correcto.");
                }

                //el preventor se comenta porque ya no pertenece mas al modelo Visita
                /*var preventor = db.Empleados.FirstOrDefault(e => e.Id == entity.PreventorId && e.Rol == RolEmpleado.Preventor);
                if (preventor != null)
                {
                    entity.Preventor = preventor;
                }
                else
                {
                    throw new Exception($"El Preventor con Id {entity.PreventorId} no fue encontrado en la base de datos o no tiene el rol correcto.");
                }*/

                var usuario = db.Users.FirstOrDefault(u => u.Id == entity.UsuarioId);
                if (usuario != null)
                {
                    entity.Usuario = usuario;
                    db.Entry(entity.Usuario).State = EntityState.Unchanged;
                }
                else
                {
                    throw new Exception($"El Usuario con Id {entity.UsuarioId} no fue encontrado en la base de datos.");
                }

                // Agregar formulario (si existen) esto ver si se cargan desp
                //esta lista en el modelo la comente
                /*if (entity.Formularios != null && entity.Formularios.Any())
                {
                    foreach (var formulario in entity.Formularios)
                    {
                        // Agregar formulario
                        db.Entry(formulario).State = EntityState.Added;
                    }
                }*/

                // Agregar visita de servicio
                db.VisitaServicios.Add(entity);

                // Guardar cambios
                db.SaveChanges();
            }
        }

        public IEnumerable<VisitaServicio> List()
        {

            var db = new ApiDbContext();
            var VisitaServicios = db.VisitaServicios.ToList();
            return VisitaServicios;


        }

        public void Update(VisitaServicio entity)
        {
            var db = new ApiDbContext();
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}