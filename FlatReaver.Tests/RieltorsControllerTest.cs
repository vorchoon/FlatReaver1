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
    public class RieltorsControllerTest
    {

        const string Host = "http://localhost:32018";


        public RieltorsControllerTest()
        {

        }


        [Fact]
        public async void ShouldGetAnswer()
        {
            HttpClient client = await PrepareClient();

            var answer = await client.GetAsync(new Uri(Host + "/api/rieltors"));

            dynamic content = JsonConvert.DeserializeObject(await answer.Content.ReadAsStringAsync());
            dynamic count = content.Count;
            Assert.NotNull(count);

        }


        private static async System.Threading.Tasks.Task<HttpClient> PrepareClient()
        {
            var client = new HttpClient();

            var userAdmin = new { Login = "Irina", Password = "1200" };

            var containerUser = new StringContent(JsonConvert.SerializeObject(userAdmin));

            containerUser.Headers.ContentType.MediaType = "application/json";

            var answerUser = await client.PostAsync(Host + "/api/authorize", containerUser);

            var preparedAnswer = JsonConvert.DeserializeObject<object>(await answerUser.Content.ReadAsStringAsync());

            var tokenContainer = JsonConvert.DeserializeObject<TokenContainer>((string)preparedAnswer);

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenContainer.Token);
            return client;
        }

        // Тест требует как минимум 3 записей в продакшн БД
        [Fact]
        public async void ShouldSupportPagination()
        {
            HttpClient client = await PrepareClient();


            var answer1 = await client.GetAsync(new Uri(Host + "/api/rieltors?pageIndex=0&pageSize=1"));
            dynamic content1 = JsonConvert.DeserializeObject(await answer1.Content.ReadAsStringAsync());
            dynamic count1 = content1.Count;
            Assert.Equal(1, count1);

            var answer2 = await client.GetAsync(new Uri(Host + "/api/rieltors?pageIndex=0&pageSize=3"));
            dynamic content2 = JsonConvert.DeserializeObject(await answer2.Content.ReadAsStringAsync());
            dynamic count2 = content2.Count;
            Assert.Equal(3, count2);

            var answer3 = await client.GetAsync(new Uri(Host + "/api/rieltors?pageIndex=2&pageSize=1"));
            dynamic content3 = JsonConvert.DeserializeObject(await answer3.Content.ReadAsStringAsync());
            dynamic count3 = content3.Count;
            Assert.Equal(1, count3);


        }

        [Fact]
        public async void ShouldAllowCRUD()
        {
            HttpClient client = await PrepareClient();

            var originRieltor = new Rieltor() { Firstname = "Миша2216", Lastname = "Гробуновский335589" };


            var containerC = new StringContent(JsonConvert.SerializeObject(originRieltor));
            containerC.Headers.ContentType.MediaType = "application/json";
            var answerC = await client.PostAsync(Host + "/api/rieltors", containerC);
            Assert.Equal(HttpStatusCode.OK, answerC.StatusCode);

            var answerR = await client.GetAsync(new Uri(Host + "/api/rieltors"));
            List<Rieltor> contentR = JsonConvert.DeserializeObject<List<Rieltor>>(await answerR.Content.ReadAsStringAsync());
            var modificatedRieltor = contentR.First(e => e.Firstname == "Миша2216" && e.Lastname == "Гробуновский335589");
            modificatedRieltor.Firstname = "Михаил180825";
            modificatedRieltor.Lastname = "Печкин65484";

            var containerU = new StringContent(JsonConvert.SerializeObject(modificatedRieltor));
            containerU.Headers.ContentType.MediaType = "application/json";
            var answerU = await client.PutAsync(Host + "/api/rieltors", containerU);
            Assert.Equal(HttpStatusCode.OK, answerU.StatusCode);


            var answerD = await client.DeleteAsync(Host + "/api/rieltors/" + modificatedRieltor.Id);
            Assert.Equal(HttpStatusCode.OK, answerD.StatusCode);
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
