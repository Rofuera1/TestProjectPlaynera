using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    private BasicObject CurrentlyDragging;
    private Vector3 PlannedPosition;
    private Vector3 PlannedPositionRef = Vector3.zero;

    private const float Uplift_Constant = 1f;
    private const float DraggingSmoothTime = 0.2f;
    private const float MinimDroppingTime = 0.2f;
    private const float DroppingSpeed = 8f;
    private bool Dragging = false;

    public void StartDraggingObject(BasicObject Obj)
    {
        CurrentlyDragging = Obj;

        PlannedPosition = Obj.transform.position + Vector3.up * Uplift_Constant;
        PlannedPositionRef = Vector3.zero;

        StartCoroutine(DraggingCoroutine());
    }

    public void DragObjectToPlace(Vector3 GlobalPosition)
    {
        PlannedPosition = GlobalPosition + Vector3.up * Uplift_Constant;
    }

    public void DropCurrentObject(Vector3 DropAt)
    {
        Dragging = false;

        if (!CurrentlyDragging)
            return;

        float timeForDropping = Vector2.Distance(CurrentlyDragging.transform.position, DropAt) / DroppingSpeed;
        timeForDropping = Mathf.Max(timeForDropping, MinimDroppingTime);

        CurrentlyDragging.transform.DOMove(DropAt, timeForDropping).SetEase(Ease.OutCubic);   
        CurrentlyDragging = null;
    }

    private IEnumerator DraggingCoroutine()
    {
        Dragging = true;

        while(Dragging)
        {
            CurrentlyDragging.transform.position = Vector3.SmoothDamp(CurrentlyDragging.transform.position, PlannedPosition, ref PlannedPositionRef, DraggingSmoothTime);

            yield return null;
        }
    }
}
