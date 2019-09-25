using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// =================================================================================================================================================================
/// <summary> Contrôle de certaines interactions de la souris avec les objets (GameObjects). </summary>

public class MouseManager : MonoBehaviour
{
	public static MouseManager Instance;
	public Canvas canvas;

	// =================================================================================================================================================================
	/// <summary> Initialisation du script. </summary>

	void Start()
	{
		Instance = this;
	}

	// =================================================================================================================================================================
	/// <summary> Vérifier si la souris est au-dessus (retourne TRUE) ou à côté (retourne FALSE) de l'objet spécifié. </summary>

	public bool IsOnGameObject(GameObject gameObject)
	{
		Vector2 inputMousePos = Input.mousePosition;
		Vector3[] menuPos = new Vector3[4];
		gameObject.GetComponent<RectTransform>().GetWorldCorners(menuPos);
		Vector3[] gameObjectPos = new Vector3[2];
		gameObjectPos[0] = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, menuPos[0]);
		gameObjectPos[1] = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, menuPos[2]);
		return (gameObject.activeSelf && inputMousePos.x >= gameObjectPos[0].x && inputMousePos.x <= gameObjectPos[1].x && inputMousePos.y >= gameObjectPos[0].y && inputMousePos.y <= gameObjectPos[1].y);
	}
}
