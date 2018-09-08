using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowItem : MonoBehaviour
{
	// Update is called once per frame
	void Update()
    {
		if (Scenes.Scenes.present == Scenes.Scene.ShowItem)
        {
            //환하게 변경
            GameObject.Find("Directional Light").transform.rotation = new Quaternion(0, 0, 0, 1);
            GameObject.Find("Directional Light").transform.Rotate(Vector3.left * 90 * GameData.Speed / GameData.DAY);

            //보여줄 아이템을 획득한 아이템으로 변경(모양)
            //http://www.devkorea.co.kr/bbs/board.php?bo_table=m03_qna&wr_id=72483 - GetComponentsInChildren의 설명
            MeshFilter[] meshfilterTmp = GameObject.Find("All Items").GetComponentsInChildren<MeshFilter>();
            foreach (MeshFilter meshfilter in meshfilterTmp)
            {
                if (meshfilter.name.Equals(GameData.Getitems()[GameData.GetMusics().IndexOf(RythmData.MyRythm.info.title)]))
                {
                    gameObject.GetComponent<MeshFilter>().mesh = meshfilter.mesh;
                }
            }
            
            //보여줄 아이템 외 아이템들은 보여주지 않음
            Renderer[] itemRenderer = GameObject.Find("All Items").GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in itemRenderer)
            {
                renderer.enabled = false;
            }

            //시간을 멈춤
            Time.timeScale = 0;
        }
    }

    void OnGUI()
    {
        if (Scenes.Scenes.present == Scenes.Scene.ShowItem)
        {
            float btnwidth = Screen.width / 7f, btnheight = Screen.height / 16f; //100, 20 - 대략

            if (GUI.Button(new Rect((Screen.width - btnwidth) / 2, (Screen.height - btnheight) / 2 + Screen.height / 2.5f, btnwidth, btnheight), "확인", Scenes.Scenes.GUIAlign("button", (int)btnwidth / 8)))
            {
                //확인 버튼을 누르면 카메라를 리듬게임 시작 전으로 돌리고 태양위치를 원래대로 돌린후 시간을 동작
                Scenes.Scenes.present = Scenes.Scene.InSmithy;
                SceneManager.LoadScene("SmithyScene");

                Time.timeScale = 1;
            }
        }
    }
}
