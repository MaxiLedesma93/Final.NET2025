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

    private readonly IConfiguration config;

    

    public PagoController(ILogger<HomeController> logger, IRepositorioPago repo,
     IRepositorioContrato repoContra, IConfiguration config)
    {   this.repo = repo;
        this.config = config;
        this.repoContra = repoContra;
        _logger = logger;
    }
    [Authorize]
    public IActionResult Listado()
    {
        try
        {
            var lista = repo.ObtenerTodos();
            if(lista.Count != 0)
            { validaEstado(lista); }
            
            ViewBag.id = TempData["id"];
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

    [Authorize(Policy = "Administrador")]
    public IActionResult Eliminar(int id)
    {
       
        try
        {
            repo.Baja(id);
            Pago p = repo.ObtenerPorId(id);
            p.Activo = "Inactivo";
            TempData["Mensaje"] = "Eliminación realizada correctamente";
            return RedirectToAction(nameof(Listado));
        }
        catch(Exception ex)
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
           if(ModelState.IsValid)
           {
                if(pago.IdPago > 0)
                {
                    repo.Modificacion(pago);
                }
                else
                {
                    repo.Alta(pago);
                    pago.Est = 0;
                    TempData["id"] = pago.IdPago;
                }
           }
           else 
           {
             return View(pago); 
           }
            return RedirectToAction(nameof(Listado));
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
    private void validaEstado(IList<Pago> lista)
    {
        foreach(Pago p in lista)
            {
                if(p.Est == 0)
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