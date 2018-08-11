using System;
using System.IO;
using System.Xml.Serialization;

namespace Tunnelier {
  public class Settings {
    private SettingsCollection collection;

    public SettingsCollection Collection {
      get {
        return this.collection;
      }
    }

    public Settings(string path) {
      if (!File.Exists(path)) {
        Logger.Error(-1, $"File '{path}' does not exist");
        Environment.Exit(-1);
      }
      XmlSerializer serializer = new XmlSerializer(typeof(SettingsCollection));
      StreamReader reader = new StreamReader(path);
      collection = (SettingsCollection)serializer.Deserialize(reader);
      reader.Close();
    }
  }

  [Serializable()]
  [XmlRoot("Settings")]
  public class SettingsCollection {
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
