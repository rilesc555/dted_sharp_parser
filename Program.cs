// See https://aka.ms/new-console-template for more information
using latlon;
using tile;

Tile sampleTile = new("/Users/riley/Projects/n41_w071_1arc_v3.dt2");

Console.WriteLine(sampleTile.getElevation(new LatLon(sampleTile.DSI.NorthWestCorner.Latitude, sampleTile.DSI.NorthWestCorner.Longitude)));
