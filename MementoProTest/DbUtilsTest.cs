using Models;

namespace MementoProTest
{
    public class DbUtilsTest
    {
        private readonly string DummyUserLogin = "_mazepa_",
                                DummyUserPassword = "Mazepa1!";

        [Fact]
        public void TestGetUser1()
        {
            var foundUser = DbUtils.GetUser(DummyUserLogin, DummyUserPassword);
            Assert.True(foundUser != null);
        }
        private void TestGetUser2()
        {
            var foundUser = DbUtils.GetUser("", "");
            Assert.True(foundUser == null);
        }

        [Fact]
        public void TestIsUserExist1()
        {
            var userExist = DbUtils.IsUserExist(DummyUserLogin, DummyUserPassword);
            Assert.True(userExist == true);
        }
        private void TestIsUserExist2()
        {
            var isUserExist = DbUtils.IsUserExist("", "");
            Assert.True(isUserExist == false);
        }

        [Fact]
        public void TestRegisterUser1()
        {
            var wasRegistered = DbUtils.RegisterUser(DummyUserLogin, DummyUserPassword);
            Assert.True(wasRegistered == false);
        }

        private void TestRegisterUser2()
        {
            var wasRegistered = DbUtils.RegisterUser("Newuserlogin", "Newuserpassword");
            Assert.True(wasRegistered == true);
        }

        [Fact]
        public void TestAuthorizeUser1()
        {
            var wasUserAuthorized = DbUtils.AuthorizeUser(DummyUserLogin, DummyUserPassword);
            Assert.True(wasUserAuthorized == true);
        }
        private void TestAuthorizeUser2()
        {
            var wasUserAuthorized = DbUtils.AuthorizeUser("", "");
            Assert.True(wasUserAuthorized == false);
        }

        [Fact]
        public void TestDeleteUser()
        {
            var userToRemove = DbUtils.GetUser(DummyUserLogin, DummyUserPassword);
            var wasUserDeleted = Session.Db.Users.Local.Remove(userToRemove!);
            Assert.True(wasUserDeleted == true);
        }
    }
}