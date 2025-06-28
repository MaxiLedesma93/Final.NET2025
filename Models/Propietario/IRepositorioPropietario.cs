using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tp_Inmobiliaria_Ledesma_Lillo.Models
{
    public interface IRepositorioPropietario : IRepositorio<Propietario>
    {
        Propietario ObtenerPorEmail(string email);
     //   IList<Propietario> BuscarPorNombre(string nombre);
    }
}