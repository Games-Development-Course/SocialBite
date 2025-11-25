using UnityEngine;

public class DraggablePiece : MonoBehaviour
{
    [Header("Drag Settings")]
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool isDragging = false;
    private SpriteRenderer spriteRenderer;
    private int originalSortingOrder;

    [Header("Piece Settings")]
    public bool isDistractor = false;
    public string targetSlotName;

    [Header("Rotation Settings")]
    public bool enableRotation = true;
    public float rotationStep = 90f;
    public KeyCode rotateClockwiseKey = KeyCode.E;
    public KeyCode rotateCounterClockwiseKey = KeyCode.Q;

    [Header("Group Settings")]
    public int groupID = 1;

    private LevelController levelController;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            originalSortingOrder = spriteRenderer.sortingOrder;

        levelController = FindFirstObjectByType<LevelController>();
    }

    void Update()
    {
        HandleRotationInput();
    }

    void OnMouseDown()
    {
        if (levelController != null && !levelController.CanInteract()) return;

        isDragging = true;

        if (spriteRenderer != null)
            spriteRenderer.sortingLayerName = "Dragging";
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;
    }

    void OnMouseUp()
    {
        if (!isDragging) return;
        isDragging = false;

        if (spriteRenderer != null)
            spriteRenderer.sortingLayerName = "Pieces";

        CheckPlacement();
    }

    void HandleRotationInput()
    {
        if (!enableRotation) return;
        if (!isDragging) return;

        if (Input.GetKeyDown(rotateClockwiseKey))
            RotatePiece(-rotationStep);

        if (Input.GetKeyDown(rotateCounterClockwiseKey))
            RotatePiece(rotationStep);
    }

    void RotatePiece(float angle)
    {
        transform.Rotate(0f, 0f, angle);
    }

    void CheckPlacement()
    {
        // בדיקה מוקדמת לפי קבוצה
        if (!PieceGroupManager.Instance.CanPlace(groupID))
        {
            if (levelController != null)
                levelController.ShowFeedback("אי אפשר לשים שני חלקים מאותה קבוצה!", false);

            ReturnToStart();
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.7f);
        bool foundSlot = false;

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Slot"))
            {
                Slot slot = collider.GetComponent<Slot>();

                if (slot != null && slot.slotName == targetSlotName && !isDistractor)
                {
                    SnapToSlot(slot.transform.position);
                    foundSlot = true;
                    break;
                }

                if (!isDistractor && levelController != null)
                    levelController.ShowFeedback("נסה לשים במקום אחר", false);
            }
        }

        if (!foundSlot)
        {
            ReturnToStart();
        }
    }
    void SnapToSlot(Vector3 slotPos)
    {
        // בדיקת קבוצה
        if (!PieceGroupManager.Instance.CanPlace(groupID))
        {
            if (levelController != null)
                levelController.ShowFeedback("אי אפשר לשים שני חלקים מאותה קבוצה!", false);

            ReturnToStart();
            return;
        }

        slotPos.z = 0;
        transform.position = slotPos;

        var col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        // רישום קבוצה
        PieceGroupManager.Instance.RegisterPlacement(groupID);

        // 🔥 כאן נוסיף:
        ArrowTurnManager.Instance.SwitchTurn();   // ← מדלג בין +540 ל־540- רק בהנחה מוצלחת
        PuzzleCompletionManager.Instance.RegisterPiecePlaced();

        if (levelController != null)
        {
            levelController.ShowFeedback("כל הכבוד!", true);
            levelController.OnPiecePlaced(this.name);
        }
    }


    public void ReturnToStart()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        var col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = true;
    }
}