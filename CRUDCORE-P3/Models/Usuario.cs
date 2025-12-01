using System.ComponentModel.DataAnnotations;

public class Usuario
{
    [Key]                      
    public int IdUsuario { get; set; }

    public string Nombre { get; set; }
    public string Correo { get; set; }
    public string Password { get; set; }
}
