using UnityEngine;
using UnityEngine.Serialization;

public class ShakerComponent : MonoBehaviour
{
    [SerializeField] private Transform target;

    private float _currentTime = 0f;

    private Vector3 _originalPosition;
    public float duration;
    public Vector3 shakeIntensity = Vector3.one;
    public float frequency = 1f;

    void Start()
    {
        _originalPosition = target.localPosition;
    }

    void Update()
    {
        if (_currentTime > 0){
            _currentTime -= Time.deltaTime;
            _currentTime = Mathf.Max(_currentTime, 0.0f);

            float offsetX = Perlin.Noise(_currentTime * frequency) * _currentTime * shakeIntensity.x;
            float offsetY = Perlin.Noise((_currentTime + 10) * frequency) * _currentTime * shakeIntensity.y;
            float offsetZ = Perlin.Noise((_currentTime + 20) * frequency) * _currentTime * shakeIntensity.z;
            target.localPosition = _originalPosition + new Vector3(offsetX, offsetY, offsetZ);
        }
    }

    public void Shake()
    {
        _currentTime = duration;
    }
}