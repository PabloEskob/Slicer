using UnityEngine;
using DG.Tweening;
using TMPro;

public class DisplayPoint : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _scale;
    [SerializeField] private float _time;
    [SerializeField] private Point point;
    
    private Vector3 _startScale;
    private static readonly int Flight = Animator.StringToHash("Flight");

    private void OnEnable()
    {
        point.CountChanged += Changed;
        point.CountReset += FadeCombo;
    }

    private void OnDisable()
    {
        point.CountChanged -= Changed;
        point.CountReset -= FadeCombo;
    }

    private void Start()
    {
        _startScale = _text.transform.localScale;
    }
    
    private void Changed(int combo)
    {
        _animator.SetBool(Flight,false);
        _text.text = "+" + combo;
        _text.transform.DOScale(_startScale * _scale, _time);
        _text.transform.DOScale(_startScale, _time).SetDelay(_time);
    }

    private void FadeCombo()
    {
       _animator.SetBool(Flight,true);
    }
}
