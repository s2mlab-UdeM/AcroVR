using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///	A tool when in VR production
/// </summary>

public class CanvasFollow : MonoBehaviour
{
	/// <summary>
	/// This tool is made for the canvas to follow the camera. It is missing the rotation logic. 
	/// Redundant if Canvas is interactable while attached to VR Camera 
	/// </summary>
	// Variables
	
	[Header("Canvas Following")]
	[Tooltip("Drag/Drop GameObject here")]
	[SerializeField]
	private GameObject toFollow;
	[Tooltip("Toggle feature on/off")]
	public bool isFacingCameraSync = true;

	[SerializeField]
	private float offsetX = 2f;
	[SerializeField]
	private float offsetY = 3f;
	[SerializeField]
	private float offsetZ = -10f;
	[SerializeField]
	private float offsetRotationX = 0f;
	[SerializeField]
	private float offsetRotationY = 0f;
	[SerializeField]
	private float offsetRotationZ = 0f;

	// Read only Variables
	private Vector3 offset;
	private float gameObjectPositionX = 0;
	private float gameObjectPositionY = 0;
	private float gameObjectPositionZ = 0;

	void Start()
	{
	}

	void LateUpdate()
	{
		EstablishingValues();
		CanvasOffset();
	}


	///===///  Canvas follow functions
	#region		<-- TOP

	/// Canvas new position
	void EstablishingValues()
	{
		//CanvasOffset();
		//transform.position = offset;
		isFacingCameraSync = toFollow.GetComponent<CameraFollow>().isFacingCamera;

		//toFollow.transform.position = ToolBox.GetInstance().GetManager<DrawManager>().avatarVector3;

		gameObjectPositionX = toFollow.transform.position.x;
		gameObjectPositionY = toFollow.transform.position.y;
		gameObjectPositionZ = toFollow.transform.position.z;
	}

	/// Offset position values
	void CanvasOffset()
	{
		///// Simplify reference
		//gameObjectPositionX = toFollow.gameObject.transform.position.x;
		//gameObjectPositionY = toFollow.gameObject.transform.position.y;
		//gameObjectPositionZ = toFollow.gameObject.transform.position.z;

		///// New Global Vector3 offset
		//offset = new Vector3(gameObjectPositionX + offsetX, gameObjectPositionY + offsetY, gameObjectPositionZ + offsetZ);

		transform.position = offset;
		float rotationY = 0f;
		float newOffsetZ = offsetZ;

		if (!isFacingCameraSync)
		{
			newOffsetZ = -offsetZ;
			rotationY = -180f;
		}
		transform.position = new Vector3(gameObjectPositionX + offsetX, gameObjectPositionY + offsetY, gameObjectPositionZ + newOffsetZ);
		transform.rotation = Quaternion.Euler(0f + offsetRotationX, rotationY + offsetRotationY, 0f + offsetRotationZ);

	}

	#endregion		<-- BOTTOM
}