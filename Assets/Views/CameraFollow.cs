using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    void LateUpdate()
    {
        // Solo sigue la posición
        transform.position = player.position + offset;

        // Mantiene la rotación fija
        transform.rotation = Quaternion.identity;
    }
}
