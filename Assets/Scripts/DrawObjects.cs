using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// =================================================================================================================================================================
/// <summary> Fonctions utilisées pour dessiner des objets (formes), comme ligne, cercle, ... </summary>

public class DrawObjects : MonoBehaviour
{
	public static DrawObjects Instance;

	float ThetaScale;

	// =================================================================================================================================================================
	/// <summary> Initialisation du script. </summary>

	void Start()
	{
		Instance = this;
		ThetaScale = 0.01f;
	}

	// =================================================================================================================================================================
	/// <summary> Dessiner un cercle. </summary>

	public void Circle(LineRenderer lineRendererObject, float radius, Vector3 center)
	{
		int nLines;
		float Theta = 0f;

		nLines = (int)((1f / ThetaScale) + 1.1f);
		Vector3[] pos = new Vector3[nLines];
		lineRendererObject.positionCount = nLines;
		for (int i = 0; i < nLines; i++)
		{
			float x = radius * Mathf.Cos(Theta);
			float y = radius * Mathf.Sin(Theta);
			pos[i] = center + new Vector3(x, y, 0);
			Theta += (2.0f * Mathf.PI * ThetaScale);
		}
		lineRendererObject.SetPositions(pos);
	}

	// =================================================================================================================================================================
	/// <summary> Dessiner une ligne. </summary>

	public void Line(LineRenderer lineRendererObject, Vector3 position1, Vector3 position2)
	{
		Vector3[] pos = new Vector3[2];
		lineRendererObject.positionCount = 2;
		pos[0] = position1;
		pos[1] = position2;
		lineRendererObject.SetPositions(pos);
	}

	// =================================================================================================================================================================
	/// <summary> Dessiner un triangle. </summary>

	public void Triangle(LineRenderer lineRendererObject, Vector3 position1, Vector3 position2, Vector3 position3)
	{
		Vector3[] pos = new Vector3[4];
		lineRendererObject.positionCount = 4;
		pos[0] = position1;
		pos[1] = position2;
		pos[2] = position3;
		pos[3] = position1;
		lineRendererObject.SetPositions(pos);
	}

	// =================================================================================================================================================================
	/// <summary> Effacer un objet. </summary>

	public void Delete(LineRenderer lineRendererObject)
	{
		lineRendererObject.positionCount = 0;
		//Destroy(lineRendererObject);
	}
}
