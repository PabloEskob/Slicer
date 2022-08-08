using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Point : MonoBehaviour
{
    [SerializeField] private Knife _knife;
    [SerializeField] private float _scoreResetPoint;
    
    private int _count;
    private Coroutine _coroutine;

    public event UnityAction<int> CountChanged;
    public event UnityAction CountReset;
    
    private void OnEnable()
    {
        _knife.AddedPoint += Add;
    }
    
    private void OnDisable()
    {
        _knife.AddedPoint -= Add;
    }
    
    private void Add()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _count++;
        _coroutine = StartCoroutine(Reset());
        CountChanged?.Invoke(_count);
    }
    
    private IEnumerator Reset()
    {
        var waitForSecond = new WaitForSeconds(_scoreResetPoint);
        yield return waitForSecond;
        _count = 0;
        CountReset?.Invoke();
    }
}