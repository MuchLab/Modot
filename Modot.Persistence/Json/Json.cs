using System.Collections.Generic;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Modot.Persistence;
public static class Json
{
    private static string ResourcePathPrefix = "res://";
    /// <summary>
    /// 把json字符串反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <param name="options"></param>
    /// <returns></returns>
	public static T FromJson<T>(string json, JsonSerializerOptions options = null) where T : class{
		if (options != null)
			return JsonSerializer.Deserialize<T>(json, options);
		return JsonSerializer.Deserialize<T>(json, DefaultOptions);
	}

    public static List<Dictionary<string, object>> FromJsonFile(string path, JsonSerializerOptions options = null){
        using FileStream openStream = File.OpenRead(path.Replace(ResourcePathPrefix,""));
		if (options != null)
			return JsonSerializer.Deserialize<List<Dictionary<string, object>>>(openStream, options);
		return JsonSerializer.Deserialize<List<Dictionary<string, object>>>(openStream, DefaultOptions);
	}

    /// <summary>
    /// 从json文件中读取数据并反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="options">自定义选项</param>
    /// <returns></returns>
    public static T FromJsonFile<T>(string path, JsonSerializerOptions options = null) where T : class{
        using FileStream openStream = File.OpenRead(path.Replace(ResourcePathPrefix,""));
		if (options != null)
			return JsonSerializer.Deserialize<T>(openStream, options);
		return JsonSerializer.Deserialize<T>(openStream, DefaultOptions);
	}
    /// <summary>
    /// 从json文件中读取数据并反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="options">自定义选项</param>
    /// <returns></returns>
    public static async Task<T> FromJsonFileAsync<T>(string path, JsonSerializerOptions options = null) where T : class{
        using FileStream openStream = File.OpenRead(path.Replace(ResourcePathPrefix,""));
		if (options != null)
			return await JsonSerializer.DeserializeAsync<T>(openStream, options);
		return await JsonSerializer.DeserializeAsync<T>(openStream, DefaultOptions);
	}

    public static async Task<List<Dictionary<string, object>>> FromJsonFileAsync(string path, JsonSerializerOptions options = null){
        using FileStream openStream = File.OpenRead(path.Replace(ResourcePathPrefix,""));
		if (options != null)
			return await JsonSerializer.DeserializeAsync<List<Dictionary<string, object>>>(openStream, options);
		return await JsonSerializer.DeserializeAsync<List<Dictionary<string, object>>>(openStream, DefaultOptions);
	}

    /// <summary>
    /// 序列化对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="serializedObject"></param>
    /// <param name="options">自定义选项</param>
    /// <returns></returns>
	public static string ToJson<T>(T serializedObject, JsonSerializerOptions options = null){
		if (options != null)
			return JsonSerializer.Serialize(serializedObject, options);
		return JsonSerializer.Serialize(serializedObject, DefaultOptions);
	}
    /// <summary>
    /// 将对象序列化后写入文件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="serializedObject"></param>
    /// <param name="path"></param>
    /// <param name="options">自定义选项</param>
    public static async void ToJsonFileAsync<T>(T serializedObject, string path, JsonSerializerOptions options = null){
		await using FileStream createStream = File.Create(path.Replace(ResourcePathPrefix,""));
		if (options != null){
			await JsonSerializer.SerializeAsync(createStream, serializedObject, options);
			return;
		}
		await JsonSerializer.SerializeAsync(createStream, serializedObject, DefaultOptions);
	}

    public static async void ToJsonFileAsync(List<Dictionary<string, object>> data, string path, JsonSerializerOptions options = null){
        await using FileStream createStream = File.Create(path.Replace(ResourcePathPrefix,""));
		if (options != null){
			await JsonSerializer.SerializeAsync(createStream, data, options);
			return;
		}
		await JsonSerializer.SerializeAsync(createStream, data, DefaultOptions);
    }
	

	public static JsonSerializerOptions DefaultOptions = new JsonSerializerOptions{
        //不允许尾部的逗号
		AllowTrailingCommas=false,
        //按格式输出
		WriteIndented=true,
        //忽略默认的属性或字段
		DefaultIgnoreCondition=JsonIgnoreCondition.WhenWritingDefault,
        //开启忽略字段，默认情况下是false
		IncludeFields=true,
        //CjkUnifiedIdeographs 代表中文(Chinese)、日文(Japanese )、韩文(Korean)的字符集合
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.CjkUnifiedIdeographs, UnicodeRanges.CjkSymbolsandPunctuation, UnicodeRanges.HalfwidthandFullwidthForms),
        //数字会转换成字符串，字符串会转换成数字
        NumberHandling =  JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString,
        //字符串和枚举可以互转
        Converters = {new JsonStringEnumConverter()}
	};
}