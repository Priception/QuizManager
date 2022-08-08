using QuizManager;
using QuizManager.Pages;
using QuizManager.Scripts.DatabaseScripts;
using TestingFramework.Wrapper;

namespace QuizManagerTests
{
    public class Tests
    {
        public void Setup()
        {

        }

        [Test]
        [Category("Backend")]
        public void ReadColourDetailsFromFile()
        {
            string path = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\QuizManager\\Config\\colours.txt";
            FileHandler filehandler = new FileHandler();
            string test1 = filehandler.ReadFromColoursFile(0, path);
            Assert.IsNotEmpty(test1);
            string test2 = filehandler.ReadFromColoursFile(1, path);
            Assert.IsNotEmpty(test2);
            string test3 = filehandler.ReadFromColoursFile(2, path);
            Assert.IsNotEmpty(test3);
        }

        [Test]
        [Category("Backend")]
        public void CreateAndReadColourDetailsFromFile()
        {
            string path = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\QuizManager\\Config\\colours.txt";
            File.Delete(path);
            FileHandler filehandler = new FileHandler();
            string test1 = filehandler.ReadFromColoursFile(0, path);
            Assert.IsNotEmpty(test1);
            string test2 = filehandler.ReadFromColoursFile(1, path);
            Assert.IsNotEmpty(test2);
            string test3 = filehandler.ReadFromColoursFile(2, path);
            Assert.IsNotEmpty(test3);
        }

        [Test]
        [Parallelizable]
        [Category("Backend")]
        public void DatabaseAccessTest()
        {
            AccessDatabase accessDatabase = new AccessDatabase();
            List<string> check = accessDatabase.DatabaseTest();
            Assert.IsNotNull(check);
        }

        [Test]
        [Parallelizable]
        [Category("Backend")]
        public void ReadJoinCodeFromDatabase()
        {
            AccessDatabase accessDatabase = new AccessDatabase();
            List<int> check = accessDatabase.ReadJoinNumbersFromQuizInfo();
            Assert.IsNotNull(check);
        }

        [Test]
        [Parallelizable]
        [Category("Backend")]
        public void AddNewQuizInfoTest()
        {
            string name = string.Concat("TestName",DateTime.Now.ToString());
            AccessDatabase accessDatabase = new AccessDatabase();
            int check = accessDatabase.AddNewQuizInfoToDatabase(name, "Maths");
            if (check == 0)
            {
                Assert.Fail();
            }
        }


    }
}