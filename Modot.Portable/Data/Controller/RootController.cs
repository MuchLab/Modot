
using System;

namespace Modot.Portable;
public class RootController : Controller
{
    public override DataResponse HandleRequest(DataRequest request)
    {
        if(_nextController != null)
            return _nextController.HandleRequest(request);
        else{
            Scene.Instance.DebugText.DrawWarn("There isnt a controller in request chain");
            return new DataResponse(null, ResponseType.NOTFOUND);
        }
    }
}