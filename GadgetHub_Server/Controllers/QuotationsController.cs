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
    public class QuotationsController : ApiController
    {
        private GadgetHub_dbEntities3 db = new GadgetHub_dbEntities3();

        // GET: api/Quotations
        public IQueryable<Quotations> GetQuotations()
        {
            return db.Quotations;
        }

        // GET: api/Quotations/5
        [ResponseType(typeof(Quotations))]
        public IHttpActionResult GetQuotations(int id)
        {
            Quotations quotations = db.Quotations.Find(id);
            if (quotations == null)
            {
                return NotFound();
            }

            return Ok(quotations);
        }

        // PUT: api/Quotations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutQuotations(int id, Quotations quotations)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != quotations.QuotationID)
            {
                return BadRequest();
            }

            db.Entry(quotations).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuotationsExists(id))
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

        // POST: api/Quotations
        [ResponseType(typeof(Quotations))]
        public IHttpActionResult PostQuotations(Quotations quotations)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Quotations.Add(quotations);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (QuotationsExists(quotations.QuotationID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = quotations.QuotationID }, quotations);
        }

        // DELETE: api/Quotations/5
        [ResponseType(typeof(Quotations))]
        public IHttpActionResult DeleteQuotations(int id)
        {
            Quotations quotations = db.Quotations.Find(id);
            if (quotations == null)
            {
                return NotFound();
            }

            db.Quotations.Remove(quotations);
            db.SaveChanges();

            return Ok(quotations);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuotationsExists(int id)
        {
            return db.Quotations.Count(e => e.QuotationID == id) > 0;
        }
    }
}