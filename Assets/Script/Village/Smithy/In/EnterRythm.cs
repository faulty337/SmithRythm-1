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
        GUIStyle btnStyle = GUI.skin.button;

        if (Scenes.Scenes.present == Scenes.Scene.InSmithy)
        {
            float btnWidth = Screen.width / 7f, btnHeight = Screen.height / 16f; //100, 20 - 대략
            btnStyle.fontSize = (int)btnWidth / 8;

            if (GUI.Button(new Rect(btnWidth / 10, btnHeight / 2, btnWidth, btnHeight), "나가기", btnStyle))
            {
                Scenes.Scenes.present = Scenes.Scene.InVillage;
                SceneManager.LoadScene("MainScene");
            }
        }

        if (Scenes.Scenes.present == Scenes.Scene.SelectMusic)
        {
            float btnWidth = Screen.width / 1.2f, btnHeight = Screen.height / 12f;
            btnStyle.fontSize = (int)btnWidth / 8;

            //주석친 부분은 노래 선택시 스크롤이 버튼 클릭 방식을 쓸 경우 해제
            scrollpos = GUI.BeginScrollView(new Rect((Screen.width - btnWidth) / 2, btnHeight, btnWidth, btnHeight * 10), scrollpos, new Rect(0, 0, 0, btnHeight * GameData.GetMusics().Count));
            //if (startMusicIndex > 0 && GUI.Button(new Rect((Screen.width - btnWidth) / 2, 0, btnWidth, btnHeight), "▲")) startMusicIndex--;

            for (int i = 0; i <= PlayerData.Level; i++)
            //for (int i = startMusicIndex; i < startMusicIndex + 10; i++)
            {
                //if (i >= GameData.GetMusics().Count) break;
                //if (i > PlayerData.Level) break;
                if (GUI.Button(new Rect(0, btnHeight * i, btnWidth, btnHeight), GameData.GetMusics()[i], btnStyle))
                //if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnHeight * (i - startMusicIndex + 1), btnWidth, btnHeight), GameData.GetMusics()[i], btnStyle))
                {
                    RythmData.MyRythm.info.title = GameData.GetMusics()[i];

                    Scenes.Scenes.present = Scenes.Scene.SmithRythm;
                    GameObject[] temp = new GameObject[2];
                    temp[0] = GameObject.Find("Anvil Camera");
                    temp[1] = GameObject.Find("Smithy Camera");
                    Scenes.Scenes.ConvertCamera(temp);
                }
            }

            //if (startMusicIndex + 10 < GameData.GetMusics().Count - 1 && GUI.Button(new Rect((Screen.width - btnWidth) / 2, btnHeight * 11, btnWidth, btnHeight), "▼", btnStyle)) startMusicIndex++;

            GUI.EndScrollView();

            btnWidth = Screen.width / 35f;
            btnStyle.fontSize = (int)btnWidth / 8;
            if (GUI.Button(new Rect(Screen.width * 34 / 35f, 0, Screen.width / 35f, Screen.height / 16f), "x", btnStyle)) Scenes.Scenes.present = Scenes.Scene.InSmithy;
        }
    }
}
