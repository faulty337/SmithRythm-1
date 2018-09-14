using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private static List<string> item; //만들 무기 - 노래에 따라서 상응 시킬 것
    private static List<string> music; //만들 무기에 상응하는 노래를 저장하는 변수 - 노래가 완성될 때 저장 시킬 것

    public static string GetItem(int index)
    {
        return item[index];
    }

    public static int CountItem()
    {
        return item.Count;
    }

    public static string GetMusic(int index)
    {
        return music[index];
    }

    public static int CountMusic()
    {
        return music.Count;
    }

    void Awake()
    {
        if (Scenes.Scenes.present == Scenes.Scene.Initialization)
        {
            //게임 데이터 가져오기 - 시간, 게임 속도
            Initialize(); //게임 테스트 중에는 게임을 계속 초기화함

            music = new List<string>(PlayerPrefs.GetString("Music", "").Split(','));
            item = new List<string>(PlayerPrefs.GetString("GameItem", "").Split(','));
        }
    }

    void Initialize()
    {
        //음악 파일과 그와 관련된 파일 포맷을 만들어 놓을 것
        PlayerPrefs.SetString("Music", "Romance,Long Long Ago");
        PlayerPrefs.SetString("GameItem", "sickle,grains,rake,shovel,treeaxe,sword"); //낫, 곡갱이, 긁갱이, 삽, 도끼, 검
    }
}