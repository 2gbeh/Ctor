namespace Ctor.Entities;

public class DeviceEntity
{
  public string? IpAddress { get; set; }
  public string? UserAgent { get; set; }
  public Geolocation? Geolocation { get; set; }
}

public class Geolocation
{
  public double Lat { get; set; }
  public double Long { get; set; }
  public double? Accuracy { get; set; }
}