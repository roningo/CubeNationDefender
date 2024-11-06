using UnityEngine;

//player can't place tower on switch from turret, turn off and on ThirdPersonShooterController solve this case
public class AwakePlayer : MonoBehaviour
{
    private ThirdPersonShooterController _thirdPersonShooterController;

    // Start is called before the first frame update
    void Awake()
    {
        _thirdPersonShooterController = GetComponentInParent<ThirdPersonShooterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_thirdPersonShooterController.enabled)
            _thirdPersonShooterController.enabled = true;
    }
}