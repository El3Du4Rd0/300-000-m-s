using TMPro;
using UnityEngine;

public enum TipoDato { vidasActual, monedasActual, reboteActual }
public class UI_counter : MonoBehaviour
{
    public TipoDato tipoDato;
    private GameObject jugador;
    private TMP_Text miTexto;
    string dato;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        miTexto = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (jugador == null)
        {
            jugador = GameObject.FindWithTag("Player");
            return;
        }
        ConfigFuerza modelJugador = jugador.GetComponent<ConfigFuerza>();
        switch (tipoDato)
        {
            case TipoDato.vidasActual:
                dato = modelJugador.vidasActual.ToString();
                break;
            case TipoDato.monedasActual:
                dato = modelJugador.monedasActual.ToString();
                break;
            case TipoDato.reboteActual:
                dato = modelJugador.reboteActual.ToString();
                break;
        }

        miTexto.text = dato;
    }
}
