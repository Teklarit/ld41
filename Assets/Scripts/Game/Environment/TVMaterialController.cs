using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVMaterialController : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f;

    private float _factor1 = 1000;

    private Material _material;

	void Start ()
    {
        _material = GetComponent<Renderer>().material;
	}
	
	void Update ()
    {
        _factor1 += Time.deltaTime * _speed;
        if (_factor1 > 1500)
            _factor1 -= 500;

        _material.SetFloat("_Factor1", _factor1);
	}
}
