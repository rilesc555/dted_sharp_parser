using System;
using System.IO;
using System.Text;
using definitions;
using latlon;

namespace records.uhl {
    public class UserHeaderLabel {
        LatLon? Origin { get; init; }
        float Longitude_interval { get; init; }
        float Latitude_interval { get; init; }
        int? Vertical_accuracy { get; init; }
        byte[]? Security_code { get; init; }
        byte[]? Reference { get; init; }
        Tuple<int, int>? Shape { get; init; }
        bool Multiple_accuracy { get; init; }
        byte[]? Data { get; init; }

        private static readonly byte[] _SENTINEL = "UHL1"u8.ToArray();

        private UserHeaderLabel() {}
        public static UserHeaderLabel FromBytes(byte[] data) {
            if (data.Length < Helpers.UHL_SIZE) {
                throw new ArgumentException($"The Data Set Identification record is {Helpers.UHL_SIZE} bytes but was provided {data.Length} bytes");
            }

            using (var bufferedData = new MemoryStream(data)) {
                var sentinel = new byte[3];
                bufferedData.Read(sentinel, 0, 4);
                if (!Helpers.CompareArrays(sentinel, _SENTINEL)) {
                    throw new Helpers.InvalidFileException("Data Set Identification Records must start with 'UHL'");
                }

                LatLon origin = LatLon.FromDted(Helpers.ReadString(bufferedData, 8), Helpers.ReadString(bufferedData, 8));
                float longitude_interval = Helpers.ReadFloat(bufferedData, 4);
                float latitude_interval = Helpers.ReadFloat(bufferedData, 4);
                int vertical_accuracy = Helpers.ReadInt(bufferedData, 4);
                byte[] security_code = Helpers.ReadBytes(bufferedData, 3);
                byte[] reference = Helpers.ReadBytes(bufferedData, 12);
                Tuple<int, int> shape = new (Helpers.ReadInt(bufferedData, 4), Helpers.ReadInt(bufferedData, 4));
                bool multiple_accuracy = Helpers.ReadInt(bufferedData, 1) == 1;

                return new UserHeaderLabel() {
                    Origin = origin,
                    Longitude_interval = longitude_interval,
                    Latitude_interval = latitude_interval,
                    Vertical_accuracy = vertical_accuracy,
                    Security_code = security_code,
                    Reference = reference,
                    Shape = shape,
                    Multiple_accuracy = multiple_accuracy,
                    Data = data
                };
            }
        }
    }
}