using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class Yaml {
    private static string ResourcePathPrefix = "res://";

    private static IDeserializer deserializer;
    public static T FromYaml<T>(string path) where T : class{
        if(deserializer == null)
        {
            deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)  // see height_in_inches in sample yml 
                .Build();
        }
        string yml = "";
        using (StreamReader streamReader = new StreamReader(path.Replace(ResourcePathPrefix, "")))
        {
            yml = streamReader.ReadToEnd(); 
        }
        return deserializer.Deserialize<T>(yml);
    }
}