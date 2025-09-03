using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tp_Inmobiliaria_Ledesma_Lillo.Models;
using Tp_Inmobiliaria_Ledesma_Lillo.Net.Controllers;

namespace Tp_Inmobiliaria_Ledesma_Lillo.Controllers;

public class PagoController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepositorioPago repo;
    private readonly IRepositorioContrato repoContra;
    private readonly IRepositorioUsuario repoUsuario;
    private readonly IConfiguration config;

    

    public PagoController(ILogger<HomeController> logger, IRepositorioPago repo,
     IRepositorioContrato repoContra, IRepositorioUsuario repoUsuario, IConfiguration config)
    {   this.repo = repo;
        this.config = config;
        this.repoContra = repoContra;
        this.repoUsuario = repoUsuario;
        _logger = logger;
    }
    [Authorize]
    public IActionResult Listado(int id)
    {
        try
        {
            var lista = repo.ObtenerPagosPorContrato(id);
            if(lista.Count != 0)
            { validaEstado(lista); }
            
            ViewBag.id = id;
            // TempData es para pasar datos entre acciones
				// ViewBag/Data es para pasar datos del controlador a la vista
				// Si viene alguno valor por el tempdata, lo paso al viewdata/viewbag
				if (TempData.ContainsKey("Mensaje"))
					ViewBag.Mensaje = TempData["Mensaje"];
            return View(lista);
        }
        catch(Exception ex)
        {
            return Json(new { Error = ex.Message });
        }
    }

    [HttpGet]
    [Authorize]
    public IActionResult Crear(int id)
	{
           
            try
			{
                Contrato? c = repoContra.ObtenerPorId(id);
                IList<Pago> lista = repo.ObtenerPagosPorContrato(id);
                ViewBag.tamanio = lista.Count + 1;
                ViewBag.monto = c.Monto;
                ViewBag.idContrato = c.IdContrato;
                ViewBag.ApellidoInq = c.Inquilino.Apellido;
                return View();
			}
			catch (Exception ex)
			{
                return Json(new { Error = ex.Message });
			}
	}

    public IActionResult Anular(int id)
    {
       
        try
        {

            repo.Anular(id);
            Pago p = repo.ObtenerPorId(id);
            p.Activo = "Inactivo";
            Usuario usuario = repoUsuario.ObtenerPorEmail(User.Identity.Name);
            p.UsuarioBajaId = usuario.IdUsuario;
            repo.Modificacion(p);
            TempData["Mensaje"] = "Anulacion de pago realizada correctamente";
            return RedirectToAction(nameof(Listado),new {id = p.ContratoId});
        }
        catch(Exception ex)
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
            Pago p = repo.ObtenerPorId(id);
            TempData["Mensaje"] = "Entidad eliminada con exito";
            return RedirectToAction(nameof(Listado));
        }
        catch (Exception ex)
        {
            return Json(new { Error = ex.Message });
        }
    }

    [HttpPost]
    [Authorize]
    public IActionResult Guardar(Pago pago)
    {
        try
        {
            
           if (ModelState.IsValid)
            {
                Usuario usuario = repoUsuario.ObtenerPorEmail(User.Identity.Name);
                if (pago.IdPago > 0)
                {
                    repo.Modificacion(pago);
                }
                else
                {
                    pago.Est = 1;
                    pago.UsuarioAltaId = usuario.IdUsuario;
                    repo.Alta(pago);

                    TempData["id"] = pago.IdPago;
                }
            }
            else
            {
                return View(pago);
            }
            return RedirectToAction(nameof(Listado), new {id = pago.ContratoId});
        }
        catch(Exception ex)
        {
            return Json(new { Error = ex.Message });
        }
    }

    [Authorize]
    public IActionResult Editar(int id)
    {
        try
        {
            if(id > 0)
            {
                
                var pago = repo.ObtenerPorId(id);
                pago.FechaPago = DateTime.Today;
                TempData["Mensaje"] = "Datos guardados correctamente";
                return View(pago);
            }
            else
                { return View();}
        }
        catch(Exception ex)
        {
            return Json(new { Error = ex.Message });
        }
    }

    [Authorize]
    public IActionResult Detalle(int id)
    {
        

        Pago? pago = repo.ObtenerPorId(id);

        return View(pago);
    }

    [Authorize]
    public IActionResult PagosEliminados()
    {
        
        var lista = repo.ObtenerPagosEliminados();
        validaEstado(lista);
        if(lista.Count == 0)
        {
            ViewBag.Mensaje = "No se encontraron registros";
        }
        return View(lista);
    }


    [Authorize]
    public IActionResult PagarMulta(int IdContrato)
    {
         IList<Pago> lista = repo.ObtenerPagosPorContrato(IdContrato);
        Pago ultimoPago = lista == null ? null : lista.LastOrDefault<Pago>();
        ultimoPago.FechaPago = DateTime.Now;
        ultimoPago.Detalle = "Pagado : Multa + Meses Adeudados";
        repo.Modificacion(ultimoPago);
        TempData["Mensaje"] = "Pago realizado con Exito.";
        return RedirectToAction("Listado", "Contrato");
    }
    [Authorize]
    private void validaEstado(IList<Pago> lista)
    {
        foreach(Pago p in lista)
            {
                if(p.Est == 1)
                {
                    p.Activo = "Activo";
                }
                else
                {
                    p.Activo = "Anulado";
                }
            }
    }
}