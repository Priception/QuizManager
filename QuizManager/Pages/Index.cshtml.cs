using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizManager.Scripts.DatabaseScripts;

namespace QuizManager.Pages
{
    public class IndexModel : PageModel
    {

        public void OnGet()
        {

        }

        public string GetColours(int colour)
        {
            FileHandler filehandler = new FileHandler();
            return filehandler.ReadFromColoursFile(colour);
        }

        public void OnPost()
        {
            string quizName = Request.Form["QuizName"];
            string quizType = Request.Form["QuizType"];

            AccessDatabase accessDatabase = new AccessDatabase();
            int joinValue = accessDatabase.AddNewQuizInfoToDatabase(quizName, quizType);

            ViewData["confirmation"] = $"{quizName}, information will be sent to {quizType}";
        }
    }
}