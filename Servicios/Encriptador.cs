using System;
using System.Security.Cryptography;

namespace cash_server.Servicios
{
    public class EncryptionService
    {
        public string EncryptPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]); // Generar sal aleatoria

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20); // Tamaño del hash

            // Combinar sal y hash
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string hashedPassword = Convert.ToBase64String(hashBytes);
            return hashedPassword;
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            // Convertir el hash base64 a bytes
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            // Obtener la sal de los primeros 16 bytes del hash
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Derivar la clave usando la misma sal y parámetros
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20); // Tamaño del hash

            // Comparar el hash derivado con el hash almacenado
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false; // Las contraseñas no coinciden
                }
            }
            return true; // Las contraseñas coinciden
        }
    }
}
