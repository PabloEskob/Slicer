using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField] private float _speedSlope;
    [SerializeField] private float _minTiltAngle;
    [SerializeField] private float _maxTiltAngle;
    [SerializeField] private float _timeReturnAngle;

    private float _offset;
    private Quaternion _intialRotate;

    private void Start()
    {
        _intialRotate = transform.localRotation;
    }

    public void ChangeAngle(float verticalMove)
    {
        if (verticalMove == 0)
        {
            _offset = 0;
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, _intialRotate,
                _timeReturnAngle * Time.deltaTime);
        }
        else
        {
            _offset -= verticalMove * _speedSlope * Time.deltaTime;
            _offset = Mathf.Clamp(_offset, _minTiltAngle, _maxTiltAngle);
            transform.localRotation = Quaternion.Euler(0, _offset, 0);
        }
    }
}