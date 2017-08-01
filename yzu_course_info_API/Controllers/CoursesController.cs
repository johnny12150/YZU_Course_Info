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
    public class CoursesController : ODataController
    {
        private Entities db = new Entities();

        // GET: api/Courses
        [EnableQuery]
        public IQueryable<Course> GetCourses()
        {
            return db.Course;
        }

        // GET: api/Courses(5)
        [EnableQuery]
        public SingleResult<Course> GetCourse([FromODataUri] string key)
        {
            return SingleResult.Create(db.Course.Where(course => course.cId == key));
        }

        // PUT: api/Courses(5)
        public IHttpActionResult Put([FromODataUri] string key, Delta<Course> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Course course = db.Course.Find(key);
            if (course == null)
            {
                return NotFound();
            }

            patch.Put(course);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(course);
        }

        // POST: api/Courses
        public IHttpActionResult Post(Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Course.Add(course);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CourseExists(course.cId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(course);
        }

        // PATCH: api/Courses(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] string key, Delta<Course> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Course course = db.Course.Find(key);
            if (course == null)
            {
                return NotFound();
            }

            patch.Patch(course);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(course);
        }

        // DELETE: api/Courses(5)
        public IHttpActionResult Delete([FromODataUri] string key)
        {
            Course course = db.Course.Find(key);
            if (course == null)
            {
                return NotFound();
            }

            db.Course.Remove(course);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: api/Courses(5)/Comment
        [EnableQuery]
        public IQueryable<Comment> GetComment([FromODataUri] string key)
        {
            return db.Course.Where(m => m.cId == key).SelectMany(m => m.Comment);
        }

        // GET: api/Courses(5)/Teacher
        [EnableQuery]
        public SingleResult<Teacher> GetTeacher([FromODataUri] string key)
        {
            return SingleResult.Create(db.Course.Where(m => m.cId == key).Select(m => m.Teacher));
        }

        // GET: api/Courses(5)/CourseRoom
        [EnableQuery]
        public IQueryable<CourseRoom> GetCourseRoom([FromODataUri] string key)
        {
            return db.Course.Where(m => m.cId == key).SelectMany(m => m.CourseRoom);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CourseExists(string key)
        {
            return db.Course.Count(e => e.cId == key) > 0;
        }
    }
}
