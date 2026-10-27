namespace ApiRelogios.Models;

public class Relogio
{
    public int Id { get; set; }
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public decimal Preco { get; set; }
}
