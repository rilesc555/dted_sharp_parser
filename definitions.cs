namespace definitions
{
    public static class Helpers
    {
        public const int UHL_SIZE = 80;
        public const int DSI_SIZE = 648;
        public const int ACC_SIZE = 2700;

        public const string _UTF8 = "utf-8";

        public static bool CompareArrays(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length) return false;
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i]) return false;
            }
            return true;
        }

        public class InvalidFileException : Exception
        {
            public InvalidFileException(string message) : base(message)
            {
            }
        }
    
        public static string ReadString(Stream stream, int length) {
            var bytes = new byte[length];
            stream.Read(bytes, 0, length);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        public static int ReadInt(Stream stream, int length)
        {
            var bytes = new byte[4]; // Always create a 4-byte array
            stream.Read(bytes, 0, length);

            string temp = System.Text.Encoding.ASCII.GetString(bytes);

            return int.Parse(temp);
        }

        public static byte[] ReadBytes(Stream stream, int length)
        {
            var bytes = new byte[length];
            stream.Read(bytes, 0, length);
            return bytes;
        }

        public static DateTime? ReadDate(Stream stream, int length)
        {
            var dateString = ReadString(stream, length);
            if (dateString == "0000")
            {
                return null;
            }
            else
            {
                return DateTime.ParseExact(dateString, "yyMM", null);
            }
        }

        public static float ReadFloat(Stream stream, int length)
        {
            var bytes = new byte[length];
            stream.Read(bytes, 0, length);
            return BitConverter.ToSingle(bytes, 0);
        }

    }
}