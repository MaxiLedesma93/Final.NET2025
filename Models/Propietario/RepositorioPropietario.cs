using Tp_Inmobiliaria_Ledesma_Lillo.Models;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
namespace Tp_Inmobiliaria_Ledesma_Lillo.Models;

public class RepositorioPropietario : RepositorioBase, IRepositorioPropietario
{
    
		public RepositorioPropietario(IConfiguration configuration) : base(configuration)
		{
			
		}

		public IList<Propietario> ObtenerTodos()
		{
			var propietarios = new List<Propietario>();

			using(var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"Select {nameof(Propietario.IdPropietario)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Apellido)}, 
                         {nameof(Propietario.Dni)}, {nameof(Propietario.Telefono)}, {nameof(Propietario.Email)}, {nameof(Propietario.Clave)}
						  from propietarios";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using(var reader = command.ExecuteReader())

                while(reader.Read())
                {
                    propietarios.Add(new Propietario{
                        IdPropietario = reader.GetInt32(nameof(Propietario.IdPropietario)),
						Nombre = reader.GetString(nameof(Propietario.Nombre)),
						Apellido = reader.GetString(nameof(Propietario.Apellido)),
						Dni = reader.GetString(nameof(Propietario.Dni)),
						Telefono = reader.GetString(nameof(Propietario.Telefono)),
						Email = reader.GetString(nameof(Propietario.Email)),
						Clave = reader.GetString(nameof(Propietario.Clave)),
                    });
                }
				connection.Close();
            }
            return propietarios;
        }
		}

		public Propietario? ObtenerPorId(int id)
		{
			Propietario? propietario = null;
			
			using(var connection = new MySqlConnection(connectionString))
           {
            var sql = @$"SELECT {nameof(Propietario.IdPropietario)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Apellido)}, 
                        {nameof(Propietario.Dni)}, {nameof(Propietario.Telefono)}, {nameof(Propietario.Email)}, {nameof(Propietario.Clave)}
						FROM propietarios 
						WHERE {nameof(Propietario.IdPropietario)} = @{nameof(Propietario.IdPropietario)}";
            using (var command = new MySqlCommand(sql, connection))
            {
				command.Parameters.AddWithValue($"@{nameof(Propietario.IdPropietario)}", id);
                connection.Open();
                using (var reader = command.ExecuteReader())

                if(reader.Read())
                {
                    propietario = new Propietario{
                        IdPropietario = reader.GetInt32(nameof(Propietario.IdPropietario)),
						Nombre = reader.GetString(nameof(Propietario.Nombre)),
						Apellido = reader.GetString(nameof(Propietario.Apellido)),
						Dni = reader.GetString(nameof(Propietario.Dni)),
						Telefono = reader.GetString(nameof(Propietario.Telefono)),
						Email = reader.GetString(nameof(Propietario.Email)),
						Clave = reader.GetString(nameof(Propietario.Clave)),
                    };
                }
				connection.Close();
				
            }
            return propietario;
        }
		}

		public int Alta(Propietario propietario)
		{
			int id = 0;
		using(var connection = new MySqlConnection(connectionString))
		{
			var sql = @$"INSERT INTO propietarios ({nameof(Propietario.Nombre)}, {nameof(Propietario.Apellido)}, {nameof(Propietario.Dni)},
			{nameof(Propietario.Telefono)}, {nameof(Propietario.Email)}, {nameof(Propietario.Clave)})
				VALUES (@{nameof(Propietario.Nombre)}, @{nameof(Propietario.Apellido)}, @{nameof(Propietario.Dni)},
				@{nameof(Propietario.Telefono)}, @{nameof(Propietario.Email)}, @{nameof(Propietario.Clave)});
				SELECT LAST_INSERT_ID();";
			using(var command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue($"@{nameof(Propietario.Nombre)}", propietario.Nombre);
				command.Parameters.AddWithValue($"@{nameof(Propietario.Apellido)}", propietario.Apellido);
				command.Parameters.AddWithValue($"@{nameof(Propietario.Dni)}", propietario.Dni);
				command.Parameters.AddWithValue($"@{nameof(Propietario.Telefono)}", propietario.Telefono);
				command.Parameters.AddWithValue($"@{nameof(Propietario.Email)}", propietario.Email);
				command.Parameters.AddWithValue($"@{nameof(Propietario.Clave)}", propietario.Clave);

				connection.Open();
				id = Convert.ToInt32(command.ExecuteScalar());
				propietario.IdPropietario = id;
				connection.Close();
			}
		}
		return id;
		}

		public int Modificacion(Propietario propietario)
	{
		int res = -1;
		using(var connection = new MySqlConnection(connectionString))
		{
			var sql = @$"UPDATE propietarios
				SET {nameof(Propietario.Nombre)} = @{nameof(Propietario.Nombre)},
				{nameof(Propietario.Apellido)} = @{nameof(Propietario.Apellido)},
				{nameof(Propietario.Dni)} = @{nameof(Propietario.Dni)},
				{nameof(Propietario.Telefono)} = @{nameof(Propietario.Telefono)},
				{nameof(Propietario.Email)} = @{nameof(Propietario.Email)},
				{nameof(Propietario.Clave)} = @{nameof(Propietario.Clave)}
			
				WHERE {nameof(Propietario.IdPropietario)} = @{nameof(Propietario.IdPropietario)}";
			using(var command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue($"@{nameof(Propietario.Nombre)}", propietario.Nombre);
				command.Parameters.AddWithValue($"@{nameof(Propietario.Apellido)}", propietario.Apellido);
				command.Parameters.AddWithValue($"@{nameof(Propietario.Dni)}", propietario.Dni);
				command.Parameters.AddWithValue($"@{nameof(Propietario.Telefono)}", propietario.Telefono);
				command.Parameters.AddWithValue($"@{nameof(Propietario.Email)}", propietario.Email);
				command.Parameters.AddWithValue($"@{nameof(Propietario.Clave)}", propietario.Clave);
				command.Parameters.AddWithValue($"@{nameof(Propietario.IdPropietario)}", propietario.IdPropietario);
				
				connection.Open();
				command.ExecuteNonQuery();
				connection.Close();
			}
		}
		return res;
	}


	public int Baja(int id)
	{
		using(var connection = new MySqlConnection(connectionString))
		{
			var sql = @$"DELETE FROM propietarios
				WHERE {nameof(Propietario.IdPropietario)} = @{nameof(Propietario.IdPropietario)}";
			using(var command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue($"@{nameof(Propietario.IdPropietario)}", id);
				connection.Open();
				command.ExecuteNonQuery();
				connection.Close();
			}
		}
		return 0;
	}
	public Propietario? ObtenerPorEmail(String email)
		{
			Propietario? propietario = null;
			
			using(var connection = new MySqlConnection(connectionString))
           {
            var sql = @$"SELECT {nameof(Propietario.IdPropietario)}, {nameof(Propietario.Nombre)},
						{nameof(Propietario.Apellido)}, 
                        {nameof(Propietario.Dni)}, {nameof(Propietario.Telefono)}, 
						{nameof(Propietario.Email)}, {nameof(Propietario.Clave)}
						FROM propietarios 
						WHERE {nameof(Propietario.Email)} = @{nameof(Propietario.Email)}";
            using (var command = new MySqlCommand(sql, connection))
            {
				command.Parameters.AddWithValue($"@{nameof(Propietario.Email)}", email);
                connection.Open();
                using (var reader = command.ExecuteReader())

                if(reader.Read())
                {
                    propietario = new Propietario{
                        IdPropietario = reader.GetInt32(nameof(Propietario.IdPropietario)),
						Nombre = reader.GetString(nameof(Propietario.Nombre)),
						Apellido = reader.GetString(nameof(Propietario.Apellido)),
						Dni = reader.GetString(nameof(Propietario.Dni)),
						Telefono = reader.GetString(nameof(Propietario.Telefono)),
						Email = reader.GetString(nameof(Propietario.Email)),
						Clave = reader.GetString(nameof(Propietario.Clave)),
                    };
                }
				connection.Close();
				
            }
            return propietario;
        }
		}
}