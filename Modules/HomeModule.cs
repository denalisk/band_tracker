using Nancy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Nancy.ViewEngines.Razor;

namespace BandTrackerApp
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = _ =>
            {
                return View["index.cshtml", ModelMaker()];
            };

            Post["/bands"] = _ =>
            {
                Band newBand = new Band(Request.Form["new-band"]);
                newBand.Save();
                return View["index.cshtml", ModelMaker()];
            };

            Post["/venues"] = _ =>
            {
                Venue newVenue = new Venue(Request.Form["new-venue"]);
                newVenue.Save();
                return View["index.cshtml", ModelMaker()];
            };

            Get["/bands/{id}"] = parameters =>
            {
                Dictionary<string, object> model = ModelMaker();
                model.Add("band", Band.Find(parameters.id));
                return View["band.cshtml", model];
            };

            Get["/venues/{id}"] = parameters =>
            {
                Dictionary<string, object> model = ModelMaker();
                model.Add("venue", Venue.Find(parameters.id));
                return View["venue.cshtml", model];
            };
        }

        public static Dictionary<string, object> ModelMaker()
        {
            Dictionary<string, object> model = new Dictionary<string, object>
            {
                {"bands", Band.GetAll()},
                {"venues", Venue.GetAll()}
            };
            return model;
        }
    }
}
