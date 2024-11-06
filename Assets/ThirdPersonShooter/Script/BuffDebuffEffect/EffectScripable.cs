using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class EffectScripable : ScriptableObject
{
    public String effectName;
    [Tooltip("Set Tick Rate 0 for fix value effect")]
    public float tickRate;
    public float maxLifeTime;

    [HideInInspector] public float lifeTimeLeft;

    [HideInInspector] public Stopwatch durationTimer = new();
    [HideInInspector] public MonoBehaviour activeMonoBehaviour;

    public void StartEffect(MonoBehaviour mono)
    {
        activeMonoBehaviour = mono;
        lifeTimeLeft = maxLifeTime;
        durationTimer.Reset();
        OverrideSetup();
        if (tickRate != 0)
            //if tick timing effect like damage overtime or heal over time 
            mono.StartCoroutine(TickingEffect());
        else
            //if life time value effect
            mono.StartCoroutine(LifeTimeEffect());
    }

    public void ResetEffect()
    {
        lifeTimeLeft = maxLifeTime;
        durationTimer.Reset();
        durationTimer.Start();
    }

    public IEnumerator TickingEffect()
    {
        float timePerTick = 1 / tickRate;

        durationTimer.Start();
        while (durationTimer.IsRunning && durationTimer.Elapsed.Seconds < maxLifeTime)
        {
            lifeTimeLeft -= timePerTick;
            OverrideEffect();
            yield return new WaitForSeconds(timePerTick);
        }
        durationTimer.Reset();
        yield break;
    }

    public IEnumerator LifeTimeEffect()
    {
        durationTimer.Start();
        //not yet 
        // while (durationTimer.IsRunning && durationTimer.Elapsed.Seconds < maxLifeTime)
        // {
        //     OverrideEffect();
        //     yield return new WaitForSeconds(timePerTick);
        // }
        durationTimer.Reset();
        yield break;
    }

    // override this for setup effect
    public virtual void OverrideSetup() { }

    // override this for effect action
    public virtual void OverrideEffect() { }
}



