using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Tp_Inmobiliaria_Ledesma_Lillo.Models;
using Tp_Inmobiliaria_Ledesma_Lillo.Net.Controllers;

namespace Tp_Inmobiliaria_Ledesma_Lillo.Controllers;

public class ContratoController : Controller 
{
    private readonly ILogger<HomeController> _logger;
    
    private readonly IConfiguration config;
    private readonly IRepositorioContrato repo;

    private readonly IRepositorioInmueble repoInmu;
    private readonly IRepositorioInquilino repoInqui;
    

    public ContratoController(IRepositorioContrato repo, IRepositorioInmueble repoInmu,
      ILogger<HomeController> logger, IConfiguration config, IRepositorioInquilino repoInqui)
    {   this.config = config;
        this.repo  =repo;
        this.repoInqui = repoInqui;
        this.repoInmu = repoInmu;
        _logger = logger;
    }

    [Authorize]
    public IActionResult Listado(int? dias, DateTime? fecInf, DateTime? fecSup, string? dir)
    {
        try
        {   ViewBag.dias = dias;
            IList<Contrato> filtrados = new List<Contrato>();
            var lista=repo.ObtenerTodos();
            var listaInm = repoInmu.ObtenerTodos();
            ViewBag.Inmuebles = listaInm;
            if(dir!=null){
                filtrados = repo.ObtenerPorInmuebleDir(dir);
                return View(filtrados);
            }
            
            if(fecInf!=null&&fecSup!=null){
               
                
                lista=repo.ObtenerTodosVigentes((DateTime)fecInf, (DateTime)fecSup);
                return View(lista);
                
            }
            if(dias==0||dias==null){
               
                ViewBag.id = TempData["id"];
                // TempData es para pasar datos entre acciones
                    // ViewBag/Data es para pasar datos del controlador a la vista
                    // Si viene alguno valor por el tempdata, lo paso al viewdata/viewbag
                    if (TempData.ContainsKey("Mensaje"))
                        ViewBag.Mensaje = TempData["Mensaje"];
                return View(lista);
            }
            else{
                       
                DateTime fechLim = DateTime.Today.AddDays((double)dias);
                filtrados = repo.ObtenerPorFechaVenc(fechLim);
                return View(filtrados);
            }
            
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
               
                ViewBag.Inmuebles = repoInmu.ObtenerTodos();               
                ViewBag.Inquilinos = repoInqui.ObtenerTodos();

                var contrato = repo.ObtenerPorId(id);
                TempData["Mensaje"] = "Datos guardados correctamente";
                return View(contrato);
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
            
           if(ModelState.IsValid)
           {
                if(contrato.IdContrato > 0)
                {   
                    repo.Modificacion(contrato);
                }
                else{
                    
                    repo.Alta(contrato);
                    TempData["id"] = contrato.IdContrato; 
                }
           }
           else 
           {
            return View(contrato); 
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
       

        Contrato? contrato = repo.ObtenerPorId(id);

        ViewBag.Inquilino = contrato.Inquilino.Nombre + " " + contrato.Inquilino.Apellido; 
        ViewBag.Inmueble = contrato.Inmueble.Direccion;

        return View(contrato);
    }
}