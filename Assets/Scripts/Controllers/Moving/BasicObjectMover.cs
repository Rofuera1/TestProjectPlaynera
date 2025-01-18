using UnityEngine;

public class BasicObjectMover : MoverReceiver
{
    public Camera Camera;
    public ObjectMover Mover;
    public Transform PlaneForBasicObjects;

    public bool CanStartMove(Vector2 Position)
    {
        return FindBasicObject(Position);
    }

    public override void OnStartMove(Vector2 Position)
    {
        BasicObject obj = FindBasicObject(Position);

        Mover.StartDraggingObject(obj);
    }

    public override void OnMove(Vector2 Position, Vector2 Delta)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Position);
        worldPosition.z = PlaneForBasicObjects.position.z;

        Mover.DragObjectToPlace(worldPosition);
    }

    public override void OnEndMove(Vector2 Position)
    {
        Vector3 worldPosition = Camera.ScreenToWorldPoint(Position);
        worldPosition.z = PlaneForBasicObjects.position.z;

        Mover.DropCurrentObject(FindNearestSurface(worldPosition));
    }

    private BasicObject FindBasicObject(Vector2 TouchPosition)
    {
        LayerMask mask = LayerMask.GetMask("BasicObjects");
        Vector3 worldPosition = Camera.ScreenToWorldPoint(TouchPosition);
        RaycastHit2D hitInfo = Physics2D.Raycast(worldPosition, Vector2.zero, 1f, mask);

        if (hitInfo)
        {
            BasicObject obj = hitInfo.collider.GetComponent<BasicObject>();
            if (obj) return obj;
        }

        return null;
    }

    private Vector2 FindNearestSurface(Vector2 fromWorldsPosition)
    {
        LayerMask mask = LayerMask.GetMask("Ground") | LayerMask.GetMask("WallObjects");
        RaycastHit2D hitInfo = Physics2D.Raycast(fromWorldsPosition, Vector2.down, 100f, mask);

        if (hitInfo)
        {
            return hitInfo.point;
        }

        return Vector3.zero;
    }
}
