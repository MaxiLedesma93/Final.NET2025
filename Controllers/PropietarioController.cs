
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tp_Inmobiliaria_Ledesma_Lillo.Models;
using Tp_Inmobiliaria_Ledesma_Lillo.Net.Controllers;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace Tp_Inmobiliaria_Ledesma_Lillo.Controllers;

public class PropietarioController : Controller 
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration config;
    private readonly IRepositorioPropietario repo;
    

    public PropietarioController(IRepositorioPropietario repo, ILogger<HomeController> logger,
     IConfiguration config)
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
    public IActionResult Editar(int id)
    {
        try
        {
            if(id > 0)
            {
                var propietario = repo.ObtenerPorId(id);
                TempData["Mensaje"] = "Datos guardados correctamente";
                return View(propietario);
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
    public IActionResult Guardar(Propietario propietario)
    {
       
        try
        {
           if(ModelState.IsValid)
           {
                if(propietario.IdPropietario > 0)
                {   
                    repo.Modificacion(propietario);

                }
                else{
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
							password: propietario.Clave,
							salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
							prf: KeyDerivationPrf.HMACSHA1,
							iterationCount: 1000,
							numBytesRequested: 256 / 8));
                    propietario.Clave = hashed;
                    repo.Alta(propietario);
                    TempData["id"] = propietario.IdPropietario;
                }
           }
           else 
           {
            return View(propietario); 
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
        
        Propietario? p = repo.ObtenerPorId(id);
        return View(p);
    }
}