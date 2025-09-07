using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MeteorSpawning : MonoBehaviour
{
    [Header("Meteor Animators")]
    public Animator MeteorAnimator;
    public Animator CameraAnimator;

    [Header("Meteor Settings")]
    public GameObject meteorPrefab;
    public float spawnIntervalMax = 2f;
    public float MeteorSpeed = 5f;
    public float spawnIntervalMin = 0.5f;
    
    [Header("Spawn Go To Settings")]
    public float MaxYPosition = 6f;
    public float MinYPosition = -6f;
    public float MaxXPosition = 8f;
    public float MinXPosition = -8f;

    public Vector3 SpawnPoint;

    public GameObject meteor;
    private Vector3 meteorgotoposition;
    private bool isMoving = false;
    public GameObject MeteorContainer; // Typo korrigiert: war "MeteorCoantainer"
    public int ItemChance = 10; // Chance in Prozent, dass ein Item erscheint

    // Liste für duplizierte Meteore und deren Zielpositionen
    private List<GameObject> duplicatedMeteors = new List<GameObject>();
    private List<Vector3> duplicatedTargets = new List<Vector3>();

    // Listen für Items und deren Zielpositionen
    private List<GameObject> movingItems = new List<GameObject>();
    private List<Vector3> itemTargets = new List<Vector3>();

    public List<GameObject> Items;
    public GameObject ItemContainer;
    public bool AutoLevel = true;

    [Header("Limit")]
    public int MaxMeteors = 20;

    public GameObject player; // Im Inspector zuweisen!

    public Sprite SmallMeteorSprite;
    public bool MeteorsSmall;
    public bool canSpawn;

    void Start()
    {
        canSpawn = true;
        SpawnMeteorite();
    }

    void Update()
    {


        if (AutoLevel && GameDataManager.Instance != null)
        {
            MeteorSpeed = 3f + (GameDataManager.Instance.data.level * 0.2f);
        }

        // Hauptmeteor bewegen
        if (isMoving && meteor != null)
        {
            meteor.transform.position = Vector3.MoveTowards(
                meteor.transform.position,
                meteorgotoposition,
                MeteorSpeed * Time.deltaTime
            );

            if (Vector3.Distance(meteor.transform.position, meteorgotoposition) < 0.01f)
            {
                StartCoroutine(DestroyMeteor());
            }
        }

        // Duplizierte Meteore bewegen und ggf. zerstören
        for (int i = duplicatedMeteors.Count - 1; i >= 0; i--)
        {
            if (i >= duplicatedMeteors.Count) continue; // Sicherheitscheck
            
            GameObject dup = duplicatedMeteors[i];
            Vector3 target = duplicatedTargets[i];
            
            if (dup != null)
            {
                dup.transform.position = Vector3.MoveTowards(
                    dup.transform.position,
                    target,
                    MeteorSpeed * Time.deltaTime
                );

                if (dup.transform.position.y <= -9f)
                {
                    Debug.Log("Small meteor destroyed below screen");
                    Destroy(dup);
                    duplicatedMeteors.RemoveAt(i);
                    duplicatedTargets.RemoveAt(i);
                }
            }
            else
            {
                // Null-Referenz entfernen
                duplicatedMeteors.RemoveAt(i);
                duplicatedTargets.RemoveAt(i);
            }
        }

        // Items bewegen und ggf. zerstören
        for (int i = movingItems.Count - 1; i >= 0; i--)
        {
            if (i >= movingItems.Count) continue; // Sicherheitscheck
            
            GameObject item = movingItems[i];
            Vector3 target = itemTargets[i];
            
            if (item != null)
            {
                item.transform.position = Vector3.MoveTowards(
                    item.transform.position,
                    target,
                    MeteorSpeed * Time.deltaTime
                );

                if (item.transform.position.y <= -9f)
                {
                    Destroy(item);
                    movingItems.RemoveAt(i);
                    itemTargets.RemoveAt(i);
                }
            }
            else
            {
                // Null-Referenz entfernen
                movingItems.RemoveAt(i);
                itemTargets.RemoveAt(i);
            }
        }

        // Respawn nur, wenn keine Meteore mehr da sind und canSpawn true ist
        if (MeteorContainer.transform.childCount == 0 && duplicatedMeteors.Count == 0 && canSpawn == true)
        {
            Debug.Log("No meteors left, respawning...");
            SpawnMeteorite();
        }
    }

    public void SpawnMeteorite()
    {
        for (int i = 0; i < MeteorContainer.transform.childCount; i++)
        MeteorContainer.transform.GetChild(i).GetComponent<Animator>().SetBool("MeteorSmall", false);
        if (!canSpawn) return; // Zusätzliche Sicherheit
        
        canSpawn = false; // Sofort auf false setzen um mehrfaches Spawnen zu verhindern
        
        MeteorsSmall = false;
        
        if (CameraAnimator != null)
            CameraAnimator.SetTrigger("Shake");
        
        meteor = Instantiate(meteorPrefab);
        
        if (meteor.GetComponent<CircleCollider2D>() == null)
            meteor.AddComponent<CircleCollider2D>();
            
        meteor.transform.SetParent(MeteorContainer.transform);
        meteor.name = "Meteor";
        meteor.transform.position = SpawnPoint;
        
        meteorgotoposition = new Vector3(
            Random.Range(MinXPosition, MaxXPosition),
            Random.Range(MinYPosition, MaxYPosition),
            0f
        );
        
        isMoving = true;
        for (int i = 0; i < MeteorContainer.transform.childCount; i++)
        MeteorContainer.transform.GetChild(i).GetComponent<Animator>().SetBool("MeteorSmall", false);
        
        // Level erhöhen mit Null-Check
        if (GameDataManager.Instance != null && GameDataManager.Instance.data != null)
        {
            GameDataManager.Instance.data.level += 1;
            Debug.Log("Level increased to: " + GameDataManager.Instance.data.level);
        }
    }

    public void DuplicateMeteor()
    {
        if (meteor == null) return;
        int amount = Random.Range(3, 7);
        Vector3 meteorPosition = meteor.transform.position;

        for (int i = 0; i < amount; i++)
        {
            GameObject dup = Instantiate(meteorPrefab);

            if (dup.GetComponent<CircleCollider2D>() == null)
                dup.AddComponent<CircleCollider2D>();

            dup.GetComponent<CircleCollider2D>().isTrigger = true;
            dup.transform.localScale = meteor.transform.localScale / 3f;

            if (dup.GetComponent<SpriteRenderer>() != null && SmallMeteorSprite != null)
                dup.GetComponent<SpriteRenderer>().sprite = SmallMeteorSprite;
            for (int s = 0; s < MeteorContainer.transform.childCount; s++)
                MeteorContainer.transform.GetChild(s).GetComponent<Animator>().SetBool("MeteorSmall", true);

            dup.name = "MeteorSmall";

            // Explosionseffekt
            Vector3 explosionOffset = new Vector3(
                Random.Range(-0.5f, 0.5f),
                Random.Range(0.1f, 0.2f),
                0f
            );
            dup.transform.position = meteorPosition + explosionOffset;
            dup.transform.SetParent(MeteorContainer.transform);

            Vector3 target = new Vector3(
                Random.Range(MinXPosition, MaxXPosition),
                Random.Range(-10f, -11f),
                0f
            );

            duplicatedMeteors.Add(dup);
            duplicatedTargets.Add(target);
            for (int s = 0; s < MeteorContainer.transform.childCount; s++)
            MeteorContainer.transform.GetChild(s).GetComponent<Animator>().SetBool("MeteorSmall", true);
        }

        // Meteorliste begrenzen
        while (duplicatedMeteors.Count > MaxMeteors)
        {
            if (duplicatedMeteors[0] != null)
                Destroy(duplicatedMeteors[0]);
            duplicatedMeteors.RemoveAt(0);
            duplicatedTargets.RemoveAt(0);
        }
        
        // Hauptmeteor zerstören
        Destroy(meteor);
        meteor = null;
        isMoving = false;
    }

    public void DuplicateItem()
    {
        if (meteor == null) return;
        
        
        Vector3 meteorPosition = meteor.transform.position;
        int amount = Random.Range(3, 7);
        
        // Kleine Meteore spawnen
        for (int i = 0; i < amount; i++)
        {
            MeteorsSmall = true;
            GameObject dup = Instantiate(meteorPrefab);
            
            if (dup.GetComponent<CircleCollider2D>() == null)
                dup.AddComponent<CircleCollider2D>();
                
            dup.transform.localScale = meteor.transform.localScale / 2f;
            dup.name = "MeteorFragment";
            
            Vector3 explosionOffset = new Vector3(
                Random.Range(-0.2f, 0.2f),
                Random.Range(0.1f, 0.3f),
                0f
            );
            dup.transform.position = meteorPosition + explosionOffset;
            dup.transform.SetParent(MeteorContainer.transform);
            
            Vector3 target = new Vector3(
                Random.Range(MinXPosition, MaxXPosition),
                Random.Range(-10f, -11f),
                0f
            );
            
            duplicatedMeteors.Add(dup);
            duplicatedTargets.Add(target);
        }
        
        // Items spawnen
        if (Items != null && Items.Count > 0)
        {
            int amountitem = Random.Range(1, 3);
            for (int i = 0; i < amountitem; i++)
            {
                int itemIndex = Random.Range(0, Items.Count);
                GameObject item = Instantiate(Items[itemIndex]);
                
                if (item.GetComponent<CircleCollider2D>() == null)
                    item.AddComponent<CircleCollider2D>();
                    
                item.GetComponent<CircleCollider2D>().isTrigger = true;

                Vector3 explosionOffset = new Vector3(
                    Random.Range(-0.2f, 0.2f),
                    Random.Range(0.1f, 0.3f),
                    0f
                );
                item.transform.position = meteorPosition + explosionOffset;
                item.name = "Item" + Items[itemIndex].name;
                item.transform.SetParent(ItemContainer.transform);

                // Zielposition: X-Position des Spielers
                float playerX = (player != null) ? player.transform.position.x : 0f;
                Vector3 itemTarget = new Vector3(
                    playerX,
                    Random.Range(-10f, -11f),
                    0f
                );
                
                movingItems.Add(item);
                itemTargets.Add(itemTarget);
            }
        }
        
        // Hauptmeteor zerstören
        Destroy(meteor);
        meteor = null;
        isMoving = false;
    }

    public IEnumerator DestroyMeteor()
    {
        isMoving = false;
        Debug.Log("Meteor reached the target position.");
        
        if (meteor != null && meteor.GetComponent<Animator>() != null)
            meteor.GetComponent<Animator>().SetTrigger("Explosion");
            
        yield return new WaitForSeconds(0.9f);

        // Item-Chance korrigiert: 10% Chance für Items, 90% für Meteore
        int randomChance = Random.Range(1, 101);
        
        if (randomChance <= ItemChance) // 10% Chance für Items
        {
            Debug.Log("Item spawned.");
            DuplicateItem();
        }
        else // 90% Chance für Meteore
        {
            Debug.Log("Meteor duplicated!");
            DuplicateMeteor();
        }
        
        // canSpawn wird nach einer kurzen Verzögerung wieder auf true gesetzt
        yield return new WaitForSeconds(0.1f);
        canSpawn = true;
    }
}