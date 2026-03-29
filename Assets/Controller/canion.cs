using UnityEngine;
using UnityEngine.InputSystem;

public class canion : MonoBehaviour  
{
    public Sprite dentroDelCanion;
    public Sprite fueraDelCanion;
        
    public GameObject prefabProyectil;
    public GameObject prefabCanion;
    public Transform puntoSalida;

    private SpriteRenderer sr;
    private bool yaDisparo = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = dentroDelCanion;
    }

    void Update()
    {
        if ( (Keyboard.current.spaceKey.wasPressedThisFrame ||
            Mouse.current.leftButton.wasPressedThisFrame) && !yaDisparo)
        {
            yaDisparo = true;

            // Cambiar sprite del cañón
            sr.sprite = fueraDelCanion;

            // Instanciar proyectil
            if (prefabProyectil != null && puntoSalida != null)
            {
                GameObject nuevo = Instantiate(
                    prefabProyectil,
                    puntoSalida.position,
                    Quaternion.Euler(0, 0, 45f)
                );

                FindAnyObjectByType<ScenaryGenerator>().player = nuevo.transform;

                // Lanzarlo
                ProyectilGameMaster script = nuevo.GetComponent<ProyectilGameMaster>();
                if (script != null)
                {
                    script.canon = this;
                    script.Lanzar();
                }
            }
        }
    }

    public void JugadorSeDetuvo(Transform jugador)
    {
        // Crear nuevo cañón en la posición del jugador
        Instantiate(prefabCanion, jugador.position + new Vector3(0, 2f, 0), Quaternion.identity);

        // Destruir este cañón
        Destroy(gameObject);

        // Destruir al jugador
        Destroy(jugador.gameObject);
    }
}