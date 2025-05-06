using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
	float orthoSize;
	[SerializeField] Cinemachine.CinemachineVirtualCamera _virtualCam;
	private void Start()
	{
		orthoSize = _virtualCam.m_Lens.OrthographicSize;
	}

	float moveSpeed = 15f;
	float zoomSpeed = 2f;

	private void Update()
	{
		Vector2 moveVel = new Vector2()
		{
			x = Input.GetAxisRaw("Horizontal"),
			y = Input.GetAxisRaw("Vertical"),
		}.normalized * moveSpeed * Time.deltaTime;

		transform.position += (Vector3)moveVel;

		float zoomVel = zoomSpeed * -Input.mouseScrollDelta.y;
		orthoSize += zoomVel;

		float minOrthoSize = 7f, maxOrthoSize = 16f;
		orthoSize = Mathf.Clamp(orthoSize, minOrthoSize, maxOrthoSize);

		_virtualCam.m_Lens.OrthographicSize = orthoSize;
	}

}
