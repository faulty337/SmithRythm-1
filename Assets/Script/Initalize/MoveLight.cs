using UnityEngine;
using Scenes;

//게임 오브젝트 회전에 관한 설명
//https://m.blog.naver.com/PostView.nhn?blogId=azanghs&logNo=90179216640&proxyReferer=https%3A%2F%2Fwww.google.co.kr%2F

public class MoveLight : MonoBehaviour
{
    void Start()
    {
        for (float i = 0; i < GameData.PresentTime; i += GameData.Speed / (7200 * GameData.DAY))
        {
            gameObject.transform.Rotate(Vector3.left * Time.fixedDeltaTime * GameData.Speed / GameData.DAY);
        }
    }

    void FixedUpdate()
    {
        if (Scenes.Scenes.present != Scene.Initialization && Scenes.Scenes.present != Scene.ShowItem)
        {
            gameObject.transform.Rotate(Vector3.left * Time.fixedDeltaTime / GameData.DAY * GameData.Speed);
            GameData.PresentTime += GameData.Speed / (7200 * GameData.DAY);
            /*
            계산법
            원의 총 각도는 360도
            0시는 0도를 가르킴
            1도당 1초로 흘러가게 두면 하루는 360초 = 6분 - Vector3.left * Time.fixedDeltaTime
            1시간은 360 * 10초, 1일은 360 * 10 * 24초이므로
            6분이 하루라고 계산 될 때 1 / (10 * 24) = 1 / 240(GameData.DAY)배속이면
            실제시간과 유사하게 태양이 회전함
            태양 위치와 각을 바꾸는 실험을 한 결과 : 땅의 밝기는 태양 위치와는 관계가 없었음

            시간 계산의 7200은 360도 도는데 0.05(Time.fixedDeltaTime)씩 증가하는 식으로 계산하면
            메시지로 표시할 때 360 / 0.05 = 7200개 이므로 7200으로 계산
            */
        }
    }
}
