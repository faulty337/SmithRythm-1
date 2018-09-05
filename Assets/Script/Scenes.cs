using UnityEngine;

namespace Scenes
{
    public enum Scene
    {
        Initialization, //초기
        MovingVillage, InVillage, //마을로 메인 카메라 이동, 마을 내부
        MovingSmithy, InSmithy, //대장간 카메라 전환, 대장간 내부
        SelectMusic, SmithRythm, ShowItem //노래 선택창, 게임 카메라 전환 및 게임 중, 획득 아이템 확인
    }

    public class Scenes
    {
        public static Scene present = Scene.Initialization; //현재 게임의 상태(씬)
        public static GameObject[] mainCam; //활성화된 카메라

        public static void ConvertCamera(GameObject cam)
        {
            //사용되지 않을 카메라들은 전부 비활성화
            DeActiveMainCamera();

            //사용하려는 카메라 활성화
            cam.GetComponent<Camera>().enabled = true;
            cam.GetComponent<AudioListener>().enabled = true;

            //메인 카메라를 활성화된 카메라로 변경
            mainCam = new GameObject[1];
            mainCam[0] = cam;
        }

        public static void ConvertCamera(GameObject[] cam)
        {
            //사용되지 않는 카메라들은 전부 비활성화
            DeActiveMainCamera();

            //사용하려는 카메라 활성화
            for (int i = 0; i < cam.Length; i++)
            {
                cam[i].GetComponent<Camera>().enabled = true;
                if (i == 0) cam[i].GetComponent<AudioListener>().enabled = true;
                else cam[i].GetComponent<AudioListener>().enabled = false;
            }

            //메인 카메라를 활성화된 카메라로 변경
            mainCam = new GameObject[cam.Length];
            for (int i = 0; i < cam.Length; i++)
            {
                mainCam[i] = cam[i];
                mainCam[i].GetComponent<Camera>().rect = new Rect(i / (float)cam.Length, 0, 1 / (float)cam.Length, 1);
            }
        }

        public static void ConvertCamera(GameObject[] cam, Rect[] rect)
        {
            //사용되지 않는 카메라들은 전부 비활성화
            DeActiveMainCamera();

            //사용하려는 카메라 활성화
            for (int i = 0; i < cam.Length; i++)
            {
                cam[i].GetComponent<Camera>().enabled = true;
                if (i == 0) cam[i].GetComponent<AudioListener>().enabled = true;
                else cam[i].GetComponent<AudioListener>().enabled = false;
            }

            //메인 카메라를 활성화된 카메라로 변경
            mainCam = new GameObject[cam.Length];
            for (int i = 0; i < cam.Length; i++)
            {
                mainCam[i] = cam[i];
                mainCam[i].GetComponent<Camera>().rect = rect[i];
            }
        }

        static void DeActiveMainCamera()
        {
            //게임 시작 처음에는 카메라 설정이 되어 있지 않음
            if (mainCam == null) return;

            for (int i = 0; i < mainCam.Length; i++)
            {
                if (mainCam[i] == null) continue; //Scene 전환 시에는 다른 Scene의 카메라는 Destroy됨
                mainCam[i].GetComponent<Camera>().enabled = false;
                mainCam[i].GetComponent<Camera>().rect = new Rect(0, 0, 1, 1);
                mainCam[i].GetComponent<AudioListener>().enabled = false;
            }
        }
    }
}