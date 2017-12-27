using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

	public	Transform	target;
	private	Vector3		offsest;

	void	Start()
	{
        //this.offsest	= GetComponent<Transform>().position - target.position;
        this.offsest    = GetComponent<Transform>().position - Vector3.zero;
    }

	void	Update()
	{
		GetComponent<Transform>().position = target.position + this.offsest;
	}
}
