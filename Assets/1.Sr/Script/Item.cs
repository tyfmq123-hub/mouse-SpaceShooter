using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType { Coin, PowerUp, Boom }
    public ItemType itemType;
    public float speed = 2f;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        rigid.linearVelocity = Vector2.down * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Player player = collision.GetComponent<Player>();

        switch (itemType)
        {
            case ItemType.Coin:
                GameManager.Instance.AddScore(300);
                break;
            case ItemType.PowerUp:
                player.PowerUp();
                break;
            case ItemType.Boom:
                player.GetBoom();
                break;
        }

        gameObject.SetActive(false);
    }
}
