using System;
using System.Reflection;

namespace Modot.Portable;
public abstract class Controller{
    protected Controller _nextController;
    protected Database _database;
    public Controller(){}
    public Controller(Database database){
        _database = database;
    }
    public void SetNextController(Controller controller){
        _nextController = controller;
    }

    public abstract DataResponse HandleRequest(DataRequest request);

    protected RequestCondition GetRequestCondition(string requestPath){
        
        RequestCondition requestCondition = new RequestCondition();
        //如果path包含?则有查询参数需要
        if(requestPath.Contains("?")){
            var splited = requestPath.Split("?");
            Type type = null;
            Assembly[] assemblyArray = AppDomain.CurrentDomain.GetAssemblies();
            int assemblyArrayLength = assemblyArray.Length;
            for (int i = 0; i < assemblyArrayLength; ++i)
            {
                type = assemblyArray[i].GetType(splited[0]);
                if (type != null)
                {
                    requestCondition.Type = type;
                }
            }

            foreach (var keyValueStr in splited[1].Split("&"))
            {
                var split = keyValueStr.Split("=");
                requestCondition.AddKeyValuePair(split[0], split[1]);
            }
        }else{
            requestCondition.Type = Type.GetType(requestPath);
        }
        return requestCondition;
    }
    protected DataResponse HandleUnMatchTypeRequest(DataRequest request){
        if(_nextController != null){
            return _nextController.HandleRequest(request);
        }else{
            return new DataResponse(null, ResponseType.NOTFOUND);
        }
    }
}