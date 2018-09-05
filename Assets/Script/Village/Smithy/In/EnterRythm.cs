using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterRythm : MonoBehaviour
{
    Vector2 scrollpos;
    //int startMusicIndex;

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
            scrollpos = new Vector2(0, 0);
            //startMusicIndex = 0;
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
            float BtnWidth = Screen.width / 1.2f, BtnHeight = Screen.height / 12f;

            //주석친 부분은 노래 선택시 스크롤이 버튼 클릭 방식을 쓸 경우 해제
            scrollpos = GUI.BeginScrollView(new Rect((Screen.width - BtnWidth) / 2, BtnHeight, BtnWidth, BtnHeight * 10), scrollpos, new Rect(0, 0, 0, BtnHeight * GameData.GetMusics().Count));
            //if (startMusicIndex > 0 && GUI.Button(new Rect((Screen.width - BtnWidth) / 2, 0, BtnWidth, BtnHeight), "▲")) startMusicIndex--;

            for (int i = 0; i <= PlayerData.Level; i++)
            //for (int i = startMusicIndex; i < startMusicIndex + 10; i++)
            {
                //if (i >= GameData.GetMusics().Count) break;
                //if (i > PlayerData.Level) break;
                if (GUI.Button(new Rect(0, BtnHeight * i, BtnWidth, BtnHeight), GameData.GetMusics()[i]))
                //if (GUI.Button(new Rect((Screen.width - BtnWidth) / 2, BtnHeight * (i - startMusicIndex + 1), BtnWidth, BtnHeight), GameData.GetMusics()[i]))
                {
                    RythmData.MyRythm.info.title = GameData.GetMusics()[i];

                    Scenes.Scenes.present = Scenes.Scene.SmithRythm;
                    GameObject[] temp = new GameObject[2];
                    temp[0] = GameObject.Find("Anvil Camera");
                    temp[1] = GameObject.Find("Smithy Camera");
                    Scenes.Scenes.ConvertCamera(temp);
                }
            }

            //if (startMusicIndex + 10 < GameData.GetMusics().Count - 1 && GUI.Button(new Rect((Screen.width - BtnWidth) / 2, BtnHeight * 11, BtnWidth, BtnHeight), "▼")) startMusicIndex++;

            GUI.EndScrollView();

            if (GUI.Button(new Rect(Screen.width * 34 / 35f, 0, Screen.width / 35f, Screen.height / 16f), "x")) Scenes.Scenes.present = Scenes.Scene.InSmithy;
        }
    }
}
