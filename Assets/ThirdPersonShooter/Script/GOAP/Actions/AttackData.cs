using CrashKonijn.Goap.Classes.References;
using UnityEngine;

namespace ThirdPersonShooter.Script.GOAP.Actions
{
    public class AttackData : CommonData
    {
        public static readonly int Attack = Animator.StringToHash("Attack");

        [GetComponent] public Animator Animator { get; set; }
    }
}