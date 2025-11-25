using UnityEngine;
using UnityEngine.UI;

public class ArrowTurnManager : MonoBehaviour
{
    public Image arrowImage;

    private RectTransform rt;

    private bool onLeft = true;  // מתחילים בצד שמאל (-270)

    public static ArrowTurnManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        rt = arrowImage.rectTransform;

        // מוודאים שהחץ מתחיל ב-270- כמו שביקשת
        rt.anchoredPosition = new Vector2(-270f, rt.anchoredPosition.y);
        onLeft = true;
    }

    public void SwitchTurn()
    {
        // אם כרגע בצד שמאל → הולכים לימין
        if (onLeft)
        {
            rt.anchoredPosition = new Vector2(270f, rt.anchoredPosition.y);
        }
        else
        {
            // אם בצד ימין → הולכים לשמאל
            rt.anchoredPosition = new Vector2(-270f, rt.anchoredPosition.y);
        }

        // מחליפים לפעם הבאה
        onLeft = !onLeft;
    }
}
