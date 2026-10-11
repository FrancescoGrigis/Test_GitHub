using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors();

// Connection string
string connectionString =
    @"Server=(localdb)\MSSQLLocalDB;Database=DemoDB;Trusted_Connection=True;TrustServerCertificate=True;";

// 1. GET: Per leggere solo le pizze attive (isActive = true) e passargli ID, Nome, Descrizione, Prezzo
app.MapGet("/api/pizze", async () =>
{
    try
    {
        using SqlConnection conn = new SqlConnection(connectionString);
        await conn.OpenAsync();

        // Filtriamo direttamente dal database quelle con isActive = 1 (true)
        string query = "SELECT Id, Nome, Descrizione, Prezzo FROM Pizze WHERE IsActive = 1";
        using SqlCommand cmd = new SqlCommand(query, conn);

        using SqlDataReader reader = await cmd.ExecuteReaderAsync();

        var listaPizze = new List<PizzaDto>();

        while (await reader.ReadAsync())
        {
            listaPizze.Add(new PizzaDto(
                reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                reader["Nome"]?.ToString() ?? "",
                reader["Descrizione"]?.ToString() ?? "",
                reader["Prezzo"] != DBNull.Value ? Convert.ToDecimal(reader["Prezzo"]) : 0
            ));
        }

        return Results.Ok(listaPizze);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
        return Results.BadRequest(new { Errore = ex.Message });
    }
});

app.Run();

// DTO per rappresentare la Pizza
public record PizzaDto(
    int Id,
    string Nome,
    string Descrizione,
    decimal Prezzo
);