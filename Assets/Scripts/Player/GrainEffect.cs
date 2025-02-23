using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class GrainEffect : MonoBehaviour
{

    public static GrainEffect Instance { get; private set; }

    [SerializeField] private PostProcessVolume volume;

    private Grain grain;
    private ChromaticAberration chromaticAberration;

    private float baseGrainIntensity;
    private float baseChromaticAberration;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        volume.profile.TryGetSettings(out grain);
        volume.profile.TryGetSettings(out chromaticAberration);

        baseGrainIntensity = grain.intensity.value;
        baseChromaticAberration = chromaticAberration.intensity.value;
    }

    public void SetGrainIntensity(float intensity)
    {
        if (grain == null)
        {
            volume.profile.TryGetSettings(out grain);
        }
        grain.intensity.value = Mathf.Lerp(grain.intensity.value, intensity, Time.deltaTime);
    }

    public void SetChromaticAberration(float intensity)
    {
        if (chromaticAberration == null)
        {
            volume.profile.TryGetSettings(out chromaticAberration);
        }
        chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, intensity, Time.deltaTime);
    }

    public void FrightEffect()
    {
        SetGrainIntensity(3f);
        SetChromaticAberration(1f);
    }

    public void StopFrightEffect()
    {
        SetGrainIntensity(baseGrainIntensity);
        SetChromaticAberration(baseChromaticAberration);
    }

}
