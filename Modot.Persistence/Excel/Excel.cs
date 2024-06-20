using System.Collections.Generic;
using System.IO;
using MiniExcelLibs;

namespace Modot.Persistence.Excel;

public static class Excel
{
    public static List<T> FromExcel<T>(string path) where T : class, new(){
        var rows = new List<T>();
        using (var stream = File.OpenRead(path))
            rows = stream.Query<T>().ToList();
        return rows;
    }
}

