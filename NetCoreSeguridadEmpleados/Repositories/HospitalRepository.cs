using Microsoft.EntityFrameworkCore;
using NetCoreSeguridadEmpleados.Data;
using NetCoreSeguridadEmpleados.Models;
using System.Threading.Tasks;

namespace NetCoreSeguridadEmpleados.Repositories
{
    public class HospitalRepository
    {
        private HospitalContext context;

        public HospitalRepository(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<Empleado>> GetEmpleadosAsync()
        {
            return await this.context.Empleados.ToListAsync();
        }

        public async Task<Empleado> FindEmpleadoAsync(int idEmpleado)
        {
            return await this.context.Empleados.FirstOrDefaultAsync(e => e.IdEmpleado == idEmpleado);
        }

        public async Task<List<Empleado>> GetEmpleadosPorDepartamentoAsync(int idDepartamento)
        {
            return await this.context.Empleados
                .Where(e => e.IdDepartamento == idDepartamento)
                .ToListAsync();
        }

        public async Task UpdateSalarioEmpleadosAsync(int idDepartamento, int incremento)
        {
            List<Empleado> empleados = await this.GetEmpleadosPorDepartamentoAsync(idDepartamento);

           foreach(Empleado emp in empleados)
           {
                emp.Salario += incremento;
           }

           await this.context.SaveChangesAsync();
        }

        public async Task<Empleado> LogInEmpleadoAsync(string apellido, int idEmpleado)
        {
            return await this.context.Empleados
                .FirstOrDefaultAsync(e => e.Apellido == apellido
                    && e.IdEmpleado == idEmpleado);
        }

    }
}
