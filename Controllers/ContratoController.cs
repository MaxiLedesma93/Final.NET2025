using System.Collections;
using Google.Protobuf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Org.BouncyCastle.Utilities;
using Tp_Inmobiliaria_Ledesma_Lillo.Models;
using Tp_Inmobiliaria_Ledesma_Lillo.Net.Controllers;

namespace Tp_Inmobiliaria_Ledesma_Lillo.Controllers;

public class ContratoController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly IConfiguration config;
    private readonly IRepositorioContrato repo;
    private readonly IRepositorioUsuario repoUsuario;

    private readonly IRepositorioInmueble repoInmu;
    private readonly IRepositorioInquilino repoInqui;

    private readonly IRepositorioPago repoPago;


    public ContratoController(IRepositorioContrato repo, IRepositorioInmueble repoInmu,
        ILogger<HomeController> logger, IConfiguration config, IRepositorioInquilino repoInqui,
        IRepositorioUsuario repoUsuario, IRepositorioPago repoPago)
    {
        this.config = config;
        this.repo = repo;
        this.repoInqui = repoInqui;
        this.repoInmu = repoInmu;
        this.repoUsuario = repoUsuario;
        this.repoPago = repoPago;
        _logger = logger;
    }

    [Authorize]
    public IActionResult Listado(int? dias, DateTime? fecInf, DateTime? fecSup, string? dir)
    {
        try
        {
            ViewBag.dias = dias;
            IList<Contrato> filtrados = new List<Contrato>();
            IList<Contrato> vigentes = new List<Contrato>();
            var lista = repo.ObtenerTodos();
            Usuario usuario = repoUsuario.ObtenerPorEmail(User.Identity.Name);
            foreach (var contrato in lista)
            {
                if (contrato.FecFin < DateTime.Now)
                {
                    contrato.Estado = false;
                    repo.Modificacion(contrato);
                }
                else
                {
                    vigentes.Add(contrato);
                }
            }
            var listaInm = repoInmu.ObtenerTodos();
            ViewBag.Inmuebles = listaInm;
            if (dir != null)
            {
                filtrados = repo.ObtenerPorInmuebleDir(dir);
                if (filtrados.Count == 0)
                {
                    ViewBag.ListaVacia = "No se encontraron registros.";
                }
                return View(filtrados);
            }

            if (fecInf != null && fecSup != null)
            {


                lista = repo.ObtenerTodosVigentes((DateTime)fecInf, (DateTime)fecSup);
                if (lista.Count == 0)
                {
                    ViewBag.ListaVacia = "No se encontraron registros.";
                }
                return View(lista);

            }
            if (dias == 0 || dias == null)
            {

                ViewBag.id = TempData["id"];
                // TempData es para pasar datos entre acciones
                // ViewBag/Data es para pasar datos del controlador a la vista
                // Si viene alguno valor por el tempdata, lo paso al viewdata/viewbag
                if (TempData.ContainsKey("Mensaje"))
                    ViewBag.Mensaje = TempData["Mensaje"];
                if (vigentes.Count == 0)
                {
                    ViewBag.ListaVacia = "No se encontraron registros.";
                }
                return View(vigentes);
            }
            else
            {

                DateTime fechLim = DateTime.Today.AddDays((double)dias);
                filtrados = repo.ObtenerPorFechaVenc(fechLim);
                if (filtrados.Count == 0)
                {
                    ViewBag.ListaVacia = "No se encontraron registros.";
                }
                return View(filtrados);
            }

        }
        catch (Exception ex)
        {
            return Json(new { Error = ex.Message });
        }
    }

    [Authorize]
    public IActionResult Vencidos()
    {
        IList<Contrato> vencidos = repo.ObtenerVencidos();
        var listaInm = repoInmu.ObtenerTodos();
        ViewBag.Inmuebles = listaInm;
        return View(vencidos);
    }

    [Authorize]
    public IActionResult Editar(int id)
    {
        try
        {
            if (id > 0)
            {

                ViewBag.Inmuebles = repoInmu.ObtenerTodos();
                ViewBag.Inquilinos = repoInqui.ObtenerTodos();

                var contrato = repo.ObtenerPorId(id);
                TempData["Mensaje"] = "Datos guardados correctamente";
                return View(contrato);
            }
            else
            { return View(); }
        }
        catch (Exception ex)
        {
            return Json(new { Error = ex.Message });
        }
    }

    [Authorize]
    public IActionResult Crear() //carga el formulario vacio
    {
        try
        {
            ViewBag.Inmuebles = repoInmu.ObtenerTodos();
            ViewBag.Inquilinos = repoInqui.ObtenerTodos();
            return View();
        }
        catch (Exception ex)
        {
            return Json(new { Error = ex.Message });
        }
    }

    [Authorize]
    public IActionResult Guardar(Contrato contrato)
    {

        try
        {

            if (ModelState.IsValid)
            {
                Usuario usuario = repoUsuario.ObtenerPorEmail(User.Identity.Name);
                Contrato existe = repo.ValidarInmuebleIdyFechas(contrato.InmuebleId, contrato.FecInicio, contrato.FecFin);
                if (existe == null)
                {
                    contrato.UsuarioAltaId = usuario.IdUsuario;
                    if (contrato.IdContrato > 0)
                    {
                        repo.Modificacion(contrato);
                        TempData["Mensaje"] = "Datos guardados correctamente";
                        return RedirectToAction(nameof(Listado));
                    }
                    else
                    {
                        repo.Alta(contrato);
                        TempData["id"] = contrato.IdContrato;
                        return RedirectToAction(nameof(Listado));
                    }

                }
                else
                {
                    if (contrato.IdContrato > 0)
                    {
                        repo.Modificacion(contrato);
                        TempData["Mensaje"] = "Datos guardados correctamente";
                        return RedirectToAction(nameof(Listado));
                    }
                    else
                    {
                        TempData["Mensaje"] = "Ya existe un contrato para el inmueble en esas fechas!";
                        return RedirectToAction(nameof(Listado));
                    }
                }

            }
            else
            {
                return View(contrato);
            }

        }
        catch (Exception ex)
        {
            return Json(new { Error = ex.Message });
        }

    }

    [Authorize(Policy = "Administrador")]
    public IActionResult Eliminar(int id)
    {

        try
        {
            repo.Baja(id);
            TempData["Mensaje"] = "Eliminación realizada correctamente";
            return RedirectToAction(nameof(Listado));
        }
        catch (Exception ex)
        {
            return Json(new { Error = ex.Message });
        }
    }

    [Authorize]
    public IActionResult Detalle(int id)
    {


        Contrato contrato = repo.ObtenerPorId(id);

        ViewBag.Inquilino = contrato.Inquilino.Nombre + " " + contrato.Inquilino.Apellido;
        ViewBag.Inmueble = contrato.Inmueble.Direccion;

        if (contrato.UsuarioAltaId != null)
        {
            Usuario? usuarioAlta = repoUsuario.ObtenerPorId((int)contrato.UsuarioAltaId);
            if (usuarioAlta != null)
            {
                ViewBag.EmailUsuarioAlta = usuarioAlta.Email;
            }
        }
        if (contrato.UsuarioBajaId != null)
        {
            Usuario? usuarioBaja = repoUsuario.ObtenerPorId((int)contrato.UsuarioBajaId);
            if (usuarioBaja != null)
            {
                ViewBag.EmailUsuarioBaja = usuarioBaja.Email;
            }
        }




        IList<Pago> pagos = repoPago.ObtenerPagosPorContrato(id);
        foreach (var pago in pagos)
        {
            if (pago.Detalle == "Pagado : Multa + Meses Adeudados" || pago.Detalle == "Pago Pendiente: Multa + Meses Adeudados")
            {
                ViewBag.ValorMulta = pago.Importe;
                ViewBag.PagoDetalle = pago.Detalle;

            }
        }
        return View(contrato);
    }

    [Authorize]
    public IActionResult AnularContrato(int id)
    {
        try
        {
            if (id > 0)
            {
                ViewBag.Inmuebles = repoInmu.ObtenerTodos();
                ViewBag.Inquilinos = repoInqui.ObtenerTodos();
                var contrato = repo.ObtenerPorId(id);
                ViewBag.Inquilino = contrato.Inquilino.Nombre + " " + contrato.Inquilino.Apellido;
                ViewBag.Inmueble = contrato.Inmueble.Direccion;
                IList<Pago> pagos = repoPago.ObtenerPagosPorContrato(id);
                foreach (var pago in pagos)
                {
                    if (pago.Detalle == "Pagado : Multa + Meses Adeudados" || pago.Detalle == "Pago Pendiente: Multa + Meses Adeudados")
                    {
                        ViewBag.ValorMulta = pago.Importe;
                        ViewBag.PagoDetalle = pago.Detalle;
                    }
                }

                return View(contrato);
            }
            else
            { return View(); }
        }
        catch (Exception ex)
        {
            return Json(new { Error = ex.Message });
        }
    }

    [Authorize]
    public IActionResult FinalizarContrato(DateTime FecAnulacion, int IdContrato)
    {

        try
        {
            Contrato contrato = repo.ObtenerPorId(IdContrato);
            Usuario usuario = repoUsuario.ObtenerPorEmail(User.Identity.Name);
            contrato.FecAnulacion = FecAnulacion;
            contrato.Estado = false;
            contrato.UsuarioBajaId = usuario.IdUsuario;
            repo.Modificacion(contrato);
            //compara entre fecha de inicio y fecha de fin para ver cantidad de dias. diasTotales/2 
            //luego calcula dias entre fecha de inicio y FecAnulacion para ver si es mayor 
            // a la mitad de los dias.
            TimeSpan diasTotalesSpan = contrato.FecFin - contrato.FecInicio;
            int diasTotales = diasTotalesSpan.Days;
            TimeSpan diasRealesSpan = FecAnulacion - contrato.FecInicio;
            int diasReales = diasRealesSpan.Days;
            IList<Pago> lista = repoPago.ObtenerPagosPorContrato(IdContrato);
            //DateTime? fechaUltPAgo = new DateTime();
            Pago ultimoPago = lista == null ? null : lista.LastOrDefault<Pago>();
            //fechaUltPAgo = ultimoPago.FechaPago;12 + 3
            int diferenciaMeses = 1;
            if (ultimoPago == null)
            {
                diferenciaMeses = ((FecAnulacion.Year - contrato.FecInicio.Year) * 12) +
                    FecAnulacion.Month - contrato.FecInicio.Month;
            }
            else
            {
                diferenciaMeses = ((FecAnulacion.Year - ultimoPago.FechaPago.Value.Year) * 12) +
                    FecAnulacion.Month - ultimoPago.FechaPago.Value.Month;
            }

            if (diasReales < (int)diasTotales / 2)
            {
                // Importe x 2 meses 
                // Detalle Pago Pendiente: Multa.
                // Detalle Pago Pendiente: Meses Adeudados.
                int numPago = lista.Count + 1;
                Pago pago = new Pago();
                pago.ContratoId = IdContrato;
                pago.NumPago = numPago;
                pago.Importe = (contrato.Monto * 2) + (contrato.Monto * diferenciaMeses);
                pago.Detalle = "Pago Pendiente: Multa + Meses Adeudados";
                pago.Est = 1;
                pago.UsuarioAltaId = usuario.IdUsuario;
                repoPago.Alta(pago);

            }
            else
            {
                // Importe x 1 mes 
                // Detalle Pago Pendiente: Multa.
                // Detalle Pago Pendiente: Meses Adeudados.
                int numPago = lista.Count + 1;
                Pago pago = new Pago();
                pago.ContratoId = IdContrato;
                pago.NumPago = numPago;
                pago.Importe = (contrato.Monto) + (contrato.Monto * diferenciaMeses);
                pago.Detalle = "Pago Pendiente: Multa + Meses Adeudados";
                pago.Est = 1;
                pago.UsuarioAltaId = usuario.IdUsuario;
                repoPago.Alta(pago);
            }
            TempData["Mensaje"] = "Contrato Anulado con Exito.";
            return RedirectToAction(nameof(Detalle), new { id = contrato.IdContrato });
        }
        catch (Exception ex)
        {
            return Json(new { Error = ex.Message });
        }
    }

    [Authorize]
    public IActionResult RenovarContrato(int id)
    {
        try
        {

            if (id > 0)
            {
                var contrato = repo.ObtenerPorId(id);
                ViewBag.Inquilino = contrato.Inquilino.Nombre + " " + contrato.Inquilino.Apellido;
                ViewBag.Inmueble = contrato.Inmueble.Direccion;
                return View(contrato);
            }
            else
            {
                TempData["Mensaje"] = "No existe el contrato a renovar.";
                return RedirectToAction(nameof(Listado));
            }
           
            


        }
        catch (Exception ex)
        {
            return Json(new { Error = ex.Message });
        }
       
    }
}


