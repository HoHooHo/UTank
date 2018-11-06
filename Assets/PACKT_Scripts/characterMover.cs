using UnityEngine;
using System.Collections;

public class characterMover : MonoBehaviour {
	public float moveSpeed = 6.0F;
	//public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	private Vector3 moveDirection = Vector3.zero;
	public bool f;
	public bool b;
	public bool l;
	public bool r;
	public float horizMove;
	public float vertMove;

	void Update() 
	{
		CharacterController controller = GetComponent<CharacterController>();
		if (controller.isGrounded) {
			//if(f && vertMove <= 1f)
			//{
			//	vertMove -= 0.1f * Time.deltaTime;
			//}
			//if(b && vertMove >= -1f)
			//{
			//	vertMove += 0.1f * Time.deltaTime;
			//}
			//if(l && horizMove >= -1f)
			//{
			//	horizMove -= 0.1f * Time.deltaTime;
			//}
			//if(r && horizMove <= 1f)
			//{
			//	horizMove += 0.1f * Time.deltaTime;
			//}
			//moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			//moveDirection = new Vector3(Input.GetAxis("Horizontal"), -Input.GetAxis("Vertical"), 0);
			moveDirection = new Vector3(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0);
			//moveDirection = new Vector3(horizMove, vertMove, 0);
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= moveSpeed;
			//if (Input.GetButton("Jump"))
			//	moveDirection.y = jumpSpeed;

		}
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
	}
	/*void Forward()
	{
		f = true;
		b = false;
		l = false;
		r = false;
	}
	void Back()
	{
		f = false;
		b = true;
		l = false;
		r = false;
	}
	void Left()
	{
		f = false;
		b = false;
		l = true;
		r = false;
	}
	void Right()
	{
		f = false;
		b = false;
		l = false;
		r = true;
	}*/
}
