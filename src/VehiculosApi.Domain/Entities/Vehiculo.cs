namespace VehiculosApi.Domain.Entities;

public class Vehiculo
{
    public int Id { get; set; }
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public int Anio { get; set; }
    public string Color { get; set; } = string.Empty;
    public string Placa { get; set; } = string.Empty;
    public string Vin { get; set; } = string.Empty;
    public decimal Kilometraje { get; set; }
    public string TipoCombustible { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public string Estado { get; set; } = "Disponible";
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
}
