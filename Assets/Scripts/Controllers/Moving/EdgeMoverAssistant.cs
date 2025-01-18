using System.Collections;
using UnityEngine;

public class EdgeMoverAssistant : MonoBehaviour
{
    public System.Action<Vector2, Vector2> OnMove;
    public System.Action OnEndMove;

    private Vector2 ScreenDimensions;

    private Vector2 StartTouchPosition;
    private Vector2 CurrentTouchPosition;

    private bool ShouldAssist;
    private bool IsAssisting;

    private const float THRESHOLD_BEFORE_ASSISTING = 0.5f;

    private void Awake()
    {
        ScreenDimensions = new Vector2(Screen.width, Screen.height);
    }

    public bool OnTouched(Vector2 Position)
    {
        CurrentTouchPosition = Position;

        if (!IsTouchInScreenEdge(Position))
        {
            StopAssisting();
            return false;
        }
        else if (!IsAssisting)
        {
            StartAssisting(Position);
            return true;
        }
        return false;
    }

    public void OnEndedTouch()
    {
        StopAssisting();
    }

    private void StopAssisting()
    {
        ShouldAssist = false;

        OnEndMove?.Invoke();
    }

    private void StartAssisting(Vector2 Position)
    {
        ShouldAssist = true;
        StartTouchPosition = Position;

        StartCoroutine(ThresholdForAssisting());
    }

    private IEnumerator ThresholdForAssisting()
    {
        IsAssisting = true;
        float ThresholdValue = 0f;

        while (ShouldAssist && ThresholdValue < THRESHOLD_BEFORE_ASSISTING)
        {
            ThresholdValue += Time.deltaTime;

            yield return null;
        }

        Vector2 Delta = ScreenEdgeSide(StartTouchPosition);

        while (ShouldAssist)
        {
            OnMove?.Invoke(CurrentTouchPosition, -Delta);

            yield return null;
        }

        IsAssisting = false;
    }

    private Vector2 ScreenEdgeSide(Vector2 Position)
    {
        if (Position.x < ScreenDimensions.x * 0.15f) return Vector2.left;
        else if (Position.x > ScreenDimensions.x * 0.85f) return Vector2.right;
        else return Vector2.zero;
    }

    private bool IsTouchInScreenEdge(Vector2 Position)
    {
        return (Position.x < ScreenDimensions.x * 0.15f || Position.x > ScreenDimensions.x * 0.85f);
    }
}
