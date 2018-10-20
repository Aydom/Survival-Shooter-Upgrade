using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {

	public Transform Target;
	public float minModifier = 7;
	public float maxModifier = 11;

	Vector3 _velocity = Vector3.zero;
	bool _isFollowing = false;

	public void StartFollowing()
	{
		_isFollowing = true;
	}

	void Update()
	{
		if (_isFollowing) {
			transform.position = Vector3.SmoothDamp (transform.position, Target.position, ref _velocity, Time.deltaTime * Random.Range (minModifier, maxModifier));
		}
	}

}
