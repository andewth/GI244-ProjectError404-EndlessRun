using UnityEngine;

public class MoveSpeed : MonoBehaviour
{
    public static float speed;

    private float timeElapsed = 0f;
    private float nextSpeedIncreaseTime;
    private float maxSpeed = 24f;

    void Awake()
    {
        speed = 5f;
        nextSpeedIncreaseTime = 12f;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        while (timeElapsed >= nextSpeedIncreaseTime)
        {
            speed *= 1.5f;

            if (speed > maxSpeed)
            {
                speed = maxSpeed;
                Debug.Log("Speed Max: " + speed);
                PlayerController.Instance.UpdateUI("speed");
            }
            else
            {
                Debug.Log("Speed to: " + speed);
                PlayerController.Instance.UpdateUI("speed");
            }

            nextSpeedIncreaseTime += 12f;
        }
    }
}
