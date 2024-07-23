using System.Collections.Generic;
using Modot.Portable;

public abstract class Database : IDatabase<Data>
{
    protected List<Data> _database;

    public abstract void Connect(string path);

    public virtual List<Data> Get(RequestCondition request)
    {
        List<Data> list = new List<Data>();
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