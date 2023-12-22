// See https://aka.ms/new-console-template for more information
using latlon;
using tile;

Tile sampleTile = new Tile("/Users/riley/Projects/n41_w071_1arc_v3.dt2");

short ReverseBytes(short value)
    {
        return (short)((value << 8) | ((value >> 8) & 0xFF));
    }

short testShort = BitConverter.ToInt16(new byte[] { 0, 5 }, 0);

Console.WriteLine(testShort);

Console.WriteLine(ReverseBytes(testShort));

