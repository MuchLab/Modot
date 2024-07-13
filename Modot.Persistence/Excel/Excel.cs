using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MiniExcelLibs;

namespace Modot.Persistence;

public static class Excel {
    private static string ResourcePathPrefix = "res://";
    public static async Task<IEnumerable<T>> FromExcelFileAsync<T>(string path, string sheetName, ExcelType type = ExcelType.XLSX) where T : class, new(){
        using (var stream = File.OpenRead(path.Replace(ResourcePathPrefix, "")))
        return await stream.QueryAsync<T>(excelType: type, sheetName: sheetName);
    }

    public static List<Dictionary<string, object>> FromExcelFile(string path, string sheetName, ExcelType type = ExcelType.XLSX){
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        DataTable dataTable;
        using (var stream = File.OpenRead(path.Replace(ResourcePathPrefix, ""))){
            dataTable = stream.QueryAsDataTable(excelType: type, sheetName: sheetName);
        }
        Dictionary<string, object> row;
        foreach (DataRow dr in dataTable.Rows)
        {
            row = new Dictionary<string, object>();
            foreach (DataColumn col in dataTable.Columns)
            {
                row.Add(col.ColumnName, dr[col]);
            }
            rows.Add(row);
        }
        return rows;
    }
    
    public static List<T> FromExcelFile<T>(string path, string sheetName, ExcelType type = ExcelType.XLSX) where T : class, new(){
        using (var stream = File.OpenRead(path.Replace(ResourcePathPrefix, "")))
        return stream.Query<T>(excelType: type, sheetName: sheetName).ToList();
    }
    public static void ToExcelFile<T>(string path, string sheetName, ExcelType type = ExcelType.XLSX) where T : class, new(){
        
    }
}