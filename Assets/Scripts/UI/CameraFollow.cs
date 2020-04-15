using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///	 
/// </summary>

public class CameraFollow : MonoBehaviour
{
	// Variables
	[Header("ToolTipSection")]
	[Tooltip("Toggle feature on/off")]
	public bool isFacingCamera = false;

	[SerializeField]
	private float offsetX = -1.5f;
	[SerializeField]
	private float offsetY = 1.4f;
	[SerializeField]
	private float offsetZ = -3.8f;
	[SerializeField]
	private float offsetRotationX = 0f;
	[SerializeField]
	private float offsetRotationY = 0f;
	[SerializeField]
	private float offsetRotationZ = 0f;

	// Read only Variables
	private Vector3 toFollowVector3;
	private Vector3 offset;
	private float gameObjectPositionX = 0f;
	private float gameObjectPositionY = 0f;
	private float gameObjectPositionZ = 0f;

	void Start()
	{
		EstablishingValues();
	}

	void Update()
	{
		CameraPosition();
	}

	///===///  Follow function
	#region		<-- TOP

	/// Setting initial values
	private void EstablishingValues()
	{
		toFollowVector3 = ToolBox.GetInstance().GetManager<DrawManager>().avatarVector3;

		gameObjectPositionX = toFollowVector3.x;
		gameObjectPositionY = toFollowVector3.y;
		gameObjectPositionZ = toFollowVector3.z;
	}

	/// Modifying initial position
	void CameraPosition()
	{
		transform.position = offset;
		float rotationY = 0f;
		float newOffsetZ = offsetZ;

		if (isFacingCamera == false)
		{
			newOffsetZ = -offsetZ;
			rotationY = -180f;
		}
		transform.position = new Vector3(gameObjectPositionX + offsetX, gameObjectPositionY + offsetY, gameObjectPositionZ + newOffsetZ);
		transform.rotation = Quaternion.Euler(0f + offsetRotationX, rotationY + offsetRotationY, 0f + offsetRotationZ);
	}

	#endregion		<-- BOTTOM
}