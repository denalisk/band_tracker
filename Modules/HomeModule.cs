using Nancy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Nancy.ViewEngines.Razor;

namespace MyApp
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ =>
            {
                return View["index.cshtml"];
            }
        }
    }
}
