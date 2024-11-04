using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu]
public class DamageOverTimeEffect : EffectScripableTest
{
    public float damage;

    public override void OverrideEffect() {
        Health health = activeMonoBehaviour.GetComponent<Health>();
    }
}



