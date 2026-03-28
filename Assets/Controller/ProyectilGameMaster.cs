using UnityEngine;
using UnityEngine.InputSystem;

public class ProyectilGameMaster : MonoBehaviour
{
    public ConfigFuerza config;

    private Vector2 ultimaVelocidad;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		rb.bodyType = RigidbodyType2D.Static;
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
			rb.bodyType = RigidbodyType2D.Dynamic;
            Vector2 direccion = transform.right;
            rb.AddForce(direccion * config.velocidadInicial, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        ultimaVelocidad = rb.linearVelocity;
    }

    private void OnCollisionEnter2D(Collision2D colision)
    {
        var normal = colision.contacts[0].normal;
        var direccionRebote = Vector2.Reflect(ultimaVelocidad.normalized, normal);

        rb.linearVelocity = direccionRebote * ultimaVelocidad.magnitude;

        if (colision.gameObject.CompareTag("Suelo"))
        {
            Debug.Log("Impacto en suelo");
        }
    }
}