using UnityEngine;

public class tomarYsoltar : MonoBehaviour
{
    [Header("Configuracion")]
    public GameObject puntoMAn;
    [SerializeField] private float distanciaMaxima = 3.5f;

    private GameObject pickObjet = null;
    private Camera cam;

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
        // Soltar con la tecla 'R' (o 'E' si ya lo tienes agarrado)
        if (pickObjet != null)
        {
            if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.E))
            {
                SoltarObjeto();
            }
            return;
        }

        // Tomar con la tecla 'E'
        if (Input.GetKeyDown(KeyCode.E) && pickObjet == null)
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

        // Metodo 2: Por cercania (si estas cerca en el suelo y presionas E)
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

        // Si encontro el objeto, lo toma
        if (objetoEncontrado != null)
        {
            TomarObjeto(objetoEncontrado);
        }
    }

    private void TomarObjeto(GameObject obj)
    {
        pickObjet = obj;

        // Desactivar fisicas mientras lo llevamos
        Rigidbody rb = pickObjet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        // Desactivar colisionador para que no empuje al jugador
        Collider col = pickObjet.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        // Ubicar en la mano manteniendo la escala correcta
        pickObjet.transform.SetParent(puntoMAn.transform, true);
        pickObjet.transform.position = puntoMAn.transform.position;
        pickObjet.transform.localRotation = Quaternion.identity;
    }

    private void SoltarObjeto()
    {
        if (pickObjet == null) return;

        // Reactivar colisionador
        Collider col = pickObjet.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        // Reactivar gravedad y fisicas
        Rigidbody rb = pickObjet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        // Desvincular de la mano
        pickObjet.transform.SetParent(null);
        pickObjet = null;
    }
}
