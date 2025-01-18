using UnityEngine;

public class MoverStrategy : MonoBehaviour
{
    public BasicObjectMover ObjectsMover;
    public SceneryMover SceneMover;
    public EdgeMoverAssistant EdgeAssistant;

    private MoverReceiver CurrentMover;

    public MoverReceiver ChooseMover(Vector2 Position)
    {
        return CurrentMover = ObjectsMover.CanStartMove(Position) ? ObjectsMover : SceneMover as MoverReceiver;
    }

    public void TryAssistOnEdge(Vector2 Position)
    {
        if ((CurrentMover is BasicObjectMover) && EdgeAssistant.OnTouched(Position))
        {
            EdgeAssistant.OnMove += HandleEdgeMovement;
            EdgeAssistant.OnEndMove += HandleEdgeEndMovement;
        }
    }

    public void EndEdgeAssisting() =>
        EdgeAssistant.OnEndedTouch();

    private void HandleEdgeMovement(Vector2 Touch, Vector2 Delta)
    {
        ObjectsMover.OnMove(Touch, Delta); 
        SceneMover.OnMove(Touch, Delta);
    }

    private void HandleEdgeEndMovement()
    {
        EdgeAssistant.OnMove -= HandleEdgeMovement;
        EdgeAssistant.OnEndMove -= HandleEdgeEndMovement;
    }
}
