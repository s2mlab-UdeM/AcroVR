using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Drawing;
//using System.Drawing.Imaging;

public class PlayGif : MonoBehaviour
{
    public float speed = 1;
    List<Texture2D> gifFrames = new List<Texture2D>();
    Renderer render;

    void Start () {
/*        var gifImage = Image.FromFile("Assets/Art/background.gif");
        var dimension = new FrameDimension(gifImage.FrameDimensionsList[0]);
        int frameCount = gifImage.GetFrameCount(dimension);
        for (int i = 0; i < frameCount; i++)
        {
            gifImage.SelectActiveFrame(dimension, i);
            var frame = new Bitmap(gifImage.Width, gifImage.Height);
            System.Drawing.Graphics.FromImage(frame).DrawImage(gifImage, Point.Empty);
            var frameTexture = new Texture2D(frame.Width, frame.Height);
            for (int x = 0; x < frame.Width; x++)
                for (int y = 0; y < frame.Height; y++)
                {
                    System.Drawing.Color sourceColor = frame.GetPixel(x, y);
//                    frameTexture.SetPixel(frame.Width - 1 - x, y, new Color32(sourceColor.R, sourceColor.G, sourceColor.B, sourceColor.A)); // for some reason, x is flipped
                    frameTexture.SetPixel(x,-y, new Color32(sourceColor.R, sourceColor.G, sourceColor.B, sourceColor.A)); // for some reason, x is flipped
                }
            frameTexture.Apply();
            gifFrames.Add(frameTexture);
        }
        
        render = GetComponent<Renderer>();*/
    }

    void Update () {
        render.material.mainTexture = gifFrames[(int)(Time.frameCount * speed) % gifFrames.Count];
    }

    //    void OnGUI()
    //    {
    //       GUI.DrawTexture(new Rect(drawPosition.x, drawPosition.y, gifFrames[0].width, gifFrames[0].height), gifFrames[(int)(Time.frameCount * speed) % gifFrames.Count]);
    //    }
}
