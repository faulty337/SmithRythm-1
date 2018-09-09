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

        public static GUIStyle GUIAlign(string name, int fontSize)
        {
            GUIStyle style = new GUIStyle();
            if (name.Equals("button"))
            {
                style = GUI.skin.button;
                style.alignment = TextAnchor.MiddleCenter;
            }
            else if (name.Equals("label"))
            {
                style = GUI.skin.label;
                style.alignment = TextAnchor.MiddleLeft;
            }

            style.fontSize = fontSize;

            return style;
        }
    }
}