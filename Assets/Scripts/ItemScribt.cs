using System.Collections;
using UnityEngine;

public class ItemScribt : MonoBehaviour
{
    public GameObject ItemContainer;
    public GameObject Player;
    public Animator PlayerAnimator;
    public PlayerMovment playerMovment;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < ItemContainer.transform.childCount; i++)
        {
            if (ItemContainer.transform.GetChild(i).GetComponent<CircleCollider2D>().IsTouching(Player.GetComponent<BoxCollider2D>()))
            {
                //Destroy(ItemContainer.transform.GetChild(i));
                Debug.Log("Item Collected");
                PlayerAnimator.SetTrigger("ItemCollect");
                switch (ItemContainer.transform.GetChild(i).name)
                {
                    case "ItemSpeed":
                        StartCoroutine(ItemEffectSpeed());
                        break;
                    case "ItemBike":
                        StartCoroutine(ItemEffectBike());
                        break;
                    case "ItemShrink":
                        StartCoroutine(ItemEffectshrink());
                        break;

                }


            }

        }
    }
    IEnumerator ItemEffectSpeed()
    {
        Debug.Log("ItemEffet stardet for Speed");
        playerMovment.speed += 0.02f;
        yield return new WaitForSeconds(2f);
        playerMovment.speed -= 0.02f;

    }
    IEnumerator ItemEffectBike()
    {
        playerMovment.speed = +1;
        yield return new WaitForSeconds(2f);
        playerMovment.speed = -1;
    }
    IEnumerator ItemEffectshrink()
    {
        if (Player != null)
        {
            Player.GetComponent<Animator>().SetTrigger("Shrink");
            yield return null;
        }
        else
        {
            Debug.Log("There is no player set / no animator");
        }
    }
}
