using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NetCoreSeguridadEmpleados.Filters
{
    public class AuthorizeEmpleadosAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //POR AHORA, SOLAMENTE NOS INTERESA VALIDAR SI EXISTE O NO EL EMPLEADO
            var user = context.HttpContext.User;
            if(user.Identity.IsAuthenticated == false)
            {
                context.Result = GetRoute("Managed", "LogIn");
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
