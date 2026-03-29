using UnityEngine;

public class Win : MonoBehaviour
{
    public ConfigFuerza config;
    public TMPro.TextMeshProUGUI textoVelocidad;

    private bool yaGano = false;

    void Start()
    {
        textoVelocidad.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!yaGano && config.velocidadActual >= 3000f)
        {
            yaGano = true;

            Debug.Log("Ganaste");
            textoVelocidad.gameObject.SetActive(true);
        }
    }
}
