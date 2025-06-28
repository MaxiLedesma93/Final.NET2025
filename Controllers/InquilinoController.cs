
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tp_Inmobiliaria_Ledesma_Lillo.Models;
using Tp_Inmobiliaria_Ledesma_Lillo.Net.Controllers;

namespace Tp_Inmobiliaria_Ledesma_Lillo.Controllers;

public class InquilinoController : Controller 
{
    private readonly ILogger<HomeController> _logger;
    private readonly IRepositorioInquilino repo;
    private readonly IConfiguration config;

    

    public InquilinoController(ILogger<HomeController> logger, IRepositorioInquilino repo,
    		 IConfiguration config)
    {   this.config = config;
        this.repo = repo;
        _logger = logger;
    }

    [Authorize]
    public IActionResult Listado()
    {
        try
        {
           
            var lista = repo.ObtenerTodos();
            ViewBag.id = TempData["id"];
            return View(lista);
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
              
                var inquilino = repo.ObtenerPorId(id);
                return View(inquilino);
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

    [Authorize]
    public IActionResult Guardar(Inquilino inquilino)
    {
        
        try
        {
           if(ModelState.IsValid)
           {
                if(inquilino.IdInquilino > 0)
                {
                    repo.Modificacion(inquilino);
                }
                else{
                    repo.Alta(inquilino);
                    TempData["id"] = inquilino.IdInquilino; 
                }
           }
           else 
           {
            return View(inquilino); 
           }
            return RedirectToAction(nameof(Listado));
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
            TempData["Mensaje"] = "Eliminación realizada correctamente";
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
       
        Inquilino? inq = repo.ObtenerPorId(id);
        return View(inq);
    }
}