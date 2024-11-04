using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu]
public class EffectScripable : ScriptableObject
{
    public String effectName;
    public float damage;
    public float tickRate;
    public float maxLifeTime;

    public float lifeTimeLeft;

    private Stopwatch _durationTimer = new();
    private IEnumerator _effectCoroutine;
    private MonoBehaviour _activeMonoBehaviour;

    private bool _coroutineRunning = false;

    public void StartEffect(MonoBehaviour mono, Health health)
    {
        _effectCoroutine = Effect(health);
        _activeMonoBehaviour = mono;
        lifeTimeLeft = maxLifeTime;
        _durationTimer.Reset();
        mono.StartCoroutine(_effectCoroutine);
    }

    public void ResetEffect()
    {
        lifeTimeLeft = maxLifeTime;
        _durationTimer.Reset();
        _durationTimer.Start();
    }

    public IEnumerator Effect(Health health)
    {
        _coroutineRunning = true;
        float timePerTick = 1 / tickRate;

        _durationTimer.Start();
        while (_durationTimer.IsRunning && _durationTimer.Elapsed.Seconds < maxLifeTime)
        {
            lifeTimeLeft -= timePerTick;
            health.ReceivedDamage(damage);
            yield return new WaitForSeconds(timePerTick);
        }
        _durationTimer.Reset();
        _coroutineRunning = false;
        yield break;
    }

    private void OnDestroy()
    {
        if (this && _coroutineRunning) _activeMonoBehaviour.StopCoroutine(_effectCoroutine);
    }
}



