using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyVFXManager : MonoBehaviour
{
    public VisualEffect FootStep;
    public VisualEffect AttackVFX;
    public VisualEffect HitSplashVFX;

    public ParticleSystem HitParticle;
    public void BurstFootStep()
    {
        FootStep.SendEvent("OnPlay");
    }

    public void PlayAttackVFX()
    {
        AttackVFX.SendEvent("OnPlay");
    }

    public void PlayHitEffect(Vector3 AttackerPos)
    {
        Vector3 ForceForward = transform.position - AttackerPos;
        ForceForward.Normalize();
        ForceForward.y = 0f;

        HitParticle.transform.rotation = Quaternion.LookRotation(ForceForward);
        HitParticle.Play();

        Vector3 SplashPos = transform.position;
        SplashPos.y += 2f;
        VisualEffect NewSplashVFX = Instantiate(HitSplashVFX,SplashPos,Quaternion.identity);
        NewSplashVFX.SendEvent("OnPlay");
        Destroy(NewSplashVFX,10f);
    }
}
