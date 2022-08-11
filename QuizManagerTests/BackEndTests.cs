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
        [Category("Backend")]
        public void WriteColourDetailsToFile()
        {
            string path = Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "\\QuizManager\\Config\\colours.txt";
            FileHandler filehandler = new FileHandler();
            string colour1 = "#00a2e8";
            string colour2 = "#006a97";
            string colour3 = "#7f7f7f";

            bool ispassed = filehandler.WriteColoursToFile(colour1, colour2, colour3, path);
            Assert.IsTrue(ispassed);
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

        [Test]
        [Parallelizable]
        [Category("Backend")]
        public void AddNewQuizTableTest()
        {
            string name = string.Concat("TestName", DateTime.Now.ToString());
            AccessDatabase accessDatabase = new AccessDatabase();
            int check = accessDatabase.AddNewQuizInfoToDatabase(name, "Maths");
            string check2 = accessDatabase.CreateNewQuestionTable(check);
            Assert.IsNotNull(check2);
        }

        [Test]
        [Parallelizable]
        [Category("Backend")]
        public void UpdateCurrentQuizDatabase()
        {
            string name = string.Concat("TestName", DateTime.Now.ToString());
            AccessDatabase accessDatabase = new AccessDatabase();
            int check = accessDatabase.AddNewQuizInfoToDatabase(name, "Maths");
            string number = check.ToString();
            name = name.Replace(":", "");
            name = name.Replace(" ", "");
            name = name.Replace("/", "");
            bool check2 = accessDatabase.WriteToCurrentQuiz(name, number);
            Assert.IsTrue(check2); 
            check2 = accessDatabase.WriteToCurrentQuiz("", "");
            Assert.IsTrue(check2);
        }

        [Test]
        [Parallelizable]
        [Category("Backend")]
        public void UpdateCurrentQuizQuestion()
        {
            string name = string.Concat("TestName", DateTime.Now.ToString());
            AccessDatabase accessDatabase = new AccessDatabase();
            int check = accessDatabase.AddNewQuizInfoToDatabase(name, "Maths");
            string check2 = accessDatabase.CreateNewQuestionTable(check);
            Assert.IsNotNull(check2);
            bool check3 = accessDatabase.UpdateCurrentQuizQuestionTable(check, 1, 1, "Something interesting", "Hello", "There", "General", "Kenobi");
            Assert.IsTrue(check3);
        }


    }
}
//(int joinValue, int questionNum, int answer, string quesDescription,
//                                                    string answerdes1, string answerdes2, string answerdes3, string answerdes4)