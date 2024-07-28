using System.Collections.Generic;
using Modot.Portable;

public abstract class Database<T> : IDatabase<T> where T : Data
{
    protected List<T> _database;

    public abstract void Connect(string path);

    public virtual List<T> Get(RequestCondition request)
    {
        List<T> list = new List<T>();
        foreach (var item in _database)
        {
            bool match = true;
            foreach (var keyValuePair in request.KeyValuePairs)
            {
                var value = request.Type.GetProperty(keyValuePair.key).GetValue(item) as string;
                if(value != keyValuePair.value){
                    match = false;
                    break;
                }
            }
            if(match)
                list.Add(item);
        }
        return list;
    }
}