using System;
using System.IO;
using System.Text;
using definitions;
using latlon;

namespace dsi {
public class DataSetIdentification
    {

    public string? SecurityCode { get; init; }
    public byte[]? ReleaseMarkings { get; init; }
    public string? HandlingDescription { get; init; }
    public string? ProductLevel { get; init; }
    public byte[]? Reference { get; init; }
    public int? Edition { get; init; }
    public string? MergeVersion { get; init; }
    public DateTime? MaintenanceDate { get; init; }
    public DateTime? MergeDate { get; init; }
    public byte[]? MaintenanceCode { get; init; }
    public byte[]? ProducerCode { get; init; }
    public byte[]? ProductSpecification { get; init; }
    public DateTime? SpecificationDate { get; init; }
    public string? VerticalDatum { get; init; }
    public string? HorizontalDatum { get; init; }
    public string? CollectionSystem {  get; protected set; }
    public DateTime? CompilationDate { get; init; }

    public LatLon? Origin { get; init; }
    public LatLon? SouthWestCorner { get; init; }
    public LatLon? NorthWestCorner { get; init; }
    public LatLon? NorthEastCorner { get; init; }
    public LatLon? SouthEastCorner { get; init; }
    public float? Orientation { get; init; }
    public float LatitudeInterval { get; init; }
    public float LongitudeInterval { get; init; }
    public Tuple<int, int>? Shape { get; init; }
    public float? Coverage { get; init; }
    private byte[]? Data { get; init; }



    private static readonly byte[] _SENTINEL = "DSI"u8.ToArray();

    // Private constructor
    private DataSetIdentification() {}

    public static DataSetIdentification FromBytes(byte[] data)
    {
        if (data.Length < Helpers.DSI_SIZE)
        {
            throw new ArgumentException($"The Data Set Identification record is {Helpers.DSI_SIZE} bytes but was provided {data.Length} bytes");
        }

        using (var bufferedData = new MemoryStream(data)) 
        {
            var sentinel = new byte[3];
            bufferedData.Read(sentinel, 0, 3);
            if (!Helpers.CompareArrays(sentinel, _SENTINEL))
            {
                throw new Helpers.InvalidFileException("Data Set Identification Records must start with 'DSI'");
            }
            
            string SecurityCode = Helpers.ReadString(bufferedData, 1);
            byte[] ReleaseMarkings = Helpers.ReadBytes(bufferedData, 2);
            string HandlingDescription = Helpers.ReadString(bufferedData, 27);
            byte[] _data = Helpers.ReadBytes(bufferedData, 26);
            string ProductLevel = Helpers.ReadString(bufferedData, 5);
            byte[] Reference = Helpers.ReadBytes(bufferedData, 15);
            _data = Helpers.ReadBytes(bufferedData, 8);
            int Edition = Helpers.ReadInt(bufferedData, 2);
            string MergeVersion = Helpers.ReadString(bufferedData, 1);
            DateTime? MaintenanceDate = Helpers.ReadDate(bufferedData, 4);
            DateTime? MergeDate = Helpers.ReadDate(bufferedData, 4);
            byte[] MaintenanceCode = Helpers.ReadBytes(bufferedData, 4);
            byte[] ProducerCode = Helpers.ReadBytes(bufferedData, 8);
            _data = Helpers.ReadBytes(bufferedData, 16);
            byte[] ProductSpecification = Helpers.ReadBytes(bufferedData, 11);
            DateTime? SpecificationDate = Helpers.ReadDate(bufferedData, 4);
            string VerticalDatum = Helpers.ReadString(bufferedData, 3);
            string HorizontalDatum = Helpers.ReadString(bufferedData, 5);
            string CollectionSystem = Helpers.ReadString(bufferedData, 10);
            DateTime? CompilationDate = Helpers.ReadDate(bufferedData, 4);
            _data = Helpers.ReadBytes(bufferedData, 22);
            LatLon Origin = LatLon.FromDted(Helpers.ReadString(bufferedData, 9), Helpers.ReadString(bufferedData, 10));
            LatLon SouthWestCorner = LatLon.FromDted(Helpers.ReadString(bufferedData, 7), Helpers.ReadString(bufferedData, 8));
            LatLon NorthWestCorner = LatLon.FromDted(Helpers.ReadString(bufferedData, 7), Helpers.ReadString(bufferedData, 8));
            LatLon NorthEastCorner = LatLon.FromDted(Helpers.ReadString(bufferedData, 7), Helpers.ReadString(bufferedData, 8));
            LatLon SouthEastCorner = LatLon.FromDted(Helpers.ReadString(bufferedData, 7), Helpers.ReadString(bufferedData, 8));
            float Orientation = Helpers.ReadFloat(bufferedData, 9) / 10;
            float LatitudeInterval = Helpers.ReadFloat(bufferedData, 4) / 10;
            float LongitudeInterval = Helpers.ReadFloat(bufferedData, 4) / 10;
            //read number of latitude lines followed by number of longitude lines
            Tuple<int, int> tempShape = new(Helpers.ReadInt(bufferedData, 4), Helpers.ReadInt(bufferedData, 4));
            //switch shape to be in <longitude><latitude> format
            Tuple<int, int> Shape = new(tempShape.Item2, tempShape.Item1);
            int Coverage = Helpers.ReadInt(bufferedData, 2);

            return new DataSetIdentification{
                SecurityCode = SecurityCode,
                ReleaseMarkings = ReleaseMarkings,
                HandlingDescription = HandlingDescription,
                ProductLevel = ProductLevel,
                Reference = Reference,
                Edition = Edition,
                MergeVersion = MergeVersion,
                MaintenanceDate = MaintenanceDate,
                MergeDate = MergeDate,
                MaintenanceCode = MaintenanceCode,
                ProducerCode = ProducerCode,
                ProductSpecification = ProductSpecification,
                SpecificationDate = SpecificationDate,
                VerticalDatum = VerticalDatum,
                HorizontalDatum = HorizontalDatum,
                CollectionSystem = CollectionSystem,
                CompilationDate = CompilationDate,
                Origin = Origin,
                SouthWestCorner = SouthWestCorner,
                NorthWestCorner = NorthWestCorner,
                NorthEastCorner = NorthEastCorner,
                SouthEastCorner = SouthEastCorner,
                Orientation = Orientation,
                LatitudeInterval = LatitudeInterval,
                LongitudeInterval = LongitudeInterval,
                Shape = Shape,
                Coverage = Coverage,
                Data = data
            };
            }
        }
    
    public int BlockLength() {
        return 12 + (2 * this.Shape.Item2);
    }
}
}