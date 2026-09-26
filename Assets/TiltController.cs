using UnityEngine;

public class TiltController : MonoBehaviour
{
    public float leanSpeed = 5f;
    public float maxLeanDistance = 3f; // how far left/right astronaut can move
    private float currentLean = 0f;

    void Update()
    {
        // TEMP: simulate lean with A/D or Left/Right until real sensor input exists
        float input = Input.GetAxis("Horizontal"); // -1 (left) to 1 (right)

        currentLean += input * leanSpeed * Time.deltaTime;
        currentLean = Mathf.Clamp(currentLean, -maxLeanDistance, maxLeanDistance);

        Vector3 pos = transform.position;
        pos.x = currentLean;
        transform.position = pos;
    }

    // Call this instead of using Input, once you have a real sensor value (-1 to 1)
    public void SetLeanFromSensor(float sensorValue)
    {
        currentLean = Mathf.Clamp(sensorValue * maxLeanDistance, -maxLeanDistance, maxLeanDistance);
        Vector3 pos = transform.position;
        pos.x = currentLean;
        transform.position = pos;
    }
}
