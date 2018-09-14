using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private static int fullLevel = 7;
    private static List<string> item; //만든 무기를 저장하는 변수 - string으로 저장

    //플레이어의 레벨을 저장하는 변수
    //처음에는 하급 농기구만 만들 수 있으므로 0으로 지정
    //레벨에 따라서 할 수 있는 리듬게임 노래를 제한 할 것
    public static int Level
    {
        get
        {
            return PlayerPrefs.GetInt("Level", 0);
        }

        set
        {
            if (value < fullLevel)
            {
                if (Level < value) PlayerPrefs.SetInt("Level", value);
            }
            else
            {
                //만렙일 경우 더이상 경험치가 쌓이지 않게 만렙으로 조정
                PlayerPrefs.SetInt("Level", fullLevel);
            }
        }
    }

    //경험치
    public static int Exp
    {
        get
        {
            return PlayerPrefs.GetInt("Exp", 0);
        }

        set
        {
            if (Exp + value >= Mathf.Pow(Exp + 2, 2) + Exp)
            {
                Level++;
                Exp = 0;
            }
            else
            {
                Exp += value;
            }
        }
    }

    public static int CountItem()
    {
        return item.Count;
    }

    public static string GetItem(int index)
    {
        return item[index];
    }

    public static void AddItem(string addItem)
    {
        if (PlayerPrefs.GetString("Item", "").Equals(""))
        {
            PlayerPrefs.SetString("Item", addItem);
        }
        else
        {
            PlayerPrefs.SetString("Item", PlayerPrefs.GetString("Item") + "," + addItem);
        }

        item.Add(addItem);
    }

    public static void DeleteItem(string deleteItem)
    {
        if (!PlayerPrefs.GetString("Item", "").Equals(""))
        {
            if (item.IndexOf(deleteItem) == -1)
            {
                Debug.Log("아이템이 없습니다.");
            }
            else
            {
                string itemList = PlayerPrefs.GetString("Item");
                if (item.IndexOf(deleteItem) == 0)
                {
                    PlayerPrefs.SetString("Item", itemList.Remove(0, deleteItem.Length));
                }
                else
                {
                    PlayerPrefs.SetString("Item", itemList.Remove(itemList.IndexOf(deleteItem) - 1, deleteItem.Length + 1));
                }

                item.Remove(deleteItem);
            }
        }
        else
        {
            Debug.Log("아이템창이 비어있습니다.");
        }
    }

    void Awake()
    {
        if (Scenes.Scenes.present == Scenes.Scene.Initialization)
        {
            //플레이어 데이터 가져오기 - 획득 아이템, 레벨(Level 변수의 반환값)
            Initialize(); //게임 테스트 중에는 게임을 계속 초기화함

            //사용자 데이터 가져오기 - 만들 수 있는 아이템의 최고 인덱스, 만든 아이템 등
            item = new List<string>(PlayerPrefs.GetString("Item", "").Split(','));
            fullLevel = GameData.CountItem() - 1;
        }
    }

    void Initialize()
    {
        PlayerPrefs.SetString("Item", "");
        PlayerPrefs.SetInt("Level", 0);
        PlayerPrefs.SetInt("Exp", 0);
    }
}