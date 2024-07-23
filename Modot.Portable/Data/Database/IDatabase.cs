using System.Collections.Generic;
namespace Modot.Portable;
public interface IDatabase<T> where T : Data {

    void Connect(string path);
    List<T> Get(RequestCondition request);
}