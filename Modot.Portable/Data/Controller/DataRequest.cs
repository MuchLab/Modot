using System;
using System.Collections.Generic;

namespace Modot.Portable;
//"Tower?name=sdsada&"
public class DataRequest {
    public string Path { get; private set; }
    public RequestAction Action { get; private set; }

    public DataRequest(string requestPath, RequestAction action = RequestAction.Get){
        Action = action;
        Path = requestPath;
    }
}

public enum RequestAction{
    Get
}

public class RequestCondition{
    public struct KeyValuePair{
        public string key;
        public string value;
    }
    public Type Type { get; set; }
    public List<KeyValuePair> KeyValuePairs { get; private set; }
    public RequestCondition(){
        KeyValuePairs = new List<KeyValuePair>();
    }
    public RequestCondition(Type type){
        Type = type;
        KeyValuePairs = new List<KeyValuePair>();
    }
    public void AddKeyValuePair(string key, string value){
        KeyValuePairs.Add(new KeyValuePair{key = key, value = value});
    }
}
