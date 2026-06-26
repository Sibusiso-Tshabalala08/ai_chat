using System;
using System.Collections.Generic;
using System.Linq;

namespace demo
{// start of namespace

    // -----------------------------------------------------------------------
    // QUIZ GAME CLASS
    // Holds the bank of cybersecurity quiz questions (Part 3 / Task 2) and
    // tracks the user's progress and score through a quiz attempt. Built
    // using a List of QuizQuestion objects, as suggested in the brief.
    // -----------------------------------------------------------------------
    public class QuizGame
    {// start of class

        // The full bank of available questions (12 questions, mixed MCQ/True-False)
        private List<QuizQuestion> questionBank;

        // The randomised subset of questions used for the current quiz attempt
        private List<QuizQuestion> activeQuestions;

        // Index of the question the user is currently answering
        private int currentIndex;

        // Number of questions answered correctly so far this attempt
        private int score;

        // How many questions are used in a single quiz attempt
        private const int QuestionsPerQuiz = 10;


        // -----------------------------------------------------------------------
        // CONSTRUCTOR
        // Loads the full question bank as soon as the quiz game is created
        // -----------------------------------------------------------------------
        public QuizGame()
        {
            questionBank = BuildQuestionBank();
        }


        // -----------------------------------------------------------------------
        // START QUIZ METHOD
        // Resets the score and shuffles a fresh set of questions for a new attempt
        // -----------------------------------------------------------------------
        public void StartQuiz()
        {
            currentIndex = 0;
            score = 0;

            Random rnd = new Random();

            // Shuffle the question bank and take the first N questions for this attempt
            activeQuestions = questionBank
                .OrderBy(q => rnd.Next())
                .Take(QuestionsPerQuiz)
                .ToList();

        }// end of StartQuiz


        // True once every question in the current attempt has been answered
        public bool IsFinished
        {
            get { return activeQuestions == null || currentIndex >= activeQuestions.Count; }
        }

        // Number of correct answers so far this attempt
        public int Score
        {
            get { return score; }
        }

        // Total number of questions in this attempt
        public int TotalQuestions
        {
            get { return activeQuestions == null ? 0 : activeQuestions.Count; }
        }

        // 1-based question number, used for the "Question X of Y" progress label
        public int CurrentQuestionNumber
        {
            get { return currentIndex + 1; }
        }


        // -----------------------------------------------------------------------
        // GET CURRENT QUESTION METHOD
        // Returns the question the user should currently be answering, or
        // null if the attempt has finished
        // -----------------------------------------------------------------------
        public QuizQuestion GetCurrentQuestion()
        {
            if (IsFinished) return null;
            return activeQuestions[currentIndex];

        }// end of GetCurrentQuestion


        // -----------------------------------------------------------------------
        // SUBMIT ANSWER METHOD
        // Checks the chosen option against the correct answer, updates the
        // score, advances to the next question, and returns the explanation
        // text so the GUI can show immediate feedback
        // -----------------------------------------------------------------------
        public bool SubmitAnswer(int chosenIndex, out string explanation)
        {
            QuizQuestion question = GetCurrentQuestion();
            explanation = question.Explanation;

            bool isCorrect = (chosenIndex == question.CorrectIndex);

            if (isCorrect)
                score = score + 1;

            currentIndex = currentIndex + 1;

            return isCorrect;

        }// end of SubmitAnswer


        // -----------------------------------------------------------------------
        // GET FINAL SCORE MESSAGE METHOD
        // Returns encouraging feedback based on the user's final score,
        // matching the tone requested in the brief
        // -----------------------------------------------------------------------
        public string GetFinalScoreMessage()
        {
            if (TotalQuestions == 0) return "No quiz has been completed yet.";

            double percentage = (double)score / TotalQuestions * 100;

            if (percentage >= 80)
                return "Great job! You're a cybersecurity pro!";
            else if (percentage >= 50)
                return "Good effort! A little more practice and you'll be a pro.";
            else
                return "Keep learning to stay safe online!";

        }// end of GetFinalScoreMessage


        // -----------------------------------------------------------------------
        // BUILD QUESTION BANK METHOD
        // Prepares the bank of cybersecurity questions covering phishing,
        // password safety, safe browsing, social engineering, firewalls,
        // ransomware and two-factor authentication. Mixes multiple-choice
        // and true/false formats for variety, as required by the brief.
        // -----------------------------------------------------------------------
        private List<QuizQuestion> BuildQuestionBank()
        {
            List<QuizQuestion> questions = new List<QuizQuestion>();

            questions.Add(new QuizQuestion(
                "What should you do if you receive an email asking for your password?",
                new List<string> { "Reply with your password", "Delete the email", "Report the email as phishing", "Ignore it" },
                2,
                "Reporting phishing emails helps prevent scams.",
                QuestionType.MultipleChoice));

            questions.Add(new QuizQuestion(
                "A strong password should include a mix of letters, numbers, and symbols.",
                new List<string> { "True", "False" },
                0,
                "Mixing character types makes passwords much harder to crack.",
                QuestionType.TrueFalse));

            questions.Add(new QuizQuestion(
                "Which of these is the safest way to browse on public Wi-Fi?",
                new List<string> { "Connect normally", "Use a VPN", "Disable your firewall", "Share your location" },
                1,
                "A VPN encrypts your traffic, protecting it from prying eyes on public networks.",
                QuestionType.MultipleChoice));

            questions.Add(new QuizQuestion(
                "It is safe to reuse the same password across multiple accounts.",
                new List<string> { "True", "False" },
                1,
                "If one account is breached, reused passwords put all your other accounts at risk.",
                QuestionType.TrueFalse));

            questions.Add(new QuizQuestion(
                "What is 'social engineering' in cybersecurity?",
                new List<string> { "A type of antivirus software", "Manipulating people into giving up confidential information", "A method of encrypting files", "A firewall configuration" },
                1,
                "Social engineering relies on psychological manipulation rather than technical hacking.",
                QuestionType.MultipleChoice));

            questions.Add(new QuizQuestion(
                "Two-factor authentication (2FA) adds an extra layer of security to your accounts.",
                new List<string> { "True", "False" },
                0,
                "2FA requires a second verification step, making accounts much harder to break into.",
                QuestionType.TrueFalse));

            questions.Add(new QuizQuestion(
                "Which of these is a common sign of a phishing email?",
                new List<string> { "Personalised greeting with no errors", "Urgent threats and spelling mistakes", "Coming from a known colleague", "No links or attachments" },
                1,
                "Urgency and poor spelling or grammar are classic phishing red flags.",
                QuestionType.MultipleChoice));

            questions.Add(new QuizQuestion(
                "You should always check that a website uses HTTPS before entering sensitive information.",
                new List<string> { "True", "False" },
                0,
                "HTTPS means your connection to the site is encrypted.",
                QuestionType.TrueFalse));

            questions.Add(new QuizQuestion(
                "What is ransomware?",
                new List<string> { "Software that speeds up your PC", "Malware that locks your files and demands payment", "A type of firewall", "A password manager" },
                1,
                "Ransomware encrypts your files and demands payment for their release.",
                QuestionType.MultipleChoice));

            questions.Add(new QuizQuestion(
                "Clicking links in unexpected text messages from unknown numbers is safe.",
                new List<string> { "True", "False" },
                1,
                "This is a common 'smishing' (SMS phishing) tactic - avoid clicking unknown links.",
                QuestionType.TrueFalse));

            questions.Add(new QuizQuestion(
                "What does a firewall primarily do?",
                new List<string> { "Speeds up your internet", "Controls network traffic based on security rules", "Backs up your files", "Removes viruses automatically" },
                1,
                "A firewall filters incoming and outgoing traffic based on defined security rules.",
                QuestionType.MultipleChoice));

            questions.Add(new QuizQuestion(
                "Regularly backing up your data helps protect against ransomware attacks.",
                new List<string> { "True", "False" },
                0,
                "Backups mean you can restore your files without paying a ransom.",
                QuestionType.TrueFalse));

            return questions;

        }// end of BuildQuestionBank

    }// end of class
}// end of namespace