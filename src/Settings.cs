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
  [System.Xml.Serialization.XmlRoot("Settings")]
  public class SettingsCollection {
    [System.Xml.Serialization.XmlElement("InputPort")]
    public int InputPort { get; set; }

    [System.Xml.Serialization.XmlElement("RedirectPort")]
    public int RedirectPort { get; set; }

    [System.Xml.Serialization.XmlElement("RedirectIp")]
    public string RedirectIp { get; set; }
  }
}
