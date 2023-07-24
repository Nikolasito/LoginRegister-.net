using System.Security.Cryptography;
using System.Text;


namespace Login_Register.Recursos
{
    public class Utilidades
    {
        public static string EncriptarClave(string clave)
        {
            StringBuilder sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;

                byte[] result = hash.ComputeHash(enc.GetBytes(clave)); //Array de tipo byte

                foreach (byte b in result)
                {
                    sb.Append(b.ToString("x2")); //Concatenar el resultado//"x2" es para que la cadena se formatee
                }

                return sb.ToString();   
            }
        }
    }
}
