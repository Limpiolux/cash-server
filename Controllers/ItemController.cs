﻿using cash_server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace cash_server.Controllers
{
    [RoutePrefix("item")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ItemController: ApiController
    {
        private readonly ItemData _itemData;

        public ItemController()
        {
            _itemData = new ItemData();
        }
        //dado idForm pasado como parámetro devuelve los items, formulario, y subitems pertenecientes a ese Formulario.
        [HttpGet]
        [Route("getitemsbyformid/{formId}")]
        public IHttpActionResult GetItemsByFormId(int formId)
        {
            try
            {
                var items = _itemData.List().Where(i => i.FormId == formId).ToList();
                if (items.Any())
                {
                    return Json(items);
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new { message = "No se encontraron Items para el Formulario especificado" });
                }
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
            }
        }
    }
}