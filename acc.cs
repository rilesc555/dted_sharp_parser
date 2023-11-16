using System;
using System.IO;
using definitions;

namespace acc{
    public class AccuracyDescription
    {
        public int? AbsoluteHorizontal { get; private set; }
        public int? AbsoluteVertical { get; private set; }
        public int? RelativeHorizontal { get; private set; }
        public int? RelativeVertical { get; private set; }
        private readonly byte[] _data;
        private static readonly byte[] _SENTINEL = "ACC"u8.ToArray();

        private AccuracyDescription(int? absoluteHorizontal, int? absoluteVertical, int? relativeHorizontal, int? relativeVertical, byte[] data)
        {
            AbsoluteHorizontal = absoluteHorizontal;
            AbsoluteVertical = absoluteVertical;
            RelativeHorizontal = relativeHorizontal;
            RelativeVertical = relativeVertical;
            _data = data;
        }

        public static AccuracyDescription FromBytes(byte[] data)
        {
            if (data.Length < Helpers.ACC_SIZE)
            {
                throw new ArgumentException($"The Accuracy Description record is {Helpers.ACC_SIZE} bytes but was provided {data.Length} bytes");
            }

            using (var bufferedData = new MemoryStream(data))
            {
                var sentinel = new byte[3];
                bufferedData.Read(sentinel, 0, 3);
                if (!Helpers.CompareArrays(sentinel, _SENTINEL))
                {
                    throw new Helpers.InvalidFileException($"Accuracy Description Records must start with '{System.Text.Encoding.ASCII.GetString(_SENTINEL)}'. Found: {System.Text.Encoding.ASCII.GetString(sentinel)}");
                }

                var absHorBytes = new byte[4];
                var absVertBytes = new byte[4];
                var relHorBytes = new byte[4];
                var relVertBytes = new byte[4];
                bufferedData.Read(absHorBytes, 0, 4);
                bufferedData.Read(absVertBytes, 0, 4);
                bufferedData.Read(relHorBytes, 0, 4);
                bufferedData.Read(relVertBytes, 0, 4);

                return new AccuracyDescription(
                    TryConvertToInt(absHorBytes),
                    TryConvertToInt(absVertBytes),
                    TryConvertToInt(relHorBytes),
                    TryConvertToInt(relVertBytes),
                    data
                );
            }
        }

        private static int? TryConvertToInt(byte[] bytes)
        {
            if (bytes.Length < 4) return null;
            return BitConverter.ToInt32(bytes, 0);
        }
    }

    
}