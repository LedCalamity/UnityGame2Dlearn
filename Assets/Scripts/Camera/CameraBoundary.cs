using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CameraBoundary : MonoBehaviour
{
    public Collider2D BoundaryCollider => GetComponent<Collider2D>();
}
