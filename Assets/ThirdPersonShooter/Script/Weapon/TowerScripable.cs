using UnityEngine;

[CreateAssetMenu]
public class TowerScripable : ScriptableObject
{
    public CustomTrigger attackRageTrigger;

    public GameObject core, barrel;
    public float turningSpeed = 20f;

    public float fireRange = 7f;
}

