using System;
using UnityEngine;
using UnityEngine.UI;

// =================================================================================================================================================================
/// <summary> Fonctions utilisées pour exécuter les animations. </summary>

public class AnimationF : MonoBehaviour
{
	public static AnimationF Instance;
	public Camera cameraAnimation;
	public GameObject panelAnimator;
	public Text textChrono;
	public Dropdown dropDownPlayMode;
	public Dropdown dropDownPlayView;
	public Button buttonPlay;
	public Image buttonPlayImage;
	public GameObject buttonStop;
	public Dropdown dropDownPlaySpeed;
	public Button buttonGraph;
	public Image buttonGraphImage;
	public GameObject panelResultsGraphics;

	public Text textScrollViewMessages;

	LineRenderer[] lineStickFigure;
	LineRenderer lineCenterOfMass;
	LineRenderer[] lineFilledFigure;
	LineRenderer[] lineFloor;

	public bool animateON = false;
	int frameN = 0;
	int firstFrame = 0;
	int numberFrames = 0;
	float tagXMin = 0;
	float tagXMax = 0;
	float tagYMin = 0;
	float tagYMax = 0;
	float tagZMin = 0;
	float tagZMax = 0;
	float factorTags2Screen = 1;
	float factorTags2ScreenX = 1;
	float factorTags2ScreenY = 1;
	float animationMaxDimOnScreen = 20;         // Dimension maximum de la silhouette à l'écran en unité de "Line renderer" (même dimension dans les 3 directions x, y, z)

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
		lineFloor = null;

		dropDownPlayMode.interactable = false;
		dropDownPlayView.interactable = false;
		buttonPlay.interactable = false;
		buttonPlayImage.color = Color.gray;
		dropDownPlaySpeed.interactable = false;
		buttonGraph.interactable = false;
		buttonGraphImage.color = Color.gray;

		if (Screen.width / Screen.height >= 1.7)
			animationMaxDimOnScreen = 20;
		else
			animationMaxDimOnScreen = 18;
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

			if (numberFrames > 1)
				textChrono.text = string.Format("{0:0.0}", timeElapsed);

			// Affichage de la silhouette à chaque frame, pour le nombre de frames spécifié.

			if (frameN < numberFrames)
				PlayOneFrame();
			else
				PlayEnd();
		}
	}

	// =================================================================================================================================================================
	/// <summary> La liste déroulante PlayView a été modifier. </summary>

	public void DropDownPlayView()
	{
		if (dropDownPlayView.captionText.text == MainParameters.Instance.languages.Used.animatorPlayViewFrontal)
		{
			cameraAnimation.transform.position = new Vector3(0, 18, 0);
			cameraAnimation.transform.rotation = Quaternion.Euler(90, 0, 0);
		}
		else
		{
			cameraAnimation.transform.position = new Vector3(-18, 0, 0);
			cameraAnimation.transform.rotation = Quaternion.Euler(0, 90, 90);
		}
	}

	// =================================================================================================================================================================
	/// <summary> Bouton Play a été appuyer. </summary>

	public void ButtonPlay()
	{
		// Afficher le bouton Stop et désactiver les autres contrôles du logiciel, durant l'animation

		buttonStop.SetActive(true);
		Main.Instance.EnableDisableControls(false, true);

		// Lecture des paramètres de décolage

		MainParameters.Instance.joints.condition = MovementF.Instance.dropDownCondition.value;
		MainParameters.Instance.joints.takeOffParam.rotation = float.Parse(MovementF.Instance.inputFieldSomersaultPosition.text);
		MainParameters.Instance.joints.takeOffParam.tilt = float.Parse(MovementF.Instance.inputFieldTilt.text);
		MainParameters.Instance.joints.takeOffParam.anteroposteriorSpeed = float.Parse(MovementF.Instance.inputFieldHorizontalSpeed.text);
		MainParameters.Instance.joints.takeOffParam.verticalSpeed = float.Parse(MovementF.Instance.inputFieldVerticalSpeed.text);
		MainParameters.Instance.joints.takeOffParam.somersaultSpeed = float.Parse(MovementF.Instance.inputFieldSomersaultSpeed.text);
		MainParameters.Instance.joints.takeOffParam.twistSpeed = float.Parse(MovementF.Instance.inputFieldTwistSpeed.text);

		// Lecture du mode d'exécution de l'animation

		playMode = dropDownPlayMode.captionText.text;

		// Lecture de la vitesse d'exécution de l'animation

		string playSpeed = dropDownPlaySpeed.captionText.text;
		if (playSpeed == MainParameters.Instance.languages.Used.animatorPlaySpeedSlow3)
			factorPlaySpeed = 10;
		else if (playSpeed == MainParameters.Instance.languages.Used.animatorPlaySpeedSlow2)
			factorPlaySpeed = 3;
		else if (playSpeed == MainParameters.Instance.languages.Used.animatorPlaySpeedSlow1)
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

		// Calculer un facteur de correspondance entre le volume utilisé par la silhouette et la dimension du volume disponible pour l'affichage
		// Pour cela, il nous faut calculer les valeurs minimum et maximum des DDLs de la silhouette, dans les 3 dimensions
		// Même si on modifie la dimension de la silhouette, on conserve quand même les proportions de la sihouette dans les 3 dimensions, donc le facteur est unique pour les 3 dimensions
		// Ici, vu qu'on ne sait pas encore les mouvements exécutés, alors on ignore le mouvement des membres et du tronc ( DDLs racines = 0), ça va nous donner une bonne idée de la dimension du volume nécessaire
		// On ajoute une marge de 10% sur les dimensions mimimum et maximum, pour être certain que les mouvements ne dépasseront pas la dimension du panneau utilisé

		if (playMode == MainParameters.Instance.languages.Used.animatorPlayModeSimulation)
		{
			float[] tagX, tagY, tagZ;
			tagXMin = tagYMin = tagZMin = 9999;
			tagXMax = tagYMax = tagZMax = -9999;
			qf = new double[q1.GetUpperBound(0) + 1];
			for (int i = 0; i <= q1.GetUpperBound(1); i++)
			{
				qf = MathFunc.MatrixGetColumnD(q1, i);
				if (playMode == MainParameters.Instance.languages.Used.animatorPlayModeGesticulation)       // Mode Gesticulation: Les DDL racine doivent être à zéro
					for (int j = 0; j < MainParameters.Instance.joints.lagrangianModel.q1.Length; j++)
						qf[MainParameters.Instance.joints.lagrangianModel.q1[j] - 1] = 0;

				EvaluateTags(qf, out tagX, out tagY, out tagZ);
				tagXMin = Math.Min(tagXMin, Mathf.Min(tagX));
				tagXMax = Math.Max(tagXMax, Mathf.Max(tagX));
				tagYMin = Math.Min(tagYMin, Mathf.Min(tagY));
				tagYMax = Math.Max(tagYMax, Mathf.Max(tagY));
				tagZMin = Math.Min(tagZMin, Mathf.Min(tagZ));
				tagZMax = Math.Max(tagZMax, Mathf.Max(tagZ));
			}
			AddMarginOnMinMax(0.1f);
			EvaluateFactorTags2Screen();
		}
		else
			PlayReset();

		// Afficher la silhouette pour toute l'animation

		Play(q1, 0, q1.GetUpperBound(1) + 1);
	}

	// =================================================================================================================================================================
	/// <summary> Bouton Stop a été appuyer. </summary>

	public void ButtonStop()
	{
		PlayEnd();
	}

	// =================================================================================================================================================================
	/// <summary> Bouton Graphiques des résultats a été appuyer. </summary>

	public void ButtonGraph()
	{
		Main.Instance.EnableDisableControls(false, true);
		EnableDisableAnimationOutline(false);
		GraphManager.Instance.mouseTracking = false;
		panelResultsGraphics.SetActive(true);
	}

	// =================================================================================================================================================================
	/// <summary> Bouton OK du panneau Graphiques des résultats a été appuyer. </summary>

	public void ButtonGraphOK()
	{
		Main.Instance.EnableDisableControls(true, true);
		EnableDisableAnimationOutline(true);
		GraphManager.Instance.mouseTracking = true;
		panelResultsGraphics.SetActive(false);
	}

	// =================================================================================================================================================================
	/// <summary> Démarrer l'exécution de l'animation. </summary>

	public void Play(float[,] qq, int frFrame, int nFrames)
	{
		MainParameters.StrucJoints joints = MainParameters.Instance.joints;

		// Initialisation de certains paramètres

		q = MathFunc.MatrixCopy(qq);
		frameN = 0;
		firstFrame = frFrame;
		numberFrames = nFrames;
		if (nFrames > 1)
		{
			if (joints.tc > 0)							// Il y a eu contact avec le sol, alors seulement une partie des données sont utilisé
				timeFrame = joints.tc / (numberFrames - 1);
			else										// Aucun contact avec le sol, alors toutes les données sont utilisé
				timeFrame = joints.duration / (numberFrames - 1);
		}
		else
			timeFrame = 0;
		animateON = true;
		GraphManager.Instance.mouseTracking = false;

		// Création et initialisation des "GameObject Line Renderer"

		if (lineStickFigure == null && lineCenterOfMass == null && lineFilledFigure == null)
		{
			GameObject lineObject = new GameObject();
			LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
			lineRenderer.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
			lineRenderer.startWidth = 0.04f;
			lineRenderer.endWidth = 0.04f;
			lineRenderer.gameObject.layer = 8;

			lineFloor = new LineRenderer[4];
			for (int i = 0; i < 4; i++)
			{
				lineFloor[i] = Instantiate(lineRenderer);
				lineFloor[i].startColor = Color.green;
				lineFloor[i].endColor = Color.green;
				lineFloor[i].startWidth = 0.1f;
				lineFloor[i].endWidth = 0.1f;
				lineFloor[i].name = string.Format("LineFloor{0}", i + 1);
				lineFloor[i].transform.parent = panelAnimator.transform;
			}

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
		for (int i = 0; i < 4; i++)
			DrawObjects.Instance.Delete(lineFloor[i]);

		// Initialisation du vecteur Q

		qf = MathFunc.MatrixGetColumnD(q, firstFrame + frameN);
		if (playMode == MainParameters.Instance.languages.Used.animatorPlayModeGesticulation)		// Mode Gesticulation: Les DDL racine doivent être à zéro
			for (int i = 0; i < MainParameters.Instance.joints.lagrangianModel.q1.Length; i++)
				qf[MainParameters.Instance.joints.lagrangianModel.q1[i] - 1] = 0;

		// Calcul des "tags", selon le modèle lagrangien utilisé

		float[] tagX;
		float[] tagY;
		float[] tagZ;
		EvaluateTags(qf, out tagX, out tagY, out tagZ);

		// Si le facteur de correspondance n'a pas été calculer précédemment, alors il nous faut le calculer
		// Calculer un facteur de correspondance entre le volume utilisé par la silhouette et la dimension du volume disponible pour l'affichage
		// Pour cela, il nous faut calculer les valeurs minimum et maximum des DDLs de la silhouette, dans les 3 dimensions
		// Même si on modifie la dimension de la silhouette, on conserve quand même les proportions de la sihouette dans les 3 dimensions, donc le facteur est unique pour les 3 dimensions
		// Pour le mode simulation, on ajoute une marge de 10% sur les dimensions mimimum et maximum, pour être certain que les mouvements ne dépasseront pas la dimension du panneau utilisé
		// Pour le mode gesticulation, on ajouter une marge de 25% sur les dimensions mimimum et maximum, au cas où les bras ne sont pas levé et qui le seront pas la suite.

		int newTagLength = tagX.Length;
		if (tagXMin == 0 && tagXMax == 0 && tagYMin == 0 && tagYMax == 0 && tagZMin == 0 && tagZMax == 0)
		{
			tagXMin = Mathf.Min(tagX);
			tagXMax = Mathf.Max(tagX);
			tagYMin = Mathf.Min(tagY);
			tagYMax = Mathf.Max(tagY);
			tagZMin = Mathf.Min(tagZ);
			tagZMax = Mathf.Max(tagZ);
			if (playMode == MainParameters.Instance.languages.Used.animatorPlayModeSimulation)
				AddMarginOnMinMax(0.1f);
			else
				AddMarginOnMinMax(0.25f);
			EvaluateFactorTags2Screen();
		}

		// On applique le facteur de correspondance pour optimiser l'affichage de la silhouette
		// On centre la silhouette au milieu de l'espace disponible à l'écran (PanelAnimator)

		float tagHalfMaxMinZ = (tagZMax - tagZMin) * factorTags2Screen / 2;
		for (int i = 0; i < newTagLength; i++)
		{
			tagX[i] = (tagX[i] - tagXMin) * factorTags2Screen - (tagXMax - tagXMin) * factorTags2Screen / 2;
			tagY[i] = (tagY[i] - tagYMin) * factorTags2Screen - (tagYMax - tagYMin) * factorTags2Screen / 2;
			tagZ[i] = (tagZ[i] - tagZMin) * factorTags2Screen - tagHalfMaxMinZ;
		}

		// On conserve la nouvelle matrice de "tags" sous une forme Vector3

		Vector3[] tag = new Vector3[newTagLength];
		for (int i = 0; i < newTagLength; i++)
			tag[i] = new Vector3(tagX[i], tagY[i], tagZ[i]);

		// Afficher la silhouette et le plancher si nécessaire

		for (int i = 0; i < joints.lagrangianModel.stickFigure.Length / 2; i++)
			DrawObjects.Instance.Line(lineStickFigure[i], tag[joints.lagrangianModel.stickFigure[i, 0] - 1], tag[joints.lagrangianModel.stickFigure[i, 1] - 1]);
		DrawObjects.Instance.Circle(lineCenterOfMass, 0.08f, tag[newTagLength - 1]);
		for (int i = 0; i < joints.lagrangianModel.filledFigure.Length / 4; i++)
			DrawObjects.Instance.Triangle(lineFilledFigure[i], tag[joints.lagrangianModel.filledFigure[i, 0] - 1], tag[joints.lagrangianModel.filledFigure[i, 1] - 1], tag[joints.lagrangianModel.filledFigure[i, 2] - 1]);

		if (numberFrames > 1 && playMode == MainParameters.Instance.languages.Used.animatorPlayModeSimulation)
		{
			float tagHalfMaxMinX = (tagXMax - tagXMin) * factorTags2ScreenX / 2;
			float tagHalfMaxMinY = (tagYMax - tagYMin) * factorTags2ScreenY / 2;
			float originZ = -tagZMin * factorTags2Screen - tagHalfMaxMinZ;
			DrawObjects.Instance.Line(lineFloor[0], new Vector3(-tagHalfMaxMinX, -tagHalfMaxMinY, originZ), new Vector3(tagHalfMaxMinX, -tagHalfMaxMinY, originZ));
			DrawObjects.Instance.Line(lineFloor[1], new Vector3(tagHalfMaxMinX, -tagHalfMaxMinY, originZ), new Vector3(tagHalfMaxMinX, tagHalfMaxMinY, originZ));
			DrawObjects.Instance.Line(lineFloor[2], new Vector3(tagHalfMaxMinX, tagHalfMaxMinY, originZ), new Vector3(-tagHalfMaxMinX, tagHalfMaxMinY, originZ));
			DrawObjects.Instance.Line(lineFloor[3], new Vector3(-tagHalfMaxMinX, tagHalfMaxMinY, originZ), new Vector3(-tagHalfMaxMinX, -tagHalfMaxMinY, originZ));
		}

		// Afficher le déplacement d'un curseur selon l'échelle des temps, dans le graphique qui affiche les positions des angles pour l'articulation sélectionné

		//if (numberFrames > 1)														// Ça ralenti trop l'animation, on désactive pour le moment
		//	GraphManager.Instance.DisplayCursor((firstFrame + frameN) * joints.lagrangianModel.dt);

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
		tagZMax = 0;
	}

	// =================================================================================================================================================================
	/// <summary> Actions à faire quand l'exécution de l'animation est terminé. </summary>

	public void PlayEnd()
	{
		animateON = false;
		GraphManager.Instance.mouseTracking = true;
		if (numberFrames > 1)
		{
			DisplayNewMessage(false, false, string.Format(" {0} = {1:0.00} s", MainParameters.Instance.languages.Used.displayMsgSimulationDuration, timeElapsed));
			DisplayNewMessage(false, true, string.Format(" {0}", MainParameters.Instance.languages.Used.displayMsgEndSimulation));
		}

		// Enlever le bouton Stop et activer les autres contrôles du logiciel

		buttonStop.SetActive(false);
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
	/// <summary> Activer ou désactiver la silhouette. </summary>

	void EnableDisableAnimationOutline(bool status)
	{
		if (lineStickFigure != null)
		{
			for (int i = 0; i < MainParameters.Instance.joints.lagrangianModel.stickFigure.Length / 2; i++)
				lineStickFigure[i].enabled = status;
		}
		if (lineCenterOfMass != null)
			lineCenterOfMass.enabled = status;
		if (lineFilledFigure != null)
		{
			for (int i = 0; i < MainParameters.Instance.joints.lagrangianModel.filledFigure.Length / 4; i++)
				lineFilledFigure[i].enabled = status;
		}
		if (lineFloor != null)
		{
			for (int i = 0; i < 4; i++)
				lineFloor[i].enabled = status;
		}
	}

	// =================================================================================================================================================================
	/// <summary> Ajouter une marge de sécurité sur les dimensions mimimum et maximum, pour être certain que les mouvements ne dépasseront pas la dimension du panneau utilisé. </summary>

	void AddMarginOnMinMax(float factor)
	{
		float margin;

		margin = (tagXMax - tagXMin) * factor;
		tagXMin -= margin;
		tagXMax += margin;

		margin = (tagYMax - tagYMin) * factor;
		tagYMin -= margin;
		tagYMax += margin;

		margin = (tagZMax - tagZMin) * factor;
		tagZMin -= margin;
		tagZMax += margin;
	}

	// =================================================================================================================================================================
	/// <summary> Calcul du facteur de correspondance entre la dimension du volume des Tags et la dimension du volume disponible à l'écran. </summary>

	void EvaluateFactorTags2Screen()
	{
		// Calcul du facteur de correspondance.

		factorTags2ScreenX = animationMaxDimOnScreen / (tagXMax - tagXMin);
		factorTags2ScreenY = animationMaxDimOnScreen / (tagYMax - tagYMin);
		if (tagXMax - tagXMin > tagYMax - tagYMin && tagXMax - tagXMin > tagZMax - tagZMin)
			factorTags2Screen = factorTags2ScreenX;
		else if (tagYMax - tagYMin > tagZMax - tagZMin)
			factorTags2Screen = factorTags2ScreenY;
		else
			factorTags2Screen = animationMaxDimOnScreen / (tagZMax - tagZMin);
	}
}
