using System.Collections.Generic;
using System.IO;
using System.Linq;
using MiniExcelLibs;

namespace Modot.Portable.Persistence;

public static class Excel {
    private static string ResourcePathPrefix = "res://";
    public static List<T> FromExcel<T>(string path) where T : class, new(){
        List<T> rows = new List<T>();
        using (var stream = File.OpenRead(path.Replace(ResourcePathPrefix, "")))
        {
            rows = stream.Query<T>(excelType: ExcelType.XLSX).ToList();
        }
        return rows;
    }
    public static void ToExcel<T>(string path) where T : class, new(){
        
    }
}