using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class EffectScripableTest : ScriptableObject
{
    public String effectName;
    public float tickRate;
    public float maxLifeTime;

    public float lifeTimeLeft;

    public Stopwatch durrationTimer = new();
    public MonoBehaviour activeMonoBehaviour;

    private bool _coroutineRunning = false;

    public void StartEffect(MonoBehaviour mono)
    {
        activeMonoBehaviour = mono;
        lifeTimeLeft = maxLifeTime;
        durrationTimer.Reset();
        mono.StartCoroutine(TickingEffect());
    }

    public void ResetEffect()
    {
        lifeTimeLeft = maxLifeTime;
        durrationTimer.Reset();
        durrationTimer.Start();
    }

    public IEnumerator TickingEffect()
    {
        _coroutineRunning = true;
        float timePerTick = 1 / tickRate;

        durrationTimer.Start();
        while (durrationTimer.IsRunning && durrationTimer.Elapsed.Seconds < maxLifeTime)
        {
            lifeTimeLeft -= timePerTick;

            // do effect
            // health.ReceivedDamage(damage);
            OverrideEffect();
            
            yield return new WaitForSeconds(timePerTick);
        }
        durrationTimer.Reset();
        _coroutineRunning = false;
        yield break;
    }

    public virtual void OverrideEffect() { }

    private void OnDestroy()
    {
        if (this && _coroutineRunning) activeMonoBehaviour.StopCoroutine(TickingEffect());
    }
}



