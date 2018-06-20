using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// =================================================================================================================================================================
/// <summary> Fonctions utilisées pour exécuter les animations. </summary>

public class AnimationF : MonoBehaviour
{
	public static AnimationF Instance;
	public GameObject panelAnimator;

	//LineRenderer[] lineStickFigure;
	//LineRenderer lineCenterOfMass;
	//LineRenderer[] lineFilledFigure;

	//bool animateON = false;
	//int frameN = 0;
	//double[] Q;
	//int factorDisplay = 10;

	// =================================================================================================================================================================
	/// <summary> Initialisation du script. </summary>

	void Start ()
	{
		Instance = this;
		//lineStickFigure = null;
		//lineCenterOfMass = null;
		//lineFilledFigure = null;
	}

	// =================================================================================================================================================================
	/// <summary> Exécution du script à chaque frame. </summary>

	void Update()
	{
		//if (!animateON) return;

		//// Affichage de la silhouette à chaque frame (vu qu'on utilise des données déjà enregistrées, la simulation sera plus lente qu'en réalité, car l'exécution d'un
		//// frame d'Unity (via routine Update) est plus lente que un frame des données enregistrées.

		//if (frameN < MainParameters.Instance.numberOfFrames)
		//{
		//	PlayOneFrame();
		//	frameN++;
		//}
		//else
		//	animateON = false;
	}

	// =================================================================================================================================================================
	/// <summary> Démarrer l'exécution de l'animation. </summary>

	public void Play()
	{
		//// Initialisation de certains paramètres

		//Q = new double[MainParameters.Instance.lagrangianModel.nDDL];
		//frameN = 0;
		//animateON = true;

		//// Création et initialisation des "GameObject Line Renderer"

		//if (lineStickFigure == null && lineCenterOfMass == null && lineFilledFigure == null)
		//{
		//	GameObject lineObject = new GameObject();
		//	LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
		//	lineRenderer.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
		//	lineRenderer.startWidth = 0.3f;
		//	lineRenderer.endWidth = 0.3f;

		//	lineStickFigure = new LineRenderer[MainParameters.Instance.lagrangianModel.stickFigure.Length / 2];
		//	for (int i = 0; i < MainParameters.Instance.lagrangianModel.stickFigure.Length / 2; i++)
		//	{
		//		lineStickFigure[i] = Instantiate(lineRenderer);
		//		lineStickFigure[i].name = string.Format("LineStickFigure{0}", i + 1);
		//		lineStickFigure[i].transform.parent = panelAnimator.transform;
		//	}

		//	lineCenterOfMass = Instantiate(lineRenderer);
		//	lineCenterOfMass.startColor = Color.red;
		//	lineCenterOfMass.endColor = Color.red;
		//	lineCenterOfMass.name = "LineCenterOfMass";
		//	lineCenterOfMass.transform.parent = panelAnimator.transform;

		//	lineFilledFigure = new LineRenderer[MainParameters.Instance.lagrangianModel.filledFigure.Length / 4];
		//	for (int i = 0; i < MainParameters.Instance.lagrangianModel.filledFigure.Length / 4; i++)
		//	{
		//		lineFilledFigure[i] = Instantiate(lineRenderer);
		//		lineFilledFigure[i].startColor = Color.yellow;
		//		lineFilledFigure[i].endColor = Color.yellow;
		//		lineFilledFigure[i].name = string.Format("LineFilledFigure{0}", i + 1);
		//		lineFilledFigure[i].transform.parent = panelAnimator.transform;
		//	}
		//	Destroy(lineObject);
		//}
	}

	// =================================================================================================================================================================
	/// <summary> Exécution d'un frame de l'animation. </summary>

	void PlayOneFrame()
	{
	//	// Effacer la silhouette précédente en premier

	//	for (int i = 0; i < MainParameters.Instance.lagrangianModel.stickFigure.Length / 2; i++)
	//		DrawObjects.Instance.Delete(lineStickFigure[i]);
	//	DrawObjects.Instance.Delete(lineCenterOfMass);
	//	for (int i = 0; i < MainParameters.Instance.lagrangianModel.filledFigure.Length / 4; i++)
	//		DrawObjects.Instance.Delete(lineFilledFigure[i]);

	//	//if (MainParameters.Instance.displayType == MainParameters.ListDisplayType.Dynamique)
	//	//{
	//	//	animateON = false;

	//	//	//% Structure pour transmettre des paramètres au logiciel de simulation
	//	//	//Somersault.VitVerticale = SomersaultData.TakeOffParameters.VerticalSpeed;
	//	//	//Somersault.VitAntero = SomersaultData.TakeOffParameters.AnteroposteriorSpeed;
	//	//	//Somersault.VitSalto = SomersaultData.TakeOffParameters.SomersaultSpeed * 2 * pi;
	//	//	//Somersault.VitVrille = SomersaultData.TakeOffParameters.TwistSpeed * 2 * pi;
	//	//	//Somersault.Inclinaison = SomersaultData.TakeOffParameters.Tilt * pi / 180;
	//	//	//Somersault.Rotation = SomersaultData.TakeOffParameters.Rotation * pi / 180;

	//	//	//q0(abs(ws.root_tilt)) = Somersault.Inclinaison;
	//	//	//q0(abs(ws.root_somersault)) = Somersault.Rotation;

	//	//	//q0dot(abs(ws.root_foreward)) = Somersault.VitAntero;
	//	//	//q0dot(abs(ws.root_upward)) = Somersault.VitVerticale;
	//	//	//q0dot(abs(ws.root_somersault)) = Somersault.VitSalto;
	//	//	//q0dot(abs(ws.root_twist)) = Somersault.VitVrille;

	//	//	return;
	//	//}

	//	// Initialisation du vecteur Q

	//	//for (int i = 0; i < MainParameters.Instance.lagrangianModel.nDDL; i++)
	//	//	Q[i] = MathFunc.Instance.Fnval(MainParameters.Instance.splines.T[frameN], MainParameters.Instance.splines.T, MainParameters.Instance.splines.coefs[i].pp);
	//	//Q[i] = MainParameters.Instance.jointsAngles[frameN].Q[i];

	//	// Calcul des "tags"

	//	double[] tag1 = MathFunc.Instance.Tags(Q);
	//	int newTagLength = tag1.Length / 3;
	//	Vector3[] tag = new Vector3[newTagLength];
	//	for (int i = 0; i < newTagLength; i++)
	//		tag[i] = new Vector3((float)tag1[i] * factorDisplay, (float)tag1[i + newTagLength] * factorDisplay, (float)tag1[i + newTagLength * 2] * factorDisplay);

	//	// Afficher la silhouette

	//	for (int i = 0; i < MainParameters.Instance.lagrangianModel.stickFigure.Length / 2; i++)
	//		DrawObjects.Instance.Line(lineStickFigure[i], tag[MainParameters.Instance.lagrangianModel.stickFigure[i, 0] - 1], tag[MainParameters.Instance.lagrangianModel.stickFigure[i, 1] - 1]);
	//	DrawObjects.Instance.Circle(lineCenterOfMass, factorDisplay / 60f , tag[newTagLength - 1]);

	//	for (int i = 0; i < MainParameters.Instance.lagrangianModel.filledFigure.Length / 4; i++)
	//		DrawObjects.Instance.Triangle(lineFilledFigure[i], tag[MainParameters.Instance.lagrangianModel.filledFigure[i, 0] - 1],
	//			tag[MainParameters.Instance.lagrangianModel.filledFigure[i, 1] - 1], tag[MainParameters.Instance.lagrangianModel.filledFigure[i, 2] - 1]);
	}
}
