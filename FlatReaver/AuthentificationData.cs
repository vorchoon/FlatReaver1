using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FlatReaver
{
    /// <summary>
    /// Помощник для средств авторизации
    /// </summary>
    public static class AuthentificationData
    {
        public static string TokenIssuer = Program.Configuration["SecurityTokenIssuer"];

        public static string TokenAudience = Program.Configuration["Host"];

        private static string Keyword = Program.Configuration["SecurityKeyword"];

        public static int TokenLifetime = Convert.ToInt32(Program.Configuration["SecurityTokenLifetime"]);

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Keyword));
        }

        /// <summary>
        /// Формирует строку из Hex представления хэша
        /// </summary>
        /// <param name="bytes">Байтмассив хэша</param>
        /// <returns>Hex-строка</returns>
        public static string ToHexString(byte[] bytes)
        {
            var chars = new char[bytes.Length * 2 + 2];

            chars[0] = '0';
            chars[1] = 'x';

            for (int i = 0; i < bytes.Length; i++)
            {
                chars[2 * i + 2] = ToHexDigit(bytes[i] / 16);
                chars[2 * i + 3] = ToHexDigit(bytes[i] % 16);
            }

            return new string(chars);
        }

        /// <summary>
        /// Вычисляет Hex представление байта
        /// </summary>
        /// <param name="i">Байт</param>
        /// <returns>Hex-код байта</returns>
        private static char ToHexDigit(int i)
        {
            if (i < 10)
                return (char)(i + '0');
            return (char)(i - 10 + 'A');
        }
    }
}
