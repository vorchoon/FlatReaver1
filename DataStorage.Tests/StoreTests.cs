using System;
using System.Collections.Generic;
using Xunit;
using DataStore;

namespace DataStorage.Tests
{
    public class StoreTests
    {
        private Store Store;

        public StoreTests()
        {
            Store = new Store("Server=localhost;Database=FlatReaverStorageTest;Trusted_Connection=True;");
        }


        [Fact]
        public void ShouldSupportCRUD()
        {

            dynamic protoUser = new { Login = "TestLogin654894943544684", PasswordHash = "TestHash00000000000000" };

            User user = new User() { Login = "TestLogin654894943544684", PasswordHash = "TestHash00000000000000" };


            try
            {

                Assert.Null(Store.ReadSingle<User>(protoUser));

                Store.Create(user);

                User findedUser = Store.ReadSingle<User>(protoUser);

                Assert.NotNull(findedUser);

                Assert.Equal("TestLogin654894943544684", findedUser.Login);

                findedUser.Login = "TestLogin00000000000000001";

                var newProtoUser = new { Login = "TestLogin00000000000000001", PasswordHash = "TestHash00000000000000" };

                Store.Update(findedUser);

                User newFindedUser = Store.ReadSingle<User>(newProtoUser);

                Assert.Equal((string)newProtoUser.Login, newFindedUser.Login);

                Store.Delete(user);


                Assert.Null(Store.ReadSingle<User>(newProtoUser));


            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                try
                {
                    Store.Delete(user);
                }
                catch
                {
                }
            }
        }

        [Fact]
        public void ShouldSupportForeignKeys()
        {
            var division1 = new Division() { Name = "CloudyAlbion" };
            var rieltor1A = new Rieltor() { Firstname = "Cloudia", Lastname = "Albionetta"};
            var rieltor1B = new Rieltor() { Firstname = "Clous", Lastname = "Alborro"};
            var division2 = new Division() { Name = "SilentHill" };
            var rieltor2 = new Rieltor() { Firstname = "Silentia", Lastname = "Hillawa" };

            try
            {
                Store.Create(division1);
                Store.Create(division2);
                Store.Create(rieltor1A);
                Store.Create(rieltor1B);
                Store.Create(rieltor2);

                var division1Object = new { Name = "CloudyAlbion" };
                var division2Object = new { Name = "SilentHill" };
                var rieltor1AObject = new { Lastname = "Albionetta" };
                var rieltor1BObject = new { Lastname = "Alborro" };
                var rieltor2Object = new { Lastname = "Hillawa" };

                var findedDivision1 = Store.ReadSingle<Division>(division1Object);
                var findedDivision2 = Store.ReadSingle<Division>(division2Object);

                var findedRieltor1A = Store.ReadSingle<Rieltor>(rieltor1AObject);
                var findedRieltor1B = Store.ReadSingle<Rieltor>(rieltor1BObject);
                var findedRieltor2 = Store.ReadSingle<Rieltor>(rieltor2Object);

                findedRieltor1A.DivisionId = division1.Id;
                findedRieltor1B.DivisionId = division1.Id;
                findedRieltor2.DivisionId = division2.Id;

                Store.Update(findedRieltor1A);
                Store.Update(findedRieltor1B);
                Store.Update(findedRieltor2);

                findedRieltor1B.DivisionId = 999999999999999999;

                Action actionTest1 = new Action(() => Store.Update(findedRieltor1B));

                Action actionTest2 = new Action(() => Store.Delete(division2));

                Assert.ThrowsAny<Exception>(actionTest1);
                Assert.ThrowsAny<Exception>(actionTest2);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                try
                {
                    Store.Delete(rieltor2);
                }
                catch
                {

                }
                try
                {
                    Store.Delete(rieltor1A);
                }
                catch
                {

                }
                try
                {
                    Store.Delete(rieltor1B);
                }
                catch
                {
                    
                }
                try
                {
                    Store.Delete(division1);
                }
                catch
                {

                }
                try
                {

                    Store.Delete(division2);
                }
                catch
                {

                }
            }
        }

        [Fact]
        public void ShouldSupportPagination()
        {
            var rieltors = new List<Rieltor>();
            for (var i = 0; i <= 99; i++)
            {
                var rieltor = new Rieltor() { Firstname = "Vasya" + i, Lastname = "Ivanov" + i };
                rieltors.Add(rieltor);
                Store.Create(rieltor);
            }

            try
            {

                var pageMax = Store.Read<Rieltor>(null, false, false);
                var page1Big = Store.Read<Rieltor>(null, false, false, 60, 0);
                var page2Big = Store.Read<Rieltor>(null, false, false, 60, 1);
                var page1Small = Store.Read<Rieltor>(null, false, false, 38, 0);
                var page2Small = Store.Read<Rieltor>(null, false, false, 38, 1);
                var page3Small = Store.Read<Rieltor>(null, false, false, 38, 2);

                Assert.Equal(100, pageMax.Count);
                Assert.Equal(60, page1Big.Count);
                Assert.Equal(40, page2Big.Count);
                Assert.Equal(38, page1Small.Count);
                Assert.Equal(38, page2Small.Count);
                Assert.Equal(24, page3Small.Count);


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                foreach (var rieltor in rieltors)
                {
                    Store.Delete(rieltor);
                }
            }
        }

        [Fact]
        public void ShouldFindItemsWithLike()
        {
            var rieltor1 = new Rieltor() { Firstname = "TestIvanRieltor", Lastname = "TestSidorovRieltor" };

            var rieltor2 = new Rieltor() { Firstname = "TestIrinaRieltor", Lastname = "TestKamenskayaRieltor" };

            var rieltor3 = new Rieltor() { Firstname = "TestBorisRieltor", Lastname = "TestSidorovRieltor" };

            Store.Create(rieltor1);
            Store.Create(rieltor2);
            Store.Create(rieltor3);

            var strictlObject = new { Firstname = "TestIvanRieltor", Lastname = "TestSidorovRieltor" };

            var lowerObject = new { Firstname = "test", Lastname = "" };

            var upperObject = new { Firstname = "TEST", Lastname = "" };

            var partialObject = new { Firstname = "boris", Lastname = "kamen" };

            //1
            var findedRieltorsStrict = Store.Read<Rieltor>(strictlObject, true, true);

            //2
            var findedRieltorsStrictAny = Store.Read<Rieltor>(strictlObject, true, false);

            //3
            var findedRieltorsAnyUpper = Store.Read<Rieltor>(upperObject, false, false);

            //3
            var findedRieltorsAllUpper = Store.Read<Rieltor>(upperObject, false, true);

            //3
            var findedRieltorsAnyLower = Store.Read<Rieltor>(lowerObject, false, false);

            //3
            var findedRieltorsAllLower = Store.Read<Rieltor>(lowerObject, false, true);

            //2
            var findedRieltorsPartialSoft = Store.Read<Rieltor>(partialObject, false, false);

            //0
            var findedRieltorsPartialStrict = Store.Read<Rieltor>(partialObject, false, true);
            try
            {


                Assert.Equal(1, findedRieltorsStrict.Count);
                Assert.Equal(2, findedRieltorsStrictAny.Count);
                Assert.Equal(3, findedRieltorsAnyUpper.Count);
                Assert.Equal(3, findedRieltorsAllUpper.Count);
                Assert.Equal(3, findedRieltorsAnyLower.Count);
                Assert.Equal(3, findedRieltorsAllLower.Count);
                Assert.Equal(2, findedRieltorsPartialSoft.Count);
                Assert.Equal(0, findedRieltorsPartialStrict.Count);
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                Store.Delete(rieltor1);
                Store.Delete(rieltor2);
                Store.Delete(rieltor3);
            }






        }

    }
}
