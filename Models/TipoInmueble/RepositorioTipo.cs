using MySql.Data.MySqlClient;

namespace Tp_Inmobiliaria_Ledesma_Lillo.Models;

public class RepositorioTipo : RepositorioBase, IRepositorioTipo

   
{
    
		public RepositorioTipo(IConfiguration configuration) : base(configuration)
		{
			
		}

    public IList<Tipo> ObtenerTodos()
		{
			var tipos = new List<Tipo>();

			using(var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"Select {nameof(Tipo.IdTipo)}, {nameof(Tipo.Descripcion)} 
						  from tipos";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using(var reader = command.ExecuteReader())

                while(reader.Read())
                {
                    tipos.Add(new Tipo{
                        IdTipo = reader.GetInt32(nameof(Tipo.IdTipo)),
						Descripcion = reader.GetString(nameof(Tipo.Descripcion)),
                    });
                }
				connection.Close();
            }
            return tipos;
        }
		}

		public Tipo? ObtenerPorId(int id)
		{
			Tipo? tipo = null;
			
			using(var connection = new MySqlConnection(connectionString))
           {
            var sql = @$"Select {nameof(Tipo.IdTipo)}, {nameof(Tipo.Descripcion)} 
						  from tipos
						WHERE {nameof(Tipo.IdTipo)} = @{nameof(Tipo.IdTipo)}";
            using (var command = new MySqlCommand(sql, connection))
            {
				command.Parameters.AddWithValue($"@{nameof(Tipo.IdTipo)}", id);
                connection.Open();
                using (var reader = command.ExecuteReader())

                if(reader.Read())
                {
                    tipo = new Tipo{
                        IdTipo = reader.GetInt32(nameof(Tipo.IdTipo)),
						Descripcion = reader.GetString(nameof(Tipo.Descripcion)),
                    };
                }
				connection.Close();
				
            }
            return tipo;
        }
		}

		public int Alta(Tipo tipo)
		{
			int id = 0;
		using(var connection = new MySqlConnection(connectionString))
		{
			var sql = @$"INSERT INTO tipos ({nameof(Tipo.Descripcion)})
				VALUES (@{nameof(Tipo.Descripcion)});
				SELECT LAST_INSERT_ID();";
			using(var command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue($"@{nameof(Tipo.Descripcion)}", tipo.Descripcion);

				connection.Open();
				id = Convert.ToInt32(command.ExecuteScalar());
				tipo.IdTipo = id;
				connection.Close();
			}
		}
		return id;
		}

        public int Baja(int id)
	{
		using(var connection = new MySqlConnection(connectionString))
		{
			var sql = @$"DELETE FROM tipos
				WHERE {nameof(Tipo.IdTipo)} = @{nameof(Tipo.IdTipo)}";
			using(var command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue($"@{nameof(Tipo.IdTipo)}", id);
				connection.Open();
				command.ExecuteNonQuery();
				connection.Close();
			}
		}
		return 0;
	}
		public int Modificacion(Tipo tipo)
		{
			int res = -1;
			using(var connection = new MySqlConnection(connectionString))
			{
				var sql = @$"UPDATE tipos
					SET {nameof(Tipo.Descripcion)} = @{nameof(Tipo.Descripcion)},
					
				
					WHERE {nameof(Tipo.IdTipo)} = @{nameof(Tipo.IdTipo)}";
				using(var command = new MySqlCommand(sql, connection))
				{
					
					command.Parameters.AddWithValue($"@{nameof(Tipo.IdTipo)}", tipo.IdTipo);
					
					connection.Open();
					command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
}