using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterSmithy : MonoBehaviour
{
    void Update()
    {
        if (Scenes.Scenes.present == Scenes.Scene.InVillage)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject target = GetClickedObject();
                if (target != null && target.Equals(gameObject))
                {
                    Scenes.Scenes.present = Scenes.Scene.InSmithy;
                    StartGame.startPos = GameObject.Find("Main Camera").transform.position;
                    SceneManager.LoadScene("SmithyScene");
                }
            }
        }
    }

    //http://blog.naver.com/ateliersera/220439790504 마우스 클릭
    private GameObject GetClickedObject()
    {
        RaycastHit hit;
        GameObject target = null;
        //마우스 포인트 근처 좌표를 만든다.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //마우스 근처에 오브젝트가 있는지 확인
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            target = hit.collider.gameObject;
        }

        return target;
    }
}
