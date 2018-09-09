using UnityEngine;
using UnityEngine.SceneManagement;
using CamaraEffect;
using RythmData;

public class ShowItem : MonoBehaviour
{
    public MeshFilter[] itemMesh;

    void Awake()
    {
        if (Scenes.Scenes.present == Scenes.Scene.Initialization)
        {
            SceneManager.LoadScene("MainScene");
        }
        else if (Scenes.Scenes.present == Scenes.Scene.ShowItem)
        {
            //요구 내용 : 아이템 제작 완료 창 및 아이템을 획득 시킬 것
            //추가해야할 내용 : 경험치 증가 관련
            PlayerData.AddItem(GameData.Getitems()[GameData.GetMusics().IndexOf(MyRythm.info.title)]);
            CameraEffect.fade = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!CameraEffect.fade && CameraEffect.alph != 0) return;

        if (Scenes.Scenes.present == Scenes.Scene.ShowItem)
        {
            //획득한 아이템을 보여줌
            foreach (MeshFilter meshfilter in itemMesh)
            {
                if (meshfilter.name.Equals(GameData.Getitems()[GameData.GetMusics().IndexOf(RythmData.MyRythm.info.title)]))
                {
                    gameObject.GetComponent<MeshFilter>().mesh = meshfilter.mesh;
                }
            }

            //시간을 멈춤
            Time.timeScale = 0;
        }

        if (CameraEffect.fade && CameraEffect.alph == 1)
        {
            if (Scenes.Scenes.present == Scenes.Scene.InSmithy)
            {
                SceneManager.LoadScene("SmithyScene");
            }
        }
    }

    void OnGUI()
    {
        if (!CameraEffect.fade && CameraEffect.alph != 0) return;

        if (Scenes.Scenes.present == Scenes.Scene.ShowItem)
        {
            float btnwidth = Screen.width / 7f, btnheight = Screen.height / 16f; //100, 20 - 대략

            if (GUI.Button(new Rect((Screen.width - btnwidth) / 2, (Screen.height - btnheight) / 2 + Screen.height / 2.5f, btnwidth, btnheight), "확인", Scenes.Scenes.GUIAlign("button", (int)btnwidth / 8)))
            {
                //확인 버튼을 누르면 카메라를 리듬게임 시작 전으로 돌리고 태양위치를 원래대로 돌린후 시간을 동작
                Time.timeScale = 1;

                Scenes.Scenes.present = Scenes.Scene.InSmithy;
                CameraEffect.fade = true;
            }
        }
    }
}
