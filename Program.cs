// follows, simplifies, and changes to C# the python package at https://github.com/bbonenfant/dted

using latlon;
using tile;

Tile sampleTile = new("/Users/riley/Projects/n41_w071_1arc_v3.dt2");

/* to find elevation at a point, first create a LatLon object using "new LatLon(latitude, longitude)", where latitude and longitude are doubles
then call tileName.getElevation(LatLon)

TODO:
* handle error values
* add checksum verification
* add find greatest function
* add find lowest function
* add find average function
* refine getting elevation at a point
 */
 
Console.WriteLine(sampleTile.getElevation(new LatLon(sampleTile.DSI.NorthWestCorner.Latitude, sampleTile.DSI.NorthWestCorner.Longitude)));
