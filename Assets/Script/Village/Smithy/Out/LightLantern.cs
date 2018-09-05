using UnityEngine;

//EmissionColor 수정
//http://blog.naver.com/PostView.nhn?blogId=showmeii1201&logNo=220351777394&redirect=Dlog&widgetTypeCall=true

public class LightLantern : MonoBehaviour
{
    void Start()
    {
        Light();
    }

	// Update is called once per frame
	void Update()
    {
        Light();
    }

    void Light()
    {
        if (GameData.PresentTime - (int)GameData.PresentTime >= 0.5)
        {
            gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0, 0, 0));
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(1, 1, 1));
        }
    }
}
