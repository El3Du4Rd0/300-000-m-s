using System;
using UnityEngine;

public class ProyectilGameMaster : MonoBehaviour
{
	public GameObject apendices;
	public ConfigFuerza config;

	[Header("Física Personalizada")]
	public float gravedadPersonalizada = 9.81f; // Variable pública para la gravedad

	public float multiplicadorRebote;
	public float multiplicadorFriccion;

	public Vector2 velocidadActualVisual;        // Variable pública para ver la velocidad
	public float velocidadActualMagnitud;        // Variable pública para ver la velocidad
	public Vector2 velocidadGravidad;
	private Vector2 ultimaVelocidad;
	public Rigidbody2D rb;

	public Vector2 dirreccionReboteDebu;

    private bool yaSeDetuvo = false;
    public canion canon;

	public float suavizadoFrenado = 0.1f;


	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.bodyType = RigidbodyType2D.Static;

		// Eliminamos fricci�n y gravedad global de Unity para usar la nuestra
		rb.linearDamping = 0f;
		rb.angularDamping = 0f;
		rb.gravityScale = 0f; // Importante: ponemos esto en 0 para controlar la gravedad nosotros
	}

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    public void Lanzar()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        transform.rotation = Quaternion.identity;

        Vector2 direccion = new Vector2(1, 1).normalized;
        rb.AddForce(direccion * config.velocidadInicial, ForceMode2D.Impulse);
    }

    void Update()
    {
        if (!yaSeDetuvo && rb.linearVelocity.magnitude < 0.1f)
        {
            yaSeDetuvo = true;

            // Avisar al cañón
            canon.JugadorSeDetuvo(transform);
        }
    }

    void FixedUpdate()
	{
		// 1. Aplicamos nuestra gravedad manualmente
		// Multiplicamos por FixedDeltaTime para que sea constante independientemente de los FPS
		rb.linearVelocity += Vector2.down * gravedadPersonalizada * Time.fixedDeltaTime;

		velocidadGravidad = Vector2.down * gravedadPersonalizada * Time.fixedDeltaTime;

		// 2. Guardamos la velocidad antes del impacto
		ultimaVelocidad = rb.linearVelocity;

		if (config.velocidaMaximaActual < rb.linearVelocity.x)
		{
			Debug.Log("Limitando Velocidad");
			float nuevaMagnitud = Mathf.Lerp(rb.linearVelocity.magnitude, config.velocidaMaximaActual, suavizadoFrenado);

			// Aplicamos la nueva velocidad manteniendo la dirección original
			rb.linearVelocity = rb.linearVelocity.normalized * nuevaMagnitud;
		}

		// 3. Mostramos la velocidad en el inspector (solo lectura visual)
		velocidadActualVisual = rb.linearVelocity;
		velocidadActualMagnitud = rb.linearVelocity.magnitude;
	}

	private void OnCollisionEnter2D(Collision2D colision)
	{
		var normal = colision.contacts[0].normal;
		var direccionRebote = Vector2.Reflect(ultimaVelocidad.normalized, normal);
		dirreccionReboteDebu = direccionRebote;
		if (config.reboteActual > 0)
		{
			rb.linearVelocity = new Vector2(direccionRebote.x, direccionRebote.y * (ultimaVelocidad.magnitude * multiplicadorRebote) - direccionRebote.y * 67);
		}
		// Mantenemos la magnitud que tra�a para que el rebote sea perfecto
		rb.linearVelocity = new Vector2(direccionRebote.x - (ultimaVelocidad.magnitude * multiplicadorFriccion), direccionRebote.y * (ultimaVelocidad.magnitude * multiplicadorRebote) - direccionRebote.y * 67);
	}


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstaculo"))
        {
            config.vidasActual--;
            config.velocidadActual = config.velocidadActual / 2;
        }
    }
	public void DashVelocidad(int veces)
	{

		Vector2 direccion = new Vector2(1, 0).normalized;
		rb.AddForce(direccion * (config.velocidadActual * 1f * (1 + (veces * 0.33f))), ForceMode2D.Impulse);
	}
	public void DashBomba(int veces)
	{
		Vector2 direccion = new Vector2(1, 1).normalized;
		rb.AddForce(new Vector2(direccion.x * (config.velocidadActual * 1f * (1 + (veces * 0.33f))), 
			direccion.y * (config.velocidadActual * 1.25f * (1 + (veces * 0.33f)))), 
			ForceMode2D.Impulse);
	}

}