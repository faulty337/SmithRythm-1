using UnityEngine;
using UnityEngine.SceneManagement;
using CamaraEffect;
using RythmData;

public class ShowItem : MonoBehaviour
{
    int index; //보여줄 아이템의 인덱스

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
            for (int i = 0; i < GameData.CountMusic(); i++)
            {
                if (GameData.GetMusic(i).Equals(MyRythm.info.title))
                {
                    index = i;
                    break;
                }
            }
            PlayerData.AddItem(GameData.GetItem(index));

            //Material 색 바꾸기
            //https://m.blog.naver.com/PostView.nhn?blogId=wangkisa&logNo=173279215&proxyReferer=https%3A%2F%2Fwww.google.co.kr%2F
            Texture itemTexture = Resources.Load("Item/" + GameData.GetItem(index), typeof(Texture)) as Texture;
            Mesh itemMesh = Resources.Load("Item/" + GameData.GetItem(index), typeof(Mesh)) as Mesh;
            //획득한 아이템을 보여줌
            gameObject.GetComponent<MeshFilter>().mesh = itemMesh;
            gameObject.GetComponent<Renderer>().material.mainTexture = itemTexture;

            CameraEffect.fade = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!CameraEffect.fade && CameraEffect.alph != 0) return;

        if (Scenes.Scenes.present == Scenes.Scene.ShowItem)
        {
            
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
                Scenes.Scenes.present = Scenes.Scene.InSmithy;
                CameraEffect.fade = true;
            }
        }
    }
}
