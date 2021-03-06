using UnityEngine;
using System.Collections;

public class cameraControl : MonoBehaviour {
	 	
	public float moveSpeed = 0.2f;
	Vector3 newPosition;
	
	void Start () {
		newPosition = transform.position;
	}
	
	void Update()
	{
		newPosition = transform.position;
		
		//Moves camera to the clicked point
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
			{
				newPosition = hit.point;
				newPosition.y += 1f;
			}
		}
		transform.position = Vector3.Lerp (transform.position,newPosition,moveSpeed*Time.deltaTime);
		
		if(Input.GetKey(KeyCode.E))//rotates camera to the right
		{
			transform.Rotate(new Vector3(0,moveSpeed *10* Time.deltaTime,0));
		}
		if(Input.GetKey(KeyCode.R))//rotates camera to the left
		{
			transform.Rotate(new Vector3(0,-moveSpeed *10* Time.deltaTime,0));
		}
		
		if(Input.GetKey(KeyCode.RightArrow))//moves camera to the right
		{
			newPosition = transform.position + new Vector3(moveSpeed * Time.deltaTime,0,0);
			//transform.Translate(new Vector3(moveSpeed * Time.deltaTime,0,0));
		}
		if(Input.GetKey(KeyCode.LeftArrow))//moves camera to the left
		{
			newPosition = transform.position + new Vector3(-moveSpeed * Time.deltaTime,0,0);
			//transform.Translate(new Vector3(-moveSpeed * Time.deltaTime,0,0));
		}
		if(Input.GetKey(KeyCode.DownArrow))//moves camera back
		{
			newPosition = transform.position + new Vector3(0, 0,-moveSpeed * Time.deltaTime);
			//transform.Translate(new Vector3(0,0, -moveSpeed * Time.deltaTime));
		}
		if(Input.GetKey(KeyCode.UpArrow))// moves the camera to the front
		{
			newPosition = transform.position + new Vector3(0, 0, moveSpeed * Time.deltaTime);
			//transform.Translate(new Vector3(0,0, moveSpeed * Time.deltaTime));
		}
		if(Input.GetKey(KeyCode.D))//moves the camera down 
		{
			newPosition = transform.position + new Vector3(0, -moveSpeed * Time.deltaTime,0);
			//transform.Translate(new Vector3(0,-moveSpeed * Time.deltaTime, 0));
		}
		if(Input.GetKey(KeyCode.S))//moves camera up
		{
			newPosition = transform.position + new Vector3(0, moveSpeed * Time.deltaTime,0);
			//transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0));
		}
		
		Vector3 temp = Vector3.Lerp (transform.position,newPosition,moveSpeed*Time.deltaTime);
		Vector3 temp1 = temp - transform.position;
		transform.Translate(temp1);
		//transform.position = Vector3.Lerp (transform.position,newPosition,moveSpeed*Time.deltaTime);
	}
	
	void MoveCamera(float x, float y, float z) { 
		transform.position += new Vector3(transform.position.x +x, transform.position.y+y, transform.position.z+z);

	}

}

