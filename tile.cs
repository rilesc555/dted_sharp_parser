using definitions;
using latlon;
using acc;
using dsi;
using uhl;

namespace tile
{
    public class Tile {
        string File { get; init; }
        public UserHeaderLabel UHL { get; set; }
        public DataSetIdentification DSI { get; set; }
        public AccuracyDescription ACC { get; set; }
        public int[][] Data { get; set; }

        public Tile(string filePath) {

            this.File = filePath;

            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read)){
                byte[] buffer = new byte[Helpers.UHL_SIZE];
                int uhlbytes = stream.Read(buffer, 0, Helpers.UHL_SIZE);
                if (uhlbytes == Helpers.UHL_SIZE) {
                    this.UHL = UserHeaderLabel.FromBytes(buffer);
                }
                byte[] buffer2 = new byte[Helpers.DSI_SIZE];
                int dsibytes = stream.Read(buffer2, 0, Helpers.DSI_SIZE);
                if (dsibytes == Helpers.DSI_SIZE) {
                    this.DSI = DataSetIdentification.FromBytes(buffer2);
                }
                byte[] buffer3 = new byte[Helpers.ACC_SIZE];
                int accbytes = stream.Read(buffer3, 0, Helpers.ACC_SIZE);
                if (accbytes == Helpers.ACC_SIZE) {
                    this.ACC = AccuracyDescription.FromBytes(buffer3);
                }
            }
            // matrix is in <longitude><latitude> format
            this.Data = new int[this.DSI.Shape.Item1][];   
            
            this.LoadData();

            Console.WriteLine("Loaded tile");

        }
        private void LoadData() {
            byte[] dataRecord;
            using (FileStream stream = new FileStream(File, FileMode.Open, FileAccess.Read)){
                long startingPosition = (long)(Helpers.UHL_SIZE + Helpers.DSI_SIZE + Helpers.ACC_SIZE);
                stream.Position = startingPosition;
                int length = (int)stream.Length - (int)startingPosition;
                dataRecord = new byte[length];
                int bytes = stream.Read(dataRecord, 0, length);
            }
        
            if (DSI != null) {
                int blockLength = DSI.BlockLength();
                for (int column = 0; column < DSI.Shape.Item1; column++)
                {
                    int start = column * blockLength;
                    int length = blockLength;
                    byte[] block = new byte[length];
                    Array.Copy(dataRecord, start, block, 0, length);
                    var columnVector = parseData(block, column);
                    
                    this.Data[column] = columnVector;
                }
                // using (StreamWriter writer = new StreamWriter("/Users/riley/Projects/dtedsharp/parsedData.csv")) {
                //     MathNet.Numerics.Data.Text.DelimitedWriter.Write(writer, this.Data, ",");
                // }
            }

        }
        // returns vector representing a longitudinal slice going from south to north
        private int[] parseData(byte[] block, int column) {
            int[] parsedData = new int[DSI.Shape.Item2];
            for (int i = 0; i < parsedData.Length; i++) {
                short value = BitConverter.ToInt16(block, i * 2 + 8);
                if (BitConverter.IsLittleEndian) {
                    value = ReverseBytes(value);
                }
                parsedData[i] = value;
            }

            return parsedData;
            }
        

        private static short ReverseBytes(short value)
        {
            return (short)((value << 8) | ((value >> 8) & 0xFF));
        }
    
        public int getElevation(LatLon latlon) {
            double originLat = this.DSI.Origin.Latitude;
            double originLon = this.DSI.Origin.Longitude;

            int lon_count = this.DSI.Shape.Item1;
            int lat_count = this.DSI.Shape.Item2;

            int lat_index = (int)Math.Round(latlon.Latitude - originLat) * (lat_count - 1);
            int lon_index = (int)Math.Round(latlon.Longitude - originLon) * (lon_count - 1);

             return this.Data[lon_index][lat_index];

        }
    }
}