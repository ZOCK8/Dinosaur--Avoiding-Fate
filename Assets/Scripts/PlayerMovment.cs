using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerMovment : MonoBehaviour
{
    public GameObject player;
    public GameObject enemyGameobjektcontainer;
    public Animator playerAniamtor;
    public float speed = 5f;
    public GameObject HartContainer;
    public int Health = 3;
    public bool Delay;
    private Vector3 lastPosition;
    void Start()
    {
        Heart();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != lastPosition)
        {
            Debug.Log("Player moving");
            player.GetComponent<Animator>().SetBool("Walk", true);

        }

        else if (transform.position == lastPosition)
        {
            player.GetComponent<Animator>().SetBool("Walk", false);
            player.GetComponent<Animator>().SetTrigger("Normal");
        }
        lastPosition = transform.position;

        Heart();
        Health = GameDataManager.Instance.data.Hearts;
        if (Input.GetKey(KeyCode.Y))
        {


        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                player.GetComponent<SpriteRenderer>().flipX = true;
                player.transform.Translate(Vector3.left * speed * Time.deltaTime);
                // Move player left
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                player.GetComponent<SpriteRenderer>().flipX = false;
                player.transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
        for (int i = 0; i < enemyGameobjektcontainer.transform.childCount; i++)
            if (enemyGameobjektcontainer.transform.GetChild(i).GetComponent<CircleCollider2D>().IsTouching(player.GetComponent<BoxCollider2D>()) && !Delay)
            {
                StartCoroutine(HeartDelay());
                playerAniamtor.SetTrigger("Damage");
                GameDataManager.Instance.data.Hearts -= 1;
                Heart();
                Debug.Log("Player Health: " + Health);
            }

    }
    void Heart()
    {
        switch (GameDataManager.Instance.data.Hearts)
        {
            case 4:
                HartContainer.transform.GetChild(0).gameObject.SetActive(true);
                HartContainer.transform.GetChild(1).gameObject.SetActive(true);
                HartContainer.transform.GetChild(2).gameObject.SetActive(true);
                HartContainer.transform.GetChild(3).gameObject.SetActive(true);
                break;
            case 3:
                HartContainer.transform.GetChild(0).gameObject.SetActive(true);
                HartContainer.transform.GetChild(1).gameObject.SetActive(true);
                HartContainer.transform.GetChild(2).gameObject.SetActive(true);
                HartContainer.transform.GetChild(3).gameObject.SetActive(true);
                break;
            case 2:
                HartContainer.transform.GetChild(0).gameObject.SetActive(true);
                HartContainer.transform.GetChild(1).gameObject.SetActive(true);
                HartContainer.transform.GetChild(2).gameObject.SetActive(false);
                HartContainer.transform.GetChild(3).gameObject.SetActive(false);
                break;
            case 1:
                HartContainer.transform.GetChild(0).gameObject.SetActive(true);
                HartContainer.transform.GetChild(1).gameObject.SetActive(false);
                HartContainer.transform.GetChild(2).gameObject.SetActive(false);
                HartContainer.transform.GetChild(3).gameObject.SetActive(false);
                break;
            case <= 0:
                HartContainer.transform.GetChild(0).gameObject.SetActive(false);
                HartContainer.transform.GetChild(1).gameObject.SetActive(false);
                HartContainer.transform.GetChild(2).gameObject.SetActive(false);
                HartContainer.transform.GetChild(3).gameObject.SetActive(false);
                GameDataManager.Instance.data.level = 0;
                GameDataManager.Instance.data.Hearts = 4;
                Debug.Log("Game Over");
                SceneManager.LoadScene("GameOver");
                break;
        }
    }
    IEnumerator HeartDelay()
    {
        Delay = true;
        yield return new WaitForSeconds(0.8f);
        Delay = false;
    }
} 

