using System;
using System.IO;
using System.Xml.Serialization;

namespace Tunnelier.Services.Config {
  public class Config {
    private readonly ConfigCollection collection;

    public ConfigCollection Collection => collection;

    public Config() {
      string path = Directory.GetCurrentDirectory() + "/config.xml";
      if (!File.Exists(path)) {
        throw new Exception($"Configuration file '{path}' not found");
      }

      XmlSerializer serializer = new XmlSerializer(typeof(ConfigCollection));
      StreamReader reader = new StreamReader(path);
      collection = (ConfigCollection)serializer.Deserialize(reader);
      reader.Close();
    }
  }
}
