using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
namespace Tp_Inmobiliaria_Ledesma_Lillo.Models;

public class RepositorioInquilino: RepositorioBase , IRepositorioInquilino
{

	public RepositorioInquilino(IConfiguration configuration) : base(configuration)
	{
		
	}

	public IList<Inquilino> ObtenerTodos()
	{
		var inquilinos = new List<Inquilino>();

		using(var connection = new MySqlConnection(connectionString))
	{
		var sql = @$"Select {nameof(Inquilino.IdInquilino)}, {nameof(Inquilino.Nombre)}, {nameof(Inquilino.Apellido)}, 
						{nameof(Inquilino.Dni)}, {nameof(Inquilino.Telefono)}, {nameof(Inquilino.Email)} 
						from inquilinos";
		using (var command = new MySqlCommand(sql, connection))
		{
			connection.Open();
			using(var reader = command.ExecuteReader())

			while(reader.Read())
			{
				inquilinos.Add(new Inquilino{
					IdInquilino = reader.GetInt32(nameof(Inquilino.IdInquilino)),
					Nombre = reader.GetString(nameof(Inquilino.Nombre)),
					Apellido = reader.GetString(nameof(Inquilino.Apellido)),
					Dni = reader.GetString(nameof(Inquilino.Dni)),
					Telefono = reader.GetString(nameof(Inquilino.Telefono)),
					Email = reader.GetString(nameof(Inquilino.Email)),
					//Clave = reader.GetString(nameof(Inquilino.Clave)),
				});
			}
			connection.Close();
		}
		return inquilinos;
	}
	}

	public Inquilino? ObtenerPorId(int id)
	{
		Inquilino? inquilino = null;
		
		using(var connection = new MySqlConnection(connectionString))
	{
		var sql = @$"SELECT {nameof(Inquilino.IdInquilino)}, {nameof(Inquilino.Nombre)}, {nameof(Inquilino.Apellido)}, 
					{nameof(Inquilino.Dni)}, {nameof(Inquilino.Telefono)}, {nameof(Inquilino.Email)}
					FROM inquilinos 
					WHERE {nameof(Inquilino.IdInquilino)} = @{nameof(Inquilino.IdInquilino)}";
		using (var command = new MySqlCommand(sql, connection))
		{
			command.Parameters.AddWithValue($"@{nameof(Inquilino.IdInquilino)}", id);
			connection.Open();
			using(var reader = command.ExecuteReader())

			if(reader.Read())
			{
				inquilino = new Inquilino{
					IdInquilino = reader.GetInt32(nameof(Inquilino.IdInquilino)),
					Nombre = reader.GetString(nameof(Inquilino.Nombre)),
					Apellido = reader.GetString(nameof(Inquilino.Apellido)),
					Dni = reader.GetString(nameof(Inquilino.Dni)),
					Telefono = reader.GetString(nameof(Inquilino.Telefono)),
					Email = reader.GetString(nameof(Inquilino.Email)),
					//Clave = reader.GetString(nameof(Inquilino.Clave)),
				};
			}
			connection.Close();
			
		}
		return inquilino;
	}
	}

	public int Alta(Inquilino inquilino)
	{
		int id = 0;
	using(var connection = new MySqlConnection(connectionString))
	{
		var sql = @$"INSERT INTO inquilinos ({nameof(Inquilino.Nombre)}, 
			{nameof(Inquilino.Apellido)}, 
			{nameof(Inquilino.Dni)},
			{nameof(Inquilino.Telefono)}, 
			{nameof(Inquilino.Email)})

			VALUES (@{nameof(Inquilino.Nombre)}, 
			@{nameof(Inquilino.Apellido)}, 
			@{nameof(Inquilino.Dni)},
			@{nameof(Inquilino.Telefono)}, 
			@{nameof(Inquilino.Email)});
			
			SELECT LAST_INSERT_ID();";
		using(var command = new MySqlCommand(sql, connection))
		{
			command.Parameters.AddWithValue($"@{nameof(Inquilino.Nombre)}", inquilino.Nombre);
			command.Parameters.AddWithValue($"@{nameof(Inquilino.Apellido)}", inquilino.Apellido);
			command.Parameters.AddWithValue($"@{nameof(Inquilino.Dni)}", inquilino.Dni);
			command.Parameters.AddWithValue($"@{nameof(Inquilino.Telefono)}", inquilino.Telefono);
			command.Parameters.AddWithValue($"@{nameof(Inquilino.Email)}", inquilino.Email);

			connection.Open();
			id = Convert.ToInt32(command.ExecuteScalar());
			inquilino.IdInquilino = id;
			connection.Close();
		}
	}
	return id;
	}

	public int Modificacion(Inquilino inquilino)
{
	using(var connection = new MySqlConnection(connectionString))
	{
		var sql = @$"UPDATE inquilinos
			SET {nameof(Inquilino.Nombre)} = @{nameof(Inquilino.Nombre)},
			{nameof(Inquilino.Apellido)} = @{nameof(Inquilino.Apellido)},
			{nameof(Inquilino.Dni)} = @{nameof(Inquilino.Dni)},
			{nameof(Inquilino.Telefono)} = @{nameof(Inquilino.Telefono)},
			{nameof(Inquilino.Email)} = @{nameof(Inquilino.Email)}
			WHERE {nameof(Inquilino.IdInquilino)} = @{nameof(Inquilino.IdInquilino)}";
		using(var command = new MySqlCommand(sql, connection))
		{
			command.Parameters.AddWithValue($"@{nameof(Inquilino.Nombre)}", inquilino.Nombre);
			command.Parameters.AddWithValue($"@{nameof(Inquilino.Apellido)}", inquilino.Apellido);
			command.Parameters.AddWithValue($"@{nameof(Inquilino.Dni)}", inquilino.Dni);
			command.Parameters.AddWithValue($"@{nameof(Inquilino.Telefono)}", inquilino.Telefono);
			command.Parameters.AddWithValue($"@{nameof(Inquilino.Email)}", inquilino.Email);
			command.Parameters.AddWithValue($"@{nameof(Inquilino.IdInquilino)}", inquilino.IdInquilino);
			connection.Open();
			command.ExecuteNonQuery();
			connection.Close();
		}
	}
	return 0;
}


	public int Baja(int id)
	{
		using(var connection = new MySqlConnection(connectionString))
		{
			var sql = @$"DELETE FROM inquilinos
				WHERE {nameof(Inquilino.IdInquilino)} = @{nameof(Inquilino.IdInquilino)}";
			using(var command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue($"@{nameof(Inquilino.IdInquilino)}", id);
				connection.Open();
				command.ExecuteNonQuery();
				connection.Close();
			}
		}
		return 0;
    }
}