using QuizManager;
using QuizManager.Pages;
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
    }
}