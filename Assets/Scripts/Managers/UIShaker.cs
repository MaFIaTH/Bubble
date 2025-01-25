using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using Cinemachine;
using NaughtyAttributes;

public class UIShaker : MonoBehaviour
{
    public Transform uiElement;   // Assign your UI element here
    public Transform shakeProxy;  // Assign the proxy GameObject here
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = uiElement.localPosition;
    }

    void Update()
    {
        // Apply the shake effect from the proxy's movement
        uiElement.localPosition = initialPosition + (Vector3)shakeProxy.localPosition;
    }

    [Button("Trigger Shake")]
    private void TestShake()
    {
        TriggerShake(shakeProxy.GetComponent<CinemachineImpulseSource>());
    }
    // Call this method to trigger the shake manually
    public void TriggerShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulse();
    }
}

