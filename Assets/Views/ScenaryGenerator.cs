using System.Collections.Generic;
using UnityEngine;

public class ScenaryGenerator : MonoBehaviour
{
    public Transform player;
    public GameObject[] prefabsBloques;

    public float longitudBloque = 10f;
    public int bloquesIniciales = 5;

    private float siguientePosX = 0f;
    private List<GameObject> bloquesActivos = new List<GameObject>();

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Generar bloques iniciales
        for (int i = 0; i < bloquesIniciales; i++)
        {
            GenerarBloque();
        }
    }

    void Update()
    {
        // Si el jugador se acerca al final, generamos otro bloque
        if (player.position.x + (bloquesIniciales * longitudBloque) > siguientePosX)
        {
            GenerarBloque();
            EliminarBloqueViejo();
        }
    }

    void GenerarBloque()
    {
        int index = Random.Range(0, prefabsBloques.Length);
        GameObject bloque = Instantiate(prefabsBloques[index], new Vector3(siguientePosX, 0, 0), Quaternion.identity);

        bloquesActivos.Add(bloque);
        siguientePosX += longitudBloque;
    }

    void EliminarBloqueViejo()
    {
        if (bloquesActivos.Count == 0) return;

        GameObject bloque = bloquesActivos[0];

        // Si el bloque está muy atrás del player
        if (player.position.x - bloque.transform.position.x > longitudBloque * 2)
        {
            Destroy(bloque);
            bloquesActivos.RemoveAt(0);
        }
    }
}
