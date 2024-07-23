
using System.Collections.Generic;

namespace Modot.Portable;
public class DataResponse{
    public List<Data> Data { get; private set; }
    public ResponseType ResponseType { get; private set; }
    public DataResponse(List<Data> data, ResponseType responseType){
        Data = data;
        ResponseType = responseType;
    }
}

public enum ResponseType{
    //操作成功
    SUCCESS = 200,
    //类型没有找到
    NOTFOUND = 404,
    //操作失败
    Failed = 400
}