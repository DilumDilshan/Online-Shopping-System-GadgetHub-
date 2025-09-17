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
    public class OrderRequestsController : ApiController
    {
        private GadgetHub_dbEntities3 db = new GadgetHub_dbEntities3();

        // GET: api/OrderRequests
        public IQueryable<OrderRequests> GetOrderRequests()
        {
            return db.OrderRequests;
        }

        // GET: api/OrderRequests/5
        [ResponseType(typeof(OrderRequests))]
        public IHttpActionResult GetOrderRequests(int id)
        {
            OrderRequests orderRequests = db.OrderRequests.Find(id);
            if (orderRequests == null)
            {
                return NotFound();
            }

            return Ok(orderRequests);
        }

        // PUT: api/OrderRequests/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrderRequests(int id, OrderRequests orderRequests)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != orderRequests.OrderRequestID)
            {
                return BadRequest();
            }

            db.Entry(orderRequests).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderRequestsExists(id))
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

        // POST: api/OrderRequests
        [ResponseType(typeof(OrderRequests))]
        public IHttpActionResult PostOrderRequests(OrderRequests orderRequests)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.OrderRequests.Add(orderRequests);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (OrderRequestsExists(orderRequests.OrderRequestID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = orderRequests.OrderRequestID }, orderRequests);
        }

        // DELETE: api/OrderRequests/5
        [ResponseType(typeof(OrderRequests))]
        public IHttpActionResult DeleteOrderRequests(int id)
        {
            OrderRequests orderRequests = db.OrderRequests.Find(id);
            if (orderRequests == null)
            {
                return NotFound();
            }

            db.OrderRequests.Remove(orderRequests);
            db.SaveChanges();

            return Ok(orderRequests);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderRequestsExists(int id)
        {
            return db.OrderRequests.Count(e => e.OrderRequestID == id) > 0;
        }
    }
}