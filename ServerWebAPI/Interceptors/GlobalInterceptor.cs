using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ServerWebAPI.BLL;
using ServerWebAPI.Models;
using System.Net;

namespace ServerWebAPI.Interceptors
{
    public class GlobalInterceptor : IAsyncActionFilter
    {
        private readonly UserBLL _userBLL;

        public GlobalInterceptor(UserBLL userBLL)
        {
            _userBLL = userBLL;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // 在进入控制器之前执行的代码
            var routeData = context.RouteData;
            // 获取控制器名称和动作方法名称
            string? controllerName = routeData.Values["controller"]?.ToString();
            string? actionName = routeData.Values["action"]?.ToString();
            string currentRoute = controllerName + "/" + actionName;
            //排除的路由
            string[] passRoutesArray = { "User/Register", "User/Login", "Client/CheckUpdate", "File/GetAllowedMimes", "WebSocket/Connect", "File/GetFile" };
            bool isPass = passRoutesArray.Any(r => r.Equals(currentRoute, StringComparison.OrdinalIgnoreCase));
            if (isPass)
            {
                //不执行拦截逻辑
                await next();
                return;
            }
            var headers = context.HttpContext.Request.Headers;
            string? token = headers["Authorization"];
            if (token == null)
            {
                context.Result = new ObjectResult("未登录")
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
                return;
            }
            TUser? user = await _userBLL.GetByToken(token);
            if (user == null)
            {
                context.Result = new ObjectResult("登录失效或没有权限")
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized
                };
                return;
            }
            // 通过
            // 将用户信息存储在 HttpContext.Items 中
            context.HttpContext.Items["User"] = user;

            await next();

            // [在控制器执行后，返回客户端之前执行的代码]

        }
    }
}
