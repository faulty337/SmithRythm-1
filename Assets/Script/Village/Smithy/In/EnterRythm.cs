using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterRythm : MonoBehaviour
{
    void Awake()
    {
        if (Scenes.Scenes.present == Scenes.Scene.Initialization)
        {
            SceneManager.LoadScene("MainScene");
        }
        else if (Scenes.Scenes.present == Scenes.Scene.InSmithy)
        {
            Scenes.Scenes.ConvertCamera(GameObject.Find("Smithy Camera"));
        }
    }

    void OnMouseDown()
    {
        if (Scenes.Scenes.present == Scenes.Scene.InSmithy)
        {
            Scenes.Scenes.present = Scenes.Scene.SelectMusic;
        }
    }

    void OnGUI()
    {
        if (Scenes.Scenes.present == Scenes.Scene.InSmithy)
        {
            float BtnWidth = Screen.width / 7f, BtnHeight = Screen.height / 16f; //100, 20 - 대략
            if (GUI.Button(new Rect(BtnWidth / 10, BtnHeight / 2, BtnWidth, BtnHeight), "나가기"))
            {
                Scenes.Scenes.present = Scenes.Scene.InVillage;
                SceneManager.LoadScene("MainScene");
            }
        }

        if (Scenes.Scenes.present == Scenes.Scene.SelectMusic)
        {
            //노래 선택화면이 나오도록 수정할 것
        }
    }
}