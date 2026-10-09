using UnityEngine;
using UnityEngine.UIElements;

public class bola : MonoBehaviour
{
    public GameObject piedra;
    public float primerDelay = 2f;
    public float intervalo = 5f;
    public float vida = 15f;
    public float velocidadInicial = 30f;

    [Header("Dirección")]
    public bool invertirDireccion = false; // Márcalo si sale hacia atrás para corregirlo sin rotar el objeto

    void Start()
    {
        InvokeRepeating("CrearPiedra", primerDelay, intervalo);
    }

    void CrearPiedra()
    {
        GameObject nueva = Instantiate(piedra, transform.position, Quaternion.identity);

        Rigidbody rbPiedra = nueva.GetComponent<Rigidbody>();
        if (rbPiedra != null)
        {
            // Si la casilla está marcada, multiplicamos por -1 para invertir el sentido
            float direccion = invertirDireccion ? -1f : 1f;
            rbPiedra.linearVelocity = transform.forward * velocidadInicial * direccion;
        }

        Destroy(nueva, vida);
    }
}
