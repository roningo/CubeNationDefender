using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EffectSystem : MonoBehaviour
{
    public List<EffectScripable> effectList;

    private void Update()
    {
        if (!effectList.Any(e => e.lifeTimeLeft <= 0)) return; 
        
        //remove outdated effect
        List<EffectScripable> removeList = effectList.FindAll(e => e.lifeTimeLeft <= 0);
        foreach (EffectScripable removeEffect in removeList)
        {
            //need to destroy just in case
            Destroy(removeEffect);
            effectList.Remove(removeEffect);
        }
    }

    public void AddEffect(EffectScripable effect)
    {
        // to make each EffectScripable invidual 
        EffectScripable newEffect = Instantiate(effect);
        int id = GetEffectIndex(newEffect);

        if (id < 0)
        {
            // add new effect
            newEffect.StartEffect(this);
            effectList.Add(newEffect);
        }
        else
            //reset effect time if same effect already exist
            effectList[id].ResetEffect();
    }

    //check exits effect
    private int GetEffectIndex(EffectScripable newEffect)
    {
        if (effectList.Count <= 0) return -1;
        else if (effectList.Any(e => e.name.CompareTo(newEffect.name) == 0))
        {
            EffectScripable e = effectList.Find(e => e.name.CompareTo(newEffect.name) == 0);
            return effectList.IndexOf(e);
        }

        return -1;
    }
}