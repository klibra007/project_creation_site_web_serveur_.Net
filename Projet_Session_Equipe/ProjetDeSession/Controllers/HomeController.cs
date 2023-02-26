using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetDeSession.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace ProjetDeSession.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISession _session;
        private QuizExamenContext context;

        public HomeController(QuizExamenContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            _session = httpContextAccessor.HttpContext.Session;
        }

// Menu principal
        public IActionResult Index()
        {
            ViewBag.Message = _session.GetString("message"); // Prendre le message(si existe) du session storage (voir le index.cshtml ligne 9)
            return View();
        }

// Question 1) : Générer un nouveau quiz - Afficher la forme
        public IActionResult GenererQuiz()
        {
            ViewBag.UserName = _session.GetString("userName"); 
            ViewBag.Email = _session.GetString("email");

            // Collecter la quantite des questions de types diffrerents et les passer à la Vue utilisant ViewBag
            ViewBag.questionsFacileCount = context.Questions
                .Where(s =>
                    s.CategoryId == 1
                )
                .ToList()
                .Count;
            ViewBag.questionsMoyenCount = context.Questions
                .Where(s =>
                    s.CategoryId == 2
                )
                .ToList()
                .Count;
            ViewBag.questionsDifficileCount = context.Questions
                .Where(s =>
                    s.CategoryId == 3
                )
                .ToList()
                .Count;

                _session.SetString("message", ""); // Enlever le message du session
            return View();
        }

// Question 1) : Générer un nouveau quiz - Enregistrer les données du formulaire
        [HttpPost]
        public IActionResult GenererQuiz(string userName, string email, int nombreFacile, int nombreMoyen, int nombreDifficile)
        {
            // si la quantite des questions est égale a zero, on ajoute l'erreur dans le model avant de montrer la vue
            if (nombreDifficile == 0 && nombreFacile == 0 & nombreMoyen == 0) {
                ModelState.AddModelError("questions", "Vous devez ajouter au moins une question!");
                
            }
            if (ModelState.IsValid)
            {
                List<Question> questions=new List<Question>();
                if (nombreFacile>0) {
                    List<Question> questionsFacile = context.Questions
                    .Where(s =>
                        s.CategoryId == 1
                    )
                    .OrderBy(r => Guid.NewGuid()) // Question 1) : Le choix des questions se fait d’une façon aléatoire
                    .Take(nombreFacile)
                    .ToList();
                    questions = questions.Union(questionsFacile).ToList();
                }
                if (nombreMoyen > 0)
                {
                    List<Question> questionsMoyen = context.Questions
                    .Where(s =>
                        s.CategoryId == 2
                    )
                    .OrderBy(r => Guid.NewGuid()) // Question 1) : Le choix des questions se fait d’une façon aléatoire
                    .Take(nombreMoyen)
                    .ToList();
                    questions = questions.Union(questionsMoyen).ToList();
                }
                if (nombreDifficile > 0)
                {
                    List<Question> questionsDifficile = context.Questions
                    .Where(s =>
                        s.CategoryId == 3
                    )
                    .OrderBy(r => Guid.NewGuid()) // Question 1) : Le choix des questions se fait d’une façon aléatoire
                    .Take(nombreDifficile)
                    .ToList();
                    questions = questions.Union(questionsDifficile).ToList();
                }

                List<QuestionQuiz> questionQuizzes = new List<QuestionQuiz>();
                foreach (Question question in questions) {
                    questionQuizzes.Add(new QuestionQuiz { 
                        QuestionId = question.QuestionId,
                    });
                }
                Quiz quiz = new Quiz();
                quiz.UserName = userName;
                quiz.Email = email;
                quiz.QuestionQuizzes = questionQuizzes;

                context.Add<Quiz>(quiz);
                context.SaveChanges(); // Question 1) : Le nouveau quiz généré est stocké dans la base de données
                _session.SetString("userName", userName); // Stocker l'information de l'utilisateur dans session storage
                _session.SetString("email", email);
                _session.SetString("message", "Votre Quiz a été généré !"); // Stocker le message dans le session
                return RedirectToAction("Index");
            }
            return View();
        }

// Question 2) : Passer un quiz donné - Choix de l'utilisateur
        [HttpGet]
        public IActionResult PasserQuiz()
        {
            _session.SetString("message", ""); // Enlever le message de session
            ViewBag.UserName = _session.GetString("userName"); // Prendre l'information de l'utilisateur du session storage
            ViewBag.Email = _session.GetString("email");
            return View();
        }

// Question 2) : Passer un quiz donné - Choix de quiz que l'utilisateur selectionne
        [HttpPost]
        public IActionResult PasserQuiz(string userName, string email)
        {
            ViewBag.UserName = userName;
            ViewBag.Email = email;
            _session.SetString("userName", userName);
            _session.SetString("email", email);
            List<Quiz> quizzes = new List<Quiz>();
            quizzes = context.Quizzes
                .Where(x => x.UserName == userName && x.Email == email && x.Answers.Count == 0)
                .ToList();
            if (quizzes.Count == 0) {
                ViewBag.MessageErreur = "Utilisateur inexistant ou aucun Quiz à passer !";
                return View();
            }
            return View(quizzes);
        }

// Question 2) : Passer un quiz donné - Affichage de la liste des questions avec les options pour y répondre
        [HttpGet]
        public IActionResult AnswerQuiz(int id)
        {
            Quiz quiz=context.Quizzes.SingleOrDefault(q=>q.QuizId== id);
            
            return View(quiz);
        }

// Question 3) : Sauvegarder le résultat - Stockage des réponses dans la base de données
        [HttpPost]
        public IActionResult AnswerQuiz(IFormCollection answers)
        {
            List<Answer> answersToSave = new List<Answer>();
            foreach (string key in answers.Keys)
            {
                if (key.StartsWith("question-")) {
                    int value = Convert.ToInt32(answers[key].ToString());
                    answersToSave.Add(new Answer {
                        OptionId= Convert.ToInt32(value)
                    });
                }
                Debug.WriteLine(key);
            }
            int quizId = Convert.ToInt32(answers["QuizId"].ToString());
            Quiz quiz = context.Quizzes.SingleOrDefault(q => q.QuizId == quizId);
            quiz.Answers = answersToSave;
            context.Update(quiz);
            context.SaveChanges(); // Enregistrer le reponse dans DB
            _session.SetString("message", "Vos réponses ont été enregistrées !"); // Stocker le message de la session
            return RedirectToAction("Index");
        }

// Question 4) : Réviser la note obtenue - Choix de l'utilisateur
        [HttpGet]
        public IActionResult ReviserQuiz()
        {
            _session.SetString("message", ""); // Enlever le message de session
            ViewBag.UserName = _session.GetString("userName");
            ViewBag.Email = _session.GetString("email");
            return View();
        }

// Question 4) : Réviser la note obtenue - Choix de quiz de l'utilisateur selectionné
        [HttpPost]
        public IActionResult ReviserQuiz(string userName, string email)
        {
            ViewBag.UserName = userName;
            ViewBag.Email = email;
            _session.SetString("userName", userName);
            _session.SetString("email", email);
            List<Quiz> quizzes = new List<Quiz>();
            quizzes = context.Quizzes
                .Where(x => x.UserName == userName && x.Email == email && x.Answers.Count > 0)
                .ToList();
            if (quizzes.Count == 0)
            {
                ViewBag.MessageErreur = "Utilisateur inexistant ou aucun Quiz à réviser !";
                return View();
            }
            return View(quizzes);
        }

// Question 4) : Réviser la note obtenue - Affichage des résultats
        [HttpGet]
        public IActionResult ReviewQuiz(int id)
        {
            Quiz quiz = context.Quizzes.SingleOrDefault(q => q.QuizId == id);

            return View(quiz);
        }

        public IActionResult SupprimerAnswerQuiz(int QuizId)
        {

            var answerAsupprimer = context.Answers.Where(answer => answer.QuizId == QuizId);

            foreach (var answer in answerAsupprimer)
            {
                context.Remove<Answer>(answer);
            }

            context.SaveChanges();
            _session.SetString("message", "Votre Quiz a bien été réinitialisé! ");

            return RedirectToAction("Index");
        }

       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
