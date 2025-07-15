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
            IList<Inmueble>? ObtenerDisponibles();
            IList<Inmueble>? ObtenerSinContrato(DateTime? fecInicio, DateTime? fecFin);
        }
}