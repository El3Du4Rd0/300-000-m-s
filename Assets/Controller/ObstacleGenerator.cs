using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    public GameObject obstaculoPrefab;
    public Sprite[] spritesObstaculos;
    public float distanciaFrente = 15f;
    public float intervalo = 2f;
    public float tiempoVida = 5f;

    private Transform player;
    private List<GameObject> obstaculosActivos = new List<GameObject>();
    private bool generandoObstaculos = false;

    void Update()
    {
        // Mientras no tengamos player, intentar encontrarlo cada frame
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                // Iniciar la generación solo cuando el player ya existe
                if (!generandoObstaculos)
                {
                    generandoObstaculos = true;
                    StartCoroutine(GenerarObstaculos());
                }
            }
        }
    }

    IEnumerator GenerarObstaculos()
    {
        while (true)
        {
            CrearObstaculo();
            yield return new WaitForSeconds(intervalo);
        }
    }

    void CrearObstaculo()
    {
        if (player == null) return;

        Vector3 posicion = new Vector3(
            player.position.x + distanciaFrente,
            Random.Range(-2f, 2f),
            0
        );

        GameObject obs = Instantiate(obstaculoPrefab, posicion, Quaternion.identity);
        SpriteRenderer sr = obs.GetComponent<SpriteRenderer>();
        if (sr != null && spritesObstaculos.Length > 0)
        {
            sr.sprite = spritesObstaculos[Random.Range(0, spritesObstaculos.Length)];
        }

        obstaculosActivos.Add(obs);
        Destroy(obs, tiempoVida);
    }
}