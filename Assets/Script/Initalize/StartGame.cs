using UnityEngine;
using Scenes;

//http://iw90.tistory.com/67?category=633211 - 모바일 화면 크기 조정 방법

public class StartGame : MonoBehaviour
{
    //건물 내부는 다른 Scene으로 구성되어 있어 원래 MainScene으로 돌아오면
    //MainScene의 모든 컴포넌트가 초기와 되므로 startPos에는 건물에 들어가기전 위치를 저장
    //건물 밖으로 나올때 Main Camera를 초기화 해주는 역할로 사용
    public static Vector3 startPos;

    void Awake()
    {
        //http://prosto.tistory.com/185 - 화면 비율 조정
        //http://jungmonster.tistory.com/187
        //Screen.SetResolution(가로 픽셀, 세로 픽셀, full screen 유무);
        //Screen.SetResolution(Screen.width, (Screen.width * 16) / 9, false);
        Screen.orientation = ScreenOrientation.Landscape;
        Scenes.Scenes.ConvertCamera(GameObject.Find("Main Camera"));
        if (startPos != new Vector3(0, 0, 0)) GameObject.Find("Main Camera").transform.position = startPos;
    }
	
	// Update is called once per frame
	void Update()
    {
        if (Scenes.Scenes.present == Scene.MovingVillage)
        {
            //메인 카메라 이동 - 구름을 만든 후 같이 구성
            /*
            GameObject tempObj = GameObject.Find("Main Camera");
            Vector3 tempPos = new Vector3(tempObj.transform.position.x, tempObj.transform.position.y, tempObj.transform.position.z);
            if (tempPos.y > 200) tempObj.transform.position = new Vector3(tempPos.x - (float)0.24, tempPos.y - (float)0.66, tempPos.z + (float)0.27);
            else Scenes.Scenes.present = Scene.InVillage;
            */

            //나중에 구름이 흩어지는 장면을 구성할 것
        }
	}

    void OnGUI()
    {
        GUIStyle btnStyle = GUI.skin.button;

        if (Scenes.Scenes.present == Scene.Initialization)
        {
            float BtnWidth = Screen.width / 7f, BtnHeight = Screen.height / 16f; //100, 20 - 대략    
            btnStyle.fontSize = (int)BtnWidth / 8;

            if (GUI.Button(new Rect((Screen.width - BtnWidth) / 2, (Screen.height - BtnHeight) / 2  + Screen.height / 4, BtnWidth, BtnHeight), "Game Start", btnStyle))
            {
                Scenes.Scenes.present = Scene.MovingVillage;
                Time.timeScale = 1;
            }
        }
    }
}