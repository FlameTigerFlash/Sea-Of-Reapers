using Unity.VisualScripting;
using UnityEngine;
using Zenject.Asteroids;

public class WaterFollow : MonoBehaviour
{
    [field: SerializeField] public Transform ObjectToFollow { get; private set; }

    private void Update()
    {
        if (ObjectToFollow == null)
        {
            return;
        }

        Vector3 newPos = new Vector3(ObjectToFollow.position.x, transform.position.y, ObjectToFollow.position.z);
        transform.position = newPos;
    }
}
