using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class QuizStep
{
    public Sprite image;              
    public string[] answers = new string[4]; 
    public int correctIndex;         
}

public class QuizManager : MonoBehaviour
{
    public Image questionImage;           
    public Button[] answerButtons;        
    public TextMeshProUGUI tryAgainText;  

    public QuizStep[] steps;              
    public int maxErrors = 5;             

    private int currentStep = 0;
    private int errors = 0;

    void Start()
    {
        if (tryAgainText != null)
            tryAgainText.gameObject.SetActive(false);

        LoadStep(currentStep);
    }

    void LoadStep(int stepIndex)
    {
        QuizStep step = steps[stepIndex];

        questionImage.sprite = step.image;

        // define button texts and listeners
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;
            answerButtons[i]
                .GetComponentInChildren<TextMeshProUGUI>()
                .text = step.answers[i];

            answerButtons[i].onClick.RemoveAllListeners();

            answerButtons[i].onClick.AddListener(() =>
            {
                OnAnswerPressed(index);
            });
        }
    }

    void OnAnswerPressed(int index)
    {
        QuizStep step = steps[currentStep];

        if (index == step.correctIndex)
        {
            currentStep++;

            if (currentStep >= steps.Length)
            {
                SceneManager.LoadScene("ShapieScene");
                return;
            }

            LoadStep(currentStep);
        }
        else
        {
            errors++;

            tryAgainText.gameObject.SetActive(true);
            CancelInvoke(nameof(HideTryAgain));
            Invoke(nameof(HideTryAgain), 1.0f);

            if (errors >= maxErrors)
            {
                SceneManager.LoadScene("GameOver");
            }
        }
    }

    void HideTryAgain()
    {
        tryAgainText.gameObject.SetActive(false);
    }
}