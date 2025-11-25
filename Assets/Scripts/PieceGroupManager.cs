using UnityEngine;

public class PieceGroupManager : MonoBehaviour
{
    public static PieceGroupManager Instance;

    private int lastGroupPlaced = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // מונע כפילויות
            return;
        }

        Instance = this;
    }

    public bool CanPlace(int groupID)
    {
        Debug.Log($"[CanPlace] last={lastGroupPlaced}, request={groupID}");

        if (lastGroupPlaced == 0)
            return true;

        return groupID != lastGroupPlaced;
    }

    public void RegisterPlacement(int groupID)
    {
        lastGroupPlaced = groupID;
        Debug.Log("[RegisterPlacement] group=" + groupID);
    }
}
