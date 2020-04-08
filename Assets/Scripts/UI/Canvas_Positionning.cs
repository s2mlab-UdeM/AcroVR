using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///	 
/// </summary>

public class Canvas_Positionning : MonoBehaviour
{
	// Variables
	[Header("Canvas Follows")]
	[Tooltip("Drag/Drop Camera here")]
	[SerializeField]
	private GameObject toFollow;
	[SerializeField]
	private float offsetX = 0f;
	[SerializeField]
	private float offsetY = 0f;
	[SerializeField]
	private float offsetZ = 8.75f;

	// Read only Variables
	private Vector3 offset;
	private float CameraPositionX = 0;
	private float CameraPositionY = 0;
	private float CameraPositionZ = 0;

	void Start()
	{

	}

	void Update()
	{
		CanvasPosition();
	}


	///===///  Canvas follow functions
	#region		<-- TOP

	/// Canvas new position
	void CanvasPosition()
	{
		CanvasOffset();
		transform.position = offset;
	}

	/// Offset position values
	void CanvasOffset()
	{
		/// toFollow values
		CameraPositionX = toFollow.gameObject.transform.position.x;
		CameraPositionY = toFollow.gameObject.transform.position.y;
		CameraPositionZ = toFollow.gameObject.transform.position.z;

		/// New Global Vector3 offset
		offset = new Vector3(CameraPositionX + offsetX, CameraPositionY + offsetY, CameraPositionZ + offsetZ);

	}

	#endregion		<-- BOTTOM
}