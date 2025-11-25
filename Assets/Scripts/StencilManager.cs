using UnityEngine;
using UnityEngine.UI;

public class StencilManager : MonoBehaviour
{
    [Header("Stencil UI Image")]
    public Image stencilImage;

    [Header("Puzzle Slots")]
    public Transform[] slots;

    /// <summary>
    /// נקרא כשהפיסה הונחה בהצלחה.
    /// משכפל רק את X,Y של הסלוט ולא משנה את ה-Z.
    /// </summary>
    public void OnPiecePlaced(int slotIndex)
    {
        if (stencilImage == null)
        {
            Debug.LogError("StencilManager: stencilImage is NULL");
            return;
        }

        if (slotIndex < 0 || slotIndex >= slots.Length)
        {
            Debug.LogError("StencilManager: slot index out of range");
            return;
        }

        Transform target = slots[slotIndex];

        // שומרים את ה-Z המקורי כדי שלא יעלם
        float originalZ = stencilImage.rectTransform.position.z;

        // קביעת מיקום חדש — רק X,Y
        stencilImage.rectTransform.position = new Vector3(
            target.position.x,
            target.position.y,
            originalZ
        );

        // קביעת רוטציה — רק בזווית 2D (Z-axis)
        stencilImage.rectTransform.rotation = Quaternion.Euler(
            0,
            0,
            target.eulerAngles.z
        );
    }
}
