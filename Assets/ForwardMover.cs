using UnityEngine;

public class ForwardMover : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    // THIS FUCKING SUCKS!!
}
