using MySql.Data.MySqlClient;

namespace Tp_Inmobiliaria_Ledesma_Lillo.Extensions
{
    public static class MySqlDataReaderExtensions
    {
        public static int? GetNullableInt32(this MySqlDataReader reader, string columnName)
    {
        int ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? (int?)null : reader.GetInt32(ordinal);
    }

    public static decimal? GetNullableDecimal(this MySqlDataReader reader, string columnName)
    {
        int ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? (decimal?)null : reader.GetDecimal(ordinal);
    }

    public static DateTime? GetNullableDateTime(this MySqlDataReader reader, string columnName)
    {
        int ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? (DateTime?)null : reader.GetDateTime(ordinal);
    }

    public static bool? GetNullableBoolean(this MySqlDataReader reader, string columnName)
    {
        int ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? (bool?)null : reader.GetBoolean(ordinal);
    }

    public static string? GetNullableString(this MySqlDataReader reader, string columnName)
    {
        int ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
    }
    }
}