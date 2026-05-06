using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHp;
    public int hp;
    public System.Action OnDead;
    public int score;
    Animator anim;
    

    void OnEnable()
    {
        hp = maxHp; // ⭐ 다시 살아날 때 초기화
        if (anim != null)
        {
            anim.Rebind();
            anim.Update(0f);
        }
    }
    
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        anim.SetTrigger("Hit");

        if (hp <= 0)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.AddScore(score);

            if (Random.value < 0.3f)
                ItemManager.instance.SpawnRandom(transform.position);

            OnDead?.Invoke();
            gameObject.SetActive(false);
        }
    }
}