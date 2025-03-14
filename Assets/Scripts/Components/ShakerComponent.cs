using UnityEngine;
using UnityEngine.Serialization;

public class ShakerComponent : MonoBehaviour
{
    [SerializeField] private Transform target;

    private float _currentTime = 0f;

    private Vector3 _originalPosition;
    public float duration = 1f;
    public Vector3 shakeIntensity = Vector3.one;
    public float frequency = 1f;

    private int _seed = 1;
    
    void Start()
    {
        _originalPosition = target.localPosition;
    }

    void Update()
    {
        if (_currentTime > 0){
            _currentTime -= Time.deltaTime;
            _currentTime = Mathf.Max(_currentTime, 0.0f);

            float offsetX = Perlin.Noise((_currentTime + _seed) * frequency) * _currentTime * shakeIntensity.x;
            float offsetY = Perlin.Noise((_currentTime + 10*_seed) * frequency) * _currentTime * shakeIntensity.y;
            float offsetZ = Perlin.Noise((_currentTime + 20*_seed) * frequency) * _currentTime * shakeIntensity.z;
            target.localPosition = _originalPosition + new Vector3(offsetX, offsetY, offsetZ);
        }
    }

    public void Shake()
    {
        _seed = Random.Range(1, 100);
        _currentTime = duration;
    }
}