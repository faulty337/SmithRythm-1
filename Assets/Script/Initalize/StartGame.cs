using UnityEngine;
using Scenes;
using CamaraEffect;

//http://iw90.tistory.com/67?category=633211 - 모바일 화면 크기 조정 방법

public class StartGame : MonoBehaviour
{
    //건물 내부는 다른 Scene으로 구성되어 있어 원래 MainScene으로 돌아오면
    //MainScene의 모든 컴포넌트가 초기와 되므로 startPos에는 건물에 들어가기전 위치를 저장
    //건물 밖으로 나올때 Main Camera를 초기화 해주는 역할로 사용
    public static Vector3 startPos;

    void Awake()
    {
        if (Scenes.Scenes.present == Scenes.Scene.Initialization)
        {
            //http://prosto.tistory.com/185 - 화면 비율 조정
            Screen.orientation = ScreenOrientation.Landscape;
        }
        else if (Scenes.Scenes.present == Scene.InVillage)
        {
            CameraEffect.fade = false;
        }
    }

    void Start()
    {
        GameObject.Find("Main Camera").transform.position = startPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CameraEffect.fade && CameraEffect.alph != 0) return;

        if (Scenes.Scenes.present == Scene.MovingVillage)
        {
            //메인 카메라 이동 - 구름을 만든 후 같이 구성
            /*
            GameObject tempObj = GameObject.Find("Main Camera");
            Vector3 tempPos = new Vector3(tempObj.transform.position.x, tempObj.transform.position.y, tempObj.transform.position.z);
            if (tempPos.y > 200) tempObj.transform.position = new Vector3(tempPos.x - (float)0.24, tempPos.y - (float)0.66, tempPos.z + (float)0.27);
            else Scenes.Scenes.present = Scene.InVillage;
            */
            Scenes.Scenes.present = Scene.InVillage;
            Time.timeScale = 1;

            //나중에 구름이 흩어지는 장면을 구성할 것
        }
	}

    void OnGUI()
    {
        if (Scenes.Scenes.present == Scene.Initialization)
        {
            float btnWidth = Screen.width / 7f, btnHeight = Screen.height / 16f; //100, 20 - 대략    

            if (GUI.Button(new Rect((Screen.width - btnWidth) / 2, (Screen.height - btnHeight) / 2  + Screen.height / 4, btnWidth, btnHeight), "게임 시작", Scenes.Scenes.GUIAlign("button", (int)btnWidth / 8)))
            {
                Scenes.Scenes.present = Scene.MovingVillage;
            }
        }
    }
}