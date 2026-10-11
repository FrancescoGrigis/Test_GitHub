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

// 1. GET: Per leggere solo le pizze attive (isActive = true)
app.MapGet("/api/pizze", async () =>
{
    try
    {
        using SqlConnection conn = new SqlConnection(connectionString);
        await conn.OpenAsync();

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

// 2. POST: Per ricevere e salvare la prenotazione inviata dal form JavaScript
app.MapPost("/api/prenotazioni", async (PrenotazioneDto nuovaPrenotazione) =>
{
    try
    {
        using SqlConnection conn = new SqlConnection(connectionString);
        await conn.OpenAsync();

        string query = @"INSERT INTO Prenotazioni (Nome, Telefono, Data, Ora, Servizio, Ospiti, Timestamp) 
                         VALUES (@Nome, @Telefono, @Data, @Ora, @Servizio, @Ospiti, @Timestamp)";

        using SqlCommand cmd = new SqlCommand(query, conn);
        cmd.Parameters.AddWithValue("@Nome", nuovaPrenotazione.nome ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Telefono", nuovaPrenotazione.telefono ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Data", nuovaPrenotazione.data ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Ora", nuovaPrenotazione.ora ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Servizio", nuovaPrenotazione.servizio ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Ospiti", nuovaPrenotazione.ospiti ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Timestamp", nuovaPrenotazione.timestamp ?? (object)DBNull.Value);

        await cmd.ExecuteNonQueryAsync();

        return Results.Ok(new { messaggio = "Prenotazione registrata con successo nel database!" });
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

// DTO per ricevere i dati della prenotazione dal frontend JS
public record PrenotazioneDto(
    string nome,
    string telefono,
    string data,
    string ora,
    string servizio,
    string ospiti,
    string timestamp
);