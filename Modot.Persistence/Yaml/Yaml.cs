using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Modot.Persistence;

public class Yaml {
    private static string ResourcePathPrefix = "res://";

    private static IDeserializer deserializer;
    private static ISerializer serializer;


    public static T FromYamlFile<T>(string yaml) where T : class{
        if(deserializer == null)
        {
            deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)  // see height_in_inches in sample yml 
                .Build();
        }
        return deserializer.Deserialize<T>(yaml);
    }

    public static List<Dictionary<string, object>> FromYamlFile(string path){
        if(deserializer == null)
        {
            deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)  // see height_in_inches in sample yml 
                .Build();
        }
        using StreamReader streamReader = new StreamReader(path.Replace(ResourcePathPrefix, ""));
        string yaml = streamReader.ReadToEnd();
        return deserializer.Deserialize<List<Dictionary<string, object>>>(yaml);
    }

    /// <summary>
    /// 从Yaml文件中读取数据并反序列化为对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public static async Task<T> FromYamlFileAsync<T>(string path) where T : class{
        if(deserializer == null)
        {
            deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)  // see height_in_inches in sample yml 
                .Build();
        }
        using StreamReader streamReader = new StreamReader(path.Replace(ResourcePathPrefix, ""));
        string yaml = await streamReader.ReadToEndAsync();
        return deserializer.Deserialize<T>(yaml);
    }

    public static async Task<List<Dictionary<string, object>>> FromYamlFileAsync(string path){
        if(deserializer == null)
        {
            deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)  // see height_in_inches in sample yml 
                .Build();
        }
        using StreamReader streamReader = new StreamReader(path.Replace(ResourcePathPrefix, ""));
        string yaml = await streamReader.ReadToEndAsync();
        return deserializer.Deserialize<List<Dictionary<string, object>>>(yaml);
    }

    public static string ToYaml<T>(T serializedObject) where T : class{
        if(serializer == null)
        {
            serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)  // see height_in_inches in sample yml 
                .Build();
        }
        return serializer.Serialize(serializedObject);
    }

    /// <summary>
    /// 将对象序列化为Yaml并写入文件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="serializedObject"></param>
    public static async void ToYamlFileAsync<T>(T serializedObject, string path) where T : class{
        if(serializer == null)
        {
            serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)  // see height_in_inches in sample yml 
                .Build();
        }
        var yaml = serializer.Serialize(serializedObject);
        using StreamWriter streamWriter = new StreamWriter(path.Replace(ResourcePathPrefix, ""));
        await streamWriter.WriteAsync(yaml);
    }

    public static async void ToYamlFileAsync(List<Dictionary<string, object>> serializedObject, string path){
        if(serializer == null)
        {
            serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)  // see height_in_inches in sample yml 
                .Build();
        }
        var yaml = serializer.Serialize(serializedObject);
        using StreamWriter streamWriter = new StreamWriter(path.Replace(ResourcePathPrefix, ""));
        await streamWriter.WriteAsync(yaml);
    }
}