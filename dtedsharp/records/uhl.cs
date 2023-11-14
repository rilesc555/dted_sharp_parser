using System;
using System.IO;
using System.Text;
using definitions;
using latlon;

namespace uhl {
    public class UserHeaderLabel {
        LatLon? origin { get; init; }
        float longitude_interval { get; init; }
        float latitude_interval { get; init; }
        int? vertical_accuracy { get; init; }
        byte[]? security_code { get; init; }
        byte[]? reference { get; init; }
        Tuple<int, int>? shape { get; init; }
        bool multiple_accuracy { get; init; }
        byte[]? _data { get; init; }

        private static readonly byte[] _SENTINEL = "UHL"u8.ToArray();

        private UserHeaderLabel() {}
        public static UserHeaderLabel FromBytes(byte[] data) {
            if (data.Length < Helpers.UHL_SIZE) {
                throw new ArgumentException($"The Data Set Identification record is {Helpers.UHL_SIZE} bytes but was provided {data.Length} bytes");
            }

            using (var bufferedData = new MemoryStream(data)) {
                var sentinel = new byte[3];
                bufferedData.Read(sentinel, 0, 3);
                if (!Helpers.CompareArrays(sentinel, _SENTINEL)) {
                    throw new Helpers.InvalidFileException("Data Set Identification Records must start with 'DSI'");
                }

                
            }
        }
    }
}