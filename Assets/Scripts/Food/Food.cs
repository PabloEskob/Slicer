using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private Material _material;

    public Material Material => _material;
}