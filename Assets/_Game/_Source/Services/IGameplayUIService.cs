using UnityEngine;

public interface IGameplayUIService
{
    void ShowGameplayInterface();
    void HideGameplayInterface();
    void ShowEscMenu();
    void HideEscMenu();
    void UpdatePlayerStats(IPawnStats pawnStats);
}
