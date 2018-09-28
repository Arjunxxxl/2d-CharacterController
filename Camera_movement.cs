using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_movement : MonoBehaviour {

	public Transform player;
	public Vector3 offset;
	public float smooth = 5f;



	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		

		Vector3 newpos = player.position - offset;
		transform.position = Vector3.Lerp(transform.position, newpos, smooth*Time.deltaTime);
	}
}
