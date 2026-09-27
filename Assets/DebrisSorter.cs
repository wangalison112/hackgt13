using UnityEngine;

public class DebrisSorter : MonoBehaviour
{
    [Header("Link to Biometrics")]
    public HeartRateSimulator heartMonitor; 
    
    [Header("Player Stats")]
    public int playerHP = 100;
    
    private GameObject activeDebris; 
    private bool hasReacted = false; 

    // 1. Detect when an object enters the center screen
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rock") || other.CompareTag("Trash") || other.CompareTag("Satellite"))
        {
            activeDebris = other.gameObject;
            hasReacted = false; 
        }
    }

    // 2. Detect if an object leaves without the player doing anything
    void OnTriggerExit(Collider other)
    {
        if (activeDebris == other.gameObject)
        {
            if (activeDebris.CompareTag("Satellite") && !hasReacted)
            {
                Debug.Log("Success! Avoided the satellite.");
            }
            else if (!activeDebris.CompareTag("Satellite") && !hasReacted)
            {
                playerHP -= 10;
                Debug.Log("Missed debris! HP dropped to: " + playerHP);
            }
            activeDebris = null;
        }
    }

    // 3. Listen for key presses only if heart rate is high enough
    void Update()
    {
        if (activeDebris != null && !hasReacted && heartMonitor.currentHeartRate >= heartMonitor.targetThreshold)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ProcessSorting("Rock");
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ProcessSorting("Trash");
            }
        }
    }

    // 4. Handle the win/loss logic and color changing
    void ProcessSorting(string playerChoice)
    {
        hasReacted = true;
        string actualTag = activeDebris.tag;
        
        // Grab the 3D mesh so we can change its color
        MeshRenderer objectMesh = activeDebris.GetComponentInChildren<MeshRenderer>();

        if (actualTag == "Satellite")
        {
            ApplyColor(objectMesh, Color.red);
            playerHP -= 20;
            Debug.Log("FAIL: Touched a satellite! HP: " + playerHP);
        }
        else if (playerChoice == actualTag)
        {
            ApplyColor(objectMesh, Color.green);
            Debug.Log("CORRECT: HP: " + playerHP);
        }
        else
        {
            ApplyColor(objectMesh, Color.red);
            playerHP -= 10;
            Debug.Log("WRONG: HP: " + playerHP);
        }

        // Destroy the object 0.3 seconds later so the player has time to see the red/green flash
        Destroy(activeDebris, 0.3f); 
    }

    void ApplyColor(MeshRenderer mesh, Color color)
    {
        if (mesh != null)
        {
            mesh.material.color = color;
        }
    }
}