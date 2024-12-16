using System;
using System.Collections;
using UnityEngine;

namespace ThirdPersonShooter.Scripts
{
    public static class WaitFunction
    {
        public static void Wait(this MonoBehaviour mono, float delay, Action action)
        {
            mono.StartCoroutine(InvokeAction(delay, action));
        }

        private static IEnumerator InvokeAction(float delay, Action action)
        {
            if (delay >= 0) yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
    }
}