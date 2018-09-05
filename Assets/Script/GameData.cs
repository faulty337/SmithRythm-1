using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private static List<string> item; //만들 무기 - 노래에 따라서 상응 시킬 것
    private static List<string> music; //만들 무기에 상응하는 노래를 저장하는 변수 - 노래가 완성될 때 저장 시킬 것
    private static float speed; //게임의 속도
    public const int DAY = 10 * 24; //실제 시간과 맞추기 위한 상수

    public static List<string> Getitems()
    {
        return item;
    }

    public static List<string> GetMusics()
    {
        return music;
    }

    //게임의 시간
    public static float PresentTime
    {
        get
        {
            return PlayerPrefs.GetFloat("Time", 0);
        }

        set
        {
            //시간은 0보다 작을 수 없음
            if (value >= 0) PlayerPrefs.SetFloat("Time", value);
        }
    }

    //게임의 배속 - Time.Scale을 조정하는 방법도 있으나 이것은 리듬게임 속도까지 조정되버림
    public static float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            if (value > 0) speed = value;
        }
    }

    void Awake()
    {
        if (Scenes.Scenes.present == Scenes.Scene.Initialization)
        {
            //게임 데이터 가져오기 - 시간, 게임 속도
            Initialize(); //게임 테스트 중에는 게임을 계속 초기화함

            Speed = 24 * 60; //1일 = 24시간 = 24 * 60분 - 하루를 1분으로 계산

            music = new List<string>(PlayerPrefs.GetString("Music", "").Split(','));
            item = new List<string>(PlayerPrefs.GetString("GameItem", "").Split(','));
        }
    }

    void Initialize()
    {
        PresentTime = 0; //시간을 초기화하는 코드 - 태양 각도는 시간에 따라서 변화함
        //음악 파일과 그와 관련된 파일 포맷을 만들어 놓을 것
        PlayerPrefs.SetString("Music", "Romance,Long Long Ago");
        PlayerPrefs.SetString("GameItem", "sickle,grains,rake,shovel,treeaxe,sword"); //낫, 곡갱이, 긁갱이, 삽, 도끼, 검
    }
}