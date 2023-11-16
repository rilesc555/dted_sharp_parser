// See https://aka.ms/new-console-template for more information
using latlon;
using tile;

Tile sampleTile = new Tile("/Users/riley/Projects/dted parse/dted/test/data/n41_w071_1arc_v3.dt2");

Console.WriteLine($"{sampleTile.DSI.SouthWestCorner.Latitude}, {sampleTile.DSI.SouthWestCorner.Longitude}");
Console.WriteLine($"{sampleTile.DSI.NorthEastCorner.Latitude}, {sampleTile.DSI.SouthEastCorner.Longitude}");    

Console.WriteLine(sampleTile.getElevation(new LatLon(41.5, -70.5)));
