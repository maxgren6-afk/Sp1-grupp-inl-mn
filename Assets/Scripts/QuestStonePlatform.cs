// using Unity.VisualScripting;
using UnityEngine;

public class QuestStonePlatform : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float platformSpeed = 2f;
    public bool questOneCompletePlatform;


    private Transform currentTarget;

    private void Start()
    {
        currentTarget = target;
    }

    private void FixedUpdate()
    {
        if (questOneCompletePlatform)
        {
            transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, platformSpeed * Time.deltaTime);
        }
    }

}

