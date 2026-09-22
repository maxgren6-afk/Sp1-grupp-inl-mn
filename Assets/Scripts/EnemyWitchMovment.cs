using UnityEngine;

public class EnemyWitchMovment : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f;

    
    void FixedUpdate()
    {
        transform.Translate(new Vector2(moveSpeed, 0) * Time.deltaTime);
    }
}


