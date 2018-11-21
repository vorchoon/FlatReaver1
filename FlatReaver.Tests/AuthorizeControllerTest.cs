using System;
using System.Collections.Generic;
using Xunit;
using System.Net;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using FlatReaver;
using DataStore;

namespace FlatReaver.Tests
{
    public class AuthorizeControllerTest
    {
        const string Host = "http://localhost:32018";

        /* Требует наличия записей в бд 
         * Users Login = Irina, PasswordHash = 0xFE2D010308A6B3799A3D9C728EE74244
         * Roles Name = Admin
         * Users Login = Ivan, PasswordHash = 0x1E6E0A04D20F50967C64DAC2D639A577
         * Roles Name = User
         * User и Role должны быть связаны попарно. Irina - Admin, Ivan - User
         */
        [Fact]
        public async void ShouldAllowJWTAuthorize()
        {
            var client = new HttpClient();



            // Пытаемся получить токен с не верным паролем

            var userError = new { Login = "Irina", Password = "1100" };

            var containerUserError = new StringContent(JsonConvert.SerializeObject(userError));

            containerUserError.Headers.ContentType.MediaType = "application/json";

            var answerUserError = await client.PostAsync(Host + "/api/authorize", containerUserError);

            Assert.Equal(HttpStatusCode.NoContent, answerUserError.StatusCode);


            // Пытаемся получить токен админа с верным паролем

            var userAdmin = new { Login = "Irina", Password = "1200" };

            var containerUserAdmin = new StringContent(JsonConvert.SerializeObject(userAdmin));

            containerUserAdmin.Headers.ContentType.MediaType = "application/json";                      

            var answerUserAdmin = await client.PostAsync(Host + "/api/authorize", containerUserAdmin);

            var preparedAnswer = JsonConvert.DeserializeObject<object>(await answerUserAdmin.Content.ReadAsStringAsync());

            var tokenContainer = JsonConvert.DeserializeObject<TokenContainer>((string)preparedAnswer);

            Assert.Equal(HttpStatusCode.OK, answerUserAdmin.StatusCode);

            Assert.Equal("Irina", tokenContainer.UserName);



            // Получаем ответ под токеном админа
                        
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenContainer.Token);

            var answerAuth = await client.GetAsync(new Uri(Host + "/api/rieltors"));

            Assert.Equal(HttpStatusCode.OK, answerAuth.StatusCode);



            // Пытаемся получить ответ без токена

            client.DefaultRequestHeaders.Authorization = null;

            var answerAuthError = await client.GetAsync(new Uri(Host + "/api/rieltors"));

            Assert.Equal(HttpStatusCode.Unauthorized, answerAuthError.StatusCode);



            // Получаем токен обычного юзера

            var userUser = new { Login = "Ivan", Password = "1100" };

            var containerUserUser = new StringContent(JsonConvert.SerializeObject(userUser));

            containerUserUser.Headers.ContentType.MediaType = "application/json";

            var answerUserUser = await client.PostAsync(Host + "/api/authorize", containerUserUser);

            var preparedAnswerUserUser = JsonConvert.DeserializeObject<object>(await answerUserUser.Content.ReadAsStringAsync());

            var tokenContainerUserUser = JsonConvert.DeserializeObject<TokenContainer>((string)preparedAnswerUserUser);

            Assert.Equal(HttpStatusCode.OK, answerUserUser.StatusCode);

            Assert.Equal("Ivan", tokenContainerUserUser.UserName);


            // Делаем под юзером Get запрос

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenContainerUserUser.Token);

            var answerAuthUser = await client.GetAsync(new Uri(Host + "/api/rieltors"));

            Assert.Equal(HttpStatusCode.OK, answerAuthUser.StatusCode);


            // Делаем под юзером Delete запрос

            var answerAuthUser2 = await client.DeleteAsync(new Uri(Host + "/api/rieltors/0"));

            Assert.Equal(HttpStatusCode.Forbidden, answerAuthUser2.StatusCode);

        }

        private class TokenContainer
        {
            public TokenContainer()
            {

            }

            public string Token;

            public string UserName;
        }
    }
}