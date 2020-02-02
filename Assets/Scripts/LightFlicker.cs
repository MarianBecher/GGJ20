using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Experimental.Rendering.Universal
{
    public class LightFlicker : MonoBehaviour
    {
        [SerializeField] AnimationCurve flickerCurve;
        [SerializeField] float baseIntensity;
        float randOffset;
        Light2D flickeringLight;
        float timeCounter = 0.0f;

        void Start(){
            randOffset = Random.Range(0.0f, 1.0f);
            flickeringLight = GetComponentInChildren<Light2D>();
        }

        void Update()
        {
            flickeringLight.intensity = baseIntensity * flickerCurve.Evaluate(timeCounter+randOffset);
            timeCounter += Time.unscaledDeltaTime * 0.5f;
            timeCounter %= 1.0f;
        }
    }
}