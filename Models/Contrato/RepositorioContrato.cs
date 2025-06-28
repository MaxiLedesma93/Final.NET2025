using MySql.Data.MySqlClient;


namespace Tp_Inmobiliaria_Ledesma_Lillo.Models;

public class RepositorioContrato : RepositorioBase, IRepositorioContrato
{

    public RepositorioContrato(IConfiguration configuration) : base(configuration)
    {

    }
    public int Alta(Contrato c)
    {  
        int id = -1;
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"INSERT INTO contratos ({nameof(Contrato.InmuebleId)},
                     {nameof(Contrato.InquilinoId)},
                     {nameof(Contrato.FecInicio)},
                     {nameof(Contrato.FecFin)},
                     {nameof(Contrato.Monto)},
                     {nameof(Contrato.Estado)})
                       
					VALUES (@{nameof(Contrato.InmuebleId)},
                    @{nameof(Contrato.InquilinoId)},
                    @{nameof(Contrato.FecInicio)},
                    @{nameof(Contrato.FecFin)},
                    @{nameof(Contrato.Monto)},
                    @{nameof(Contrato.Estado)});
                   
					SELECT LAST_INSERT_ID();";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Contrato.InmuebleId)}", c.InmuebleId);
                command.Parameters.AddWithValue($"@{nameof(Contrato.InquilinoId)}", c.InquilinoId);
                command.Parameters.AddWithValue($"@{nameof(Contrato.FecInicio)}", c.FecInicio);
                command.Parameters.AddWithValue($"@{nameof(Contrato.FecFin)}", c.FecFin);
                command.Parameters.AddWithValue($"@{nameof(Contrato.Monto)}", c.Monto);
                command.Parameters.AddWithValue($"@{nameof(Contrato.Estado)}", c.Estado);
                
                connection.Open();
                id = Convert.ToInt32(command.ExecuteScalar());
                c.IdContrato = id;
                connection.Close();
            }
        }
        return id;
    }

    public int Modificacion(Contrato c)
    {
        using (var connection = new MySqlConnection(connectionString))
        {

            var sql = @$"UPDATE contratos 
            SET 
            {nameof(Contrato.InmuebleId)} = @{nameof(Contrato.InmuebleId)},
            {nameof(Contrato.InquilinoId)} = @{nameof(Contrato.InquilinoId)},
            {nameof(Contrato.FecInicio)} = @{nameof(Contrato.FecInicio)},
            {nameof(Contrato.FecFin)} = @{nameof(Contrato.FecFin)},
            {nameof(Contrato.Monto)} = @{nameof(Contrato.Monto)},
            {nameof(Contrato.Estado)} = @{nameof(Contrato.Estado)}
            
            WHERE {nameof(Contrato.IdContrato)} = @{nameof(Contrato.IdContrato)}";

            using (var command = new MySqlCommand(sql, connection))
            {

                command.Parameters.AddWithValue($@"{nameof(Contrato.IdContrato)}", c.IdContrato);
                command.Parameters.AddWithValue($@"{nameof(Contrato.InmuebleId)}", c.InmuebleId);
                command.Parameters.AddWithValue($@"{nameof(Contrato.InquilinoId)}", c.InquilinoId);
                command.Parameters.AddWithValue($@"{nameof(Contrato.FecInicio)}", c.FecInicio);
                command.Parameters.AddWithValue($@"{nameof(Contrato.FecFin)}", c.FecFin);
                command.Parameters.AddWithValue($@"{nameof(Contrato.Monto)}", c.Monto);
                command.Parameters.AddWithValue($@"{nameof(Contrato.Estado)}", c.Estado);

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
            var sql = @$"DELETE FROM contratos
                    WHERE {nameof(Contrato.IdContrato)} = @{nameof(Contrato.IdContrato)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Contrato.IdContrato)}", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return 0;

    }

    public Contrato? ObtenerPorId(int id)
    {
        Contrato? contrato = null;
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"SELECT 
            {nameof(Contrato.IdContrato)}, {nameof(Contrato.InmuebleId)},
            {nameof(Contrato.InquilinoId)}, {nameof(Contrato.FecInicio)},
            {nameof(Contrato.FecFin)}, {nameof(Contrato.Monto)}, {nameof(Contrato.Estado)},
            {nameof(Inquilino.Nombre)}, {nameof(Inquilino.Apellido)}, {nameof(Inmueble.Direccion)}
            
            FROM contratos c INNER JOIN inquilinos i ON c.InquilinoId = i.IdInquilino
            INNER JOIN inmuebles inm ON c.InmuebleId = inm.IdInmueble
            
            WHERE {nameof(Contrato.IdContrato)} = @{nameof(Contrato.IdContrato)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Contrato.IdContrato)}", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        contrato = new Contrato
                        {
                            IdContrato = reader.GetInt32(nameof(Contrato.IdContrato)),
                            InmuebleId = reader.GetInt32(nameof(Contrato.InmuebleId)),
                            InquilinoId = reader.GetInt32(nameof(Contrato.InquilinoId)),
                            FecInicio = reader.GetDateTime(nameof(Contrato.FecInicio)),
                            FecFin = reader.GetDateTime(nameof(Contrato.FecFin)),
                            Monto = reader.GetDecimal(nameof(Contrato.Monto)),
                            Estado = reader.GetBoolean(nameof(Contrato.Estado)),
                            Inquilino = new Inquilino
                            {
                                IdInquilino = reader.GetInt32(nameof(Contrato.InquilinoId)),
                                Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                                Apellido = reader.GetString(nameof(Inquilino.Apellido))
                            },
                            Inmueble = new Inmueble
                            {
                                IdInmueble = reader.GetInt32(nameof(Contrato.InmuebleId)),
                                Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            }
                        };
                    }
                }
                connection.Close();
            }
        }

        return contrato;
    }

    public IList<Contrato> ObtenerTodos(){
        IList<Contrato> contratos = new List<Contrato>();

        using (var connection = new MySqlConnection(connectionString)){

            var sql = @$" SELECT {nameof(Contrato.IdContrato)},
            {nameof(Contrato.InquilinoId)},{nameof(Contrato.InmuebleId)},
            {nameof(Contrato.FecInicio)}, {nameof(Contrato.FecFin)},
            {nameof(Contrato.Monto)}, {nameof(Contrato.Estado)}, {nameof(Inquilino.Nombre)},
            {nameof(Inquilino.Apellido)}, {nameof(Inmueble.Direccion)}
            
            FROM contratos c INNER JOIN inquilinos i ON c.InquilinoId = i.IdInquilino
            INNER JOIN inmuebles inm ON c.InmuebleId = inm.IdInmueble";
           
            using(var command = new MySqlCommand(sql, connection)){
                connection.Open();
                using(var reader = command.ExecuteReader())
                    while(reader.Read()){
                        contratos.Add(new Contrato{
                            IdContrato = reader.GetInt32(nameof(Contrato.IdContrato)),
                            InquilinoId = reader.GetInt32(nameof(Contrato.InquilinoId)),
                            InmuebleId = reader.GetInt32(nameof(Contrato.InmuebleId)),
                            FecInicio = reader.GetDateTime(nameof(Contrato.FecInicio)),
                            FecFin = reader.GetDateTime(nameof(Contrato.FecFin)),
                            Monto = reader.GetDecimal(nameof(Contrato.Monto)),
                            Estado = reader.GetBoolean(nameof(Contrato.Estado)),
                            Inquilino = new Inquilino{
                                IdInquilino = reader.GetInt32(nameof(Contrato.InquilinoId)),
                                Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                                Apellido = reader.GetString(nameof(Inquilino.Apellido))
                            },
                            Inmueble = new Inmueble{
                                IdInmueble = reader.GetInt32(nameof(Contrato.InmuebleId)),
                                Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            }
                        });
                    }
                connection.Close();
            }
        }
        return contratos;
    }

    public IList<Contrato> ObtenerTodosVigentes(DateTime fechaInicio, DateTime fechaFin)
		{
			IList<Contrato> res = new List<Contrato>();
			using (var connection = new MySqlConnection(connectionString))
			{
                var sql = @$" SELECT {nameof(Contrato.IdContrato)},
                {nameof(Contrato.InquilinoId)},{nameof(Contrato.InmuebleId)},
                {nameof(Contrato.FecInicio)}, {nameof(Contrato.FecFin)},
                {nameof(Contrato.Monto)}, {nameof(Contrato.Estado)}, {nameof(Inquilino.Nombre)},
                {nameof(Inquilino.Apellido)}, {nameof(Inmueble.Direccion)}
            
                FROM contratos c INNER JOIN inquilinos i ON c.InquilinoId = i.IdInquilino
                INNER JOIN inmuebles inm ON c.InmuebleId = inm.IdInmueble
                WHERE c.estado = 1 AND ({nameof(Contrato.FecInicio)} >= @{nameof(Contrato.FecInicio)})
                AND ({nameof(Contrato.FecFin)} <= @{nameof(Contrato.FecFin)})";
				using (var command = new MySqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue($"@{nameof(Contrato.FecInicio)}", fechaInicio);
					command.Parameters.AddWithValue($"@{nameof(Contrato.FecFin)}", fechaFin);
					connection.Open();
					using(var reader = command.ExecuteReader())
                    while(reader.Read()){
                        res.Add(new Contrato{
                            IdContrato = reader.GetInt32(nameof(Contrato.IdContrato)),
                            InquilinoId = reader.GetInt32(nameof(Contrato.InquilinoId)),
                            InmuebleId = reader.GetInt32(nameof(Contrato.InmuebleId)),
                            FecInicio = reader.GetDateTime(nameof(Contrato.FecInicio)),
                            FecFin = reader.GetDateTime(nameof(Contrato.FecFin)),
                            Monto = reader.GetDecimal(nameof(Contrato.Monto)),
                            Estado = reader.GetBoolean(nameof(Contrato.Estado)),
                            Inquilino = new Inquilino{
                                IdInquilino = reader.GetInt32(nameof(Contrato.InquilinoId)),
                                Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                                Apellido = reader.GetString(nameof(Inquilino.Apellido))
                            },
                            Inmueble = new Inmueble{
                                IdInmueble = reader.GetInt32(nameof(Contrato.InmuebleId)),
                                Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            }
                        });
                    }
                connection.Close();
			}
			return res;
		}
    }

    public IList<Contrato> ObtenerPorInmuebleId(int id){
        IList<Contrato> contratos = new List<Contrato>();

        using (var connection = new MySqlConnection(connectionString)){

            var sql = @$" SELECT {nameof(Contrato.IdContrato)},
            {nameof(Contrato.InquilinoId)},{nameof(Contrato.InmuebleId)},
            {nameof(Contrato.FecInicio)}, {nameof(Contrato.FecFin)},
            {nameof(Contrato.Monto)}, {nameof(Contrato.Estado)}, {nameof(Inquilino.Nombre)},
            {nameof(Inquilino.Apellido)}, {nameof(Inmueble.Direccion)}
            
            FROM contratos c INNER JOIN inquilinos i ON c.InquilinoId = i.IdInquilino
            INNER JOIN inmuebles inm ON c.InmuebleId = inm.IdInmueble
            WHERE {nameof(Contrato.InmuebleId)} = @{nameof(Contrato.InmuebleId)}";
           
            using(var command = new MySqlCommand(sql, connection)){
                command.Parameters.AddWithValue($"@{nameof(Contrato.InmuebleId)}", id);
                connection.Open();
                using(var reader = command.ExecuteReader())
                    while(reader.Read()){
                        contratos.Add(new Contrato{
                            IdContrato = reader.GetInt32(nameof(Contrato.IdContrato)),
                            InquilinoId = reader.GetInt32(nameof(Contrato.InquilinoId)),
                            InmuebleId = reader.GetInt32(nameof(Contrato.InmuebleId)),
                            FecInicio = reader.GetDateTime(nameof(Contrato.FecInicio)),
                            FecFin = reader.GetDateTime(nameof(Contrato.FecFin)),
                            Monto = reader.GetDecimal(nameof(Contrato.Monto)),
                            Estado = reader.GetBoolean(nameof(Contrato.Estado)),
                            Inquilino = new Inquilino{
                                IdInquilino = reader.GetInt32(nameof(Contrato.InquilinoId)),
                                Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                                Apellido = reader.GetString(nameof(Inquilino.Apellido))
                            },
                            Inmueble = new Inmueble{
                                IdInmueble = reader.GetInt32(nameof(Contrato.InmuebleId)),
                                Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            }
                        });
                    }
                connection.Close();
            }
        }
        return contratos;
    }
    public IList<Contrato> ObtenerPorInmuebleDir(string dir){
        IList<Contrato> contratos = new List<Contrato>();
        using (var connection = new MySqlConnection(connectionString)){

            var sql = @$" SELECT {nameof(Contrato.IdContrato)},
            {nameof(Contrato.InquilinoId)},{nameof(Contrato.InmuebleId)},
            {nameof(Contrato.FecInicio)}, {nameof(Contrato.FecFin)},
            {nameof(Contrato.Monto)}, {nameof(Contrato.Estado)}, {nameof(Inquilino.Nombre)},
            {nameof(Inquilino.Apellido)}, {nameof(Inmueble.Direccion)}
            
            FROM contratos c INNER JOIN inquilinos i ON c.InquilinoId = i.IdInquilino
            INNER JOIN inmuebles inm ON c.InmuebleId = inm.IdInmueble
            WHERE {nameof(Contrato.Inmueble.Direccion)} = @{nameof(Contrato.Inmueble.Direccion)}";
           
            using(var command = new MySqlCommand(sql, connection)){
                command.Parameters.AddWithValue($"@{nameof(Contrato.Inmueble.Direccion)}", dir);
                connection.Open();
                using(var reader = command.ExecuteReader())
                    while(reader.Read()){
                        contratos.Add(new Contrato{
                            IdContrato = reader.GetInt32(nameof(Contrato.IdContrato)),
                            InquilinoId = reader.GetInt32(nameof(Contrato.InquilinoId)),
                            InmuebleId = reader.GetInt32(nameof(Contrato.InmuebleId)),
                            FecInicio = reader.GetDateTime(nameof(Contrato.FecInicio)),
                            FecFin = reader.GetDateTime(nameof(Contrato.FecFin)),
                            Monto = reader.GetDecimal(nameof(Contrato.Monto)),
                            Estado = reader.GetBoolean(nameof(Contrato.Estado)),
                            Inquilino = new Inquilino{
                                IdInquilino = reader.GetInt32(nameof(Contrato.InquilinoId)),
                                Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                                Apellido = reader.GetString(nameof(Inquilino.Apellido))
                            },
                            Inmueble = new Inmueble{
                                IdInmueble = reader.GetInt32(nameof(Contrato.InmuebleId)),
                                Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            }
                        });
                    }
                connection.Close();
            }
        }
        return contratos;
    }

    public IList<Contrato> ObtenerPorFechaVenc(DateTime fechaFin){
        IList<Contrato> contratos = new List<Contrato>();
        using (var connection = new MySqlConnection(connectionString)){

            var sql = @$" SELECT {nameof(Contrato.IdContrato)},
            {nameof(Contrato.InquilinoId)},{nameof(Contrato.InmuebleId)},
            {nameof(Contrato.FecInicio)}, {nameof(Contrato.FecFin)},
            {nameof(Contrato.Monto)}, {nameof(Contrato.Estado)}, {nameof(Inquilino.Nombre)},
            {nameof(Inquilino.Apellido)}, {nameof(Inmueble.Direccion)}
            
            FROM contratos c INNER JOIN inquilinos i ON c.InquilinoId = i.IdInquilino
            INNER JOIN inmuebles inm ON c.InmuebleId = inm.IdInmueble
            WHERE {nameof(Contrato.FecFin)} <= @{nameof(Contrato.FecFin)}";
           
            using(var command = new MySqlCommand(sql, connection)){
                command.Parameters.AddWithValue($"@{nameof(Contrato.FecFin)}", fechaFin);
                connection.Open();
                using(var reader = command.ExecuteReader())
                    while(reader.Read()){
                        contratos.Add(new Contrato{
                            IdContrato = reader.GetInt32(nameof(Contrato.IdContrato)),
                            InquilinoId = reader.GetInt32(nameof(Contrato.InquilinoId)),
                            InmuebleId = reader.GetInt32(nameof(Contrato.InmuebleId)),
                            FecInicio = reader.GetDateTime(nameof(Contrato.FecInicio)),
                            FecFin = reader.GetDateTime(nameof(Contrato.FecFin)),
                            Monto = reader.GetDecimal(nameof(Contrato.Monto)),
                            Estado = reader.GetBoolean(nameof(Contrato.Estado)),
                            Inquilino = new Inquilino{
                                IdInquilino = reader.GetInt32(nameof(Contrato.InquilinoId)),
                                Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                                Apellido = reader.GetString(nameof(Inquilino.Apellido))
                            },
                            Inmueble = new Inmueble{
                                IdInmueble = reader.GetInt32(nameof(Contrato.InmuebleId)),
                                Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            }
                        });
                    }
                connection.Close();
            }
        }
        return contratos;
    }

}