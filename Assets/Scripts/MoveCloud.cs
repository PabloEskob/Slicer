using UnityEngine;

public class MoveCloud : MonoBehaviour
{
    [SerializeField] private Cloud _cloud;
    [SerializeField] private float _speed;

    private void Update()
    {
        _cloud.transform.Translate(Vector3.left*_speed*Time.deltaTime);
    }
}
