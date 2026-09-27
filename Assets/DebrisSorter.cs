using UnityEngine;
using UnityEngine.UI;
using TMPro; // Added library for TextMeshPro
using System.Collections;

public class DebrisSorter : MonoBehaviour
{
    [Header("Link to Biometrics")]
    public HeartRateSimulator heartMonitor; 
    
    [Header("Player Stats")]
    public int playerHP = 100;

    [Header("UI Feedback")]
    public Image damageFlash; 
    public TMP_Text hpText; // NEW SLOT FOR HP
    
    private GameObject activeDebris; 
    private bool hasReacted = false; 

    void Start()
    {
        UpdateHPDisplay(); // Set to 100 when the game starts
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rock") || other.CompareTag("Trash") || other.CompareTag("Satellite"))
        {
            activeDebris = other.gameObject;
            hasReacted = false; 
        }
    }

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
                UpdateHPDisplay(); 
                StartCoroutine(FlashScreen());
            }
            activeDebris = null;
        }
    }

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

    void ProcessSorting(string playerChoice)
    {
        hasReacted = true;
        string actualTag = activeDebris.tag;
        MeshRenderer objectMesh = activeDebris.GetComponentInChildren<MeshRenderer>();

        if (actualTag == "Satellite")
        {
            ApplyColor(objectMesh, Color.red);
            playerHP -= 20;
            UpdateHPDisplay(); 
            StartCoroutine(FlashScreen());
        }
        else if (playerChoice == actualTag)
        {
            ApplyColor(objectMesh, Color.green);
        }
        else
        {
            ApplyColor(objectMesh, Color.red);
            playerHP -= 10;
            UpdateHPDisplay(); 
            StartCoroutine(FlashScreen());
        }

        Destroy(activeDebris, 0.3f); 
    }

    void ApplyColor(MeshRenderer mesh, Color color)
    {
        if (mesh != null)
        {
            mesh.material.color = color;
        }
    }

    IEnumerator FlashScreen()
    {
        if (damageFlash != null)
        {
            damageFlash.color = new Color(1f, 0f, 0f, 0.15f);
            yield return new WaitForSeconds(0.3f);
            
            float fadeTime = 0.5f;
            float elapsedTime = 0f;
            
            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.deltaTime;
                float currentAlpha = Mathf.Lerp(0.4f, 0f, elapsedTime / fadeTime);
                damageFlash.color = new Color(1f, 0f, 0f, currentAlpha);
                yield return null;
            }
        }
    }

    void UpdateHPDisplay()
    {
        if (hpText != null)
        {
            hpText.text = "HP: " + playerHP;
        }
    }
}