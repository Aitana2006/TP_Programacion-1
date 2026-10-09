using UnityEngine;

/// <summary>
/// Poner este script en la pelota.
/// Guarda la posicion inicial y, cuando cae por debajo de 'limiteCaida',
/// vuelve a su posicion de inicio y cae de nuevo, infinitamente.
/// </summary>
public class pelolaRespawn : MonoBehaviour
{
    [Header("Respawn")]
    [Tooltip("Si la pelota baja de esta Y, vuelve a su posicion inicial.\n" +
             "Fijate en la escena que Y tiene el suelo y pone un valor menor.")]
    [SerializeField] private float limiteCaida = -5f;

    [Tooltip("Tiempo de espera antes de reaparecer (segundos)")]
    [SerializeField] private float tiempoEspera = 0.3f;

    private Vector3    posicionInicial;
    private Quaternion rotacionInicial;
    private Rigidbody  rb;
    private bool       reiniciando = false;

    void Start()
    {
        posicionInicial = transform.position;
        rotacionInicial = transform.rotation;
        rb = GetComponent<Rigidbody>();

        if (rb == null)
            Debug.LogWarning("[pelolaRespawn] Este objeto no tiene Rigidbody. Agregale uno.", this);
    }

    void Update()
    {
        // Si esta siendo sostenido (kinematic = true), no hacemos nada
        if (rb != null && rb.isKinematic) return;

        if (!reiniciando && transform.position.y < limiteCaida)
        {
            reiniciando = true;
            Invoke(nameof(Reiniciar), tiempoEspera);
        }
    }

    public void Reiniciar()
    {
        CancelInvoke(nameof(Reiniciar)); // Por si se llamo mas de una vez

        if (rb != null)
        {
            rb.isKinematic     = false;
            rb.useGravity      = true;
            rb.linearVelocity        = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        transform.position = posicionInicial;
        transform.rotation = rotacionInicial;

        reiniciando = false;

        Debug.Log("[pelolaRespawn] Pelota reiniciada en " + posicionInicial);
    }
}
