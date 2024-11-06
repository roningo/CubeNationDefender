using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class EffectScripable : ScriptableObject
{
    public string effectName;

    [Tooltip("Set Tick Rate 0 for fix value effect")]
    public float tickRate;

    public float maxLifeTime;

    [HideInInspector] public float lifeTimeLeft;

    private readonly Stopwatch _durationTimer = new();
    [HideInInspector] public MonoBehaviour activeMonoBehaviour;

    public void StartEffect(MonoBehaviour mono)
    {
        activeMonoBehaviour = mono;
        lifeTimeLeft = maxLifeTime;
        _durationTimer.Reset();
        OverrideSetup();
        mono.StartCoroutine(tickRate != 0 ? TickingEffect() : LifeTimeEffect());
    }

    public void ResetEffect()
    {
        lifeTimeLeft = maxLifeTime;
        _durationTimer.Reset();
        _durationTimer.Start();
    }

    //if tick timing effect like damage overtime or heal over time 
    private IEnumerator TickingEffect()
    {
        float timePerTick = 1 / tickRate;

        _durationTimer.Start();
        while (_durationTimer.IsRunning && _durationTimer.Elapsed.TotalSeconds < maxLifeTime)
        {
            lifeTimeLeft -= timePerTick;
            OverrideEffect();
            yield return new WaitForSeconds(timePerTick);
        }

        _durationTimer.Reset();
        yield break;
    }

    //if life time value effect
    private IEnumerator LifeTimeEffect()
    {
        _durationTimer.Start();
        //not yet 
        // while (durationTimer.IsRunning && durationTimer.Elapsed.Seconds < maxLifeTime)
        // {
        //     OverrideEffect();
        //     yield return new WaitForSeconds(timePerTick);
        // }
        _durationTimer.Reset();
        yield break;
    }

    // override this for setup effect
    protected virtual void OverrideSetup()
    {
    }

    // override this for effect action
    protected virtual void OverrideEffect()
    {
    }
}