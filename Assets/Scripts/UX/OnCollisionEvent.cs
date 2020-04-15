using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///	 
/// </summary>

public class OnCollisionEvent : MonoBehaviour
{
	// Variables
	[SerializeField]
	private bool rightContactCheck = false;
	[SerializeField]
	private bool leftContactCheck = false;

	[SerializeField]
	private GameObject contactBoard;
	[SerializeField]
	private Material m_DefaultCheck;
	[SerializeField]
	private Material m_1Contact;
	[SerializeField]
	private Material m_2Contact;
	private Renderer newRenderer;
	private string colliderNameEnter;
	private string colliderNameExit;


	void Start()
	{
		/// Declarations
		#region		<-- TOP

		//transform.parent.gameObject.GetComponent<MeshRenderer>().material = newMaterialRef;
		newRenderer = contactBoard.GetComponent<Renderer>();
		CheckNoContact();

		#endregion		<-- BOTTOM

	}


	///===  OnTriggerEnter && OnTriggerExit logic
	#region		<-- TOP

	/// OnTriggerEnter
	void OnTriggerEnter(Collider collider)
	{
		colliderNameEnter = collider.gameObject.name;

		if (colliderNameEnter == "ColliderCheckLeft")
		{
			leftContactCheck = true;

			if (rightContactCheck == true)
			{
				CheckBothContact();
			}
			else
			{
				CheckSingleContact();
			}
		}

		if (colliderNameEnter == "ColliderCheckRight")
		{
			rightContactCheck = true;

			if (leftContactCheck == true)
			{
				CheckBothContact();
			}
			else
			{
				CheckSingleContact();
			}
		}
	}

	///  OnTriggerExit
	void OnTriggerExit(Collider collider)
	{
		colliderNameExit = collider.gameObject.name;

		if (colliderNameExit == "ColliderCheckLeft")
		{
			leftContactCheck = false;
		}

		if (colliderNameExit == "ColliderCheckRight")
		{
			rightContactCheck = false;
		}

		if (rightContactCheck == false || leftContactCheck == false)
		{
			CheckSingleContact();
		}

		if (rightContactCheck == false && leftContactCheck == false)
		{
			newRenderer.sharedMaterial = m_DefaultCheck;
		}
		
	}

	#endregion		<-- BOTTOM


	///===  Functions for OnTriggerEnter && OnTriggerExit
	#region		<-- TOP

	private void CheckSingleContact()
	{
		newRenderer.sharedMaterial = m_1Contact;

	}

	private void CheckBothContact()
	{
		newRenderer.sharedMaterial = m_2Contact;

	}

	private void CheckNoContact()
	{
		newRenderer.sharedMaterial = m_DefaultCheck;

	}

	#endregion		<-- BOTTOM

	///===  Other checks
	public void OnClickCheckNoContact()
	{
		CheckNoContact();
	}
}