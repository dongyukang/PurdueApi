﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using PurdueIo.Models.Catalog;
using System.Web.OData.Batch;

namespace PurdueIo
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services
			// Configure Web API to use only bearer token authentication.
			config.SuppressDefaultHostAuthentication();
			config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

			// Use camel case for JSON data.
			config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);

			config.AddODataQueryFilter();
			config.MapODataServiceRoute("odata", "odata", model: GetModel());
		}
		public static Microsoft.OData.Edm.IEdmModel GetModel()
		{
			//OData
			ODataConventionModelBuilder builder = new ODataConventionModelBuilder();

			//builder.ContainerName = "ApplicationDbContext";

			EntitySetConfiguration<Course> courseEnt = builder.EntitySet<Course>("Courses");
			builder.EntitySet<Class>("Classes");
			builder.EntitySet<Section>("Sections");
			builder.EntitySet<Term>("Terms");
			builder.EntitySet<Campus>("Campuses");
			builder.EntitySet<Building>("Buildings");
			builder.EntitySet<Room>("Rooms");
			builder.EntitySet<Instructor>("Instructors");
			builder.EntitySet<Meeting>("Meetings");
			builder.EntitySet<Subject>("Subjects");

			//builder.name

			//Course Functions
			FunctionConfiguration courseByTermFunc;
			courseByTermFunc = courseEnt.EntityType.Collection.Function("ByTerm");
			courseByTermFunc.Parameter<String>("Term");
			courseByTermFunc.ReturnsCollectionFromEntitySet<Course>("Courses");

			FunctionConfiguration courseByNumberFunc;
			courseByNumberFunc = courseEnt.EntityType.Collection.Function("ByNumber");
			courseByNumberFunc.Parameter<String>("Number");
			courseByNumberFunc.ReturnsCollectionFromEntitySet<Course>("Courses");

			FunctionConfiguration courseByTermAndNumberFunc;
			courseByTermAndNumberFunc = courseEnt.EntityType.Collection.Function("ByTermAndNumber");
			courseByTermAndNumberFunc.Parameter<String>("Term");
			courseByTermAndNumberFunc.Parameter<String>("Number");
			courseByTermAndNumberFunc.ReturnsCollectionFromEntitySet<Course>("Courses");

			return builder.GetEdmModel();
		}
	}
}
