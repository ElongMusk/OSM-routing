using Itinero;
using Itinero.IO.Osm;
using Itinero.Osm.Vehicles;
using System;
using System.IO;

namespace OSM_routing
{
    class Program
    {
        public static string SourceDataPFB = "C:\\Users\\639519\\source\\repos\\OSM routing\\OSM routing\\east-yorkshire-with-hull-latest.osm.pbf";
        public static string DBFile = "uni.routerdb";
        public static string GeoJSON = "route.geojson";

        static void Main()
        {
            CreateDB();

            RouterDb routerDb = null;
            using (var stream = new FileInfo(DBFile).OpenRead())
            {
                routerDb = RouterDb.Deserialize(stream);
            }
            var router = new Router(routerDb);

            // get a profile.
            var profile = Vehicle.Pedestrian.Fastest(); // the default OSM pedestian profile.

            //This speeds up the search (goood for mobile usage)
            routerDb.AddContracted(profile);

            // create a routerpoint from a location.
            // snaps the given location to the nearest routable edge.
            var start = router.Resolve(profile, 53.770124f, -0.371437f);
            var end = router.Resolve(profile, 53.771634f, -0.366966f);

            // calculate a route.
            var route = router.Calculate(profile, start, end);
            using (var writer = new StreamWriter(@GeoJSON))
            {
                route.WriteGeoJson(writer);
            }
        }


        public static void CreateDB()
        {
            var routerDb = new RouterDb();
            using (var stream = new FileInfo(SourceDataPFB).OpenRead())
            {
                // create the network for pedestirans.
                routerDb.LoadOsmData(stream, Vehicle.Pedestrian);
            }

            // write the routerdb to disk.
            using (var stream = new FileInfo(@DBFile).Open(FileMode.Create))
            {
                routerDb.Serialize(stream);
            }
        }
    }
}
