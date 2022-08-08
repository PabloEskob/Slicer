using UnityEngine;

public class KnifeMovement : MonoBehaviour
{
    [SerializeField] private float _speedForward;
    [SerializeField] private float _speedVertical;
    [SerializeField] private float _maxHeight;
    [SerializeField] private float _minHeight;
    [SerializeField] private Blade _blade;

    private float _verticalMove;
    private bool _canMove = true;

    private void Update()
    {
        Move();
    }

    public void SetHeight(float getAxis)
    {
        _verticalMove = getAxis;
    }

    public void StopMove()
    {
        _canMove = false;
    }

    private void Move()
    {
        if (_canMove)
        {
            MoveForward();
            MoveHorizontal();
        }
    }

    private void MoveForward()
    {
        Vector3 offset = Vector3.left * _speedForward * Time.deltaTime;
        transform.Translate(offset, Space.World);
    }

    private void MoveHorizontal()
    {
        if (transform.position.y <= _maxHeight && transform.position.y >= _minHeight)
        {
            _blade.ChangeAngle(_verticalMove);
            float offset = _verticalMove * _speedVertical * Time.deltaTime;
            offset = Mathf.Clamp(offset, _minHeight - transform.position.y, _maxHeight - transform.position.y);
            transform.Translate(Vector3.up * offset, Space.World);
        }
    }
}