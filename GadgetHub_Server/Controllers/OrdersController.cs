using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GadgetHub_Server.Models;

namespace GadgetHub_Server.Controllers
{
    public class OrdersController : ApiController
    {
        private GadgetHub_dbEntities3 db = new GadgetHub_dbEntities3();

        // GET: api/Orders
        public IQueryable<Orders> GetOrders()
        {
            return db.Orders;
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Orders))]
        public IHttpActionResult GetOrders(int id)
        {
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return NotFound();
            }

            return Ok(orders);
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrders(int id, Orders orders)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != orders.OrderID)
            {
                return BadRequest();
            }

            db.Entry(orders).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Orders
        [ResponseType(typeof(Orders))]
        public IHttpActionResult PostOrders(Orders orders)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Orders.Add(orders);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (OrdersExists(orders.OrderID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = orders.OrderID }, orders);
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Orders))]
        public IHttpActionResult DeleteOrders(int id)
        {
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return NotFound();
            }

            db.Orders.Remove(orders);
            db.SaveChanges();

            return Ok(orders);
        }

        public class StatusUpdateModel
        {
            public string Status { get; set; }
        }

        [HttpPatch]
        [Route("api/Orders/UpdateStatus/{id}")]
        public IHttpActionResult UpdateStatus(int id, [FromBody] StatusUpdateModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Status))
            {
                return BadRequest("Status cannot be null or empty.");
            }

            var order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = model.Status;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrdersExists(int id)
        {
            return db.Orders.Count(e => e.OrderID == id) > 0;
        }
    }
}