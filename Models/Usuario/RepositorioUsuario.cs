
using MySql.Data.MySqlClient;


namespace Tp_Inmobiliaria_Ledesma_Lillo.Models;

public class RepositorioUsuario: RepositorioBase, IRepositorioUsuario
{	
	public RepositorioUsuario(IConfiguration configuration) : base(configuration)
	{

	}

	public IList<Usuario> ObtenerTodos()
	{
		var usuarios = new List<Usuario>();

		using (var connection = new MySqlConnection(connectionString))
		{
			var sql = @$"Select {nameof(Usuario.IdUsuario)}, {nameof(Usuario.Nombre)}, {nameof(Usuario.Apellido)}, 
                         {nameof(Usuario.Email)}, {nameof(Usuario.Clave)}, {nameof(Usuario.Avatar)},
                          {nameof(Usuario.Rol)}
						  from usuarios";
			using (var command = new MySqlCommand(sql, connection))
			{
				connection.Open();
				using (var reader = command.ExecuteReader())

					while (reader.Read())
					{
						usuarios.Add(new Usuario
						{
							IdUsuario = reader.GetInt32(nameof(Usuario.IdUsuario)),
							Nombre = reader.GetString(nameof(Usuario.Nombre)),
							Apellido = reader.GetString(nameof(Usuario.Apellido)),
							Email = reader.GetString(nameof(Usuario.Email)),
							Clave = reader.GetString(nameof(Usuario.Clave)),
							Avatar = reader.GetString(nameof(Usuario.Avatar)),
							Rol = reader.GetInt32(nameof(Usuario.Rol))
							//Clave = reader.GetString(nameof(Usuario.Clave)),
						});
					}
				connection.Close();
			}
			return usuarios;
		}
	}

	public Usuario? ObtenerPorId(int id)
	{
		Usuario? usuario = null;

		using (var connection = new MySqlConnection(connectionString))
		{
			var sql = @$"SELECT {nameof(Usuario.IdUsuario)}, {nameof(Usuario.Nombre)}, {nameof(Usuario.Apellido)}, 
                        {nameof(Usuario.Email)}, {nameof(Usuario.Clave)}, {nameof(Usuario.Avatar)}, {nameof(Usuario.Rol)}
						FROM usuarios 
						WHERE {nameof(Usuario.IdUsuario)} = @{nameof(Usuario.IdUsuario)}";
			using (var command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue($"@{nameof(Usuario.IdUsuario)}", id);
				connection.Open();
				using (var reader = command.ExecuteReader())

					if (reader.Read())
					{
						usuario = new Usuario
						{
							IdUsuario = reader.GetInt32(nameof(Usuario.IdUsuario)),
							Nombre = reader.GetString(nameof(Usuario.Nombre)),
							Apellido = reader.GetString(nameof(Usuario.Apellido)),
							Email = reader.GetString(nameof(Usuario.Email)),
							Clave = reader.GetString(nameof(Usuario.Clave)),
							Avatar = reader.GetString(nameof(Usuario.Avatar)),
							Rol = reader.GetInt32(nameof(Usuario.Rol))
							//Clave = reader.GetString(nameof(Usuario.Clave)),
						};
					}
				connection.Close();

			}
			return usuario;
		}
	}
	public Usuario ObtenerPorEmail(string email)
	{
		Usuario? usuario = null;

		using (var connection = new MySqlConnection(connectionString))
		{
			var sql = @$"SELECT {nameof(Usuario.IdUsuario)}, {nameof(Usuario.Nombre)}, {nameof(Usuario.Apellido)}, 
                        {nameof(Usuario.Email)}, {nameof(Usuario.Clave)}, {nameof(Usuario.Avatar)}, {nameof(Usuario.Rol)}
						FROM usuarios 
						WHERE {nameof(Usuario.Email)} = @{nameof(Usuario.Email)}";
			using (var command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue($"@{nameof(Usuario.Email)}", email);
				connection.Open();
				using (var reader = command.ExecuteReader())

					if (reader.Read())
					{
						usuario = new Usuario
						{
							IdUsuario = reader.GetInt32(nameof(Usuario.IdUsuario)),
							Nombre = reader.GetString(nameof(Usuario.Nombre)),
							Apellido = reader.GetString(nameof(Usuario.Apellido)),
							Email = reader.GetString(nameof(Usuario.Email)),
							Clave = reader.GetString(nameof(Usuario.Clave)),
							Avatar = reader.GetString(nameof(Usuario.Avatar)),
							Rol = reader.GetInt32(nameof(Usuario.Rol))
							//Clave = reader.GetString(nameof(Usuario.Clave)),
						};
					}
				connection.Close();

			}

			return usuario;
		}
	}
	public int Alta(Usuario usuario)
	{
		int id = 0;
		using (var connection = new MySqlConnection(connectionString))
		{
			var sql = @$"INSERT INTO usuarios ({nameof(Usuario.Nombre)}, {nameof(Usuario.Apellido)}, {nameof(Usuario.Email)},
			{nameof(Usuario.Clave)}, {nameof(Usuario.Avatar)}, {nameof(Usuario.Rol)})
			VALUES (@{nameof(Usuario.Nombre)}, @{nameof(Usuario.Apellido)}, @{nameof(Usuario.Email)},
			@{nameof(Usuario.Clave)}, @{nameof(Usuario.Avatar)}, @{nameof(Usuario.Rol)});
			SELECT LAST_INSERT_ID();";
			using (var command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue($"@{nameof(Usuario.Nombre)}", usuario.Nombre);
				command.Parameters.AddWithValue($"@{nameof(Usuario.Apellido)}", usuario.Apellido);
				command.Parameters.AddWithValue($"@{nameof(Usuario.Email)}", usuario.Email);
				command.Parameters.AddWithValue($"@{nameof(Usuario.Clave)}", usuario.Clave);
				command.Parameters.AddWithValue($"@{nameof(Usuario.Avatar)}", usuario.Avatar);
				command.Parameters.AddWithValue($"@{nameof(Usuario.Rol)}", usuario.Rol);

				connection.Open();
				id = Convert.ToInt32(command.ExecuteScalar());
				usuario.IdUsuario = id;
				connection.Close();
			}
		}
		return id;
	}

	public int Modificacion(Usuario usuario)
	{
		using (var connection = new MySqlConnection(connectionString))
		{
			var sql = @$"UPDATE usuarios
			SET {nameof(Usuario.Nombre)} = @{nameof(Usuario.Nombre)},
			{nameof(Usuario.Apellido)} = @{nameof(Usuario.Apellido)},
			{nameof(Usuario.Email)} = @{nameof(Usuario.Email)},
			{nameof(Usuario.Clave)} = @{nameof(Usuario.Clave)},
			{nameof(Usuario.Avatar)} = @{nameof(Usuario.Avatar)},
			{nameof(Usuario.Rol)} = @{nameof(Usuario.Rol)}
			
			WHERE {nameof(Usuario.IdUsuario)} = @{nameof(Usuario.IdUsuario)}";
			using (var command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue($"@{nameof(Usuario.Nombre)}", usuario.Nombre);
				command.Parameters.AddWithValue($"@{nameof(Usuario.Apellido)}", usuario.Apellido);
				command.Parameters.AddWithValue($"@{nameof(Usuario.Email)}", usuario.Email);
				command.Parameters.AddWithValue($"@{nameof(Usuario.Clave)}", usuario.Clave);
				command.Parameters.AddWithValue($"@{nameof(Usuario.Avatar)}", usuario.Avatar);
				command.Parameters.AddWithValue($"@{nameof(Usuario.Rol)}", usuario.Rol);
				command.Parameters.AddWithValue($"@{nameof(Usuario.IdUsuario)}", usuario.IdUsuario);
				connection.Open();
				command.ExecuteNonQuery();
				connection.Close();
			}
		}
		return 0;
	}
	public int EliminarAvatar(Usuario usuario)
	{
		using (var connection = new MySqlConnection(connectionString))
		{
			var sql = @$"UPDATE usuarios
			SET {nameof(Usuario.Nombre)} = @{nameof(Usuario.Nombre)},
			{nameof(Usuario.Apellido)} = @{nameof(Usuario.Apellido)},
			{nameof(Usuario.Email)} = @{nameof(Usuario.Email)},
			{nameof(Usuario.Clave)} = @{nameof(Usuario.Clave)},
			{nameof(Usuario.Avatar)} = @{nameof(Usuario.Avatar)},
			{nameof(Usuario.Rol)} = @{nameof(Usuario.Rol)}
			
			WHERE {nameof(Usuario.IdUsuario)} = @{nameof(Usuario.IdUsuario)}";
			using (var command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue($"@{nameof(Usuario.Nombre)}", usuario.Nombre);
				command.Parameters.AddWithValue($"@{nameof(Usuario.Apellido)}", usuario.Apellido);
				command.Parameters.AddWithValue($"@{nameof(Usuario.Email)}", usuario.Email);
				command.Parameters.AddWithValue($"@{nameof(Usuario.Clave)}", usuario.Clave);
				command.Parameters.AddWithValue($"@{nameof(Usuario.Avatar)}", "");
				command.Parameters.AddWithValue($"@{nameof(Usuario.Rol)}", usuario.Rol);
				command.Parameters.AddWithValue($"@{nameof(Usuario.IdUsuario)}", usuario.IdUsuario);
				connection.Open();
				command.ExecuteNonQuery();
				connection.Close();
			}
		}
		return 0;
	}

	public int Baja(int id)
	{
		using (var connection = new MySqlConnection(connectionString))
		{
			var sql = @$"DELETE FROM usuarios
			WHERE {nameof(Usuario.IdUsuario)} = @{nameof(Usuario.IdUsuario)}";
			using (var command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue($"@{nameof(Usuario.IdUsuario)}", id);
				connection.Open();
				command.ExecuteNonQuery();
				connection.Close();
			}
		}
		return 0;
	}
	}