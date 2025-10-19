using UnityEngine;

public class PortraitAspectRatio : MonoBehaviour
{
    void Start()
    {
        // Принудительная установка портретного режима
        Screen.orientation = ScreenOrientation.Portrait;
        
        // Установка разрешения
        Screen.SetResolution(1080, 1920, true);
    }
}