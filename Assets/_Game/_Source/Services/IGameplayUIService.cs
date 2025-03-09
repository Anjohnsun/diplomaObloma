using UnityEngine;

public interface IGameplayUIService
{
    void ShowGameplayInterface();
    float GetStartAnimDuration();
    void HideGameplayInterface();
    void ShowEscMenu();
    void HideEscMenu();
    void UpdatePlayerHP();
    void UpdatePlayerScore();
}
