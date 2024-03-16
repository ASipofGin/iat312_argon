using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ElementalSightTestController : MonoBehaviour
{
    [Header("Time Stats")]
    [SerializeField] private float _sightFadeOutTime = 0.5f;
    
    [Header("References")]
    [SerializeField] private ScriptableRendererFeature _elementalSight;    
    [SerializeField] private Material _material;

    [Header("Intensity Stats")]
    [SerializeField] private float _voronoiIntensityStat = 0.61f;
    [SerializeField] private float _vignetteIntensityStat = 1.15f;   
    
    private int _voronoiIntensity = Shader.PropertyToID("_VoronoiIntensity");
    private int _vignetteIntensity = Shader.PropertyToID("_VignetteIntensity");
    
    // Start is called before the first frame update
    void Start()
    {
        _elementalSight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SightActive(){
        _elementalSight.SetActive(true);
        _material.SetFloat(_voronoiIntensity, _voronoiIntensityStat);
        _material.SetFloat(_vignetteIntensity, _vignetteIntensityStat);
    }

    public void SightDisable(){
        StartCoroutine(SightDisabled());
    }

    public IEnumerator SightDisabled()
    {
        float elapsedTime = 0f;
        while (elapsedTime < _sightFadeOutTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedVoronoi = Mathf.Lerp(_voronoiIntensityStat, 0f, (elapsedTime / _sightFadeOutTime));
            float lerpedVignette = Mathf.Lerp(_vignetteIntensityStat, 0f, (elapsedTime / _sightFadeOutTime));

            _material.SetFloat(_voronoiIntensity, lerpedVoronoi);
            _material.SetFloat(_vignetteIntensity, lerpedVignette);

            yield return null; // Wait until the next frame and then continue execution from here
        }

        _elementalSight.SetActive(false);

        yield break; // Explicitly signify the end of the coroutine
    }


}
