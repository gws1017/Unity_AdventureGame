using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerVFXManager : MonoBehaviour
{
    public VisualEffect FootStep;
    public VisualEffect Slash;
    public ParticleSystem Sword01;
    public void Update_FootStep(bool state)
    {
        if (state)
            FootStep.Play();
        else
            FootStep.Stop();
    }

    public void PlaySword01()
    {
        Sword01.Play();
    }

    public void PlaySlash(Vector3 pos)
    {
        Slash.transform.position = pos;
        Slash.Play();
    }
}
