namespace ThirdPersonShooter.Scripts
{
    public class UtilsVariables
    {
        public static ActiveState CurrentActiveState = ActiveState.Equipment;
        public static bool IsGamePaused = false;

        public enum ActiveState
        {
            Equipment,
            Placement,
            Removing
        } 
    }
}
