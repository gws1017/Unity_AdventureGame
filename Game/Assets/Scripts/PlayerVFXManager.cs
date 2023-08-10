using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerVFXManager : MonoBehaviour
{
    public VisualEffect FootStep;
    public VisualEffect Slash;
    public VisualEffect HealVFX;

    public ParticleSystem Sword01;
    public ParticleSystem Sword02;
    public ParticleSystem Sword03;
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

    public void PlaySword02()
    {
        Sword02.Play();
    }

    public void PlaySword03()
    {
        Sword03.Play();
    }

    public void StopSword()
    {
        Sword01.Simulate(0);
        Sword01.Stop();

        Sword02.Simulate(0);
        Sword02.Stop();

        Sword03.Simulate(0);
        Sword03.Stop();
    }

    public void PlaySlash(Vector3 pos)
    {
        Slash.transform.position = pos;
        Slash.Play();
    }

    public void PlayHealVFX()
    {
        HealVFX.Play();
    }
}
