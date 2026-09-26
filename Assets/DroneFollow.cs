using UnityEngine;

public class DroneFollow : MonoBehaviour
{
    public Transform astronaut;
    public Vector3 offset = new Vector3(0, 1.5f, 0);
    public float smoothSpeed = 5f;

    void Update()
    {
        Vector3 targetPos = astronaut.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
    }
}