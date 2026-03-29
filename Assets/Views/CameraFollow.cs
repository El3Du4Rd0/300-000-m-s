using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    void Start()
    {
        BuscarJugador();
    }

    void BuscarJugador()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }
    void LateUpdate()
    {
        if (player == null)
        {
            BuscarJugador();
            return;
        }
        // Solo sigue la posici�n
        transform.position = player.position + offset;

        // Mantiene la rotaci�n fija
        transform.rotation = Quaternion.identity;
    }
}
