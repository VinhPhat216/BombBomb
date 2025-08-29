using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public enum ItemType
    {
        BlastRadius,
        Coin,
        ExplodeSlowly,
        ExtraBomb,
        ExtraHealth,
        KickBomb,
        RandomItem,
        SpeedIncrease,
        StunningBomb,
        SuddenDeath,
    }

    public ItemType type;

    private void OnItemPickUp(GameObject player)
    {
        switch (type)
        {
            case ItemType.BlastRadius:
                player.GetComponent<BombController>().AddExplosionRadius();
                break;

            case ItemType.Coin:

                break;

            case ItemType.ExplodeSlowly:
                player.GetComponent<BombController>().AddTime();
                break;

            case ItemType.ExtraBomb:
                player.GetComponent<BombController>().AddBomb();
                break;

            case ItemType.ExtraHealth:

                break;
            case ItemType.KickBomb:

                break;

            case ItemType.RandomItem:

                break;

            case ItemType.SpeedIncrease:
                player.GetComponent<PlayerController>().AddSpeed();
                break;

            case ItemType.StunningBomb:

                break;

            case ItemType.SuddenDeath:

                break;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnItemPickUp(collision.gameObject);
        }
    }
}

