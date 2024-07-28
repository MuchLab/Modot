using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Modot.Persistence;

namespace Modot.Portable;
public class JsonDatabase<T> : Database<T> where T : Data
{
    public override void Connect(string path)
    {
        _database = Json.FromJsonFile<List<T>>(path);
    }
}