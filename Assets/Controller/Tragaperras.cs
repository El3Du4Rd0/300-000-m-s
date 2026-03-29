using System;
using System.Collections;
using UnityEngine;

public class Tragaperras : MonoBehaviour
{
    private string[] opciones = { "Null", "Bomba", "Corazon", "Moneda", "Silla", "Vel" };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Iniciar a rodar");
            StartCoroutine(Tirar(2, result =>
            {
                Debug.Log($"Resultado: {result}");
            }));
        }
    }

    IEnumerator Tirar(int seg, Action<int> result)
    {
        int index = UnityEngine.Random.Range(0, opciones.Length);
        yield return new WaitForSeconds(seg);
        callback(opciones[index]);
    }
}
