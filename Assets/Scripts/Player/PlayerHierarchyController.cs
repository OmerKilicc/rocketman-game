using UnityEngine;

public class PlayerHierarchyController : MonoBehaviour
{
    [SerializeField] private GameObject _tipOfStick;
    [SerializeField] private Vector3 _playerStartPosition = new Vector3(0, 26.35f, 0);

    public void MakeItSticksChild()
    {
        if (_tipOfStick == null)
        {
            Debug.LogError("No tip of stick");
            return;
        }

        transform.SetParent(_tipOfStick.transform);
        transform.position = _tipOfStick.transform.position;
    }

    public void MakeItFree()
    {
        transform.SetParent(null);
        transform.position = _playerStartPosition;
    }
}