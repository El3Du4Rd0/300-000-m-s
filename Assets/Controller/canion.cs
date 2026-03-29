using UnityEngine;
using UnityEngine.InputSystem;

public class canion : MonoBehaviour  
{
    public Sprite dentroDelCanion;
    public Sprite fueraDelCanion;

    public GameObject prefabProyectil; 
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
                    puntoSalida.rotation
                );

                FindAnyObjectByType<ScenaryGenerator>().player = nuevo.transform;

                // Lanzarlo
                ProyectilGameMaster script = nuevo.GetComponent<ProyectilGameMaster>();
                if (script != null)
                {
                    script.Lanzar();
                }
            }
        }
    }
}