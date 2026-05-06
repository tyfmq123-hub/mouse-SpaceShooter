using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    void Awake()
    {
        instance = this;
    }

    public void SpawnItem(Item.ItemType itemType, Vector3 position)
    {
        string key = itemType switch
        {
            Item.ItemType.Coin    => "itemCoin",
            Item.ItemType.PowerUp => "itemPower",
            Item.ItemType.Boom    => "itemBoom",
            _                     => null
        };

        if (key == null) return;

        GameObject item = PoolManager.Instance.MakeObj(key);
        if (item != null)
            item.transform.position = position;
    }

    public void SpawnRandom(Vector3 position)
    {
        int rand = Random.Range(0, 3);
        SpawnItem((Item.ItemType)rand, position);
    }
}
