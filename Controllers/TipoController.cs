using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crmf;
using Tp_Inmobiliaria_Ledesma_Lillo.Models;
using Tp_Inmobiliaria_Ledesma_Lillo.Net.Controllers;

namespace Tp_Inmobiliaria_Ledesma_Lillo.Controllers;

public class TipoController : Controller
{
    private readonly ILogger<HomeController> _logger;
    

    private readonly IConfiguration config;
    private readonly IRepositorioTipo repo;
    

    public TipoController(IRepositorioTipo repo, ILogger<HomeController> logger, IConfiguration config)
    {   this.config = config;
        this.repo  =repo;
        _logger = logger;
    }
    
    [Authorize]
    public IActionResult Listado()
    {
        try
        {
            
            var lista = repo.ObtenerTodos();
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

    [Authorize]
    public IActionResult Crear() //carga el formulario vacio
		{
			try
			{
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
            TempData["Mensaje"] = "Eliminación realizada correctamente";
            return RedirectToAction(nameof(Listado));
        }
        catch(Exception ex)
        {
            return Json(new { Error = ex.Message });
        }
    }

    [Authorize]
    public IActionResult Guardar(Tipo tipo)
    {
        
        try
        {
           if(ModelState.IsValid)
           {
                repo.Alta(tipo);
                TempData["id"] = tipo.IdTipo; 
           }
           else 
           {
             return View(tipo); 
           }
            return RedirectToAction(nameof(Listado));
        }
        catch(Exception ex)
        {
            return Json(new { Error = ex.Message });
        }
    }

    [Authorize]
    public IActionResult Detalle(int id)
    {
       

        Tipo? tipo = repo.ObtenerPorId(id);

        return View(tipo);
    }
}