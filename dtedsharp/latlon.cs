using System;

public class LatLon
{
    public double Latitude { get; }
    public double Longitude { get; }

    public LatLon(double latitude, double longitude)
    {
        if (latitude < -90 || latitude > 90)
            throw new ArgumentException("Latitude must be between -90 and 90.");
        if (longitude < -180 || longitude > 180)
            throw new ArgumentException("Longitude must be between -180 and 180.");

        Latitude = latitude;
        Longitude = longitude;
    }

    public static LatLon FromDted(string latitudeStr, string longitudeStr)
    {
        int latSign = latitudeStr.EndsWith("S") ? -1 : 1;
        (int latDegrees, int latMinutes, double latSeconds) = ParseDmsCoordinate(latitudeStr.Substring(0, latitudeStr.Length - 1));
        var latitude = latSign * DmsToDecimal(latDegrees, latMinutes, latSeconds);
        
        int lonSign = longitudeStr.EndsWith("W") ? -1 : 1;
        (int degrees, int minutes, double seconds) = ParseDmsCoordinate(longitudeStr.Substring(0, longitudeStr.Length - 1));
        var longitude = lonSign * DmsToDecimal(degrees, minutes, seconds);
        return new LatLon(latitude, longitude);

    }

    public string Format(int precision)
    {
        if (precision < 0)
            throw new ArgumentException($"Precision value must be positive. Found: {precision}");


        string latitudeHemisphere = Latitude >= 0 ? "N" : "S";
        string longitudeHemisphere = Longitude >= 0 ? "E" : "W";

        string latitudeStr = $"{Math.Abs(Latitude).ToString($"F{precision}")}{latitudeHemisphere}";
        string longitudeStr = $"{Math.Abs(Longitude).ToString($"F{precision}")}{longitudeHemisphere}";

    }

    public static double DmsToDecimal(int degree, int minute, double second)
    {
        return degree + ((minute + (second / 60)) / 60);
    }

    public static (int, int, double) ParseDmsCoordinate(string coordinate)
    {
        int secondsIndex = coordinate[^2] == '.' ? -4 : -2;
        int minutesIndex = secondsIndex - 2;

        int degrees = int.Parse(coordinate[..minutesIndex]);
        int minutes = int.Parse(coordinate.Substring(minutesIndex, 2));
        double seconds = double.Parse(coordinate.Substring(coordinate.Length + secondsIndex));

        return (degrees, minutes, seconds);
    }
}