using UnityEngine;

public class EnterSmithy : MonoBehaviour
{
    void OnMouseDown()
    {
        if (Scenes.Scenes.present == Scenes.Scene.InVillage)
        {
            Scenes.Scenes.present = Scenes.Scene.InSmithy;
            StartGame.startPos = GameObject.Find("Main Camera").transform.position;
            //Smithy Scene를 구성 후 들어가도록 구성할 것
        }
    }
}
