using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
//kysett
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using yzu_course_info_API.Models;

namespace yzu_course_info_API
{

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 設定和服務

            // Web API 路由
            /*
            config.MapHttpAttributeRoutes();
            
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );*/

            //kysett
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Course>("Courses");
            builder.EntitySet<CourseRoom>("CourseRooms");
            builder.EntitySet<Member>("Members");
            builder.EntitySet<Comment>("Comments");
            builder.EntitySet<Teacher>("Teachers");
            config.Routes.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix:"api",
                model: builder.GetEdmModel()
                );

        }
    }
}
