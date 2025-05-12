using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private float speed;

    void Awake()
    {
        speed = MoveSpeed.speed;
    }

    void Update()
    {
        speed = MoveSpeed.speed;
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}
