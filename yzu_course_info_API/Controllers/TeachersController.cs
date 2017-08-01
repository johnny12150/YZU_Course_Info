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
    public class TeachersController : ODataController
    {
        private Entities db = new Entities();

        // GET: api/Teachers
        [EnableQuery]
        public IQueryable<Teacher> GetTeachers()
        {
            return db.Teacher;
        }

        // GET: api/Teachers(5)
        [EnableQuery]
        public SingleResult<Teacher> GetTeacher([FromODataUri] string key)
        {
            return SingleResult.Create(db.Teacher.Where(teacher => teacher.tId == key));
        }

        // PUT: api/Teachers(5)
        public IHttpActionResult Put([FromODataUri] string key, Delta<Teacher> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Teacher teacher = db.Teacher.Find(key);
            if (teacher == null)
            {
                return NotFound();
            }

            patch.Put(teacher);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(teacher);
        }

        // POST: api/Teachers
        public IHttpActionResult Post(Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Teacher.Add(teacher);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (TeacherExists(teacher.tId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(teacher);
        }

        // PATCH: api/Teachers(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] string key, Delta<Teacher> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Teacher teacher = db.Teacher.Find(key);
            if (teacher == null)
            {
                return NotFound();
            }

            patch.Patch(teacher);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(teacher);
        }

        // DELETE: api/Teachers(5)
        public IHttpActionResult Delete([FromODataUri] string key)
        {
            Teacher teacher = db.Teacher.Find(key);
            if (teacher == null)
            {
                return NotFound();
            }

            db.Teacher.Remove(teacher);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: api/Teachers(5)/Course
        [EnableQuery]
        public IQueryable<Course> GetCourse([FromODataUri] string key)
        {
            return db.Teacher.Where(m => m.tId == key).SelectMany(m => m.Course);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TeacherExists(string key)
        {
            return db.Teacher.Count(e => e.tId == key) > 0;
        }
    }
}
