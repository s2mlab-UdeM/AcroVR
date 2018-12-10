using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// =================================================================================================================================================================
/// <summary> Fonctions utilisées pour exécuter les animations. </summary>

public class AnimationF : MonoBehaviour
{
	public static AnimationF Instance;
	public GameObject panelAnimator;
	public Dropdown dropDownPlayMode;
	public Dropdown dropDownPlayView;
	public Button buttonPlay;
	public Image buttonPlayImage;
	public Dropdown dropDownPlaySpeed;
	public Button buttonGraph;
	public Image buttonGraphImage;

	public Text textScrollViewMessages;

	LineRenderer[] lineStickFigure;
	LineRenderer lineCenterOfMass;
	LineRenderer[] lineFilledFigure;
	LineRenderer lineDebug;

	bool animateON = false;
	int frameN = 0;
	float tagXMin = 0;
	float tagXMax = 0;
	float tagYMin = 0;
	float tagYMax = 0;
	float tagZMin = 0;
	float factorTags2Screen = 1;
	float elapsedTime = 0;
	float[,] q;
	double[] qf;

	// =================================================================================================================================================================
	/// <summary> Initialisation du script. </summary>

	void Start()
	{
		Instance = this;
		lineStickFigure = null;
		lineCenterOfMass = null;
		lineFilledFigure = null;

		dropDownPlayMode.interactable = false;
		dropDownPlayView.interactable = false;
		buttonPlay.interactable = false;
		buttonPlayImage.color = Color.gray;
		dropDownPlaySpeed.interactable = false;
		buttonGraph.interactable = false;
		buttonGraphImage.color = Color.gray;
	}

	// =================================================================================================================================================================
	/// <summary> Exécution de la fonction à chaque frame. </summary>

	void Update()
	{
		if (!animateON) return;

		// Affichage de la silhouette à chaque frame (vu qu'on utilise des données déjà enregistrées, la simulation sera plus lente qu'en réalité, car l'exécution d'un
		// frame d'Unity (via routine Update) est plus lente que un frame des données enregistrées.

		if (frameN < MainParameters.Instance.joints.numberFrames)
		{
			if (frameN <= 0) elapsedTime = Time.time;
			PlayOneFrame();
			frameN++;
		}
		else
		{
			elapsedTime = Time.time - elapsedTime;
			animateON = false;
			PlayEnd();
		}
	}

	// =================================================================================================================================================================
	/// <summary> Bouton Play a été appuyer. </summary>

	public void ButtonPlay()
	{
		// Affichage d'un message dans la boîte des messages

		DisplayNewMessage(true, false, string.Format(" {0}", MainParameters.Instance.languages.Used.displayMsgStartSimulation));
		DisplayNewMessage(false, false, string.Format(" {0} = {1:0.00000} s", MainParameters.Instance.languages.Used.displayMsgDtValue, MainParameters.Instance.joints.lagrangianModel.dt));

		// Exécution des calculs de simulation

		DoSimulation doSimulation = new DoSimulation();
		doSimulation.ToString();                  // Pour enlever un warning lors de la compilation

		// Initialisation du nombre de frames à afficher

		//MainParameters.Instance.joints.numberFrames = (int)(MainParameters.Instance.joints.duration / MainParameters.Instance.joints.lagrangianModel.dt) + 1;
		MainParameters.Instance.joints.numberFrames = MainParameters.Instance.joints.q.GetUpperBound(1) + 1;

		// Pour les simulations, il nous faut calculer la dimension du volume utilisé pour exécuter les mouvements (code temporaire, à modifier éventuellement)
		// On tient compte de seulement les composantes X et Y car l'affichage n'est pas limité selon la composante Z (profondeur).

		float[] tagX, tagY, tagZ;
		tagXMin = tagYMin = tagZMin = 9999;
		tagXMax = tagYMax = -9999;
		qf = new double[MainParameters.Instance.joints.q.GetUpperBound(0) + 1];
		for (int i = 0; i <= MainParameters.Instance.joints.q.GetUpperBound(1); i++)
		{
			qf = MathFunc.MatrixGetColumnD(MainParameters.Instance.joints.q, i);
			EvaluateTags(qf, out tagX, out tagZ, out tagY);                 // Intervertir les axes Y et Z pour afficher la silhouette avec une vue sagittale par défaut (à modifier plus tard j'imagine)
			tagXMin = Math.Min(tagXMin, Mathf.Min(tagX));
			tagXMax = Math.Max(tagXMax, Mathf.Max(tagX));
			tagYMin = Math.Min(tagYMin, Mathf.Min(tagY));
			tagYMax = Math.Max(tagYMax, Mathf.Max(tagY));
			tagZMin = Math.Min(tagZMin, Mathf.Min(tagZ));
		}
		EvaluateFactorTags2Screen(tagXMin, tagXMax, tagYMin, tagYMax);

		// Afficher la silhouette pour toute l'animation

		Play(MainParameters.Instance.joints.q);
	}

	// =================================================================================================================================================================
	/// <summary> Démarrer l'exécution de l'animation. </summary>

	public void Play(float[,] qq)
	{
		MainParameters.StrucJoints joints = MainParameters.Instance.joints;

		// Initialisation de certains paramètres

		q = MathFunc.MatrixCopy(qq);
		frameN = 0;
		animateON = true;

		// Création et initialisation des "GameObject Line Renderer"

		if (lineStickFigure == null && lineCenterOfMass == null && lineFilledFigure == null)
		{
			GameObject lineObject = new GameObject();
			LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
			lineRenderer.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
			lineRenderer.startWidth = 0.005f;
			lineRenderer.endWidth = 0.005f;

			lineStickFigure = new LineRenderer[joints.lagrangianModel.stickFigure.Length / 2];
			for (int i = 0; i < joints.lagrangianModel.stickFigure.Length / 2; i++)
			{
				lineStickFigure[i] = Instantiate(lineRenderer);
				lineStickFigure[i].name = string.Format("LineStickFigure{0}", i + 1);
				lineStickFigure[i].transform.parent = panelAnimator.transform;
			}

			lineCenterOfMass = Instantiate(lineRenderer);
			lineCenterOfMass.startColor = Color.red;
			lineCenterOfMass.endColor = Color.red;
			lineCenterOfMass.name = "LineCenterOfMass";
			lineCenterOfMass.transform.parent = panelAnimator.transform;

			lineFilledFigure = new LineRenderer[joints.lagrangianModel.filledFigure.Length / 4];
			for (int i = 0; i < joints.lagrangianModel.filledFigure.Length / 4; i++)
			{
				lineFilledFigure[i] = Instantiate(lineRenderer);
				lineFilledFigure[i].startColor = Color.yellow;
				lineFilledFigure[i].endColor = Color.yellow;
				lineFilledFigure[i].name = string.Format("LineFilledFigure{0}", i + 1);
				lineFilledFigure[i].transform.parent = panelAnimator.transform;
			}

			lineDebug = Instantiate(lineRenderer);
			lineDebug.name = "LineDebug";
			lineDebug.transform.parent = panelAnimator.transform;

			Destroy(lineObject);
		}
	}

	// =================================================================================================================================================================
	/// <summary> Exécution d'un frame de l'animation. </summary>

	void PlayOneFrame()
	{
		MainParameters.StrucJoints joints = MainParameters.Instance.joints;

		// Effacer la silhouette précédente en premier

		for (int i = 0; i < joints.lagrangianModel.stickFigure.Length / 2; i++)
			DrawObjects.Instance.Delete(lineStickFigure[i]);
		DrawObjects.Instance.Delete(lineCenterOfMass);
		for (int i = 0; i < joints.lagrangianModel.filledFigure.Length / 4; i++)
			DrawObjects.Instance.Delete(lineFilledFigure[i]);

		// Initialisation du vecteur Q

		qf = MathFunc.MatrixGetColumnD(q, frameN);

		// Calcul des "tags", selon le modèle lagrangien utilisé

		float[] tagX;
		float[] tagY;
		float[] tagZ;
		EvaluateTags(qf, out tagX, out tagZ, out tagY);             // Intervertir les axes Y et Z pour afficher la silhouette avec une vue sagittale par défaut (à modifier plus tard j'imagine)

		// Les calculs suivants sont incomplet et non pleinement fonctionnel, il va falloir améliorer cela dans le futur
		// Calculer un facteur de correspondance entre la silhouette et la dimension disponible pour l'affichage, pour optimiser l'espace disponible pour l'affichage
		// Même si on modifie la dimension de la silhouette, on conserve quand même les proportions de la sihouette dans les 3 dimensions, donc le facteur est unique pour les 3 dimensions
		// L'espace disponible à l'écran (pour les LineRenderer dans PanelAnimator): X = 0 à 1 et Y = 0.5 à 1.5

		int newTagLength = tagX.Length;
		if (tagXMin == 0 && tagXMax == 0 && tagYMin == 0 && tagYMax == 0 && tagZMin == 0)
		{
			tagXMin = Mathf.Min(tagX);
			tagXMax = Mathf.Max(tagX);
			tagYMin = Mathf.Min(tagY);
			tagYMax = Mathf.Max(tagY);
			tagZMin = Mathf.Min(tagZ);
			EvaluateFactorTags2Screen(tagXMin, tagXMax, tagYMin, tagYMax);
		}

		// On applique le facteur de correspondance pour optimiser l'affichage de la silhouette

		for (int i = 0; i < newTagLength; i++)
		{
			tagX[i] = (tagX[i] - tagXMin) * factorTags2Screen;
			tagY[i] = (tagY[i] - tagYMin) * factorTags2Screen;
			tagZ[i] = (tagZ[i] - tagZMin) * factorTags2Screen;
		}

		// On centre la silhouette au milieu de l'espace disponible à l'écran (PanelAnimator)
		// On conserve la nouvelle matrice de "tags" sous une forme Vector3

		Vector3[] tag = new Vector3[newTagLength];
		for (int i = 0; i < newTagLength; i++)
			tag[i] = new Vector3(tagX[i] + 0.5f, tagY[i] + 0.5f, tagZ[i]);

		// Afficher la silhouette

		for (int i = 0; i < joints.lagrangianModel.stickFigure.Length / 2; i++)
			DrawObjects.Instance.Line(lineStickFigure[i], tag[joints.lagrangianModel.stickFigure[i, 0] - 1], tag[joints.lagrangianModel.stickFigure[i, 1] - 1]);
		DrawObjects.Instance.Circle(lineCenterOfMass, 0.005f, tag[newTagLength - 1]);
		for (int i = 0; i < joints.lagrangianModel.filledFigure.Length / 4; i++)
			DrawObjects.Instance.Triangle(lineFilledFigure[i], tag[joints.lagrangianModel.filledFigure[i, 0] - 1], tag[joints.lagrangianModel.filledFigure[i, 1] - 1], tag[joints.lagrangianModel.filledFigure[i, 2] - 1]);

		//if (joints.numberFrames > 1)
		//	GraphManager.Instance.DisplayCursor(frameN * joints.lagrangianModel.dt);
	}

	// =================================================================================================================================================================
	/// <summary> Réinitialiser certains paramètres utilisés pour l'exécution de l'animation. </summary>

	public void PlayReset()
	{
		tagXMin = 0;
		tagXMax = 0;
		tagYMin = 0;
		tagYMax = 0;
		tagZMin = 0;
	}

	// =================================================================================================================================================================
	/// <summary> Actions à faire quand l'exécution de l'animation est terminé. </summary>

	void PlayEnd()
	{
		if (MainParameters.Instance.joints.numberFrames > 1)
		{
			DisplayNewMessage(false, false, string.Format(" {0} = {1:0.00} s", MainParameters.Instance.languages.Used.displayMsgSimulationDuration, elapsedTime));
			DisplayNewMessage(false, true, string.Format(" {0}", MainParameters.Instance.languages.Used.displayMsgEndSimulation));
		}
	}

	// =================================================================================================================================================================
	/// <summary> Afficher un nouveau message dans la boîte des messages. </summary>

	public void DisplayNewMessage(bool clear, bool display, string message)
	{
		if (clear) MainParameters.Instance.scrollViewMessages.Clear();
		MainParameters.Instance.scrollViewMessages.Add(message);
		if (display) textScrollViewMessages.text = string.Join(Environment.NewLine, MainParameters.Instance.scrollViewMessages.ToArray());
	}

	// =================================================================================================================================================================
	/// <summary> Calcul des "tags", selon le modèle lagrangien utilisé. </summary>

	public void EvaluateTags(double[] q, out float[] tagX, out float[] tagY, out float[] tagZ)
	{
		double[] tag1;
		if (MainParameters.Instance.joints.lagrangianModelName == MainParameters.LagrangianModelNames.Sasha23ddl)
		{
			TagsSasha23ddl tagsSasha23ddl = new TagsSasha23ddl();
			tag1 = tagsSasha23ddl.Tags(q);
		}
		else
		{
			TagsSimple tagsSimple = new TagsSimple();
			tag1 = tagsSimple.Tags(q);
		}

		int newTagLength = tag1.Length / 3;
		tagX = new float[newTagLength];
		tagY = new float[newTagLength];
		tagZ = new float[newTagLength];
		for (int i = 0; i < newTagLength; i++)
		{
			tagX[i] = (float)tag1[i];
			tagY[i] = (float)tag1[i + newTagLength];
			tagZ[i] = (float)tag1[i + newTagLength * 2];
		}
	}

	// =================================================================================================================================================================
	/// <summary> Calcul du facteur de correspondance entre la dimension du volume des Tags et la dimension du volume disponible à l'écran. </summary>

	void EvaluateFactorTags2Screen(float tagXMin, float tagXMax, float tagYMin, float tagYMax)
	{
		if (tagXMax - tagXMin > tagYMax - tagYMin)
			factorTags2Screen = 1 / (tagXMax - tagXMin);
		else
			factorTags2Screen = 1 / (tagYMax - tagYMin);
	}

	// Rotation de la silhouette (pas utilisé pour le moment)

	//float vectX;
	//float vectY;
	//float vectZ;
	//float angleXZ = 0;                                          // Angle de rotation dans le plan X-Z (positif = horaire, négatif = anti-horaire)
	//float angleXZCos = Mathf.Cos(angleXZ * Mathf.PI / 180);
	//float angleXZSin = Mathf.Sin(angleXZ * Mathf.PI / 180);
	//float angleXY = 0;                                            // Angle de rotation dans le plan X-Y (positif = horaire, négatif = anti-horaire)
	//float angleXYCos = Mathf.Cos(angleXY * Mathf.PI / 180);
	//float angleXYSin = Mathf.Sin(angleXY * Mathf.PI / 180);
	//for (int i = 0; i < newTagLength; i++)
	//{
	//	vectX = tagX[i] - tagX[newTagLength - 1];
	//	vectY = tagY[i] - tagY[newTagLength - 1];
	//	vectZ = tagZ[i] - tagZ[newTagLength - 1];
	//	tagX[i] = vectX * angleXZCos + vectZ * angleXZSin + tagX[newTagLength - 1];
	//	tagZ[i] = -vectX * angleXZSin + vectZ * angleXZCos + tagZ[newTagLength - 1];

	//	tagX[i] = vectX * angleXYCos + vectY * angleXYSin + tagX[newTagLength - 1];
	//	tagY[i] = -vectX * angleXYSin + vectY * angleXYCos + tagY[newTagLength - 1];
	//}

}
