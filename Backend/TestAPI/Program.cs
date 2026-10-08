using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

string connectionString = "Server=IL_TUO_SERVER;Database=GestionePizzeria;Trusted_Connection=True;TrustServerCertificate=True;";

app.MapPost("/api/ordini", async (OrdineDto dati) =>
{
    if (dati == null) return Results.BadRequest("Dati vuoti");

    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        string query = "INSERT INTO Ordini (NomeCliente, DettagliOrdine) VALUES (@Nome, @Dettagli)";

        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@Nome", dati.NomeCliente);
            cmd.Parameters.AddWithValue("@Dettagli", dati.DettagliOrdine);

            conn.Open();

            cmd.ExecuteNonQuery();
        }
    }

    return Results.Ok(new { status = "Salvato!" });
});

app.Run();

public record OrdineDto(string NomeCliente, string DettagliOrdine);