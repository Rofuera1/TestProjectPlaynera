using UnityEngine;

public abstract class MoverReceiver : MonoBehaviour
{
    public abstract void OnStartMove(Vector2 Position);
    public abstract void OnMove(Vector2 Position, Vector2 Delta);
    public abstract void OnEndMove(Vector2 Position);
}
