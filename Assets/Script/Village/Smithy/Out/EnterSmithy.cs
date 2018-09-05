using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterSmithy : MonoBehaviour
{
    void OnMouseDown()
    {
        if (Scenes.Scenes.present == Scenes.Scene.InVillage)
        {
            Scenes.Scenes.present = Scenes.Scene.InSmithy;
            StartGame.startPos = GameObject.Find("Main Camera").transform.position;
            SceneManager.LoadScene("SmithyScene");
        }
    }
}
