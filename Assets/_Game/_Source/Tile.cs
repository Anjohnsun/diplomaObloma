using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject _object;
    [SerializeField] private GameObject _availableTileSprite;

    GridManager _gridManager;


    public void SetAvailable(bool v)
    {
        _availableTileSprite.SetActive(v);
    }



    public bool IsFree()
    {
        return _object == null;
    }

    private void OnMouseDown()
    {
        if (_availableTileSprite.activeInHierarchy)
        {
            //_gridManager.OnAvailableTileClicked();
        }
    }
}
