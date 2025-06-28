using MySql.Data.MySqlClient;

namespace Tp_Inmobiliaria_Ledesma_Lillo.Models;

public class RepositorioInmueble : RepositorioBase, IRepositorioInmueble
{
    public RepositorioInmueble(IConfiguration configuration) : base(configuration)
    {

    }

    public IList<Inmueble> ObtenerTodos()
    {
        IList<Inmueble> inmuebles = new List<Inmueble>();

        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"Select {nameof(Inmueble.IdInmueble)}, {nameof(Inmueble.PropietarioId)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Apellido)},
                           {nameof(Inmueble.Ambientes)}, {nameof(Inmueble.Direccion)}, {nameof(Inmueble.Uso)}, {nameof(Inmueble.Latitud)},
                           {nameof(Inmueble.Longitud)}, {nameof(Inmueble.Superficie)}, {nameof(Inmueble.Importe)}, {nameof(Inmueble.Disponible)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Apellido)},
                           {nameof(Inmueble.TipoId)}, {nameof(Tipo.Descripcion)}
                           from inmuebles i INNER JOIN propietarios p ON i.PropietarioId = p.IdPropietario
                           INNER JOIN tipos t ON i.TipoId = t.IdTipo
                           WHERE i.Disponible = 0";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())

                    while (reader.Read())
                    {
                        inmuebles.Add(new Inmueble
                        {
                            IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
                            PropietarioId = reader.GetInt32(nameof(Inmueble.PropietarioId)),
                            Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Uso = reader.GetString(nameof(Inmueble.Uso)),
                            Latitud = reader.GetDecimal(nameof(Inmueble.Latitud)),
                            Longitud = reader.GetDecimal(nameof(Inmueble.Longitud)),
                            Superficie = reader.GetInt32(nameof(Inmueble.Superficie)),
                            Disponible = reader.GetInt32(nameof(Inmueble.Disponible)),
                            Importe = reader.GetInt32(nameof(Inmueble.Importe)),
                            Duenio = new Propietario
                            {
                                IdPropietario = reader.GetInt32(nameof(Inmueble.PropietarioId)),
                                Nombre = reader.GetString(nameof(Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            },
                            TipoInmueble = new Tipo
                            {
                                IdTipo = reader.GetInt32(nameof(Inmueble.TipoId)),
                                Descripcion = reader.GetString(nameof(Tipo.Descripcion)),
                            }
                        });
                    }
                connection.Close();
            }
            return inmuebles;
        }
    }

    public Inmueble? ObtenerPorId(int id)
    {
        Inmueble? inmueble = null;

        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"SELECT {nameof(Inmueble.IdInmueble)},{nameof(Inmueble.PropietarioId)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Apellido)},
                           {nameof(Inmueble.Ambientes)}, {nameof(Inmueble.Direccion)}, {nameof(Inmueble.Uso)}, {nameof(Inmueble.Latitud)},
                           {nameof(Inmueble.Longitud)}, {nameof(Inmueble.Superficie)}, {nameof(Inmueble.Importe)}, {nameof(Inmueble.Disponible)},
                           {nameof(Inmueble.TipoId)}, {nameof(Tipo.Descripcion)} 
                           FROM inmuebles i INNER JOIN propietarios p ON i.PropietarioId = p.IdPropietario
                           INNER JOIN tipos t ON i.TipoId = t.IdTipo
                           WHERE {nameof(Inmueble.IdInmueble)} = @{nameof(Inmueble.IdInmueble)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Inmueble.IdInmueble)}", id);
                connection.Open();
                using (var reader = command.ExecuteReader())

                    if (reader.Read())
                    {
                        inmueble = new Inmueble
                        {
                            IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
                            PropietarioId = reader.GetInt32(nameof(Inmueble.PropietarioId)),
                            Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Uso = reader.GetString(nameof(Inmueble.Uso)),
                            Latitud = reader.GetDecimal(nameof(Inmueble.Latitud)),
                            Longitud = reader.GetDecimal(nameof(Inmueble.Longitud)),
                            Superficie = reader.GetInt32(nameof(Inmueble.Superficie)),
                            Importe = reader.GetInt32(nameof(Inmueble.Importe)),
                            TipoId = reader.GetInt32(nameof(Inmueble.TipoId)),
                            Disponible = reader.GetInt32(nameof(Inmueble.Disponible)),
                            Duenio = new Propietario
                            {
                                IdPropietario = reader.GetInt32(nameof(Inmueble.PropietarioId)),
                                Nombre = reader.GetString(nameof(Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            },
                            TipoInmueble = new Tipo
                            {
                                IdTipo = reader.GetInt32(nameof(Inmueble.TipoId)),
                                Descripcion = reader.GetString(nameof(Tipo.Descripcion)),
                            }
                        };
                    }
                connection.Close();

            }
            return inmueble;
        }
    }

    public int Alta(Inmueble inmueble)
    {
        int id = 0;
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"INSERT INTO inmuebles ({nameof(Inmueble.Direccion)}, 
                    {nameof(Inmueble.Ambientes)},
                    {nameof(Inmueble.Superficie)}, 
                    {nameof(Inmueble.Latitud)},
                    {nameof(Inmueble.Uso)}, 
                    {nameof(Inmueble.Longitud)},
                    {nameof(Inmueble.PropietarioId)}, 
                    {nameof(Inmueble.TipoId)},
                    {nameof(Inmueble.Importe)},
                    {nameof(Inmueble.Disponible)})

                    VALUES (@{nameof(Inmueble.Direccion)}, 
                    @{nameof(Inmueble.Ambientes)}, 
                    @{nameof(Inmueble.Superficie)}, 
                    @{nameof(Inmueble.Latitud)}, 
                    @{nameof(Inmueble.Uso)}, 
                    @{nameof(Inmueble.Longitud)}, 
                    @{nameof(Inmueble.PropietarioId)}, 
                    @{nameof(Inmueble.TipoId)},
                    @{nameof(Inmueble.Importe)},
                    @{nameof(Inmueble.Disponible)});
                    
                    SELECT LAST_INSERT_ID();";
            using (var command = new MySqlCommand(sql, connection))
            {

                command.Parameters.AddWithValue($"@{nameof(Inmueble.Ambientes)}", inmueble.Ambientes);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Direccion)}", inmueble.Direccion);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Uso)}", inmueble.Uso);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Latitud)}", inmueble.Latitud);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Longitud)}", inmueble.Longitud);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Superficie)}", inmueble.Superficie);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.PropietarioId)}", inmueble.PropietarioId);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.TipoId)}", inmueble.TipoId);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Importe)}", inmueble.Importe);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Disponible)}", inmueble.Disponible);


                connection.Open();
                id = Convert.ToInt32(command.ExecuteScalar());
                inmueble.IdInmueble = id;
                connection.Close();
            }
        }
        return id;
    }

    public int Modificacion(Inmueble inmueble)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"UPDATE inmuebles
                    SET 
                    {nameof(Inmueble.Ambientes)} = @{nameof(Inmueble.Ambientes)},
                    {nameof(Inmueble.Direccion)} = @{nameof(Inmueble.Direccion)},
                    {nameof(Inmueble.Uso)} = @{nameof(Inmueble.Uso)},
                    {nameof(Inmueble.Latitud)} = @{nameof(Inmueble.Latitud)},
                    {nameof(Inmueble.Longitud)} = @{nameof(Inmueble.Longitud)},
                    {nameof(Inmueble.Superficie)} = @{nameof(Inmueble.Superficie)},
                    {nameof(Inmueble.PropietarioId)} = @{nameof(Inmueble.PropietarioId)},
                    {nameof(Inmueble.TipoId)} = @{nameof(Inmueble.TipoId)},
                    {nameof(Inmueble.Importe)} = @{nameof(Inmueble.Importe)},
                    {nameof(Inmueble.Disponible)} = @{nameof(Inmueble.Disponible)}
                    WHERE {nameof(Inmueble.IdInmueble)} = @{nameof(Inmueble.IdInmueble)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Ambientes)}", inmueble.Ambientes);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Direccion)}", inmueble.Direccion);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Uso)}", inmueble.Uso);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Latitud)}", inmueble.Latitud);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Longitud)}", inmueble.Longitud);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Superficie)}", inmueble.Superficie);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.PropietarioId)}", inmueble.PropietarioId);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.TipoId)}", inmueble.TipoId);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.IdInmueble)}", inmueble.IdInmueble);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Importe)}", inmueble.Importe);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Disponible)}", inmueble.Disponible);

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
            var sql = @$"DELETE FROM inmuebles
                    WHERE {nameof(Inmueble.IdInmueble)} = @{nameof(Inmueble.IdInmueble)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Inmueble.IdInmueble)}", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return 0;
    }

    public IList<Inmueble> obtenerInmueblesSuspendidos()
    {
        IList<Inmueble> inmuebles = new List<Inmueble>();

        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"Select {nameof(Inmueble.IdInmueble)}, {nameof(Inmueble.PropietarioId)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Apellido)},
                           {nameof(Inmueble.Ambientes)}, {nameof(Inmueble.Direccion)}, {nameof(Inmueble.Uso)}, {nameof(Inmueble.Latitud)},
                           {nameof(Inmueble.Longitud)}, {nameof(Inmueble.Superficie)}, {nameof(Inmueble.Importe)}, {nameof(Inmueble.Disponible)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Apellido)},
                           {nameof(Inmueble.TipoId)}, {nameof(Tipo.Descripcion)}
                           from inmuebles i INNER JOIN propietarios p ON i.PropietarioId = p.IdPropietario
                           INNER JOIN tipos t ON i.TipoId = t.IdTipo
                           WHERE i.Disponible = 1";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())

                    while (reader.Read())
                    {
                        inmuebles.Add(new Inmueble
                        {
                            IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
                            PropietarioId = reader.GetInt32(nameof(Inmueble.PropietarioId)),
                            Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Uso = reader.GetString(nameof(Inmueble.Uso)),
                            Latitud = reader.GetDecimal(nameof(Inmueble.Latitud)),
                            Longitud = reader.GetDecimal(nameof(Inmueble.Longitud)),
                            Superficie = reader.GetInt32(nameof(Inmueble.Superficie)),
                            Disponible = reader.GetInt32(nameof(Inmueble.Disponible)),
                            Importe = reader.GetInt32(nameof(Inmueble.Importe)),
                            Duenio = new Propietario
                            {
                                IdPropietario = reader.GetInt32(nameof(Inmueble.PropietarioId)),
                                Nombre = reader.GetString(nameof(Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            },
                            TipoInmueble = new Tipo
                            {
                                IdTipo = reader.GetInt32(nameof(Inmueble.TipoId)),
                                Descripcion = reader.GetString(nameof(Tipo.Descripcion)),
                            }
                        });
                    }
                connection.Close();
            }
            return inmuebles;
        }
    }

    public IList<Inmueble>? buscarPorPropietario(int idPropietario){
        IList<Inmueble> inmuebles = new List<Inmueble>();

        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"SELECT {nameof(Inmueble.IdInmueble)},{nameof(Inmueble.PropietarioId)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Apellido)},
                           {nameof(Inmueble.Ambientes)}, {nameof(Inmueble.Direccion)}, {nameof(Inmueble.Uso)}, {nameof(Inmueble.Latitud)},
                           {nameof(Inmueble.Longitud)}, {nameof(Inmueble.Superficie)}, {nameof(Inmueble.Importe)}, {nameof(Inmueble.Disponible)},
                           {nameof(Inmueble.TipoId)}, {nameof(Tipo.Descripcion)} 
                           FROM inmuebles i INNER JOIN propietarios p ON i.PropietarioId = p.IdPropietario
                           INNER JOIN tipos t ON i.TipoId = t.IdTipo
                           WHERE {nameof(Inmueble.PropietarioId)} = @{nameof(Inmueble.PropietarioId)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Inmueble.PropietarioId)}", idPropietario);
                connection.Open();
                using (var reader = command.ExecuteReader())

                    if (reader.Read())
                    {
                        inmuebles.Add( new Inmueble
                        {
                            IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
                            PropietarioId = reader.GetInt32(nameof(Inmueble.PropietarioId)),
                            Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Uso = reader.GetString(nameof(Inmueble.Uso)),
                            Latitud = reader.GetDecimal(nameof(Inmueble.Latitud)),
                            Longitud = reader.GetDecimal(nameof(Inmueble.Longitud)),
                            Superficie = reader.GetInt32(nameof(Inmueble.Superficie)),
                            Importe = reader.GetInt32(nameof(Inmueble.Importe)),
                            TipoId = reader.GetInt32(nameof(Inmueble.TipoId)),
                            Disponible = reader.GetInt32(nameof(Inmueble.Disponible)),
                            Duenio = new Propietario
                            {
                                IdPropietario = reader.GetInt32(nameof(Inmueble.PropietarioId)),
                                Nombre = reader.GetString(nameof(Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            },
                            TipoInmueble = new Tipo
                            {
                                IdTipo = reader.GetInt32(nameof(Inmueble.TipoId)),
                                Descripcion = reader.GetString(nameof(Tipo.Descripcion)),
                            }
                        });
                    }
                connection.Close();

            }
            return inmuebles;
        }
    }

}
