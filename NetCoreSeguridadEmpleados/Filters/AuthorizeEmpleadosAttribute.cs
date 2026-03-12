using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NetCoreSeguridadEmpleados.Repositories;

namespace NetCoreSeguridadEmpleados.Filters
{
    public class AuthorizeEmpleadosAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //POR AHORA, SOLAMENTE NOS INTERESA VALIDAR SI EXISTE O NO EL EMPLEADO
            var user = context.HttpContext.User;

            //NECESITAMOS EL ACTION Y EL CONTROLLER DE DONDE EL USUARIO HA PULSADO
            //PARA ELLO, TENEMOS RouteValues QUE CONTIENE LA INFORMACION
            //RouteData["controller"]
            //RouteData["action]
            //RouteData["idalgo], esto es con IF

            string controller =
                context.RouteData.Values["controller"].ToString();

            string action =
                context.RouteData.Values["action"].ToString();

            ITempDataProvider provider =
                context.HttpContext.RequestServices.GetService<ITempDataProvider>();

            //ESTA CLASE TIENE EL TEMPDATA DE NUESTRA APP
            var tempData = provider.LoadTempData(context.HttpContext);

            //ALMACENAMOS LA INFROMACION
            tempData["controller"] = controller;
            tempData["action"] = action;

            provider.SaveTempData(context.HttpContext, tempData);

            if (user.Identity.IsAuthenticated == false)
            {
                context.Result = GetRoute("Managed", "LogIn");
            }
            else
            {
                //COMPROBAMOS ROLES
                //TENEMOS EN CUENTA MAYUSCULAS/MINUSCULAS
                if (user.IsInRole("PRESIDENTE") == false
                    && user.IsInRole("DIRECTOR") == false
                    && user.IsInRole("ANALISTA") == false)
                {
                    context.Result = GetRoute("Managed", "ErrorAcceso");
                }
            }
        }

        //EN ALGUN MOMENTO TENDREMOS MAS REDIRECCIONES QUE SOLO AL LOGIN
        //POR LO QUE CREAMOS UN METODO PARA REDIRECCIONAR

        private RedirectToRouteResult GetRoute
            (string controller, string action)
        {
            RouteValueDictionary ruta =
                new RouteValueDictionary(new
                {
                    controller = controller,
                    action = action
                });
            RedirectToRouteResult result =
                new RedirectToRouteResult(ruta);
            return result;
        }

    }
}
