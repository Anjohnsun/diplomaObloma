using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HintHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _hintField;
    [SerializeField] private float _uiCheckDistance = 10f;

    private void Update()
    {

        if (EventSystem.current.IsPointerOverGameObject())
        {
            var pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (var result in results)
            {
                var hintUser = result.gameObject.GetComponentInParent<IHintUser>();
                if (hintUser != null)
                {
                    _hintField.text = hintUser.GetHintText();
                    return;
                }
            }
        }
        else
        {
            _hintField.text = "";
        }

            var hit = Physics2D.Raycast(
                Camera.main.ScreenToWorldPoint(Input.mousePosition),
                Vector2.zero
            );

        if (hit.collider != null && hit.collider.TryGetComponent<IHintUser>(out var gameHintUser))
        {
            _hintField.text = gameHintUser.GetHintText();
        }
        else
        {
            _hintField.text = "";
        }
    }
    /*    

        private void Update()
        {
            var hit = Physics2D.Raycast(
                Camera.main.ScreenToWorldPoint(Input.mousePosition),
                Vector2.zero
            );

            if (hit.collider != null && hit.collider.TryGetComponent<IHintUser>(out var hintUser))
            {
                _hintField.text = hintUser.GetHintText();
            } else
            {
                _hintField.text = "";
            }
        }*/
}
