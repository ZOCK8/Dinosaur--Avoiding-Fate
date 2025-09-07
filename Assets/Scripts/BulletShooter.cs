using System.Collections;
using UnityEngine;

public class BulletShooter : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform player;
    public BoxCollider2D BorderDown;
    public GameObject bulletcontainer;
    public GameObject MeteorContainer; // Typo korrigiert: war "MeteorConatainer"
    public float bulletSpeed = 5f;

    [Header("Random Target Settings")]
    public Vector2 minTarget = new Vector2(-3f, 5f);
    public Vector2 maxTarget = new Vector2(3f, 8f);

    [Header("Gravity Settings")]
    public float gravityAfterHit = 10f;
    private bool cantakedamage = true;
    public MeteorSpawning meteorSpawning;
    public bool CanShoot;
    public GameObject DropItemContainer;
    public Tutorial tutorial;
    void Start()
    {
        CanShoot = false;
    }

    private void Update()
    {
        for (int i = 0; i < DropItemContainer.transform.childCount; i++)
            if (player.GetComponent<BoxCollider2D>().IsTouching(DropItemContainer.transform.GetChild(i).GetComponent<CircleCollider2D>()))
            {
                CanShoot = true;
                Debug.Log("Player can shoot");
                Destroy(DropItemContainer.transform.GetChild(0).gameObject);
            }

        if (Input.GetKeyDown(KeyCode.Z) && CanShoot)
            {
                player.GetComponent<Animator>().SetTrigger("Attack");
                CanShoot = false;
                ShootBullet();
            }

        // Bullets am unteren Rand zerstören
        for (int i = bulletcontainer.transform.childCount - 1; i >= 0; i--)
        {
            var bullet = bulletcontainer.transform.GetChild(i).gameObject;
            var bulletCollider = bullet.GetComponent<BoxCollider2D>();

            if (bulletCollider != null && BorderDown != null && bulletCollider.IsTouching(BorderDown))
            {
                Debug.Log("Destroying bullet at border");
                StartCoroutine(DestroyBullet(bullet));
            }
        }

        // Bullet-Meteor Kollision prüfen
        for (int b = bulletcontainer.transform.childCount - 1; b >= 0; b--)
        {
            if (b >= bulletcontainer.transform.childCount) continue; // Sicherheitscheck

            var bullet = bulletcontainer.transform.GetChild(b).gameObject;
            var bulletCol = bullet.GetComponent<BoxCollider2D>();
            if (bulletCol == null) continue;

            bool bulletHit = false;

            for (int m = MeteorContainer.transform.childCount - 1; m >= 0; m--)
            {
                if (m >= MeteorContainer.transform.childCount) continue; // Sicherheitscheck

                var meteor = MeteorContainer.transform.GetChild(m).gameObject;
                var meteorCol = meteor.GetComponent<CircleCollider2D>();
                if (meteorCol == null) continue;

                if (meteorCol.IsTouching(bulletCol))
                {
                    Debug.Log("Meteor hit by bullet: " + meteor.name);

                    // Prüfen ob es der Hauptmeteor ist
                    if (meteor.name == "Meteor")
                    {
                        meteorSpawning.canSpawn = true;
                        // Hauptmeteor wurde getroffen - Explosion auslösen
                        if (meteorSpawning.meteor == meteor)
                        {
                            StartCoroutine(meteorSpawning.DestroyMeteor());
                        }
                        else
                        {
                            Destroy(meteor);
                        }
                    }
                    else
                    {
                        // Kleiner Meteor - einfach zerstören
                        Destroy(meteor);
                    }

                    StartCoroutine(DestroyBullet(bullet));
                    bulletHit = true;
                    break;
                }
            }

            if (bulletHit) continue; // Bullet wurde bereits zerstört, nächsten prüfen
        }

        // Player-Bullet Kollision prüfen
        if (cantakedamage && player != null)
        {
            var playerCollider = player.GetComponent<BoxCollider2D>();
            if (playerCollider != null)
            {
                for (int i = bulletcontainer.transform.childCount - 1; i >= 0; i--)
                {
                    var bullet = bulletcontainer.transform.GetChild(i).gameObject;
                    var bulletCollider = bullet.GetComponent<BoxCollider2D>();

                    if (bulletCollider != null && playerCollider.IsTouching(bulletCollider))
                    {
                        Debug.Log("Player hit by bullet");
                        StartCoroutine(DestroyBullet(bullet));
                        StartCoroutine(HeartDelay());
                        break; // Nur eine Kollision pro Frame verarbeiten
                    }
                }
            }
        }
    }

    void ShootBullet()
    {
        if (player == null || bulletPrefab == null) return;
        
        Vector3 spawnPos = player.position + Vector3.up * 1.6f;
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        bullet.transform.SetParent(bulletcontainer.transform);

        Vector2 randomTarget = new Vector2(
            Random.Range(minTarget.x, maxTarget.x),
            Random.Range(minTarget.y, maxTarget.y)
        );

        BulletMover mover = bullet.GetComponent<BulletMover>();
        if (mover == null)
            mover = bullet.AddComponent<BulletMover>();
            
        mover.Init(randomTarget, bulletSpeed, gravityAfterHit);
    }

    IEnumerator DestroyBullet(GameObject bullet)
    {
        if (bullet == null) yield break;

        var anim = bullet.GetComponent<Animator>();
        if (anim != null)
            anim.SetTrigger("BulletExplosion");

        Debug.Log("Playing bullet explosion animation");
        
        var rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.gravityScale = 0;

        // Kollider deaktivieren um weitere Kollisionen zu verhindern
        var collider = bullet.GetComponent<BoxCollider2D>();
        if (collider != null)
            collider.enabled = false;

        yield return new WaitForSeconds(0.5f); // Kürzere Wartezeit für bessere Performance

        if (bullet != null)
            Destroy(bullet);
    }

    IEnumerator HeartDelay()
    {
        if (GameDataManager.Instance != null && GameDataManager.Instance.data != null)
        {
            GameDataManager.Instance.data.Hearts -= 1;
            Debug.Log("Hearts remaining: " + GameDataManager.Instance.data.Hearts);
        }
        
        cantakedamage = false;
        yield return new WaitForSeconds(0.3f);
        cantakedamage = true;
    }
}

public class BulletMover : MonoBehaviour
{
    private Vector2 target;
    private float speed;
    private float gravityAfterHit;
    private Rigidbody2D rb;
    private bool reachedTarget = false;

    public void Init(Vector2 target, float speed, float gravityAfterHit)
    {
        this.target = target;
        this.speed = speed;
        this.gravityAfterHit = gravityAfterHit;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody2D>();

        rb.gravityScale = 0;
        rb.linearDamping = 0; // Kein Luftwiderstand für bessere Bewegung
    }

    private void Update()
    {
        if (reachedTarget) return;

        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            reachedTarget = true;
            rb.gravityScale = gravityAfterHit;
        }
    }
}