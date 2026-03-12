using Microsoft.AspNetCore.Authorization;
using NetCoreSeguridadEmpleados.Repositories;
using System.Security.Claims;

namespace NetCoreSeguridadEmpleados.Policies
{
    public class DeleteJefazoRequirement : AuthorizationHandler<DeleteJefazoRequirement>,
        IAuthorizationRequirement
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DeleteJefazoRequirement requirement)
        {
            if(context.Resource is HttpContext HttpContext)
            {
                var repo = HttpContext.RequestServices.GetService<HospitalRepository>();
                string idEmpleado = context.User.FindFirstValue("IdEmpleado");
                int id = int.Parse(idEmpleado);
                bool resultado = await repo.GetEmpleadosSubordinadosAsync(id);
                if (resultado)
                {
                    context.Succeed(requirement);
                }
                context.Fail();
            }
        }
    }
}
