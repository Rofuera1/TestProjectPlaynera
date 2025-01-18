using UnityEngine;

public class SceneryMover : MoverReceiver
{
    public Transform CameraTransform;
    public float Sensivity;

    private Vector3 PrefferablePosition;
    private Vector3 PrefferableRef = Vector3.zero;

    private const float SmoothTime = 0.2f;

    private void Start()
    {
        PrefferablePosition = CameraTransform.position;
    }

    public override void OnStartMove(Vector2 Position)
    {
        PrefferablePosition = CameraTransform.position;
        PrefferableRef = Vector2.zero;
    }

    public override void OnMove(Vector2 Position, Vector2 Delta)
    {
        PrefferablePosition.x -= Delta.x * Sensivity * Time.deltaTime;
    }

    public override void OnEndMove(Vector2 Position)
    {

    }

    private void LateUpdate()
    {
        CameraTransform.position = Vector3.SmoothDamp(CameraTransform.position, PrefferablePosition, ref PrefferableRef, SmoothTime);
    }
}
