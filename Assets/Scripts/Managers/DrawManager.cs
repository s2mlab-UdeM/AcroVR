using UnityEngine;
using System;
using System.Linq;
using System.Text;
using Microsoft.Research.Oslo;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;

public class DrawManager : MonoBehaviour
{
    const string dllpath = "biorbd_c.dll";
    [DllImport(dllpath)] static extern IntPtr c_biorbdModel(StringBuilder pathToModel);
    [DllImport(dllpath)] static extern int c_nQ(IntPtr model);
    [DllImport(dllpath)] static extern void c_massMatrix(IntPtr model, IntPtr q, IntPtr massMatrix);
    [DllImport(dllpath)] static extern void c_inverseDynamics(IntPtr model, IntPtr q, IntPtr qdot, IntPtr qddot, IntPtr tau);
    [DllImport(dllpath)] static extern void c_solveLinearSystem(IntPtr matA, int nbCol, int nbLigne, IntPtr matB, IntPtr solX);

    ////////////////
    public GameObject girl1;
    public GameObject girl2;
    GameObject girl1Prefab;
    GameObject girl2Prefab;
    ////////////////
    /// <summary>
    /// Hip
    private GameObject girl1LeftUp;
    private GameObject girl1RightUp;
    // Knee
    private GameObject girl1LeftLeg;
    private GameObject girl1RightLeg;
    // Shoulder
    private GameObject girl1LeftArm;
    private GameObject girl1RightArm;
    // Root
    private GameObject girl1Hip;
    /// </summary>

    /// <summary>
    /// Hip
    private GameObject girl2LeftUp;
    private GameObject girl2RightUp;
    // Knee
    private GameObject girl2LeftLeg;
    private GameObject girl2RightLeg;
    // Shoulder
    private GameObject girl2LeftArm;
    private GameObject girl2RightArm;
    // Root
    private GameObject girl2Hip;
    /// </summary>

    float[,] q;
    float[,] q_girl2;
    public double[] qf;
    double[] qf_girl2;
    public int frameN = 0;
    int firstFrame = 0;
    int numberFrames = 0;
    float timeElapsed = 0;
    float timeFrame = 0;
    float timeStarted = 0;
    public bool animateON = false;
    float factorPlaySpeed = 3f;

    float tagXMin = 0;
    float tagXMax = 0;
    float tagYMin = 0;
    float tagYMax = 0;
    float tagZMin = 0;
    float tagZMax = 0;
    float factorTags2Screen = 1;
    float factorTags2ScreenX = 1;
    float factorTags2ScreenY = 1;
    float animationMaxDimOnScreen = 20;

    string playMode = MainParameters.Instance.languages.Used.animatorPlayModeSimulation;

    float ThetaScale;

    /*    LineRenderer[] lineStickFigure;
        LineRenderer lineCenterOfMass;
        LineRenderer[] lineFilledFigure;

        public GameObject stickMan;*/

    private int cntAvatar;

    float[,] q1;
    float[,] q1_girl2;

    bool isPaused = false;
    public bool isEditing = false;

    IntPtr ptr_model;

    void Awake()
    {
        girl1Prefab = (GameObject)Resources.Load("girl1", typeof(GameObject));
        girl1 = Instantiate(girl1Prefab);

        ///////////////////////////
        // Hip
        girl1LeftUp = girl1.transform.Find("Petra.002/hips/thigh.L").gameObject;
        girl1RightUp = girl1.transform.Find("Petra.002/hips/thigh.R").gameObject;
        // Knee
        girl1LeftLeg = girl1.transform.Find("Petra.002/hips/thigh.L/shin.L").gameObject;
        girl1RightLeg = girl1.transform.Find("Petra.002/hips/thigh.R/shin.R").gameObject;
        // Shoulder
        girl1RightArm = girl1.transform.Find("Petra.002/hips/spine/chest/chest1/shoulder.R/upper_arm.R").gameObject;
        girl1LeftArm = girl1.transform.Find("Petra.002/hips/spine/chest/chest1/shoulder.L/upper_arm.L").gameObject;
        // Root
        girl1Hip = girl1.transform.Find("Petra.002/hips").gameObject;
        ///////////////////////////

        //        stickMan = GameObject.Find("StickMan");

        ThetaScale = 0.01f;
        girl1.SetActive(false);
        cntAvatar = 1;
    }

    void Update()
    {
        if (!animateON) return;

        if (frameN <= 0) timeStarted = Time.time;
        if (Time.time - timeStarted >= (timeFrame * frameN) * factorPlaySpeed)
        {
            timeElapsed = Time.time - timeStarted;

            if (frameN < numberFrames)
                PlayOneFrame();
            else
                PlayEnd();
        }
    }

/*    void FixedUpdate()
    {
        if (!animateON) return;

        if (frameN < numberFrames)
            PlayOneFrame();
        else
            PlayEnd();
    }*/
  
    public void ShowAvatar(int num)
    {
        cntAvatar = num;
        if (MainParameters.Instance.joints.nodes == null) return;
        girl1.SetActive(true);
        girl1.transform.rotation = Quaternion.identity;
        transform.parent.GetComponentInChildren<StatManager>().DestroyHandleCircle();

        q1 = MakeSimulation();

        // test0 = q1[12,51]
        // test1 = q1[12,54]
        Play_s(q1, 0, q1.GetUpperBound(1) + 1);

        if (cntAvatar > 1)
        {
            ///////////////////////////
            // Hip
            girl2Prefab = (GameObject)Resources.Load("girl2", typeof(GameObject));
            girl2 = Instantiate(girl2Prefab);
            girl2LeftUp = girl2.transform.Find("Petra.002/hips/thigh.L").gameObject;
            girl2RightUp = girl2.transform.Find("Petra.002/hips/thigh.R").gameObject;
            // Knee
            girl2LeftLeg = girl2.transform.Find("Petra.002/hips/thigh.L/shin.L").gameObject;
            girl2RightLeg = girl2.transform.Find("Petra.002/hips/thigh.R/shin.R").gameObject;
            // Shoulder
            girl2RightArm = girl2.transform.Find("Petra.002/hips/spine/chest/chest1/shoulder.R/upper_arm.R").gameObject;
            girl2LeftArm = girl2.transform.Find("Petra.002/hips/spine/chest/chest1/shoulder.L/upper_arm.L").gameObject;
            // Root
            girl2Hip = girl2.transform.Find("Petra.002/hips").gameObject;
            ////////////////////
            transform.parent.GetComponentInChildren<GameManager>().MissionLoad();
            q1_girl2 = MakeSimulation();
            q_girl2 = MathFunc.MatrixCopy(q1_girl2);
        }
    }

    public void PlayAvatar()
    {
        Play_s(q1, 0, q1.GetUpperBound(1) + 1);
    }

    public void PlayEnd()
    {
//        animateON = false;
        //        frameN = 0;
        transform.parent.GetComponentInChildren<GameManager>().InterpolationDDL();
        transform.parent.GetComponentInChildren<GameManager>().DisplayDDL(0, true);
    }

    private void Play_s(float[,] qq, int frFrame, int nFrames)
    {
        MainParameters.StrucJoints joints = MainParameters.Instance.joints;

        q = MathFunc.MatrixCopy(qq);
        frameN = 0;
        firstFrame = frFrame;
        numberFrames = nFrames;
        if (nFrames > 1)
        {
            if (joints.tc > 0)                          // Il y a eu contact avec le sol, alors seulement une partie des données sont utilisé
                timeFrame = joints.tc / (numberFrames - 1);
            else                                        // Aucun contact avec le sol, alors toutes les données sont utilisé
                timeFrame = joints.duration / (numberFrames - 1);
        }
        else
            timeFrame = 0;

        animateON = true;

/*        if (lineStickFigure == null && lineCenterOfMass == null && lineFilledFigure == null)
        {
            GameObject lineObject = new GameObject();
            LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
            lineRenderer.startWidth = 0.04f;
            lineRenderer.endWidth = 0.04f;

            lineStickFigure = new LineRenderer[joints.lagrangianModel.stickFigure.Length / 2];

            for (int i = 0; i < joints.lagrangianModel.stickFigure.Length / 2; i++)
            {
                lineStickFigure[i] = Instantiate(lineRenderer);
                lineStickFigure[i].name = string.Format("LineStickFigure{0}", i + 1);
                lineStickFigure[i].transform.parent = stickMan.transform;

                if (i <= 2 || (i >= 17 && i <= 19))                             // Côté gauche (jambe, pied, bras et main)
                {
                    lineStickFigure[i].startColor = new Color(0, 0.5882f, 0, 1);
                    lineStickFigure[i].endColor = new Color(0, 0.5882f, 0, 1);
                }
                else if ((i >= 3 && i <= 5) || (i >= 20 && i <= 22))             // Côté droit
                {
                    lineStickFigure[i].startColor = new Color(0.9412f, 0, 0.9412f, 1);
                    lineStickFigure[i].endColor = new Color(0.9412f, 0, 0.9412f, 1);
                }
            }

            lineCenterOfMass = Instantiate(lineRenderer);
            lineCenterOfMass.startColor = Color.red;
            lineCenterOfMass.endColor = Color.red;
            lineCenterOfMass.name = "LineCenterOfMass";
            lineCenterOfMass.transform.parent = stickMan.transform;

            lineFilledFigure = new LineRenderer[joints.lagrangianModel.filledFigure.Length / 4];

            for (int i = 0; i < joints.lagrangianModel.filledFigure.Length / 4; i++)
            {
                lineFilledFigure[i] = Instantiate(lineRenderer);
                lineFilledFigure[i].startColor = Color.yellow;
                lineFilledFigure[i].endColor = Color.yellow;
                lineFilledFigure[i].name = string.Format("LineFilledFigure{0}", i + 1);
                lineFilledFigure[i].transform.parent = stickMan.transform;
            }

            Destroy(lineObject);
        }*/
    }

    private void Quintic_s(float t, float ti, float tj, float qi, float qj, out float p, out float v, out float a)
    {
        if (t < ti)
            t = ti;
        else if (t > tj)
            t = tj;
        float tp0 = tj - ti;
        float tp1 = t - ti;
        float tp2 = tp1 / tp0;
        float tp3 = tp2 * tp2;
        float tp4 = tp3 * tp2 * (6 * tp3 - 15 * tp2 + 10);
        float tp5 = qj - qi;
        float tp6 = tj - t;
        float tp7 = Mathf.Pow(tp0, 5);
        p = qi + tp5 * tp4;
        v = 30 * tp5 * tp1 * tp1 * tp6 * tp6 / tp7;
        a = 60 * tp5 * tp1 * tp6 * (tj + ti - 2 * t) / tp7;
    }

    void Trajectory_s(LagrangianModelManager.StrucLagrangianModel lagrangianModel, float t, int[] qi, out float[] qd, out float[] qdotd, out float[] qddotd)
    {
        qd = new float[MainParameters.Instance.joints.lagrangianModel.nDDL];
        qdotd = new float[MainParameters.Instance.joints.lagrangianModel.nDDL];
        qddotd = new float[MainParameters.Instance.joints.lagrangianModel.nDDL];
        for (int i = 0; i < qd.Length; i++)
        {
            qd[i] = 0;
            qdotd[i] = 0;
            qddotd[i] = 0;
        }

        int n = qi.Length;

        // n=6, 6Node (HipFlexion, KneeFlexion ...)
        for (int i = 0; i < n; i++)
        {
            int ii = qi[i] - lagrangianModel.q2[0];
            MainParameters.StrucNodes nodes = MainParameters.Instance.joints.nodes[ii];
            int j = 1;
            while (j < nodes.T.Length - 1 && t > nodes.T[j]) j++;
            Quintic_s(t, nodes.T[j - 1], nodes.T[j], nodes.Q[j - 1], nodes.Q[j], out qd[ii], out qdotd[ii], out qddotd[ii]);
        }
    }

    private float[,] MakeSimulation()
    {
        MainParameters.StrucJoints joints = MainParameters.Instance.joints;
        float[] q0 = new float[joints.lagrangianModel.nDDL];
        float[] q0dot = new float[joints.lagrangianModel.nDDL];

//        float[] q0dotdot = new float[joints.lagrangianModel.nDDL];
//        Trajectory_s(joints.lagrangianModel, 0, joints.lagrangianModel.q2, out q0, out q0dot, out q0dotdot);

        for (int i = 0; i < 6; i++)
        {
            MainParameters.StrucNodes nodes = MainParameters.Instance.joints.nodes[i];
            q0[i] = nodes.Q[0];
        }

        // Biginning Pose
        // q0[12], q0dot[12], q0dotdot[12]

        int[] rotation = new int[3] { joints.lagrangianModel.root_somersault, joints.lagrangianModel.root_tilt, joints.lagrangianModel.root_twist };
        int[] rotationS = MathFunc.Sign(rotation);
        for (int i = 0; i < rotation.Length; i++) rotation[i] = Math.Abs(rotation[i]);

        int[] translation = new int[3] { joints.lagrangianModel.root_right, joints.lagrangianModel.root_foreward, joints.lagrangianModel.root_upward };
        int[] translationS = MathFunc.Sign(translation);
        for (int i = 0; i < translation.Length; i++) translation[i] = Math.Abs(translation[i]);

        float rotRadians = joints.takeOffParam.rotation * (float)Math.PI / 180;

        float tilt = joints.takeOffParam.tilt;
        if (tilt == 90)
            tilt = 90.001f;
        else if (tilt == -90)
            tilt = -90.01f;

        q0[Math.Abs(joints.lagrangianModel.root_tilt) - 1] = tilt * (float)Math.PI / 180;                                        // en radians
        q0[Math.Abs(joints.lagrangianModel.root_somersault) - 1] = rotRadians;                                         // en radians

        // q0[12]

        // q0[9] = somersault
        // q0[10] = tilt

        q0dot[Math.Abs(joints.lagrangianModel.root_foreward) - 1] = joints.takeOffParam.anteroposteriorSpeed;                       // en m/s
        q0dot[Math.Abs(joints.lagrangianModel.root_upward) - 1] = joints.takeOffParam.verticalSpeed;                                // en m/s
        q0dot[Math.Abs(joints.lagrangianModel.root_somersault) - 1] = joints.takeOffParam.somersaultSpeed * 2 * (float)Math.PI;     // en radians/s
        q0dot[Math.Abs(joints.lagrangianModel.root_twist) - 1] = joints.takeOffParam.twistSpeed * 2 * (float)Math.PI;               // en radians/s

        //q0dot[12]
        //q0dot[7] = AnteroposteriorSpeed
        //q0dot[8] = verticalSpeed
        //q0dot[9] = somersaultSpeed
        //q0dot[11] = twistSpeed

        double[] Q = new double[joints.lagrangianModel.nDDL];
        for (int i = 0; i < joints.lagrangianModel.nDDL; i++)
            Q[i] = q0[i];
        float[] tagX;
        float[] tagY;
        float[] tagZ;
        EvaluateTags_s(Q, out tagX, out tagY, out tagZ);

        // Q[12]
        // tagX[26], tagY[26], tagZ[26]

        //the last one = Center of Mass
        float[] cg = new float[3];
        cg[0] = tagX[tagX.Length - 1];
        cg[1] = tagY[tagX.Length - 1];
        cg[2] = tagZ[tagX.Length - 1];

        float[] u1 = new float[3];
        float[,] rot = new float[3, 1];
        for (int i = 0; i < 3; i++)
        {
            u1[i] = cg[i] - q0[translation[i] - 1] * translationS[i];
            rot[i, 0] = q0dot[rotation[i] - 1] * rotationS[i];
        }
        float[,] u = { { 0, -u1[2], u1[1] }, { u1[2], 0, -u1[0] }, { -u1[1], u1[0], 0 } };
        float[,] rotM = MathFunc.MatrixMultiply(u, rot);
        for (int i = 0; i < 3; i++)
        {
            q0dot[translation[i] - 1] = q0dot[translation[i] - 1] * translationS[i] + rotM[i, 0];
            q0dot[translation[i] - 1] = q0dot[translation[i] - 1] * translationS[i];
        }

        float hFeet = Math.Min(tagZ[joints.lagrangianModel.feet[0] - 1], tagZ[joints.lagrangianModel.feet[1] - 1]);
        float hHand = Math.Min(tagZ[joints.lagrangianModel.hand[0] - 1], tagZ[joints.lagrangianModel.hand[1] - 1]);

        // hFeet = min(tagZ[3],tagZ[7])
        // hHand = min(tagZ[14],tagZ[20])

        if (joints.condition < 8 && Math.Cos(rotRadians) > 0)
            q0[Math.Abs(joints.lagrangianModel.root_upward) - 1] += joints.lagrangianModel.hauteurs[joints.condition] - hFeet;
        else                                                            // bars, vault and tumbling from hands
            q0[Math.Abs(joints.lagrangianModel.root_upward) - 1] += joints.lagrangianModel.hauteurs[joints.condition] - hHand;

        //q0[8] = joints.lagrangianModel.hauteurs[joints.condition] - hFeet

        double[] x0 = new double[joints.lagrangianModel.nDDL * 2];
        for (int i = 0; i < joints.lagrangianModel.nDDL; i++)
        {
            x0[i] = q0[i];
            x0[joints.lagrangianModel.nDDL + i] = q0dot[i];
        }

        // x0[24]

        Options options = new Options();
        options.InitialStep = joints.lagrangianModel.dt;

        var sol = Ode.RK547M(0, joints.duration + joints.lagrangianModel.dt, new Vector(x0), ShortDynamics_s, options);

        ///////
//        ptr_model = c_biorbdModel(new StringBuilder("Modele_HuManS_somersault.s2mMod"));
//        var sol = Ode.RK45(0, new Vector(x0), ShortDynamicsBiorbd_s, options); //FD avec Biorbd
        ///////
       
        var points = sol.SolveFromToStep(0, joints.duration + joints.lagrangianModel.dt, joints.lagrangianModel.dt).ToArray();


        // test0 = point[51]
        // test1 = point[251]
        double[] t = new double[points.GetUpperBound(0) + 1];
        double[,] q = new double[joints.lagrangianModel.nDDL, points.GetUpperBound(0) + 1];
        double[,] qdot = new double[joints.lagrangianModel.nDDL, points.GetUpperBound(0) + 1];
        for (int i = 0; i < joints.lagrangianModel.nDDL; i++)
        {
            for (int j = 0; j <= points.GetUpperBound(0); j++)
            {
                if (i <= 0)
                    t[j] = points[j].T;

                q[i, j] = points[j].X[i];
                qdot[i, j] = points[j].X[joints.lagrangianModel.nDDL + i];
            }
        }

        // test0 = t[51], q[12,51], qdot[12,51]
        // test1 = t[251], q[12,251], qdot[12,251]
        int tIndex = 0;
        MainParameters.Instance.joints.tc = 0;
        for (int i = 0; i <= q.GetUpperBound(1); i++)
        {
            tIndex++;
            double[] qq = new double[joints.lagrangianModel.nDDL];
            for (int j = 0; j < joints.lagrangianModel.nDDL; j++)
                qq[j] = q[j, i];
            EvaluateTags_s(qq, out tagX, out tagY, out tagZ);
            if (joints.condition > 0 && tagZ.Min() < -0.05f)
            {
                MainParameters.Instance.joints.tc = (float)t[i];
                break;
            }
        }

        MainParameters.Instance.joints.t = new float[tIndex];
        float[,] qOut = new float[joints.lagrangianModel.nDDL, tIndex];
        float[,] qdot1 = new float[joints.lagrangianModel.nDDL, tIndex];
        for (int i = 0; i < tIndex; i++)
        {
            MainParameters.Instance.joints.t[i] = (float)t[i];
            for (int j = 0; j < joints.lagrangianModel.nDDL; j++)
            {
                qOut[j, i] = (float)q[j, i];
                qdot1[j, i] = (float)qdot[j, i];
            }
        }

        MainParameters.Instance.joints.rot = new float[tIndex, rotation.Length];
        MainParameters.Instance.joints.rotdot = new float[tIndex, rotation.Length];
        float[,] rotAbs = new float[tIndex, rotation.Length];
        for (int i = 0; i < rotation.Length; i++)
        {
            float[] rotCol = new float[tIndex];
            float[] rotdotCol = new float[tIndex];
            rotCol = MathFunc.unwrap(MathFunc.MatrixGetRow(qOut, rotation[i] - 1));
            rotdotCol = MathFunc.unwrap(MathFunc.MatrixGetRow(qdot1, rotation[i] - 1));
            for (int j = 0; j < tIndex; j++)
            {
                MainParameters.Instance.joints.rot[j, i] = rotCol[j] / (2 * (float)Math.PI);
                MainParameters.Instance.joints.rotdot[j, i] = rotdotCol[j] / (2 * (float)Math.PI);
                rotAbs[j, i] = Math.Abs(MainParameters.Instance.joints.rot[j, i]);
            }
        }

        return qOut;
    }

    private void EvaluateTags_s(double[] q, out float[] tagX, out float[] tagY, out float[] tagZ)
    {
        // q[12]

        double[] tag1;
        TagsSimple tagsSimple = new TagsSimple();
        tag1 = tagsSimple.Tags(q);

        // tag1[78]

        int newTagLength = tag1.Length / 3;

        // newTagLength = 26;

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

    private float[] qValuesHumans2Biorbd(float[] vecteurHumans)
    {
        //Correspondace des DDL entre les 2 modèles via un fichier matlab
        int nDDL = 14;// c_nQ(ptr_model);
        float[] vecteurBiorbd = new float[nDDL];

        vecteurBiorbd[0] = vecteurHumans[6];
        vecteurBiorbd[1] = vecteurHumans[7];
        vecteurBiorbd[2] = vecteurHumans[8];
        vecteurBiorbd[3] = -vecteurHumans[9];
        vecteurBiorbd[4] = vecteurHumans[10];
        vecteurBiorbd[5] = vecteurHumans[11];

        vecteurBiorbd[6] = vecteurHumans[0];
        vecteurBiorbd[7] = -vecteurHumans[1];
        vecteurBiorbd[8] = vecteurHumans[0];
        vecteurBiorbd[9] = -vecteurHumans[1];
        vecteurBiorbd[10] = vecteurHumans[2];
        vecteurBiorbd[11] = vecteurHumans[3];
        vecteurBiorbd[12] = vecteurHumans[4];
        vecteurBiorbd[13] = -vecteurHumans[5];

        return vecteurBiorbd;
    }

    private double[] Humans2Biorbd(double[] vecteurHumans)
    {
        //Correspondace des DDL entre les 2 modèles via un fichier matlab
        int nDDL = 14;// c_nQ(ptr_model);
        int nDDLhumans = 12;
        double[] vecteurBiorbd = new double[nDDL * 2];

        vecteurBiorbd[0] = vecteurHumans[6];
        vecteurBiorbd[1] = vecteurHumans[7];
        vecteurBiorbd[2] = vecteurHumans[8];
        vecteurBiorbd[3] = -vecteurHumans[9];
        vecteurBiorbd[4] = vecteurHumans[10];
        vecteurBiorbd[5] = vecteurHumans[11];

        vecteurBiorbd[6] = vecteurHumans[0];
        vecteurBiorbd[7] = -vecteurHumans[1];
        vecteurBiorbd[8] = vecteurHumans[0];
        vecteurBiorbd[9] = -vecteurHumans[1];
        vecteurBiorbd[10] = vecteurHumans[2];
        vecteurBiorbd[11] = vecteurHumans[3];
        vecteurBiorbd[12] = vecteurHumans[4];
        vecteurBiorbd[13] = -vecteurHumans[5];

        vecteurBiorbd[0 + nDDL] = vecteurHumans[6 + nDDLhumans];
        vecteurBiorbd[1 + nDDL] = vecteurHumans[7 + nDDLhumans];
        vecteurBiorbd[2 + nDDL] = vecteurHumans[8 + nDDLhumans];
        vecteurBiorbd[3 + nDDL] = -vecteurHumans[9 + nDDLhumans];
        vecteurBiorbd[4 + nDDL] = vecteurHumans[10 + nDDLhumans];
        vecteurBiorbd[5 + nDDL] = vecteurHumans[11 + nDDLhumans];

        vecteurBiorbd[6 + nDDL] = vecteurHumans[0 + nDDLhumans];
        vecteurBiorbd[7 + nDDL] = -vecteurHumans[1 + nDDLhumans];
        vecteurBiorbd[8 + nDDL] = vecteurHumans[0 + nDDLhumans];
        vecteurBiorbd[9 + nDDL] = -vecteurHumans[1 + nDDLhumans];
        vecteurBiorbd[10 + nDDL] = vecteurHumans[2 + nDDLhumans];
        vecteurBiorbd[11 + nDDL] = vecteurHumans[3 + nDDLhumans];
        vecteurBiorbd[12 + nDDL] = vecteurHumans[4 + nDDLhumans];
        vecteurBiorbd[13 + nDDL] = -vecteurHumans[5 + nDDLhumans];
        return vecteurBiorbd;
    }

    private double[] Biorbd2Humans(double[] vecteurBiorbd)
    {
        //Correspondace des DDL entre les 2 modèles via un fichier matlab
        int nDDL = 12; //nDDL modèle humans
        int nDDLbiorbd = 14;// c_nQ(ptr_model);
        double[] vecteurHumans = new double[nDDL * 2];

        vecteurHumans[6] = vecteurBiorbd[0];
        vecteurHumans[7] = vecteurBiorbd[1];
        vecteurHumans[8] = vecteurBiorbd[2];
        vecteurHumans[9] = -vecteurBiorbd[3];
        vecteurHumans[10] = vecteurBiorbd[4];
        vecteurHumans[11] = vecteurBiorbd[5];

        vecteurHumans[0] = vecteurBiorbd[6];
        vecteurHumans[1] = -vecteurBiorbd[7];
        vecteurHumans[2] = vecteurBiorbd[10];
        vecteurHumans[3] = vecteurBiorbd[11];
        vecteurHumans[4] = vecteurBiorbd[12];
        vecteurHumans[5] = -vecteurBiorbd[13];

        vecteurHumans[6 + nDDL] = vecteurBiorbd[0 + nDDLbiorbd];
        vecteurHumans[7 + nDDL] = vecteurBiorbd[1 + nDDLbiorbd];
        vecteurHumans[8 + nDDL] = vecteurBiorbd[2 + nDDLbiorbd];
        vecteurHumans[9 + nDDL] = -vecteurBiorbd[3 + nDDLbiorbd];
        vecteurHumans[10 + nDDL] = vecteurBiorbd[4 + nDDLbiorbd];
        vecteurHumans[11 + nDDL] = vecteurBiorbd[5 + nDDLbiorbd];

        vecteurHumans[0 + nDDL] = vecteurBiorbd[6 + nDDLbiorbd];
        vecteurHumans[1 + nDDL] = -vecteurBiorbd[7 + nDDLbiorbd];
        vecteurHumans[2 + nDDL] = vecteurBiorbd[10 + nDDLbiorbd];
        vecteurHumans[3 + nDDL] = vecteurBiorbd[11 + nDDLbiorbd];
        vecteurHumans[4 + nDDL] = vecteurBiorbd[12 + nDDLbiorbd];
        vecteurHumans[5 + nDDL] = -vecteurBiorbd[13 + nDDLbiorbd];

        return vecteurHumans;
    }

    private double[,] TransformerVecteurEnMatrice(double[] vecteur)
    {
        //Utilisée pour des matrices carrées
        double nouvelleDimension = Math.Sqrt(vecteur.Length);
        int dim = (int)nouvelleDimension;
        double[,] nouvelleMatrice = new double[dim, dim];
        for (int i = 0; i < nouvelleMatrice.GetLength(0); i++)
        {
            for (int j = 0; j < nouvelleMatrice.GetLength(1); j++)
            {
                nouvelleMatrice[j, i] = vecteur[j + nouvelleMatrice.GetLength(0) * i]; //On change le vecteur en matrice carrée
            }
        }
        return nouvelleMatrice;
    }

    private double[,] RetrecirMatriceCarre(double[,] matrice, int nouvelleTaille)
    {
        //NouvelleTaille doit être inférieure à la taille de matrice
        double[,] nouvelleMatrice = new double[nouvelleTaille, nouvelleTaille];
        for (int i = 0; i < nouvelleTaille; i++)
        {
            for (int j = 0; j < nouvelleTaille; j++)
            {
                nouvelleMatrice[i, j] = matrice[i, j];
            }
        }
        return nouvelleMatrice;
    }

    private double[] TransformerMatriceEnVecteur(double[,] matrice)
    {
        //Utilisée pour des matrices carrées
        double[] nouveauVecteur = new double[matrice.GetLength(0) * matrice.GetLength(1)];
        for (int i = 0; i < matrice.GetLength(0); i++)
        {
            for (int j = 0; j < matrice.GetLength(1); j++)
            {
                nouveauVecteur[j + i * matrice.GetLength(0)] = matrice[j, i]; //On change la matriceA carré en vecteur n fois plus grand
            }
        }
        return nouveauVecteur;
    }

    private Vector ShortDynamicsBiorbd_s(double t, Vector x)
    {
        //Declaration des pointeurs
        IntPtr ptr_massMatrix;
        IntPtr ptr_tau;
        IntPtr ptr_Q;
        IntPtr ptr_V;
        IntPtr ptr_qddot2;
        IntPtr ptr_matA;
        IntPtr ptr_solX;

        int NDDL = c_nQ(ptr_model); //Récupère le nombre de DDL du modèle biorbd
        int NROOT = 6; //On admet que la racine possède 6 ddl
        int NDDLhumans = 12;
        double[] xBiorbd = new double[NDDL * 2];

        double[] Qintegrateur = new double[NDDL];
        double[] Vintegrateur = new double[NDDL];
        double[] m_taud = new double[NDDL];
        double[] massMatrix = new double[NDDL * NDDL];

        float[] qd = new float[NDDLhumans];
        float[] qdotd = new float[NDDLhumans];
        float[] qddotd = new float[NDDLhumans];
        float[] qdBiorbd = new float[NDDL];
        float[] qdotdBiorbd = new float[NDDL];
        float[] qddotdBiorbd = new float[NDDL];

        double[] qddot2 = new double[NDDL];
        double[] qddot1integ = new double[NDDL * 2];
        double[] qddot1integHumans = new double[NDDLhumans];

        //Allocations des pointeurs, sinon génère erreurs de segmentation
        ptr_Q = Marshal.AllocCoTaskMem(sizeof(double) * Qintegrateur.Length);
        ptr_V = Marshal.AllocCoTaskMem(sizeof(double) * Vintegrateur.Length);
        ptr_qddot2 = Marshal.AllocCoTaskMem(sizeof(double) * qddot2.Length);
        ptr_massMatrix = Marshal.AllocCoTaskMem(sizeof(double) * massMatrix.Length);
        ptr_tau = Marshal.AllocCoTaskMem(sizeof(double) * m_taud.Length);

        xBiorbd = Humans2Biorbd(x); //On convertit les DDL du modèle humans pour le modèle biorbd

        for (int i = 0; i < NDDL; i++)
        {
            Qintegrateur[i] = xBiorbd[i];
            Vintegrateur[i] = xBiorbd[i + NDDL];
        }

        Trajectory_s(MainParameters.Instance.joints.lagrangianModel, (float)t, MainParameters.Instance.joints.lagrangianModel.q2, out qd, out qdotd, out qddotd);

        qdBiorbd = qValuesHumans2Biorbd(qd);
        qdotdBiorbd = qValuesHumans2Biorbd(qdotd);
        qddotdBiorbd = qValuesHumans2Biorbd(qddotd);

        for (int i = 0; i < qddot2.Length; i++)
        {
            qddot2[i] = qddotdBiorbd[i] + 10 * (qdBiorbd[i] - Qintegrateur[i]) + 3 * (qdotdBiorbd[i] - Vintegrateur[i]);
        }

        for (int i = 0; i < NROOT; i++)
        {
            qddot2[i] = 0;
        }

        Marshal.Copy(Qintegrateur, 0, ptr_Q, Qintegrateur.Length);
        Marshal.Copy(Vintegrateur, 0, ptr_V, Vintegrateur.Length);
        Marshal.Copy(qddot2, 0, ptr_qddot2, qddot2.Length);

        c_massMatrix(ptr_model, ptr_Q, ptr_massMatrix); //Génère la matrice de masse

        Marshal.Copy(ptr_massMatrix, massMatrix, 0, massMatrix.Length);

        c_inverseDynamics(ptr_model, ptr_Q, ptr_V, ptr_qddot2, ptr_tau);

        Marshal.Copy(ptr_tau, m_taud, 0, m_taud.Length);

        double[,] squareMassMatrix = new double[NDDL, NDDL];
        squareMassMatrix = TransformerVecteurEnMatrice(massMatrix); //La matrice de masse générée est sous forme d'un vecteur de taille NDDL*NDDL

        double[,] matriceA = new double[NROOT, NROOT];
        matriceA = RetrecirMatriceCarre(squareMassMatrix, NROOT); //On réduit la matrice de masse

        double[] matAGrandVecteur = new double[NROOT * NROOT];
        matAGrandVecteur = TransformerMatriceEnVecteur(matriceA); //La nouvelle matrice doit être convertie en vecteur pour qu'elle puisse être utilisée dans biorbd

        ptr_matA = Marshal.AllocCoTaskMem(sizeof(double) * matAGrandVecteur.Length);
        ptr_solX = Marshal.AllocCoTaskMem(sizeof(double) * NROOT);

        Marshal.Copy(matAGrandVecteur, 0, ptr_matA, matAGrandVecteur.Length);

        c_solveLinearSystem(ptr_matA, NROOT, NROOT, ptr_tau, ptr_solX); //Résouds l'équation Ax=b

        double[] solutionX = new double[NROOT];
        Marshal.Copy(ptr_solX, solutionX, 0, solutionX.Length);

        for (int i = 0; i < NROOT; i++)
        {
            qddot2[i] = -solutionX[i];
        }

        for (int i = 0; i < NDDL; i++)
        {
            qddot1integ[i] = Vintegrateur[i];
            qddot1integ[i + NDDL] = qddot2[i];
        }

        qddot1integHumans = Biorbd2Humans(qddot1integ); //Reconvertit les DDL du modèle biorbd vers le modèle humans

        //Desallocation des pointeurs
        Marshal.FreeCoTaskMem(ptr_Q);
        Marshal.FreeCoTaskMem(ptr_V);
        Marshal.FreeCoTaskMem(ptr_qddot2);
        Marshal.FreeCoTaskMem(ptr_massMatrix);
        Marshal.FreeCoTaskMem(ptr_tau);
        Marshal.FreeCoTaskMem(ptr_matA);
        Marshal.FreeCoTaskMem(ptr_solX);

        return new Vector(qddot1integHumans);
    }

    private Vector ShortDynamics_s(double t, Vector x)
    {
        int nDDL = MainParameters.Instance.joints.lagrangianModel.nDDL;

        double[] q = new double[nDDL];
        double[] qdot = new double[nDDL];
        for (int i = 0; i < nDDL; i++)
        {
            q[i] = x[i];
            qdot[i] = x[nDDL + i];
        }

        double[,] m12;
        double[] n1;
        Inertia11Simple inertia11Simple = new Inertia11Simple();
        double[,] m11 = inertia11Simple.Inertia11(q);

        Inertia12Simple inertia12Simple = new Inertia12Simple();
        m12 = inertia12Simple.Inertia12(q);
        NLEffects1Simple nlEffects1Simple = new NLEffects1Simple();
        n1 = nlEffects1Simple.NLEffects1(q, qdot);
        if (MainParameters.Instance.joints.condition <= 0)
        {
            double[] n1zero;
            n1zero = nlEffects1Simple.NLEffects1(q, new double[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            for (int i = 0; i < 6; i++)
                n1[i] = n1[i] - n1zero[i];
        }

        float kp = 10;
        float kv = 3;
        float[] qd = new float[nDDL];
        float[] qdotd = new float[nDDL];
        float[] qddotd = new float[nDDL];

        Trajectory_s(MainParameters.Instance.joints.lagrangianModel, (float)t, MainParameters.Instance.joints.lagrangianModel.q2, out qd, out qdotd, out qddotd);

        float[] qddot = new float[nDDL];
        for (int i = 0; i < nDDL; i++)
            qddot[i] = qddotd[i] + kp * (qd[i] - (float)q[i]) + kv * (qdotd[i] - (float)qdot[i]);

        double[,] mA = MatrixInverse.MtrxInverse(m11);

        double[] q2qddot = new double[MainParameters.Instance.joints.lagrangianModel.q2.Length];
        for (int i = 0; i < MainParameters.Instance.joints.lagrangianModel.q2.Length; i++)
            q2qddot[i] = qddot[MainParameters.Instance.joints.lagrangianModel.q2[i] - 1];
        double[,] mB = MatrixInverse.MtrxProduct(m12, q2qddot);

        double[,] n1mB = new double[mB.GetUpperBound(0) + 1, mB.GetUpperBound(1) + 1];
        for (int i = 0; i <= mB.GetUpperBound(0); i++)
            for (int j = 0; j <= mB.GetUpperBound(1); j++)
                n1mB[i, j] = -n1[i] - mB[i, j];

        double[,] mC = MatrixInverse.MtrxProduct(mA, n1mB);

        for (int i = 0; i < MainParameters.Instance.joints.lagrangianModel.q1.Length; i++)
            qddot[MainParameters.Instance.joints.lagrangianModel.q1[i] - 1] = (float)mC[i, 0];

        double[] xdot = new double[MainParameters.Instance.joints.lagrangianModel.nDDL * 2];
        for (int i = 0; i < MainParameters.Instance.joints.lagrangianModel.nDDL; i++)
        {
            xdot[i] = qdot[i];
            xdot[MainParameters.Instance.joints.lagrangianModel.nDDL + i] = qddot[i];
        }

        //xdot[24]
        return new Vector(xdot);
    }

    public void PlayOneFrame()
    {
        MainParameters.StrucJoints joints = MainParameters.Instance.joints;

        /*        for (int i = 0; i < joints.lagrangianModel.stickFigure.Length / 2; i++)
            Delete(lineStickFigure[i]);
        Delete(lineCenterOfMass);
        for (int i = 0; i < joints.lagrangianModel.filledFigure.Length / 4; i++)
            Delete(lineFilledFigure[i]);*/

        // test0 = qf[12], q[12,51]
        // test1 = qf[12], q[12,54]
        if(!isEditing)
            if (q.GetUpperBound(1) >= frameN)
            {
                qf = MathFunc.MatrixGetColumnD(q, firstFrame + frameN);
                if (playMode == MainParameters.Instance.languages.Used.animatorPlayModeGesticulation)
                    for (int i = 0; i < MainParameters.Instance.joints.lagrangianModel.q1.Length; i++)
                        qf[MainParameters.Instance.joints.lagrangianModel.q1[i] - 1] = 0;
            }

        ///////////////////////////////
        ///
        if (cntAvatar > 1)
        {
            if (q_girl2.GetUpperBound(1) >= frameN)
            {
                qf_girl2 = MathFunc.MatrixGetColumnD(q_girl2, firstFrame + frameN);
                if (playMode == MainParameters.Instance.languages.Used.animatorPlayModeGesticulation)
                    for (int i = 0; i < MainParameters.Instance.joints.lagrangianModel.q1.Length; i++)
                        qf_girl2[MainParameters.Instance.joints.lagrangianModel.q1[i] - 1] = 0;
            }
        }
        ///////////////////////////////

            /*            float[] tagX;
                        float[] tagY;
                        float[] tagZ;
                        EvaluateTags_s(qf, out tagX, out tagY, out tagZ);

                        // tagX[26]
                        // tagY[26]
                        // tagZ[26]
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

                        float tagHalfMaxMinZ = (tagZMax - tagZMin) * factorTags2Screen / 2;
                        for (int i = 0; i < newTagLength; i++)
                        {
                            tagX[i] = (tagX[i] - tagXMin) * factorTags2Screen - (tagXMax - tagXMin) * factorTags2Screen / 2;
                            tagY[i] = (tagY[i] - tagYMin) * factorTags2Screen - (tagYMax - tagYMin) * factorTags2Screen / 2;
                            tagZ[i] = (tagZ[i] - tagZMin) * factorTags2Screen - tagHalfMaxMinZ;
                        }

                        //        Vector3 tag[26]
                        Vector3[] tag = new Vector3[newTagLength];
                        for (int i = 0; i < newTagLength; i++)
                            tag[i] = new Vector3(tagX[i], tagY[i], tagZ[i]);*/

            //        for (int i = 0; i < joints.lagrangianModel.stickFigure.Length / 2; i++)
            //            Line(lineStickFigure[i], tag[joints.lagrangianModel.stickFigure[i, 0] - 1], tag[joints.lagrangianModel.stickFigure[i, 1] - 1]);
            //        Circle(lineCenterOfMass, 0.08f, tag[newTagLength - 1]);
            //        for (int i = 0; i < joints.lagrangianModel.filledFigure.Length / 4; i++)
            //            Triangle(lineFilledFigure[i], tag[joints.lagrangianModel.filledFigure[i, 0] - 1], tag[joints.lagrangianModel.filledFigure[i, 1] - 1], tag[joints.lagrangianModel.filledFigure[i, 2] - 1]);

        /////////////
        ////// Hip
        girl1LeftUp.transform.localEulerAngles = new Vector3(-(float)qf[0] * Mathf.Rad2Deg + 180, 0f, 0f);
        girl1RightUp.transform.localEulerAngles = new Vector3(-(float)qf[0] * Mathf.Rad2Deg - 180, 0f, 0f);
        // Knee
        girl1LeftLeg.transform.localEulerAngles = new Vector3((float)qf[1] * Mathf.Rad2Deg, 0f, 0f);
        girl1RightLeg.transform.localEulerAngles = new Vector3((float)qf[1] * Mathf.Rad2Deg, 0f, 0f);
        // Shoulder
        girl1LeftArm.transform.localRotation = Quaternion.AngleAxis((float)qf[2] * Mathf.Rad2Deg, Vector3.up) *
                                            Quaternion.AngleAxis(-(float)qf[3] * Mathf.Rad2Deg + 90f, Vector3.forward);

        girl1RightArm.transform.localRotation = Quaternion.AngleAxis(-(float)qf[4] * Mathf.Rad2Deg, Vector3.up) *
                                            Quaternion.AngleAxis((float)qf[5] * Mathf.Rad2Deg - 90f, Vector3.forward);

        // Root
        girl1Hip.transform.position = new Vector3((float)qf[6], (float)qf[8], (float)qf[7]);

        // Bio Order
        girl1Hip.transform.localRotation = Quaternion.AngleAxis((float)qf[9] * Mathf.Rad2Deg + 90f, Vector3.right) *
                                            Quaternion.AngleAxis((float)qf[10] * Mathf.Rad2Deg, Vector3.forward) *
                                            Quaternion.AngleAxis((float)qf[11] * Mathf.Rad2Deg, Vector3.up);

//        girl1Hip.transform.position += new Vector3(1.5f, 0, 0);

        if (cntAvatar > 1)
        {
            girl2LeftUp.transform.localEulerAngles = new Vector3(-(float)qf_girl2[0] * Mathf.Rad2Deg + 180, 0f, 0f);
            girl2RightUp.transform.localEulerAngles = new Vector3(-(float)qf_girl2[0] * Mathf.Rad2Deg - 180, 0f, 0f);
            girl2LeftLeg.transform.localEulerAngles = new Vector3((float)qf_girl2[1] * Mathf.Rad2Deg, 0f, 0f);
            girl2RightLeg.transform.localEulerAngles = new Vector3((float)qf_girl2[1] * Mathf.Rad2Deg, 0f, 0f);
            girl2LeftArm.transform.localRotation = Quaternion.AngleAxis((float)qf_girl2[2] * Mathf.Rad2Deg, Vector3.up) *
                                                Quaternion.AngleAxis(-(float)qf_girl2[3] * Mathf.Rad2Deg + 90f, Vector3.forward);
            girl2RightArm.transform.localRotation = Quaternion.AngleAxis(-(float)qf_girl2[4] * Mathf.Rad2Deg, Vector3.up) *
                                                Quaternion.AngleAxis((float)qf_girl2[5] * Mathf.Rad2Deg - 90f, Vector3.forward);
            girl2Hip.transform.localRotation = Quaternion.AngleAxis((float)qf_girl2[9] * Mathf.Rad2Deg + 90f, Vector3.right) *
                                                Quaternion.AngleAxis((float)qf_girl2[10] * Mathf.Rad2Deg, Vector3.forward) *
                                                Quaternion.AngleAxis((float)qf_girl2[11] * Mathf.Rad2Deg, Vector3.up);
            girl2Hip.transform.position = new Vector3((float)qf_girl2[6], (float)qf_girl2[8], (float)qf_girl2[7]);
        }

        if (!isPaused) frameN++;
    }

    public void PauseAvatar(bool pause)
    {
        girl1.transform.rotation = Quaternion.identity;
        isPaused = pause;
    }

    void OnGUI()
    {
        frameN = (int)GUI.HorizontalScrollbar(new Rect(Screen.width - 200, 430, 100, 30), frameN, 1.0F, 0.0F, numberFrames);

        if(transform.parent.GetComponentInChildren<AniGraphManager>().takeoffCanvas.activeSelf)
        {
            //            DrawingLine.DrawLine(new Vector2(frameN * 500/numberFrames + 32f, 325), new Vector2(frameN * 500 / numberFrames + 32f, 565), UnityEngine.Color.red, 4, false);

            if(MainParameters.Instance.joints.tc > 0)
                DrawingLine.DrawLine(new Vector2((frameN * 0.02f * 96 / MainParameters.Instance.joints.tc) + 30f, 325), new Vector2((frameN * 0.02f * 96 / MainParameters.Instance.joints.tc) + 30f, 565), UnityEngine.Color.red, 4, false);
            else
                DrawingLine.DrawLine(new Vector2((frameN * 0.02f * 500 / MainParameters.Instance.joints.duration) + 30f, 325), new Vector2((frameN * 0.02f * 500 / MainParameters.Instance.joints.duration) + 30f, 565), UnityEngine.Color.red, 4, false);

        }
    }

    private void Line(LineRenderer lineRendererObject, Vector3 position1, Vector3 position2)
    {
        Vector3[] pos = new Vector3[2];
        lineRendererObject.positionCount = 2;
        pos[0] = position1;
        pos[1] = position2;
        lineRendererObject.SetPositions(pos);
    }

    private void Circle(LineRenderer lineRendererObject, float radius, Vector3 center)
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

    private void Triangle(LineRenderer lineRendererObject, Vector3 position1, Vector3 position2, Vector3 position3)
    {
        Vector3[] pos = new Vector3[4];
        lineRendererObject.positionCount = 4;
        pos[0] = position1;
        pos[1] = position2;
        pos[2] = position3;
        pos[3] = position1;
        lineRendererObject.SetPositions(pos);
    }

    private void Delete(LineRenderer lineRendererObject)
    {
        lineRendererObject.positionCount = 0;
        //Destroy(lineRendererObject);
    }

    private void AddMarginOnMinMax(float factor)
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

    private void EvaluateFactorTags2Screen()
    {
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
