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
        Debug.Log("피격됨"); // ⭐ 추가

        hp -= damage;

        anim.SetTrigger("Hit");

        if (hp <= 0)
        {
            
            GameManager.Instance.score += score;
            OnDead?.Invoke();
            gameObject.SetActive(false);
        }
    }
}