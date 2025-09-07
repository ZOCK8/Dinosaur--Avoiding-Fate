using UnityEngine;
using System.Collections;

public class FlyingObjectController : MonoBehaviour
{
    [Header("Flug Einstellungen")]
    public Transform punktA;
    public Transform punktB;
    public float flugGeschwindigkeit = 5f;
    
    [Header("Spawn Einstellungen")]
    public GameObject objektZumSpawnen;
    public GameObject DropItemContainer;
    
    [Header("Wartezeit Einstellungen")]
    public float minWartezeit = 1f;
    public float maxWartezeit = 5f;
    
    private bool fliegtZuB = true;
    private bool hatGespawnt = false;
    private bool wartet = false;
    private float zufaelligeSpawnDistance;
    public Tutorial tutorial;
    
    void Start()
    {
        // Startposition auf Punkt A setzen
        if (punktA != null)
        {
            transform.position = punktA.position;
        }
        
        // Zufällige Spawn-Distance für den ersten Flug festlegen
        SetzeZufaelligeSpawnDistance();
        
        // Warnung falls Punkte nicht gesetzt sind
        if (punktA == null || punktB == null)
        {
            Debug.LogWarning("Punkt A oder Punkt B sind nicht zugewiesen!");
        }
    }
    
    void Update()
    {
        if (punktA == null || punktB == null || wartet)
            return;
            
        // Fliegt immer nur zu Punkt B
        if (fliegtZuB)
        {
            FliegeZuPunkt(punktB);
        }
    }
    
    void FliegeZuPunkt(Transform zielPunkt)
    {
        // Bewegung zum Zielpunkt
        float schritt = flugGeschwindigkeit * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, zielPunkt.position, schritt);
        
        // Prüfen ob Objekt gespawnt werden soll (nur auf dem Weg zu Punkt B)
        if (!hatGespawnt && objektZumSpawnen != null)
        {
            float distanzZuA = Vector3.Distance(transform.position, punktA.position);
            float gesamtDistanz = Vector3.Distance(punktA.position, punktB.position);
            float fortschritt = distanzZuA / gesamtDistanz;
            
            // Spawn an der zufällig gewählten Position
            if (fortschritt >= zufaelligeSpawnDistance)
            {
                SpawneObjekt();
                hatGespawnt = true;
            }
        }
        
        // Prüfen ob Zielpunkt erreicht wurde
        if (Vector3.Distance(transform.position, zielPunkt.position) < 0.1f)
        {
            ZielpunktErreicht();
        }
    }
    
    void SpawneObjekt()
    {
        if (objektZumSpawnen != null)
        {
            GameObject gespawntesObjekt = Instantiate(objektZumSpawnen, transform.position, Quaternion.identity);
            gespawntesObjekt.transform.SetParent(DropItemContainer.transform);
            Debug.Log("Objekt gespawnt bei: " + transform.position);
        }
    }
    
    void ZielpunktErreicht()
    {
        // Punkt B erreicht - teleportiere zurück zu A
        Debug.Log("Punkt B erreicht! Teleportiere zu Punkt A.");
        
        // Sofort zu Punkt A teleportieren
        transform.position = punktA.position;
        
        // Reset für nächsten Flug
        hatGespawnt = false;
        SetzeZufaelligeSpawnDistance(); // Neue zufällige Position für nächsten Drop
        
        // Zufällige Wartezeit starten
        StartCoroutine(WartenAnPunktA());
    }

    IEnumerator WartenAnPunktA()
    {
        wartet = true;
        float wartezeit = Random.Range(minWartezeit, maxWartezeit);

        Debug.Log("Warte " + wartezeit.ToString("F1") + " Sekunden an Punkt A... Nächster Drop bei: " + (zufaelligeSpawnDistance * 100f).ToString("F0") + "% der Strecke");

        yield return new WaitForSeconds(wartezeit);

        // Nach der Wartezeit wieder zu Punkt B fliegen
        wartet = false;

        Debug.Log("Wartezeit vorbei! Fliege wieder zu Punkt B.");
    }
    
    void SetzeZufaelligeSpawnDistance()
    {
        // Zufällige Position zwischen 10% und 90% der Strecke
        zufaelligeSpawnDistance = Random.Range(0.1f, 0.9f);
    }
    
    // Gizmos zum Visualisieren der Punkte im Scene View
    void OnDrawGizmosSelected()
    {
        if (punktA != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(punktA.position, 0.5f);
            Gizmos.DrawWireCube(punktA.position, Vector3.one * 0.2f);
        }
        
        if (punktB != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(punktB.position, 0.5f);
            Gizmos.DrawWireCube(punktB.position, Vector3.one * 0.2f);
        }
        
        // Linie zwischen den Punkten zeichnen
        if (punktA != null && punktB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(punktA.position, punktB.position);
        }
    }
}