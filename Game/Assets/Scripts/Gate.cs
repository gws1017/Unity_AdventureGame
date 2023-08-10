using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private Collider GateCollider;
    public GameObject GateVisual;

    public float OpenDuration = 2f;
    public float OpenTargetY = -1.5f;

    private void Awake()
    {
        GateCollider = GetComponent<Collider>();
    }

    IEnumerator OpenGateAnimation()
    {
        float CurrentOpenDuration = 0;
        Vector3 StartPos = GateVisual.transform.position;
        Vector3 TargetPos = StartPos + Vector3.up * OpenTargetY;
        Vector3 CurrentPos = StartPos;

        while (CurrentOpenDuration < OpenDuration)
        {
            CurrentOpenDuration += Time.deltaTime;
            GateVisual.transform.position = Vector3.Lerp(StartPos, TargetPos, CurrentOpenDuration / OpenDuration);
            yield return null;
        }
        GateCollider.enabled = false;
    }

    public void Open()
    {
        StartCoroutine(OpenGateAnimation());
    }
}
