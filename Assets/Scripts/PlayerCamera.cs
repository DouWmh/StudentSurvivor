using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerCamera : MonoBehaviour
{
    public GameObject target;
    [SerializeField] float shakeTime;
    [SerializeField] float shakeMagnitude;
    private Vector3 shakeOffset;
    private bool isShaking = false;

    Player player;
    [SerializeField] float speed = 10f;
    Volume volume;
    Vignette vignette;
    [SerializeField] float vignetteMagnitude = 0.5f;
    DepthOfField depth;
    ColorAdjustments colorAdjust;
    [SerializeField] float colorFadeSpeed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out depth);
        volume.profile.TryGet(out colorAdjust);
    }
    // Update is called once per frame
    void LateUpdate()
    {
        vignette.intensity.Override((1 - player.GetHpRatio()) * vignetteMagnitude);
        if (target == null)
        {
            return;
        }
        float targetX = target.transform.position.x;
        float targetY = target.transform.position.y;
        float cameraZ = target.transform.position.z - 10f;

        var targetPosition = new Vector3(targetX, targetY, cameraZ);
        transform.position = Vector3.MoveTowards(transform.position + shakeOffset, targetPosition, speed * Time.unscaledDeltaTime);
    }
    public void PauseGame()
    {
        depth.active = true;
    }
    public void UnPauseGame()
    {
        depth.active = false;
    }
    public IEnumerator CameraShake()
    {
        if (!isShaking)
        {
            isShaking = true;
            float shakeTimeLeft = shakeTime;
            while (shakeTimeLeft > 0)
            {
                float xShake = Random.Range(-1f, 1f) * shakeMagnitude;
                float yShake = Random.Range(-1f, 1f) * shakeMagnitude;
                shakeOffset = new Vector3(xShake, yShake, 0);
                shakeTimeLeft -= Time.unscaledDeltaTime;
                yield return null;
            }
            shakeOffset = Vector3.zero;
            isShaking = false;
        }
    }
    public IEnumerator PlayerDeath()
    {
        Time.timeScale = 0;
        float saturation = 0;
        while (saturation > -100)
        {
            colorAdjust.saturation.Override(saturation);
            saturation -= colorFadeSpeed * Time.unscaledDeltaTime;
            yield return null;
        }
        player.GetComponent<Player>().Death();
    }
}
