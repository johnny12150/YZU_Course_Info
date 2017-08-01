using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using yzu_course_info_API.Models;

namespace yzu_course_info_API.Controllers
{
    public class CommentsController : ODataController
    {
        private Entities db = new Entities();

        // GET: api/Comments
        [EnableQuery]
        public IQueryable<Comment> GetComments()
        {
            return db.Comment;
        }

        // GET: api/Comments(5)
        [EnableQuery]
        public SingleResult<Comment> GetComment([FromODataUri] string key)
        {
            //decimal k = decimal.Parse(key);
            //return SingleResult.Create(db.Comment.Where(comment => comment.comSeq == k));
            return SingleResult.Create(db.Comment.Where(comment => comment.cId == key));
        }

        // PUT: api/Comments(5)
        public IHttpActionResult Put([FromODataUri] string key, Delta<Comment> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Comment comment = db.Comment.Find(key);
            if (comment == null)
            {
                return NotFound();
            }

            patch.Put(comment);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(comment);
        }

        // POST: api/Comments
        public IHttpActionResult Post(Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Comment.Add(comment);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CommentExists(comment.cId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(comment);
        }

        // PATCH: api/Comments(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] string key, Delta<Comment> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Comment comment = db.Comment.Find(key);
            if (comment == null)
            {
                return NotFound();
            }

            patch.Patch(comment);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(comment);
        }

        // DELETE: api/Comments(5)
        public IHttpActionResult Delete([FromODataUri] string key)
        {
            //Comment comment = db.Comment.Find(key);
            decimal k = Convert.ToDecimal(key);
            Comment comment = db.Comment.Where(e => e.comSeq == k).SingleOrDefault();
            if (comment == null)
            {
                return NotFound();
            }

            db.Comment.Remove(comment);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: api/Comments(5)/Course
        [EnableQuery]
        public SingleResult<Course> GetCourse([FromODataUri] string key)
        {
            return SingleResult.Create(db.Comment.Where(m => m.cId == key).Select(m => m.Course));
        }

        // GET: api/Comments(5)/Member
        [EnableQuery]
        public SingleResult<Member> GetMember([FromODataUri] string key)
        {
            return SingleResult.Create(db.Comment.Where(m => m.cId == key).Select(m => m.Member));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommentExists(string key)
        {
            return db.Comment.Count(e => e.cId == key) > 0;
        }
    }
}
