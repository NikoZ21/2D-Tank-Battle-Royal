using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DdeleteLate : MonoBehaviour
{
    [SerializeField] private InputReader inp;

    void Start()
    {
        inp.OnMoved += HandleMove;
    }

    private void HandleMove(Vector2 movement)
    {
        Debug.Log(movement);
    }
}
