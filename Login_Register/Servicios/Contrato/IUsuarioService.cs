using Microsoft.EntityFrameworkCore;
using Login_Register.Models;


namespace Login_Register.Servicios.Contrato
{
    public interface IUsuarioService
    {
        //Forma Asincrona
        Task<Usuario> GetUsuarios(string correo, string clave); //Devuelve un usuario
        Task<Usuario> SaveUsuarios(Usuario modelo); //Guarda al usuario
    }
}
