using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tp_Inmobiliaria_Ledesma_Lillo.Models;
using Tp_Inmobiliaria_Ledesma_Lillo.Net.Controllers;

namespace Tp_Inmobiliaria_Ledesma_Lillo.Controllers;

public class InmuebleController : Controller 
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration config;
    private readonly IRepositorioPropietario repoProp;
    private readonly IRepositorioInmueble repo;
    private readonly IRepositorioTipo repoTipo;


    public InmuebleController( IRepositorioInmueble repo, IRepositorioPropietario repoProp,
    IRepositorioTipo repoTipo, ILogger<HomeController> logger, IConfiguration config)
    {   
        this.config = config;
        this.repo = repo;
        this.repoProp = repoProp;
        this.repoTipo = repoTipo;
        _logger = logger;
    }

    [Authorize]
    public IActionResult Listado()
    {
        try
        {
            ViewBag.Tipos = repoTipo.ObtenerTodos();
            var lista = repo.ObtenerTodos();
            ViewBag.id = TempData["id"];
            // TempData es para pasar datos entre acciones
				// ViewBag/Data es para pasar datos del controlador a la vista
				// Si viene alguno valor por el tempdata, lo paso al viewdata/viewbag
				if (TempData.ContainsKey("Mensaje"))
					ViewBag.Mensaje = TempData["Mensaje"];
            validaDisponibles(lista);
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
                IList<String> listaDisponibilidad = new List<String>();
                IList<String> listaUso = new List<String>();
                cargarLista(listaDisponibilidad, listaUso);
                ViewBag.Propietarios = repoProp.ObtenerTodos();
                ViewBag.Tipos = repoTipo.ObtenerTodos();
                Inmueble? inmueble = repo.ObtenerPorId(id);
                ViewBag.Disponibilidad = listaDisponibilidad;
                ViewBag.Uso = listaUso;
                validaDisponible(inmueble);
                TempData["Mensaje"] = "Datos guardados correctamente";
                return View(inmueble);
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
                ViewBag.Tipos = repoTipo.ObtenerTodos();
                ViewBag.Propietarios = repoProp.ObtenerTodos();
                return View();
			}
			catch (Exception ex)
			{
                return Json(new { Error = ex.Message });
			}
		}

    [Authorize]
    public IActionResult Guardar(Inmueble inmueble)
    {
        
        try
        {   
            
           if(ModelState.IsValid)
           {
                if(inmueble.IdInmueble > 0)
                {   
                    disp(inmueble);
                    repo.Modificacion(inmueble);
                }
                else{
                    
                    repo.Alta(inmueble);
                    disp(inmueble);
                    TempData["id"] = inmueble.IdInmueble; 
                }
           }
           else 
           {
            return View(inmueble); 
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
        

        Inmueble? inmu = repo.ObtenerPorId(id);
        ViewBag.Propietario = inmu.Duenio.Nombre + " " + inmu.Duenio.Apellido;
        ViewBag.Tipo = inmu.TipoInmueble.Descripcion;
        validaDisponible(inmu);
        return View(inmu);
    }

    [Authorize]
    public IActionResult InmueblesSuspendidos()
    {
        
        var lista = repo.obtenerInmueblesSuspendidos();
        validaDisponibles(lista);
        return View(lista);
    }

    private void validaDisponibles(IList<Inmueble> lista)
    {
        foreach(Inmueble inmu in lista)
        {
            validaDisponible(inmu);
        }
    }

    public void validaDisponible(Inmueble? inmu)
    {
        if(inmu.Disponible == 0)
        {
           inmu.Disp = "Disponible"; 
        }
        else{
                inmu.Disp = "Suspendido";
            }
    }

    private void disp(Inmueble inmu)
    {
        if(inmu.Disp == "Disponible")
        {
            inmu.Disponible = 0;
        }
        else {
            inmu.Disponible = 1;
        }
    }

    private void cargarLista(IList<String> listaD, IList<String> listaU)
    {
        listaD.Add("Suspendido");
        listaD.Add("Disponible");
        listaU.Add("Residencial");
        listaU.Add("Comercial");
    }
}