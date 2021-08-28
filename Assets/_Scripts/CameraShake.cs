using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraShake : MonoBehaviour
{
    static public CameraShake instance { get; private set; }

    [SerializeField] private float traumaDecreaseRate;
    [SerializeField] private float maxAmplitude;

    private CinemachineBasicMultiChannelPerlin perlin;
    private float trauma;


    public void AddTrauma(float trauma)
    {
        this.trauma = Mathf.Min(1, this.trauma + trauma);
    }

    private void Awake()
    {
        instance = Singleton.Setup(this, instance) as CameraShake;
        GetPerlinComponent();
    }

    void GetPerlinComponent()
    {
        var camera = GetComponent<CinemachineVirtualCamera>();
        perlin = camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        UpdateTrauma();
        UpdateCameraShake();
    }

    private void UpdateTrauma()
    {
        trauma = Mathf.Max(0, trauma - traumaDecreaseRate * Time.deltaTime);
    }

    private void UpdateCameraShake()
    {
        var shake = trauma * trauma;
        perlin.m_AmplitudeGain = shake * maxAmplitude;
    }
}
