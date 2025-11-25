using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleCompletionManager : MonoBehaviour
{
    public static PuzzleCompletionManager Instance;

    [Header("Scene To Load When Done")]
    public string nextSceneName;

    [Header("Total Pieces In Puzzle")]
    public int totalPieces = 4;

    private int placedCount = 0;

    void Awake()
    {
        Instance = this;
    }

    public void RegisterPiecePlaced()
    {
        placedCount++;

        if (placedCount >= totalPieces)
        {
            Debug.Log("All pieces placed! Loading scene: " + nextSceneName);

            if (!string.IsNullOrEmpty(nextSceneName))
                SceneManager.LoadScene(nextSceneName);
            else
                Debug.LogError("PuzzleCompletionManager: nextSceneName is EMPTY!");
        }
    }
}
