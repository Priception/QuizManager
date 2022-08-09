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

        public IActionResult OnPostQuizInfo()
        {
            string quizName = Request.Form["QuizName"];
            string quizType = Request.Form["QuizType"];

            AccessDatabase accessDatabase = new AccessDatabase();
            int joinValue = accessDatabase.AddNewQuizInfoToDatabase(quizName, quizType);

            return RedirectToPage("CreateQuizQuestions");
        }
        public IActionResult OnPostColourChange()
        {
            string colour1 = Request.Form["colour1"];
            string colour2 = Request.Form["colour2"];
            string colour3 = Request.Form["colour3"];

            FileHandler filehandler = new FileHandler();
            filehandler.WriteColoursToFile(colour1, colour2, colour3);

            return RedirectToPage("Settings");
        }

    }
}