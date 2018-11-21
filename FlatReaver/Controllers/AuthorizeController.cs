using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using DataStore;

namespace FlatReaver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : Controller
    {
        public AuthorizeController(IStore store)
        {
            Store = store;
        }

        IStore Store;

        [HttpPost]
        public ActionResult<string> Post([FromBody] dynamic user)
        {           

            var cryptoProvider = HashAlgorithm.Create(HashAlgorithmName.MD5.Name);

            var hash = cryptoProvider.ComputeHash(Encoding.Default.GetBytes((string) user.Password));

            var hashText = AuthentificationData.ToHexString(hash);

            var identity = GetIdentity(new { PasswordHash= hashText, Login = (string) user.Login });

            if (identity == null)
            {
                return null;
            }

            var token =  new JwtSecurityToken(
                    issuer: AuthentificationData.TokenIssuer,
                    audience: AuthentificationData.TokenAudience,
                    notBefore: DateTime.Now,
                    claims: identity.Claims,
                    expires: DateTime.Now.Add(TimeSpan.FromMinutes(AuthentificationData.TokenLifetime)),
                    signingCredentials: new SigningCredentials(AuthentificationData.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

            var response = new
            {
                Token = encodedToken,
                UserName = identity.Name
            };

            Response.ContentType = "application/json";

            return JsonConvert.SerializeObject(response);

        }

        private ClaimsIdentity GetIdentity(object currentUser)
        {
            var user = Store.ReadSingle<User>(currentUser);

            if (user != null)
            {
                var role = Store.ReadSingle<Role>(new { Id = user .RoleId});

                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name)
                };

                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);

                return claimsIdentity;
            }

            return null;
        }
    }
}

