using cash_server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace cash_server.Controllers
{
    [RoutePrefix("subitem")]
    [EnableCors(origins: "http://localhost:5173", headers: "*", methods: "*")]
    public class SubItemController: ApiController
    {
        private readonly SubItemData _subItemData;

        public SubItemController()
        {
            _subItemData = new SubItemData();
        }
        //dado un item pasado como parámetro devuelve los subitems pertenecientes al Item
        [HttpGet]
        [Route("getsubitemsbyitemid/{itemId}")]
        public IHttpActionResult GetSubItemsByItemId(int itemId)
        {
            try
            {
                var subItems = _subItemData.List().Where(s => s.ItemId == itemId).ToList();
                if (subItems.Any())
                {
                    return Json(subItems);
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new { message = "No se encontraron SubItems para el Item especificado" });
                }
            }
            catch (Exception)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = "Error interno del servidor" });
            }
        }

    }
}