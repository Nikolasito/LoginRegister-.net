using Microsoft.EntityFrameworkCore;
using Login_Register.Models;
using Login_Register.Servicios.Contrato;

namespace Login_Register.Servicios.Implementacion
{
    public class UsuarioService : IUsuarioService //Herencia
    {
        private readonly BdLoginRegisterContext _dbContext;

        public UsuarioService(BdLoginRegisterContext dbContext) //Se recibe como parametro el contexto
        {
            _dbContext = dbContext;
        }   
        


        public async Task<Usuario> GetUsuarios(string correo, string clave)
        {
            Usuario usuario_encontrado = await _dbContext.Usuarios.Where(u => u.Correo == correo && u.Clave == clave)
                .FirstOrDefaultAsync(); //Devolver el primer usuario, si no devuelve un null

            return usuario_encontrado;
        }

        public async Task<Usuario> SaveUsuarios(Usuario modelo)
        {
            _dbContext.Usuarios.Add(modelo); //Agregar usuarios
            await _dbContext.SaveChangesAsync(); //Guardar cambios asincronicamente
            return modelo;
        }
    }
}
