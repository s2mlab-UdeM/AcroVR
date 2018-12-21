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
	public Text textChrono;
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

	public bool animateON = false;
	int frameN = 0;
	float tagXMin = 0;
	float tagXMax = 0;
	float tagYMin = 0;
	float tagYMax = 0;
	float tagZMin = 0;
	float factorTags2Screen = 1;

	float timeElapsed = 0;
	float timeFrame = 0;
	float timeStarted = 0;
	string playMode = MainParameters.Instance.languages.Used.animatorPlayModeSimulation;
	float factorPlaySpeed = 1;

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
		// Exécuter seulement si l'animation a été démarré

		if (!animateON) return;

		// Synchroniser l'affichage pour que l'exécution de l'animation prenne le temps de simulation spécifié.
		// Synchroniser aussi l'affichage selon la vitesse d'exécution de l'animation spécifié par l'utilisateur.

		if (frameN <= 0) timeStarted = Time.time;
		if (Time.time - timeStarted >= (timeFrame * frameN) * factorPlaySpeed)
		{
			timeElapsed = Time.time - timeStarted;

			// Affichage du chronomètre

			if (MainParameters.Instance.joints.numberFrames > 1)
				textChrono.text = string.Format("{0:0.0}", timeElapsed);

			// Affichage de la silhouette à chaque frame, pour le nombre de frames spécifié.

			if (frameN < MainParameters.Instance.joints.numberFrames)
				PlayOneFrame();
			else
				PlayEnd();
		}
	}

	// =================================================================================================================================================================
	/// <summary> Bouton Play a été appuyer. </summary>

	public void ButtonPlay()
	{
		// Lecture des paramètres de décolage

		MainParameters.Instance.joints.condition = MovementF.Instance.dropDownCondition.value;
		MainParameters.Instance.joints.takeOffParam.rotation = float.Parse(MovementF.Instance.inputFieldInitialRotation.text);
		MainParameters.Instance.joints.takeOffParam.tilt = float.Parse(MovementF.Instance.inputFieldTilt.text);
		MainParameters.Instance.joints.takeOffParam.anteroposteriorSpeed = float.Parse(MovementF.Instance.inputFieldHorizontalSpeed.text);
		MainParameters.Instance.joints.takeOffParam.verticalSpeed = float.Parse(MovementF.Instance.inputFieldVerticalSpeed.text);
		MainParameters.Instance.joints.takeOffParam.somersaultSpeed = float.Parse(MovementF.Instance.inputFieldSomersaultSpeed.text);
		MainParameters.Instance.joints.takeOffParam.twistSpeed = float.Parse(MovementF.Instance.inputFieldTwistSpeed.text);

		// Lecture du mode d'exécution de l'animation

		playMode = dropDownPlayMode.captionText.text;

		// Lecture de la vitesse d'exécution de l'animation

		string playSpeed = dropDownPlaySpeed.captionText.text;
		if (playSpeed == MainParameters.Instance.languages.Used.animatorPlaySpeedSlow)
			factorPlaySpeed = 1.5f;
		else if (playSpeed == MainParameters.Instance.languages.Used.animatorPlaySpeedNormal)
			factorPlaySpeed = 1;
		else if (playSpeed == MainParameters.Instance.languages.Used.animatorPlaySpeedFast)
			factorPlaySpeed = 0.8f;

		// Affichage d'un message dans la boîte des messages

		DisplayNewMessage(true, false, string.Format(" {0}", MainParameters.Instance.languages.Used.displayMsgStartSimulation));
		DisplayNewMessage(false, false, string.Format(" {0} = {1:0.00000} s", MainParameters.Instance.languages.Used.displayMsgDtValue, MainParameters.Instance.joints.lagrangianModel.dt));

		// Exécution des calculs de simulation

		float[,] q1;
		DoSimulation doSimulation = new DoSimulation(out q1);
		doSimulation.ToString();                  // Pour enlever un warning lors de la compilation

		// Initialisation du nombre de frames à afficher

		MainParameters.Instance.joints.numberFrames = q1.GetUpperBound(1) + 1;

		// Il nous faut calculer la dimension du volume utilisé pour exécuter les mouvements
		// Pour cela, on ignore le mouvement des membres et du tronc ( DDL racine = 0), ça va nous donner une bonne idée de la dimension du volume nécessaire
		// On ajoute une marge de 10% sur les dimensions mimimum et maximum, pour être certain que les mouvements ne dépasseront pas la dimension du panneau utilisé
		// On tient compte de seulement les composantes X et Y car l'affichage n'est pas limité selon la composante Z (profondeur)

		if (playMode == MainParameters.Instance.languages.Used.animatorPlayModeSimulation)
		{
			float[] tagX, tagY, tagZ;
			tagXMin = tagYMin = tagZMin = 9999;
			tagXMax = tagYMax = -9999;
			qf = new double[q1.GetUpperBound(0) + 1];
			for (int i = 0; i < MainParameters.Instance.joints.lagrangianModel.q2.Length; i++)
				qf[MainParameters.Instance.joints.lagrangianModel.q2[i] - 1] = 0;
			for (int i = 0; i <= q1.GetUpperBound(1); i++)
			{
				for (int j = 0; j < MainParameters.Instance.joints.lagrangianModel.q1.Length; j++)
					qf[MainParameters.Instance.joints.lagrangianModel.q1[j] - 1] = q1[MainParameters.Instance.joints.lagrangianModel.q1[j] - 1, i];
				EvaluateTags(qf, out tagX, out tagZ, out tagY);     // Intervertir les axes Y et Z pour afficher la silhouette avec une vue sagittale par défaut (à modifier plus tard j'imagine)
				tagXMin = Math.Min(tagXMin, Mathf.Min(tagX));
				tagXMax = Math.Max(tagXMax, Mathf.Max(tagX));
				tagYMin = Math.Min(tagYMin, Mathf.Min(tagY));
				tagYMax = Math.Max(tagYMax, Mathf.Max(tagY));
				tagZMin = Math.Min(tagZMin, Mathf.Min(tagZ));
			}
			if (tagXMin >= 0) tagXMin *= 0.9f;
			else tagXMin *= 1.1f;
			if (tagXMax >= 0) tagXMax *= 1.1f;
			else tagXMax *= 0.9f;
			if (tagYMin >= 0) tagYMin *= 0.9f;
			else tagYMin *= 1.1f;
			if (tagYMax >= 0) tagYMax *= 1.1f;
			else tagYMax *= 0.9f;
			EvaluateFactorTags2Screen(tagXMin, tagXMax, tagYMin, tagYMax);
		}
		else
			PlayReset();

		// Afficher la silhouette pour toute l'animation

		Main.Instance.EnableDisableControls(false, true);
		Play(q1);
	}

	// =================================================================================================================================================================
	/// <summary> Démarrer l'exécution de l'animation. </summary>

	public void Play(float[,] qq)
	{
		MainParameters.StrucJoints joints = MainParameters.Instance.joints;

		// Initialisation de certains paramètres

		q = MathFunc.MatrixCopy(qq);
		frameN = 0;
		timeFrame = joints.duration / joints.numberFrames;
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
		if (playMode == MainParameters.Instance.languages.Used.animatorPlayModeGesticulation)		// Mode Gesticulation: Les DDL racine doivent être à zéro
			for (int i = 0; i < MainParameters.Instance.joints.lagrangianModel.q1.Length; i++)
				qf[MainParameters.Instance.joints.lagrangianModel.q1[i] - 1] = 0;

		// Calcul des "tags", selon le modèle lagrangien utilisé

		float[] tagX;
		float[] tagY;
		float[] tagZ;
		EvaluateTags(qf, out tagX, out tagZ, out tagY);		// Intervertir les axes Y et Z pour afficher la silhouette avec une vue sagittale par défaut (à modifier plus tard j'imagine)

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

		// Afficher le déplacement d'un curseur selon l'échelle des temps, dans le graphique qui affiche les positions des angles pour l'articulation sélectionné

		//if (joints.numberFrames > 1)														// Ça ralenti trop l'animation, on désactive pour le moment
		//	GraphManager.Instance.DisplayCursor(frameN * joints.lagrangianModel.dt);

		// Inclémenter le compteur de frames

		frameN++;
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
		animateON = false;
		if (MainParameters.Instance.joints.numberFrames > 1)
		{
			DisplayNewMessage(false, false, string.Format(" {0} = {1:0.00} s", MainParameters.Instance.languages.Used.displayMsgSimulationDuration, timeElapsed));
			DisplayNewMessage(false, true, string.Format(" {0}", MainParameters.Instance.languages.Used.displayMsgEndSimulation));
		}
		Main.Instance.EnableDisableControls(true, true);
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
		// Pour le mode gesticulation, on laisse de l'espace en haut de la silhouette, au cas où les bras ne sont pas levé et qui le seront pas la suite.

		float factorY;
		if (playMode == MainParameters.Instance.languages.Used.animatorPlayModeSimulation)
			factorY = 1.2f;
		else
			factorY = 1.5f;

		// Calcul du facteur de correspondance.

		if (tagXMax - tagXMin > tagYMax * factorY - tagYMin)
			factorTags2Screen = 1 / (tagXMax - tagXMin);
		else
			factorTags2Screen = 1 / (tagYMax * factorY - tagYMin);
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
