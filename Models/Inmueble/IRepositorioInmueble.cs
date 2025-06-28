using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tp_Inmobiliaria_Ledesma_Lillo.Models
{
        public interface IRepositorioInmueble : IRepositorio<Inmueble>
        {
              IList<Inmueble>? buscarPorPropietario(int idPropietario);
              IList<Inmueble>? obtenerInmueblesSuspendidos();
        }
}