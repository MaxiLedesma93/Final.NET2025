using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tp_Inmobiliaria_Ledesma_Lillo.Models
{
    public interface IRepositorioPago : IRepositorio<Pago>
    {
        IList<Pago> ObtenerPagosPorContrato(int id);
        IList<Pago> ObtenerPagosEliminados();

    }
}