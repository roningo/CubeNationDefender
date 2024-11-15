using UnityEngine;

//player can't place tower on exit from turret, turn off and on ThirdPersonShooterController solve this case
namespace ThirdPersonShooter.Script
{
    public class AwakePlayer : MonoBehaviour
    {
        private ThirdPersonShooterController _thirdPersonShooterController;

        // Start is called before the first frame update
        private void Awake()
        {
            _thirdPersonShooterController = GetComponentInParent<ThirdPersonShooterController>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (!_thirdPersonShooterController.enabled)
                _thirdPersonShooterController.enabled = true;
        }
    }
}