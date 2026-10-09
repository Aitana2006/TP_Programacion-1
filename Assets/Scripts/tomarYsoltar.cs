using UnityEngine;

public class tomarYsoltar : MonoBehaviour
{
    [Header("Configuracion")]
    public GameObject puntoMAn;
    [SerializeField] private float distanciaMaxima = 3.5f;

    private GameObject pickObjet = null;
    private Camera cam;

    // Guardamos el estado original del Rigidbody
    private bool rbTeniaGravedad;
    private bool rbEraKinematic;

    void Start()
    {
        // 1. Busca la camara del jugador
        cam = GetComponentInParent<CharacterController>()?.GetComponentInChildren<Camera>();
        if (cam == null) cam = Camera.main;
        if (cam == null) cam = FindFirstObjectByType<Camera>();

        // 2. Si puntoMAn no esta asignado, busca el hijo llamado puntoMAn o usa este objeto
        if (puntoMAn == null)
        {
            Transform hijo = transform.Find("puntoMAn");
            puntoMAn = (hijo != null) ? hijo.gameObject : this.gameObject;
        }
    }

    void Update()
    {
        // Soltar con la tecla 'R' o 'E'
        if (pickObjet != null)
        {
            if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.E))
            {
                SoltarObjeto();
            }

            // Mantener el objeto en la mano mientras lo llevamos
            if (pickObjet != null)
            {
                pickObjet.transform.position = puntoMAn.transform.position;
                pickObjet.transform.rotation = puntoMAn.transform.rotation;
            }
            return;
        }

        // Tomar con la tecla 'E'
        if (Input.GetKeyDown(KeyCode.E))
        {
            IntentarTomarObjeto();
        }
    }

    private void IntentarTomarObjeto()
    {
        GameObject objetoEncontrado = null;

        // Metodo 1: Por la mirada (Raycast desde el centro de la pantalla)
        if (cam != null)
        {
            Ray rayo = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if (Physics.Raycast(rayo, out RaycastHit hit, distanciaMaxima))
            {
                if (hit.collider.CompareTag("objeto") || hit.collider.CompareTag("Objeto"))
                {
                    objetoEncontrado = hit.collider.gameObject;
                }
            }
        }

        // Metodo 2: Por cercania (si estas cerca y presionas E)
        if (objetoEncontrado == null)
        {
            Collider[] cercanos = Physics.OverlapSphere(transform.position, distanciaMaxima);
            foreach (Collider c in cercanos)
            {
                if (c.CompareTag("objeto") || c.CompareTag("Objeto"))
                {
                    objetoEncontrado = c.gameObject;
                    break;
                }
            }
        }

        if (objetoEncontrado != null)
        {
            TomarObjeto(objetoEncontrado);
        }
    }

    private void TomarObjeto(GameObject obj)
    {
        pickObjet = obj;

        Rigidbody rb = pickObjet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Guardamos el estado original para restaurarlo al soltar
            rbTeniaGravedad = rb.useGravity;
            rbEraKinematic  = rb.isKinematic;

            // Detenemos completamente el objeto antes de tomarlo
            rb.linearVelocity        = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity      = false;
            rb.isKinematic     = true;
        }

        // Desactivar colisionador para que no choque con el jugador
        Collider col = pickObjet.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // Desvinculamos del padre anterior y reposicionamos en la mano
        pickObjet.transform.SetParent(null);
        pickObjet.transform.position = puntoMAn.transform.position;
        pickObjet.transform.rotation = puntoMAn.transform.rotation;
    }

    private void SoltarObjeto()
    {
        if (pickObjet == null) return;

        // Reactivar colisionador
        Collider col = pickObjet.GetComponent<Collider>();
        if (col != null) col.enabled = true;

        Rigidbody rb = pickObjet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Restaurar estado original y resetear velocidad para que caiga limpio
            rb.isKinematic     = rbEraKinematic;
            rb.useGravity      = rbTeniaGravedad;
            rb.linearVelocity        = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        pickObjet.transform.SetParent(null);
        pickObjet = null;
    }
}
