using UnityEngine;

public class MoverController : MonoBehaviour
{
    public InputController Input;
    public MoverStrategy MoverStrategy;
    public EdgeMoverAssistant EdgeAssistant;

    private MoverReceiver CurrentMover;

    private void OnTouchStarted(Vector2 CurrentTouchPosition)
    {
        CurrentMover = MoverStrategy.ChooseMover(CurrentTouchPosition);

        CurrentMover?.OnStartMove(CurrentTouchPosition);
    }
    private void OnTouchingDelta(Vector2 CurrentTouchPosition, Vector2 CurrentDelta)
    {
        CurrentMover?.OnMove(CurrentTouchPosition, CurrentDelta);
        MoverStrategy.TryAssistOnEdge(CurrentTouchPosition);
    }
    private void OnTouchEnded(Vector2 CurrentTouchPosition)
    {
        CurrentMover?.OnEndMove(CurrentTouchPosition);
        MoverStrategy.EndEdgeAssisting(); // Ничего лучше не придумал
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        Input.OnTouchStarted += OnTouchStarted;
        Input.OnTouching += OnTouchingDelta;
        Input.OnTouchEnded += OnTouchEnded;
    }
    private void UnsubscribeFromEvents()
    {
        Input.OnTouchStarted -= OnTouchStarted;
        Input.OnTouching -= OnTouchingDelta;
        Input.OnTouchEnded -= OnTouchEnded;
    }
}