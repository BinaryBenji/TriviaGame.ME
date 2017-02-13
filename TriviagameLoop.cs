using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriviagameLoop : MonoBehaviour {

    public struct Question // Global struct for one question. We insert data into it later.
    {
        public string questionText;
        public string[] answers;
        public int correctAnswerIndex;

        public Question(string questionText, string[] answers, int correctAnswerIndex)
        {
            this.questionText = questionText;
            this.answers = answers;
            this.correctAnswerIndex = correctAnswerIndex;
        }
    }

    private Question currentQuestion = new Question(" ",new string[]{" ", " ", " ", " ", " "},0);
    public Button[] answerButtons;
    public Text questionText;
    private int currentQuestionIndex;
    private int[] questionNumbersChosen = new int[5];
    private int questionsFinished;
    public GameObject[] TriviaPanels;
    public GameObject finalResultsPanel;
    public Text resultsText;
    private int numberOfCorrectAnswers;
    private Question[] questions = new Question[10];
    private bool allowSelection = true;
    public GameObject feedbackText;
    public int questionNum;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < questionNumbersChosen.Length; i++) // Avoid the 0 auto fill when tab declaration
        {
            questionNumbersChosen[i] = -1;
        }
        questions[0] = new Question("I'm blue, Dabedeeda__ ", new string[] { "bedee", "beda", "bada", "boda", "dada" }, 1);
        questions[1] = new Question("Qui a le meilleur programme politique ? ", new string[] { "Macron", "Le Pen", "Fillon", "Sarko", "Je ne vote pas pour des trous de balle pareil" }, 4);
        questions[2] = new Question("Danser la polka c'est :  ", new string[] { "Bien", "Pas Bien", "Relativement Bien", "Plutot Bien", "Danser la polka" }, 0);
        questions[3] = new Question("\"Seuls les vrais négros le savent\" - Booba Mais que savent-ils ?", new string[] { "Que le poulet c'est la baaaase", "Wallah", "Starfoullah", "Amedouxlilas", "Rien de spécial" }, 4);
        questions[4] = new Question("BHL fait une bagarre avec Eric Zemmour : ", new string[] { "Eric Zemmour gagne par K.0", "BHL attaque farine", "Eric assomme BHL avec son livre", "Rien ne se passe", "Il y a égalité" }, 4);
        questions[5] = new Question("Une mec arrive a un mariage tout nu. Pourquoi ? ", new string[] { "Il a oublié ses affaires", "C'est un mariage organisé dans le Nord", "On lui a volé son slip", "Le crocodile Lacoste les a mangé", "Sur la porte il y avait écrit no smoking" }, 4);
        questions[6] = new Question("Pourquoi les magasins ouverts 24h/24 ont-ils des serrures ? ", new string[] { "NTM", "Pour laisser passer l'air", "C'est ouvert pour les insectes", "Wlh mskn", "Che pa frer" }, 2);
        questions[7] = new Question("Pourquoi le mot « court » est-il plus long que le mot « long » ? ", new string[] { " ", " ", " ", " ", "Passer a la question suivante" }, 4);
        questions[8] = new Question("Est-il possible de retrouver le pain perdu ? ", new string[] { "La boulangerie française te sert que des biscottes", "DES BISCOTTES", "Mais oui c'est clair", "Non, il est perdu, c'est écrit dans la question", "Chez Paul" }, 0);
        questions[9] = new Question("Peut-on tirer une flèche avec l'arc de triomphe ? ", new string[] { "Tu m'as tué de rire batard", "Pas drole", "Pas drole", "Pas drole", "Pas drole" }, 0);
        chooseQuestions();
        assignQuestion(questionNumbersChosen[0]);
    }


    // Update is called once per frame
    void Update () { // Quit if escape is pressed
        quitGame();
    }

    void assignQuestion(int questionNum) // Set up interface to show a question
    {
        currentQuestion = questions[questionNum];
        questionText.text = currentQuestion.questionText; // Disp question
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<Text>().text = currentQuestion.answers[i]; // Disp answers
        }
    }

    public void checkAnswer(int buttonNum) // Check if an answer is good + move to another question with Coroutine
    {
        if (allowSelection) // We wait 1 sec after 1 pick
        {
            if (buttonNum == currentQuestion.correctAnswerIndex)
            {
                print("Correct :D ");
                numberOfCorrectAnswers++;
                feedbackText.GetComponent<Text>().text = "OK";
                feedbackText.GetComponent<Text>().color = Color.green;
            }
            else
            {
                print("Incorrect :( ");
                feedbackText.GetComponent<Text>().text = "KO";
                feedbackText.GetComponent<Text>().color = Color.red;
            }
            StartCoroutine("continueAfterFeedback");
        }
    }
    void chooseQuestions() // Select randomly question numbers 
    {
        for (int i = 0; i < questionNumbersChosen.Length; i++)
        {
            questionNum = Random.Range(0, questions.Length); // Random choice of a question
            if (numberNotContained(questionNumbersChosen, questionNum))
                questionNumbersChosen[i] = questionNum;
            else 
                i--; // Redo the random selection
        }
        currentQuestionIndex = Random.Range (0, questions.Length); // Order
    }

    bool numberNotContained(int[] numbers, int num) // Securing random questions everytime
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            if (num == numbers[i]) // Question already selected -> NO
                return false;
        }
        return true;
    }

    public void moveToNextQuestion() // Assigns new question using next question number
    {
        assignQuestion(questionNumbersChosen[questionNumbersChosen.Length - 1 - questionsFinished]);
    }

    void displayResults() // Well :)
    {
       switch (numberOfCorrectAnswers)
        {
            case 5:
                resultsText.text = "5/5, Bravo ! Tu es un brave gavux";
                break;
            case 4:
                resultsText.text = "4/5, Mouais, Moyenne basse des joueurs.";
                break;
            case 3:
                resultsText.text = "3/5, Achete toi de quoi reflechir";
                break;
            case 2:
                resultsText.text = "2/5, Ne reviens plus jamais, tu me fais honte";
                break;
            case 1:
                resultsText.text = "1/5, Oh le bordel...";
                break;
            case 0:
                resultsText.text = "0/5 FELICITATIONS !!!";
                break;
            default:
                resultsText.text = "Uh... Something went wrong.";
                print("Problem !!!");
                break;
        } 
    }

    public void restartLevel() // Replay
    {
        Application.LoadLevel(Application.loadedLevelName); 
    }

    IEnumerator continueAfterFeedback() // Pauses 1 sec + move to next question
    {
        allowSelection = false; // No selection possible after picking one answer
        feedbackText.SetActive(true); // OK or KO
        yield return new WaitForSeconds(1.0f); // Wait 1 second
        if (questionsFinished < questionNumbersChosen.Length - 1) // While we are not finished, disp questions
        {
            moveToNextQuestion();
            questionsFinished++;
        }
        else // Load final results
        {
            foreach (GameObject p in TriviaPanels) // Turn off each of the GameObjects, into TriviaPanels
            {
                p.SetActive(false);
            }
            finalResultsPanel.SetActive(true); // Load Final results
            displayResults();
        }
        allowSelection = true;
        feedbackText.SetActive(false); // OK or KO
    }

    void quitGame() // Quits
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    } 
}

