using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeartRateSimulator : MonoBehaviour
{
    [Header("Simulation Settings")] // 
    public float currentHeartRate = 80f; // starting heart rate 
    public float minHeartRate = 60f; //lowest heart rate 
    public float maxHeartRate = 180f; // max heart 
    public float targetThreshold = 130f;  // target 
    
    [Header("Wizard of Oz Controls")]
    public KeyCode increaseKey = KeyCode.UpArrow;
    public KeyCode decreaseKey = KeyCode.DownArrow;
    public float changeRate = 15f; 

    [Header("UI References")]
    public Slider judgeFacingSlider; 
    public TextMeshProUGUI heartRateReadout;

    // [Header("Visual Effects (Assign your components)")]
    // public Material fogMaterial;
    // public ParticleSystem debrisParticles;

    void Start()
    {
        if (judgeFacingSlider != null)
        {
            judgeFacingSlider.minValue = minHeartRate;
            judgeFacingSlider.maxValue = maxHeartRate;
        }
    }

    void Update()
    {
        // 1. Process Keyboard Input
        if (Input.GetKey(increaseKey))
        {
            //  'Time.deltaTime' is time passed since the last frame 
            currentHeartRate += changeRate * Time.deltaTime;
        }
        else if (Input.GetKey(decreaseKey))
        {
            currentHeartRate -= changeRate * Time.deltaTime;
        }

        // 2. Sync with Slider (if manually dragged by presenter)
        if (judgeFacingSlider != null && Input.GetMouseButton(0)) 
        {
            currentHeartRate = judgeFacingSlider.value;
        }

        // 'Mathf.Clamp' acts as a hard boundary. It forces currentHeartRate to stay strictly between your min and max limits.
        currentHeartRate = Mathf.Clamp(currentHeartRate, minHeartRate, maxHeartRate);

        UpdateUI();
        UpdateEnvironment();
    }

    void UpdateUI()
    {
        if (judgeFacingSlider != null && !Input.GetMouseButton(0)) 
        {
            judgeFacingSlider.value = currentHeartRate;
        }
        
        if (heartRateReadout != null) 
        {
            heartRateReadout.text = Mathf.RoundToInt(currentHeartRate).ToString() + " BPM";
        }
    }

    void UpdateEnvironment()
    {
        // 0.0 means at min heart rate (max fog), 1.0 means at or above threshold (clear screen)

        // 'Mathf.InverseLerp' calculates a percentage (from 0.0 to 1.0) of where the current heart rate falls between the minimum and the target.
        // 0.0 means at min heart rate (max fog), 1.0 means at or above threshold (clear screen).
        float readinessRatio = Mathf.InverseLerp(minHeartRate, targetThreshold, currentHeartRate);
        
        // Example integration:
        // float currentFogDensity = Mathf.Lerp(1f, 0f, readinessRatio);
        // fogMaterial.SetFloat("_FogDensity", currentFogDensity);
        
        // var emission = debrisParticles.emission;
        // emission.rateOverTime = Mathf.Lerp(50f, 0f, readinessRatio); 
    }
}