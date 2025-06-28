using MySql.Data.MySqlClient;

namespace Tp_Inmobiliaria_Ledesma_Lillo.Models;

public class RepositorioPago : RepositorioBase, IRepositorioPago
{

    public RepositorioPago(IConfiguration configuration) : base(configuration)
    {

    }

    public IList<Pago> ObtenerTodos()
    {
			var pagos = new List<Pago>();

			using(var connection = new MySqlConnection(connectionString))
            {
                var sql = @$"Select {nameof(Pago.IdPago)}, {nameof(Pago.NumPago)}, {nameof(Pago.FechaPago)}, {nameof(Pago.Importe)},
                                {nameof(Pago.ContratoId)}, {nameof(Pago.Detalle)}, {nameof(Pago.Est)}, {nameof(Contrato.InquilinoId)}, 
                                {nameof(Inquilino.Apellido)}, {nameof(Inquilino.Nombre)}
                            from pagos p INNER JOIN contratos c ON p.ContratoId = c.IdContrato
                            INNER JOIN inquilinos inq ON c.InquilinoId = inq.IdInquilino";
                            
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    using(var reader = command.ExecuteReader())

                    while(reader.Read())
                    {
                        pagos.Add(new Pago{
                            IdPago = reader.GetInt32(nameof(Pago.IdPago)),
                            NumPago = reader.GetInt32(nameof(Pago.NumPago)),
                            FechaPago = reader.GetDateTime(nameof(Pago.FechaPago)),
                            Importe = reader.GetDecimal(nameof(Pago.Importe)),
                            Detalle = reader.GetString(nameof(Pago.Detalle)),
                            Est = reader.GetInt32(nameof(Pago.Est)),
                            ContratoId = reader.GetInt32(nameof(Pago.ContratoId)),
                            contrato = new Contrato
                            {
                                IdContrato = reader.GetInt32(nameof(Pago.ContratoId)),
                                InquilinoId = reader.GetInt32(nameof(Contrato.InquilinoId)),
                                Inquilino = new Inquilino
                                {
                                    IdInquilino = reader.GetInt32(nameof(Contrato.InquilinoId)),
                                    Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                                    Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                                }
                            }
                        });
                    }
                    connection.Close();
                }
                return pagos;
            }
	}

    public Pago ObtenerPorId(int id)
    {
        Pago? pago = null;

        using(var connection = new MySqlConnection(connectionString))
            {
                var sql = @$"Select {nameof(Pago.IdPago)}, {nameof(Pago.NumPago)}, {nameof(Pago.FechaPago)}, {nameof(Pago.Importe)},
                                {nameof(Pago.ContratoId)}, {nameof(Pago.Detalle)}
                            from pagos
                            WHERE {nameof(Pago.IdPago)} = @{nameof(Pago.IdPago)}";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue($"@{nameof(Pago.IdPago)}", id);
                    connection.Open();
                    using(var reader = command.ExecuteReader())

                    if(reader.Read())
                    {
                        pago = new Pago{
                            IdPago = reader.GetInt32(nameof(Pago.IdPago)),
                            NumPago = reader.GetInt32(nameof(Pago.NumPago)),
                            FechaPago = reader.GetDateTime(nameof(Pago.FechaPago)),
                            Importe = reader.GetDecimal(nameof(Pago.Importe)),
                            Detalle = reader.GetString(nameof(Pago.Detalle)),
                            ContratoId = reader.GetInt32(nameof(Pago.ContratoId)),
                    };
                    }
                    connection.Close();
                }
                return pago;
            }
    }
    public IList<Pago> ObtenerPagosPorContrato(int id)
    {
			IList<Pago> pagos = new List<Pago>();

			using(var connection = new MySqlConnection(connectionString))
            {
                var sql = @$"Select {nameof(Pago.IdPago)}, {nameof(Pago.NumPago)}, {nameof(Pago.FechaPago)}, {nameof(Pago.Importe)},
                                {nameof(Pago.ContratoId)}, {nameof(Pago.Detalle)}, {nameof(Pago.Est)}, {nameof(Contrato.InquilinoId)}, 
                                {nameof(Inquilino.Apellido)}, {nameof(Inquilino.Nombre)}
                            from pagos p INNER JOIN contratos c ON p.ContratoId = c.IdContrato
                            INNER JOIN inquilinos inq ON c.InquilinoId = inq.IdInquilino
                            WHERE p.Est = 0";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue($"@{nameof(Pago.ContratoId)}", id);
                    connection.Open();
                    using(var reader = command.ExecuteReader())

                    while(reader.Read())
                    {
                        pagos.Add(new Pago{
                            IdPago = reader.GetInt32(nameof(Pago.IdPago)),
                            NumPago = reader.GetInt32(nameof(Pago.NumPago)),
                            FechaPago = reader.GetDateTime(nameof(Pago.FechaPago)),
                            Importe = reader.GetDecimal(nameof(Pago.Importe)),
                            Detalle = reader.GetString(nameof(Pago.Detalle)),
                            Est = reader.GetInt32(nameof(Pago.Est)),
                            ContratoId = reader.GetInt32(nameof(Pago.ContratoId)),
                            contrato = new Contrato
                            {
                                IdContrato = reader.GetInt32(nameof(Pago.ContratoId)),
                                InquilinoId = reader.GetInt32(nameof(Contrato.InquilinoId)),
                                Inquilino = new Inquilino
                                {
                                    IdInquilino = reader.GetInt32(nameof(Contrato.InquilinoId)),
                                    Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                                    Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                                }

                            }
                    });
                    }
                    connection.Close();
                }
                return pagos;
            }
	}

    public int Alta(Pago pago)
		{
			int id = 0;
		using(var connection = new MySqlConnection(connectionString))
		{
			var sql = @$"INSERT INTO pagos ({nameof(Pago.NumPago)}, {nameof(Pago.Importe)}, {nameof(Pago.FechaPago)},
                        {nameof(Pago.Detalle)}, {nameof(Pago.ContratoId)})
				VALUES (@{nameof(Pago.NumPago)},
                        @{nameof(Pago.Importe)},
                        @{nameof(Pago.FechaPago)},
                        @{nameof(Pago.Detalle)},
                        @{nameof(Pago.ContratoId)});
				SELECT LAST_INSERT_ID();";
			using(var command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue($"@{nameof(Pago.NumPago)}", pago.NumPago);
                command.Parameters.AddWithValue($"@{nameof(Pago.Importe)}", pago.Importe);
                command.Parameters.AddWithValue($"@{nameof(Pago.FechaPago)}", pago.FechaPago);
                command.Parameters.AddWithValue($"@{nameof(Pago.Detalle)}", pago.Detalle);
                command.Parameters.AddWithValue($"@{nameof(Pago.ContratoId)}", pago.ContratoId);

				connection.Open();
				id = Convert.ToInt32(command.ExecuteScalar());
				pago.IdPago = id;
				connection.Close();
			}
		}
		return id;
	}

    public int Modificacion(Pago pago)
	{
        int res = -1;
		using(var connection = new MySqlConnection(connectionString))
		{
			var sql = @$"UPDATE pagos
				SET {nameof(Pago.NumPago)} = @{nameof(Pago.NumPago)},
				{nameof(Pago.FechaPago)} = @{nameof(Pago.FechaPago)},
                {nameof(Pago.Importe)} = @{nameof(Pago.Importe)},
                {nameof(Pago.Detalle)} = @{nameof(Pago.Detalle)},
                {nameof(Pago.ContratoId)} = @{nameof(Pago.ContratoId)},
                {nameof(Pago.Est)} = @{nameof(Pago.Est)}

				WHERE {nameof(Pago.IdPago)} = @{nameof(Pago.IdPago)}";
			using(var command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue($"@{nameof(Pago.NumPago)}", pago.NumPago);
				command.Parameters.AddWithValue($"@{nameof(Pago.FechaPago)}", pago.FechaPago);
				command.Parameters.AddWithValue($"@{nameof(Pago.Importe)}", pago.Importe);
                command.Parameters.AddWithValue($"@{nameof(Pago.Detalle)}", pago.Detalle);
				command.Parameters.AddWithValue($"@{nameof(Pago.ContratoId)}", pago.ContratoId);
                command.Parameters.AddWithValue($"@{nameof(Pago.Est)}", pago.Est);
				command.Parameters.AddWithValue($"@{nameof(Pago.IdPago)}", pago.IdPago);
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
			var sql = @$"UPDATE pagos
                SET Est = 1
				WHERE {nameof(Pago.IdPago)} = @{nameof(Pago.IdPago)}";
			using(var command = new MySqlCommand(sql, connection))
			{
				command.Parameters.AddWithValue($"@{nameof(Pago.IdPago)}", id);
				connection.Open();
				command.ExecuteNonQuery();
				connection.Close();
			}
		}
		return 0;
	}

    public IList<Pago> ObtenerPagosEliminados()
    {
			var pagos = new List<Pago>();

			using(var connection = new MySqlConnection(connectionString))
            {
                var sql = @$"Select {nameof(Pago.IdPago)}, {nameof(Pago.NumPago)}, {nameof(Pago.FechaPago)}, {nameof(Pago.Importe)},
                                {nameof(Pago.ContratoId)}, {nameof(Pago.Detalle)}, {nameof(Pago.Est)}, {nameof(Contrato.InquilinoId)}, 
                                {nameof(Inquilino.Apellido)}, {nameof(Inquilino.Nombre)}
                            from pagos p INNER JOIN contratos c ON p.ContratoId = c.IdContrato
                            INNER JOIN inquilinos inq ON c.InquilinoId = inq.IdInquilino
                            WHERE p.Est = 1";
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    using(var reader = command.ExecuteReader())

                    while(reader.Read())
                    {
                        pagos.Add(new Pago{
                            IdPago = reader.GetInt32(nameof(Pago.IdPago)),
                            NumPago = reader.GetInt32(nameof(Pago.NumPago)),
                            FechaPago = reader.GetDateTime(nameof(Pago.FechaPago)),
                            Importe = reader.GetDecimal(nameof(Pago.Importe)),
                            Detalle = reader.GetString(nameof(Pago.Detalle)),
                            Est = reader.GetInt32(nameof(Pago.Est)),
                            ContratoId = reader.GetInt32(nameof(Pago.ContratoId)),
                            contrato = new Contrato
                            {
                                IdContrato = reader.GetInt32(nameof(Pago.ContratoId)),
                                InquilinoId = reader.GetInt32(nameof(Contrato.InquilinoId)),
                                Inquilino = new Inquilino
                                {
                                    IdInquilino = reader.GetInt32(nameof(Contrato.InquilinoId)),
                                    Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                                    Nombre = reader.GetString(nameof(Inquilino.Nombre))
                                }
                            }
                        });
                    }
                    connection.Close();
                }
                return pagos;
            }
	}
}