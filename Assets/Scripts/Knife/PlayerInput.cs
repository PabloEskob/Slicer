using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private KnifeMovement knifeMovement;

    private const string VerticalAxis = "Vertical";

    private void Update()
    {
        knifeMovement.SetHeight(Input.GetAxis(VerticalAxis));
    }
}