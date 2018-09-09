using UnityEngine;

//카메라 fade효과
//https://answers.unity.com/questions/193954/fade-camera.html
namespace CamaraEffect
{
    public class CameraEffect : MonoBehaviour
    {
        Texture2D blk;
        public static bool fade;
        public static float alph;

        // Use this for initialization
        void Start()
        {
            //make a tiny black texture
            blk = new Texture2D(1, 1);
            blk.SetPixel(0, 0, new Color(0, 0, 0, 0));
            blk.Apply();
        }

        // Update is called once per frame
        void OnGUI()
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blk);
        }

        void Update()
        {
            if (!fade && alph > 0) //FadeOut Effect
            {
                Time.timeScale = 0;
                alph -= 0.05f;
                if (alph < 0)
                {
                    alph = 0f;
                    Time.timeScale = 1;
                }
                blk.SetPixel(0, 0, new Color(0, 0, 0, alph));
                blk.Apply();
            }

            if (fade && alph < 1) //FadeIn Effect
            {
                Time.timeScale = 0;
                alph += 0.05f;
                if (alph > 1)
                {
                    alph = 1f;
                }
                blk.SetPixel(0, 0, new Color(0, 0, 0, alph));
                blk.Apply();
            }
        }
    }
}