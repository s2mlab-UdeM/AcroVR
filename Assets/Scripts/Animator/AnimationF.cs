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

	LineRenderer[] lineStickFigure;
	LineRenderer lineCenterOfMass;
	LineRenderer[] lineFilledFigure;

	bool animateON = false;
	int frameN = 0;
	double[] Q;

	// =================================================================================================================================================================
	/// <summary> Initialisation du script. </summary>

	void Start ()
	{
		Instance = this;
		lineStickFigure = null;
		lineCenterOfMass = null;
		lineFilledFigure = null;
	}

	// =================================================================================================================================================================
	/// <summary> Exécution du script à chaque frame. </summary>

	void Update()
	{
		if (!animateON) return;

		// Affichage de la silhouette à chaque frame (vu qu'on utilise des données déjà enregistrées, la simulation sera plus lente qu'en réalité, car l'exécution d'un
		// frame d'Unity (via routine Update) est plus lente que un frame des données enregistrées.

		if (frameN < MainParameters.Instance.joints.numberFrames)
		{
			PlayOneFrame();
			frameN++;
		}
		else
			animateON = false;
	}

	// =================================================================================================================================================================
	/// <summary> Bouton Play a été appuyer. </summary>

	public void ButtonPlay()
	{
		MainParameters.Instance.joints.numberFrames = (int)(MainParameters.Instance.joints.duration / MainParameters.Instance.joints.lagrangianModel.dt) + 1;

		// Afficher la silhouette pour toute l'animation

		AnimationF.Instance.Play();
	}

	// =================================================================================================================================================================
	/// <summary> Démarrer l'exécution de l'animation. </summary>

	public void Play()
	{
		MainParameters.StrucJoints joints = MainParameters.Instance.joints;

		// Initialisation de certains paramètres

		Q = new double[joints.lagrangianModel.nDDL];
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

		for (int i = 0; i < joints.lagrangianModel.nDDL; i++)
			Q[i] = joints.q0[i,frameN];

		// Calcul des "tags", selon le modèle lagrangien utilisé

		double[] tag1;
		if (joints.lagrangianModelName == MainParameters.LagrangianModelNames.Sasha23ddl)
		{
			TagsSasha23ddl tagsSasha23ddl = new TagsSasha23ddl();
			tag1 = tagsSasha23ddl.Tags(Q);
		}
		else
		{
			TagsSimple tagsSimple = new TagsSimple();
			tag1 = tagsSimple.Tags(Q);
		}

		// Formater la matrice obtenue en trois vecteurs X, Y et Z
		// Calculer un facteur de correspondance entre la silhouette et la dimension disponible pour l'affichage, pour optimiser l'espace disponible pour l'affichage
		// Même si on modifie la dimension de la silhouette, on conserve quand même les proportions de la sihouette dans les 3 dimensions, donc le facteur est unique pour les 3 dimensions
		// Intervertir les axes Y et Z et rotation de quelques degrés pour afficher la silhouette avec une vue sagittale par défaut

		int newTagLength = tag1.Length / 3;
		float[] tagX = new float[newTagLength];
		float[] tagY = new float[newTagLength];
		float[] tagZ = new float[newTagLength];
		for (int i = 0; i < newTagLength; i++)
		{
			tagX[i] = (float)tag1[i];
			tagZ[i] = (float)tag1[i + newTagLength];
			tagY[i] = (float)tag1[i + newTagLength * 2];
			//System.IO.File.AppendAllText(@"C:\Devel\AcroVR_Debug.txt", string.Format("i, tag = {0:10}, {1:10}, {2:10}, {3:10}{4}", i, tagX[i], tagY[i], tagZ[i], System.Environment.NewLine));
		}
		float tagXMin = Mathf.Min(tagX);
		float tagXMax = Mathf.Max(tagX);
		float tagYMin = Mathf.Min(tagY);
		float tagYMax = Mathf.Max(tagY);
		float factor;
		if (tagXMax - tagXMin > tagYMax - tagYMin)
			factor = 0.7f / (tagXMax - tagXMin);
		else
			factor = 0.7f / (tagYMax - tagYMin);

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

		// On applique le facteur de correspondance pour optimiser l'affichage de la silhouette
		// On centre la silhouette au milieu de l'espace disponible à l'écran (PanelAnimator), selon les dimensions X et Y au moins
		// On conserve la nouvelle matrice de "tags" sous une forme Vector3

		for (int i = 0; i < newTagLength; i++)
		{
			tagX[i] *= factor;
			tagY[i] *= factor;
			tagZ[i] *= factor;
		}
		tagXMin = Mathf.Min(tagX);
		tagXMax = Mathf.Max(tagX);
		tagYMin = Mathf.Min(tagY);
		tagYMax = Mathf.Max(tagY);

		float offsetX = (tagXMax - tagXMin) / 2 + tagXMin + 0.5f;
		float offsetY = (tagYMax - tagYMin) / 2 + tagYMin + 1;

		Vector3[] tag = new Vector3[newTagLength];
		for (int i = 0; i < newTagLength; i++)
			tag[i] = new Vector3(tagX[i] + offsetX, tagY[i] + offsetY, tagZ[i]);

		// Afficher la silhouette

		for (int i = 0; i < joints.lagrangianModel.stickFigure.Length / 2; i++)
			DrawObjects.Instance.Line(lineStickFigure[i], tag[joints.lagrangianModel.stickFigure[i, 0] - 1], tag[joints.lagrangianModel.stickFigure[i, 1] - 1]);
		DrawObjects.Instance.Circle(lineCenterOfMass, 0.005f , tag[newTagLength - 1]);
		for (int i = 0; i < joints.lagrangianModel.filledFigure.Length / 4; i++)
			DrawObjects.Instance.Triangle(lineFilledFigure[i], tag[joints.lagrangianModel.filledFigure[i, 0] - 1], tag[joints.lagrangianModel.filledFigure[i, 1] - 1], tag[joints.lagrangianModel.filledFigure[i, 2] - 1]);
	}
}
