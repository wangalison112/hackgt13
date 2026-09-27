using UnityEngine;

public class InstructionTimer : MonoBehaviour
{
    void Start()
    {
        // Destroys this UI panel and reveals the game after exactly 7 seconds
        Destroy(gameObject, 2f); 
    }
}