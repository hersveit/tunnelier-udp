using System;
using System.Xml.Serialization;

namespace Tunnelier.Services.Config {
  [Serializable()]
  [XmlRoot("Config")]
  public class ConfigCollection {
    [XmlElement("InputPort")]
    public int InputPort { get; set; }

    [XmlElement("RedirectPort")]
    public int RedirectPort { get; set; }

    [XmlElement("RedirectIp")]
    public string RedirectIp { get; set; }

    [XmlArray("BlackList")]
    [XmlArrayItem("IP")]
    public string[] BlackList { get; set; }
  }
}
