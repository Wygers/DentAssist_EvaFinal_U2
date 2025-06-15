using System.Security.Claims;
public class StaticUserService
{
    // LISTA DE USUARIOS CREADOS SIN UNA BSE DE DATOS PARA LA CLÍNICA
    private readonly List<StaticUser> _users = new()
    {
        // ODONTOLOGO SIN BD
        new StaticUser
        {
            Username = "drperez",
            Password = "odont123",
            Role = "Odontologo",
            OdontologoId = 1, // ID del odontólogo en tu base de datos
            Claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Dr. Juan Pérez"),
                new Claim(ClaimTypes.Email, "drperez@clinica.com"),
                new Claim("Matricula", "AB-12345")
            }
        },
        // ADMINISTRADOR SIN BD
        new StaticUser
        {
            Username = "admin",
            Password = "admin123",
            Role = "Administrador",
            Claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "Administrador del Sistema"),
                new Claim(ClaimTypes.Email, "admin@clinica.com")
            }
        }
    };

    public StaticUser? ValidateUser(string username, string password)
    {
        return _users.FirstOrDefault(u =>
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) &&
            u.Password == password);
    }
}
public class StaticUser
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public int? OdontologoId { get; set; } // Relación con tu modelo Odontologo
    public List<Claim> Claims { get; set; } = new List<Claim>();
}