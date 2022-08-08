using UnityEngine;
using UnityEngine.Events;

public class Knife : MonoBehaviour
{
    [SerializeField] private KnifeMovement _knifeMovement;

    private Food _food;

    public event UnityAction<GameObject,Material> SlicedFood;
    public event UnityAction AddedPoint;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Finish>())
        {
           _knifeMovement.StopMove();
           other.GetComponent<Finish>().FinishLevel();
        }

        if (other.GetComponent<Food>())
        {
            _food = other.GetComponent<Food>();
            SlicedFood?.Invoke(_food.gameObject,_food.Material);
            AddedPoint?.Invoke();
        }
    }
}
