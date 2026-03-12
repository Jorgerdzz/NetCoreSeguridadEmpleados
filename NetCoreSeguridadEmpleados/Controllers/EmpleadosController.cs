using Microsoft.AspNetCore.Mvc;
using NetCoreSeguridadEmpleados.Filters;
using NetCoreSeguridadEmpleados.Models;
using NetCoreSeguridadEmpleados.Repositories;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NetCoreSeguridadEmpleados.Controllers
{
    public class EmpleadosController : Controller
    {
        private HospitalRepository repo;

        public EmpleadosController(HospitalRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }

        public async Task<IActionResult> Details(int idEmpleado)
        {
            Empleado emp = await this.repo.FindEmpleadoAsync(idEmpleado);
            return View(emp);
        }

        [AuthorizeEmpleados]
        public IActionResult PerfilEmpleado()
        {
            return View();
        }

        [AuthorizeEmpleados]
        public async Task<IActionResult> Compis()
        {
            string dato = HttpContext.User.FindFirstValue("Departamento");
            int departamento = int.Parse(dato);
            List<Empleado> empleados = await this.repo.GetEmpleadosPorDepartamentoAsync(departamento);
            return View(empleados);
        }

        [AuthorizeEmpleados]
        [HttpPost]
        public async Task<IActionResult> Compis(int incremento)
        {
            string dato = HttpContext.User.FindFirstValue("Departamento");
            int departamento = int.Parse(dato);

            await this.repo.UpdateSalarioEmpleadosAsync(departamento, incremento);

            List<Empleado> empleados = await this.repo.GetEmpleadosPorDepartamentoAsync(departamento);
            return View(empleados);
        }
    }
}
