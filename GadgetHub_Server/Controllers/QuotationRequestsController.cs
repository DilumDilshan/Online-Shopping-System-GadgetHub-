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
    public class QuotationRequestsController : ApiController
    {
        private GadgetHub_dbEntities3 db = new GadgetHub_dbEntities3();

        // GET: api/QuotationRequests
        public IQueryable<QuotationRequests> GetQuotationRequests()
        {
            return db.QuotationRequests;
        }

        // GET: api/QuotationRequests/5
        [ResponseType(typeof(QuotationRequests))]
        public IHttpActionResult GetQuotationRequests(int id)
        {
            QuotationRequests quotationRequests = db.QuotationRequests.Find(id);
            if (quotationRequests == null)
            {
                return NotFound();
            }

            return Ok(quotationRequests);
        }

        // PUT: api/QuotationRequests/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutQuotationRequests(int id, QuotationRequests quotationRequests)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != quotationRequests.QuotationRequestID)
            {
                return BadRequest();
            }

            db.Entry(quotationRequests).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuotationRequestsExists(id))
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

        // POST: api/QuotationRequests
        [ResponseType(typeof(QuotationRequests))]
        public IHttpActionResult PostQuotationRequests(QuotationRequests quotationRequests)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.QuotationRequests.Add(quotationRequests);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (QuotationRequestsExists(quotationRequests.QuotationRequestID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = quotationRequests.QuotationRequestID }, quotationRequests);
        }

        // DELETE: api/QuotationRequests/5
        [ResponseType(typeof(QuotationRequests))]
        public IHttpActionResult DeleteQuotationRequests(int id)
        {
            QuotationRequests quotationRequests = db.QuotationRequests.Find(id);
            if (quotationRequests == null)
            {
                return NotFound();
            }

            db.QuotationRequests.Remove(quotationRequests);
            db.SaveChanges();

            return Ok(quotationRequests);
        }

        public class StatusUpdateModel
        {
            public string Status { get; set; }
        }

        [HttpPatch]
        [Route("api/QuotationRequests/UpdateStatus/{id}")]
        public IHttpActionResult UpdateStatus(int id, [FromBody] StatusUpdateModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Status))
            {
                return BadRequest("Status cannot be null or empty.");
            }

            var quotationRequest = db.QuotationRequests.Find(id);
            if (quotationRequest == null)
            {
                return NotFound();
            }

            quotationRequest.Status = model.Status;
            db.Entry(quotationRequest).State = EntityState.Modified;
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

        private bool QuotationRequestsExists(int id)
        {
            return db.QuotationRequests.Count(e => e.QuotationRequestID == id) > 0;
        }
    }
}