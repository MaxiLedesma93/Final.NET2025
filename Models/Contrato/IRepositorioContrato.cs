using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tp_Inmobiliaria_Ledesma_Lillo.Models
{
    public interface IRepositorioContrato : IRepositorio<Contrato>
    {
          IList<Contrato> ObtenerPorInmuebleId(int id);
        IList<Contrato> ObtenerTodosVigentes(DateTime fechaInicio, DateTime fechaFin);
        
        IList<Contrato> ObtenerPorInmuebleDir(string dir);

        IList<Contrato> ObtenerPorFechaVenc(DateTime fechaFin);

    }
}