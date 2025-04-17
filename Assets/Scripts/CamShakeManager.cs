using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CamShakeManager : MonoBehaviour
{
    public static CamShakeManager Instance { get; private set; }

    private CinemachineBasicMultiChannelPerlin _cameraShake;

    private Coroutine _shakeCoroutine;

    private void Awake()
    {
        if(Instance && Instance != this) Destroy(gameObject);
        else Instance = this;

        _cameraShake = GetComponent<CinemachineBasicMultiChannelPerlin>();

        ResetShake();
    }

    public void Shake(float amplitude, float frequency, float duration)
    {
        _cameraShake.AmplitudeGain = amplitude;
        _cameraShake.FrequencyGain = frequency;

        if(_shakeCoroutine != null) StopCoroutine(_shakeCoroutine);
        _shakeCoroutine = StartCoroutine(ShakeTime(duration));
    }

    private void ResetShake()
    {
        _cameraShake.AmplitudeGain = 0;
        _cameraShake.FrequencyGain = 0;
    }

    private IEnumerator ShakeTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        ResetShake();
    }
}
