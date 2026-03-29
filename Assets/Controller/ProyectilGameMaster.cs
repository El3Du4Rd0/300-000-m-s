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

	
	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.bodyType = RigidbodyType2D.Static;

		// Eliminamos fricción y gravedad global de Unity para usar la nuestra
		rb.linearDamping = 0f;
		rb.angularDamping = 0f;
		rb.gravityScale = 0f; // Importante: ponemos esto en 0 para controlar la gravedad nosotros
	}

	public void Lanzar()
	{
		//transform.rotation = Quaternion.Euler(0, 0, 52.394f);
		transform.rotation = Quaternion.Euler(0, 0, 35f);
		rb.bodyType = RigidbodyType2D.Dynamic;

		Vector2 direccion = transform.right;
		rb.linearVelocity = direccion * config.velocidadInicial;
	}

	void FixedUpdate()
	{
		// 1. Aplicamos nuestra gravedad manualmente
		// Multiplicamos por FixedDeltaTime para que sea constante independientemente de los FPS
		rb.linearVelocity += Vector2.down * gravedadPersonalizada * Time.fixedDeltaTime;

		velocidadGravidad = Vector2.down * gravedadPersonalizada * Time.fixedDeltaTime;

		// 2. Guardamos la velocidad antes del impacto
		ultimaVelocidad = rb.linearVelocity;

		// 3. Mostramos la velocidad en el inspector (solo lectura visual)
		velocidadActualVisual = rb.linearVelocity;
		velocidadActualMagnitud = rb.linearVelocity.magnitude;
	}

	private void OnCollisionEnter2D(Collision2D colision)
	{
		var normal = colision.contacts[0].normal;
		var direccionRebote = Vector2.Reflect(ultimaVelocidad.normalized, normal);
		dirreccionReboteDebu = direccionRebote;

		// Mantenemos la magnitud que traía para que el rebote sea perfecto
		rb.linearVelocity = new Vector2(direccionRebote.x - (ultimaVelocidad.magnitude * multiplicadorFriccion), direccionRebote.y * (ultimaVelocidad.magnitude * multiplicadorRebote) - direccionRebote.y * 67);
	}
}