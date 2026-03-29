using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Anuncio : MonoBehaviour
{
    public Tragaperras tragaperras;
    private TMP_Text miTexto;
    private bool displaying = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        miTexto = GetComponent<TMP_Text>();
        miTexto.text = "";
    }

    private void OnEnable()
    {
        Tragaperras.mensaje += actualizarMensaje;
    }

    void actualizarMensaje(string textoNuevo)
    {
        StartCoroutine(actualizar(textoNuevo));
    }

    IEnumerator actualizar(string textoNuevo)
    {
        if (displaying)
        {
            displaying = false;
            miTexto.text = textoNuevo;
            yield return new WaitForSeconds(2f);
            miTexto.text = "";
            displaying = true;
        }
    }
}
