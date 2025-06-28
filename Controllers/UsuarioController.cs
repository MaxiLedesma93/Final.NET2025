
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Tp_Inmobiliaria_Ledesma_Lillo.Models;
using Tp_Inmobiliaria_Ledesma_Lillo.Net.Controllers;



namespace Tp_Inmobiliaria_Ledesma_Lillo.Controllers
{
	public class UsuarioController : Controller
	{
      	private readonly IWebHostEnvironment environment;
		private readonly ILogger<HomeController> _logger;
		private readonly IRepositorioUsuario repo;
		private readonly IConfiguration config;

    public UsuarioController( ILogger<HomeController> logger,IWebHostEnvironment environment,
	 IConfiguration config, IRepositorioUsuario repo)
    {	
		this.repo = repo;
		this.environment = environment;
		this.config = config;
        _logger = logger;
    }
		// GET: Usuarios
		[Authorize(Policy = "Administrador")]
		public ActionResult Listado()
		{
			var usuarios1 = repo.ObtenerTodos();
			return View(usuarios1);
		}

		// GET: Usuarios/Details/5
		[Authorize(Policy = "Administrador")]
		public ActionResult Details(int id)
		{
			var e = repo.ObtenerPorId(id);
			return View(e);
		}

		// GET: Usuario/Crear
		[Authorize(Policy = "Administrador")]
		public ActionResult Crear()
		{
			
			return View();
		}

		// POST: Usuario/Crear
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "Administrador")]
		public ActionResult Crear(Usuario usuario)
    	{
			try
			{   
				if(!ModelState.IsValid)
				{   
					return View();
				}
				else{
					
					usuario.Clave = Hashear(usuario.Clave);
					
					repo.Alta(usuario);
					
					if (usuario.AvatarFile != null && usuario.IdUsuario >0)
					{
						string wwwPath = environment.WebRootPath;
						string path = Path.Combine(wwwPath, "Uploads");
						if (!Directory.Exists(path))
						{
							Directory.CreateDirectory(path);
						}
						//Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
						string fileName = "avatar_" + usuario.IdUsuario + Path.GetExtension(usuario.AvatarFile.FileName);
						string pathCompleto = Path.Combine(path, fileName);
						usuario.Avatar = Path.Combine("/Uploads", fileName);
						
						// Esta operación guarda la foto en memoria en la ruta que necesitamos
						using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
						{
							usuario.AvatarFile.CopyTo(stream);
						}
						
						repo.Modificacion(usuario);
					}
					
					
					TempData["id"] = usuario.IdUsuario; 
				}
			
				return RedirectToAction(nameof(Listado));
			}
			catch(Exception ex)
			{
				return Json(new { Error = ex.Message });
			}

		}
		
		// GET: Usuario/Editar/5
		[Authorize]
		[HttpGet]
		public ActionResult Perfil()
		{
			ViewData["Title"] = "Mi perfil";
			
			Usuario usuario = repo.ObtenerPorEmail(User.Identity.Name);
			return View("Perfil", usuario);
		}

		[Authorize]
		[HttpPost]
		public ActionResult Perfil(Usuario usuario){
			ViewData["Title"] = "Mi perfil";
			Usuario u = repo.ObtenerPorEmail(User.Identity.Name);
			if (usuario.AvatarFile != null && u.IdUsuario >0)
			{
				string wwwPath = environment.WebRootPath;
				string path = Path.Combine(wwwPath, "Uploads");
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				//Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
				string fileName = "avatar_" + u.IdUsuario + Path.GetExtension(usuario.AvatarFile.FileName);
				string pathCompleto = Path.Combine(path, fileName);
				u.Avatar = Path.Combine("/Uploads", fileName);
				
				// Esta operación guarda la foto en memoria en la ruta que necesitamos
				using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
				{
					usuario.AvatarFile.CopyTo(stream);
					stream.Dispose();
				}
				
				
				repo.Modificacion(u);
				TempData["mensaje"] = "datos guardados correctamente.";
			}

			return View("Perfil", u);
		}
		
		[Authorize]
		[HttpPost]
		public ActionResult borrarAvatar()
		{	var vista = nameof(Perfil);
			ViewData["Title"] = "Mi perfil";
			var user = repo.ObtenerPorEmail(User.Identity.Name);
			user.Avatar=" ";
			var ruta = Path.Combine(environment.WebRootPath, "Uploads", $"avatar_{user.IdUsuario}" + Path.GetExtension(user.Avatar));
				if (System.IO.File.Exists(ruta))
					System.IO.File.Delete(ruta);
			repo.Modificacion(user);
			return View(vista, user);
		}
		
		[Authorize]
		public ActionResult EditarPerfil()
		{
			ViewData["Title"] = "Mi perfil";
			Usuario usuario = repo.ObtenerPorEmail(User.Identity.Name);
			return View("EditarPerfil", usuario);
		}


		[HttpPost]
		[Authorize]
		public ActionResult Avatar(Usuario usuario){
			Usuario u = repo.ObtenerPorId(usuario.IdUsuario);

			if (usuario.AvatarFile != null && u.IdUsuario >0)
			{
				string wwwPath = environment.WebRootPath;
				string path = Path.Combine(wwwPath, "Uploads");
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				//Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
				string fileName = "avatar_" + u.IdUsuario + Path.GetExtension(usuario.AvatarFile.FileName);
				string pathCompleto = Path.Combine(path, fileName);
				u.Avatar = Path.Combine("/Uploads", fileName);
				
				// Esta operación guarda la foto en memoria en la ruta que necesitamos
				using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
				{
					usuario.AvatarFile.CopyTo(stream);
					stream.Dispose();
				}
				
				
				repo.Modificacion(u);
				TempData["mensaje"] = "datos guardados correctamente.";
			}
			return RedirectToAction("Perfil",usuario);
		}

		[HttpPost]
		[Authorize]
		public ActionResult EditarPerfil(Usuario usuario){

			var vista = nameof(Perfil);
			
			Usuario u = repo.ObtenerPorEmail(User.Identity.Name);
			u.Nombre = usuario.Nombre;
			u.Apellido = usuario.Apellido;
			u.Email = usuario.Email;
			u.Rol = usuario.Rol;
			
			if (usuario.AvatarFile != null && u.IdUsuario >0)
			{
				string wwwPath = environment.WebRootPath;
				string path = Path.Combine(wwwPath, "Uploads");
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				//Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
				string fileName = "avatar_" + u.IdUsuario + Path.GetExtension(usuario.AvatarFile.FileName);
				string pathCompleto = Path.Combine(path, fileName);
				u.Avatar = Path.Combine("/Uploads", fileName);
				
				// Esta operación guarda la foto en memoria en la ruta que necesitamos
				using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
				{
					usuario.AvatarFile.CopyTo(stream);
					stream.Dispose();
				}
				
				
				repo.Modificacion(u);
				TempData["mensaje"] = "datos guardados correctamente.";
				
			}
			
			repo.Modificacion(u);
			return RedirectToAction(vista,u);
		}

		[HttpPost]
		[Authorize]
		public ActionResult CambiarPass(Usuario usuario){
			var vista = nameof(Perfil);
			ViewData["Title"] = "Mi perfil";
			var user = repo.ObtenerPorEmail(User.Identity.Name);
			string hashed = Hashear(usuario.Clave);
			
			if(user.Clave==hashed){
				user.Clave = Hashear(usuario.clnueva);
				repo.Modificacion(user);
				usuario.clnueva=""; //borra la claven de texto plano.
			}
			return View(vista, user);
		}


		// GET: Usuario/Editar/5
		[Authorize(Policy = "Administrador")]
		public ActionResult Editar(int id)
		{
			ViewData["Title"] = "Editar usuario";
			Usuario u = repo.ObtenerPorId(id);
			ViewBag.Roles = u.ObtenerRoles();
			return View("Editar",u);
		}

		// POST: Usuarios/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy ="Administrador")]
		public ActionResult Editar(int id, Usuario usuario)
		{
			var vista = nameof(Editar);//de que vista provengo
			try
			{
				if (User.IsInRole("Empleado"))//no soy admin
				{
					vista = nameof(Perfil);//solo puedo ver mi perfil
					var usuarioActual = repo.ObtenerPorEmail(User.Identity.Name);
					if (usuarioActual.IdUsuario != id)//si no es admin, solo puede modificarse él mismo
						return RedirectToAction("Home");
				}
				else{
					Usuario u = new Usuario();
					u=repo.ObtenerPorId(id);
					u.Nombre = usuario.Nombre;
					u.Apellido = usuario.Apellido;
					u.Email = usuario.Email;
					//Permite ingresar clave nueva por olvido del 
					//empleado en caso de ser necesario el formulario no obliga el ingreso de la clave.
					if(User.IsInRole("Administrador")&&usuario.Clave!=null){
						u.Clave = Hashear(usuario.Clave);
					}
					u.Rol = usuario.Rol;
					if (usuario.AvatarFile != null && usuario.IdUsuario >0)
					{
						string wwwPath = environment.WebRootPath;
						string path = Path.Combine(wwwPath, "Uploads");
						if (!Directory.Exists(path))
						{
							Directory.CreateDirectory(path);
						}
						//Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
						string fileName = "avatar_" + usuario.IdUsuario + Path.GetExtension(usuario.AvatarFile.FileName);
						string pathCompleto = Path.Combine(path, fileName);
						u.Avatar = Path.Combine("/Uploads", fileName);
						
						// Esta operación guarda la foto en memoria en la ruta que necesitamos
						using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
						{
							usuario.AvatarFile.CopyTo(stream);
							stream.Dispose();
						}
						repo.Modificacion(u);
						TempData["mensaje"] = "datos guardados correctamente.";
						return RedirectToAction(vista,u);
					}
					repo.Modificacion(u);
				}
				
				// TODO: Add update logic here

				return RedirectToAction(vista);
			}
			catch (Exception ex)
			{//colocar breakpoints en la siguiente línea por si algo falla
				throw;
			}
		}

		// GET: Usuarios/Delete/5
		[Authorize(Policy = "Administrador")]
		public ActionResult Eliminar(int id)
		{
			return View();
		}

		// POST: Usuarios/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "Administrador")]
		public ActionResult Eliminar(int id, Usuario usuario)
		{
			try
			{
				var ruta = Path.Combine(environment.WebRootPath, "Uploads", $"avatar_{id}" + Path.GetExtension(usuario.Avatar));
				if (System.IO.File.Exists(ruta))
					System.IO.File.Delete(ruta);
				repo.Baja(id);
				return RedirectToAction("Listado");
			}
			catch
			{
				return View();
			}
		}

		[AllowAnonymous]
		// GET: Usuarios/Login/
		public IActionResult LoginModal()
		{
			return PartialView("_LoginModal", new LoginView());
		}

		[AllowAnonymous]
		// GET: Usuarios/Login/
		public IActionResult Login(string returnUrl)
		{
			TempData["returnUrl"] = returnUrl;
			return View();
		}

		// POST: Usuarios/Login/
		
		[AllowAnonymous]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginView login)
		{
			
			try{
				var  returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string) ? "/Home" : TempData["returnUrl"].ToString();
				if (ModelState.IsValid)
				{
					string hashed =  Hashear(login.Clave);
					var e = repo.ObtenerPorEmail(login.Usuario);
					if (e == null || e.Clave != hashed)
					{
						ModelState.AddModelError("", "El email o la clave no son correctos");
						TempData["returnUrl"] = returnUrl;
						return View();
					}else{
						Console.Write("Usuario logueado correctamente");
					}

					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, e.Email),
						new Claim("FullName", e.Nombre + " " + e.Apellido),
						new Claim(ClaimTypes.Role, e.RolNombre),
					};

					var claimsIdentity = new ClaimsIdentity(
							claims, CookieAuthenticationDefaults.AuthenticationScheme);

					await HttpContext.SignInAsync(
							CookieAuthenticationDefaults.AuthenticationScheme,
							new ClaimsPrincipal(claimsIdentity));
					
					TempData.Remove("returnUrl");
					return Redirect(returnUrl);
				}
				TempData.Remove("returnUrl");
				return Redirect(returnUrl);
			}
			catch (Exception ex)
			{
				Console.WriteLine("error en login", ex.Message);
				return View();
			}
			
		}

		// GET: /salir
		[Route("salir", Name = "logout")]
		public async Task<ActionResult> Logout()
		{
			await HttpContext.SignOutAsync(
					CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Home");
		}
		
		private string Hashear(string pal){
			string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
							password: pal,
							salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
							prf: KeyDerivationPrf.HMACSHA1,
							iterationCount: 1000,
							numBytesRequested: 256 / 8));
							
			return hashed;
		}
	}
		
	
}
