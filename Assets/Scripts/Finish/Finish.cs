using System.Collections;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private SceneLoading sceneLoading;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private float _timeLoading;

    private IEnumerator _coroutine;

    private void Start()
    {
        _coroutine = SceneLoading();
    }

    public void FinishLevel()
    {
        StartCoroutine(_coroutine);
    }

    private IEnumerator SceneLoading()
    {
        var waitForSecond = new WaitForSeconds(_timeLoading);
        _particleSystem.Play();
        yield return waitForSecond;
        sceneLoading.Loading();
    }
}

   

