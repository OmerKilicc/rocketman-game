using UnityEngine;
using UnityEngine.Serialization;

public class PlayerHierarchyController : MonoBehaviour
{
    [SerializeField] private GameObject tipOfStick;
    [SerializeField] private Vector3 playerStartPosition = new Vector3(0, 26.35f, 0);

    public void MakeItSticksChild()
    {
        if (tipOfStick == null)
        {
            Debug.LogError("No tip of stick");
            return;
        }

        transform.SetParent(tipOfStick.transform);
        transform.position = tipOfStick.transform.position;
    }

    public void MakeItFree()
    {
        transform.SetParent(null);
        transform.position = playerStartPosition;
    }
}