using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                return View(filtrados);
            }

            if (fecInf != null && fecSup != null)
            {


                lista = repo.ObtenerTodosVigentes((DateTime)fecInf, (DateTime)fecSup);
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
                return View(vigentes);
            }
            else
            {

                DateTime fechLim = DateTime.Today.AddDays((double)dias);
                filtrados = repo.ObtenerPorFechaVenc(fechLim);
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
                    contrato.UsuarioAlta = usuario.IdUsuario;
                    repo.Alta(contrato);
                    TempData["id"] = contrato.IdContrato;
                    return RedirectToAction(nameof(Listado));
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


        Contrato? contrato = repo.ObtenerPorId(id);

        ViewBag.Inquilino = contrato.Inquilino.Nombre + " " + contrato.Inquilino.Apellido;
        ViewBag.Inmueble = contrato.Inmueble.Direccion;

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
                IList<Pago> pagos = repoPago.ObtenerPagosPorContrato(id);
                foreach (var pago in pagos)
                {
                    if (pago.Detalle == "Multa" || pago.Detalle == "Pago Pendiente: Multa")
                    {
                        ViewBag.ValorMulta = pago.Importe;
                    }
                }

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
    public IActionResult FinalizarContrato(DateTime FecAnulacion, int IdContrato)
    {
       
        try
        {
           
          
            Contrato contrato = repo.ObtenerPorId(IdContrato);
            contrato.FecAnulacion = FecAnulacion;
            repo.Modificacion(contrato);
            //comparar entre fecha de inicio y fecha de fin para ver cantidad de dias. diasTotales/2 
            //luego calcular dias entre fecha de inicio y FecAnulacion para ver si es mayor 
            // a la mitad de los dias.
            TimeSpan diasTotalesSpan = contrato.FecFin - contrato.FecInicio;
            int diasTotales = diasTotalesSpan.Days;
            TimeSpan diasRealesSpan = FecAnulacion - contrato.FecInicio;
            int diasReales = diasRealesSpan.Days;
            if (diasReales < (int)diasTotales / 2)
            {
                // Importe x 2 meses 
                // Detalle Pago Pendiente: Multa.
                // Detalle Pago Pendiente: Meses Adeudados.
                IList<Pago> lista = repoPago.ObtenerPagosPorContrato(IdContrato);
                int numPago = lista.Count + 1;
                Pago pago = new Pago();
                pago.ContratoId = IdContrato;
                pago.NumPago = numPago;
                pago.Importe = contrato.Monto * 2;
                pago.Detalle = "Pago Pendiente: Multa";
                pago.Est = 1;
                repoPago.Alta(pago);
                return RedirectToAction(nameof(AnularContrato));
            }
            else
            {
                // Importe x 1 mes 
                // Detalle Pago Pendiente: Multa.
                // Detalle Pago Pendiente: Meses Adeudados.
                IList<Pago> lista = repoPago.ObtenerPagosPorContrato(IdContrato);
                int numPago = lista.Count + 1;
                Pago pago = new Pago();
                pago.ContratoId = IdContrato;
                pago.NumPago = numPago;
                pago.Importe = contrato.Monto;
                pago.Detalle = "Pago Pendiente: Multa";
                pago.Est = 1;
                repoPago.Alta(pago);
                return RedirectToAction(nameof(AnularContrato));
            }
            
            
           
        }
        catch (Exception ex)
        {
            return Json(new { Error = ex.Message });
        }
    }

}


