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

// POST prenotazione
app.MapPost("/api/prenotazioni", async (PrenotazioneDto dati) =>
{
    if (dati == null)
        return Results.BadRequest("Dati mancanti.");

    if (string.IsNullOrWhiteSpace(dati.Nome))
        return Results.BadRequest("Nome obbligatorio.");

    if (string.IsNullOrWhiteSpace(dati.Telefono))
        return Results.BadRequest("Telefono obbligatorio.");

    try
    {
        using SqlConnection conn = new SqlConnection(connectionString);

        await conn.OpenAsync();

        string query = @"
            INSERT INTO Prenotazioni
            (
                Nome,
                Telefono,
                Data,
                Ora,
                Servizio,
                Ospiti,
                Timestamp
            )
            VALUES
            (
                @Nome,
                @Telefono,
                @Data,
                @Ora,
                @Servizio,
                @Ospiti,
                @Timestamp
            )";

        using SqlCommand cmd = new SqlCommand(query, conn);

        cmd.Parameters.AddWithValue("@Nome", dati.Nome);
        cmd.Parameters.AddWithValue("@Telefono", dati.Telefono);
        cmd.Parameters.AddWithValue("@Data", dati.Data);
        cmd.Parameters.AddWithValue("@Ora", dati.Ora);
        cmd.Parameters.AddWithValue("@Servizio", dati.Servizio);

        if (int.TryParse(dati.Ospiti, out int ospiti))
            cmd.Parameters.AddWithValue("@Ospiti", ospiti);
        else
            cmd.Parameters.AddWithValue("@Ospiti", 0);

        if (DateTime.TryParse(dati.Timestamp, out DateTime timestamp))
            cmd.Parameters.AddWithValue("@Timestamp", timestamp);
        else
            cmd.Parameters.AddWithValue("@Timestamp", DateTime.Now);

        await cmd.ExecuteNonQueryAsync();

        return Results.Ok(new
        {
            Messaggio = "Prenotazione salvata con successo"
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);

        return Results.BadRequest(new
        {
            Errore = ex.Message
        });
    }
});

app.Run();

public record PrenotazioneDto(
    string Nome,
    string Telefono,
    string Data,
    string Ora,
    string Servizio,
    string Ospiti,
    string Timestamp
);