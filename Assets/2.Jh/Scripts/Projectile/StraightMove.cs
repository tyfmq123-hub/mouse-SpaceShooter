using System;
using Unity.VisualScripting;
using UnityEngine;

public class StraightMove : MonoBehaviour
{
    float speed;

    public enum Types
    {
        Player,
        Enemy
    }

    public Types type;

    private void Update()
    {
        if (type == Types.Player)
        {
            speed = 10;
        }

        if (type == Types.Enemy)
        {
            speed = -10;
        }

        transform.Translate(0, speed * Time.deltaTime, 0);
        Vector3 pos = this.transform.position;

        if (pos.x < -4 || pos.x > 4 || pos.y < -5 || pos.y > 5)
        {
            gameObject.SetActive(false);
        }
    }
}