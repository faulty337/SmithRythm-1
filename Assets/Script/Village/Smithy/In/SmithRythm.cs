using UnityEngine;
using UnityEngine.SceneManagement;
using RythmData;
using CamaraEffect;

//http://itpangpang.xyz/177 - 유니티 마우스 관련 함수
//https://blog.naver.com/live_for_dream/220883895920 - 유니티 화면 분할

enum Judge //판정 기준
{
    Perfect = 10,
    Great = 8,
    Normal = 5,
    Fail = 0
}

public class SmithRythm : MonoBehaviour
{
    AudioSource anvilAudio;

    float startTime; //노래 자체는 시작하자마자 나오므로 시작 시간을 지정할 때 사용
    float speed;
    const float MULTISPEED = 20;
    int circleIndex, bpmIndex; //터치 시 사라지는 인덱스와 bpm에 맞추어야하는 인덱스를 구분한다.

    GameObject[] circleNote; // 나오는 원들을 의미

    float energy, endEnergy; //진행도
    int score; //게임 점수
    int combo;

    public Animation SmithAnimation;
    public AnimationClip hammer;

    public ParticleSystem hammerEffect;
    
    void Awake()
    {
        if (Scenes.Scenes.present == Scenes.Scene.Initialization)
        {
            SceneManager.LoadScene("MainScene");
        }
        else if (Scenes.Scenes.present == Scenes.Scene.SmithRythm)
        {
            CameraEffect.fade = false;
        }
    }

    void Start()
    {
        energy = 100;
        endEnergy = 80;
        SmithAnimation.Play();
        hammerEffect.Stop();

        //노트 파일 읽고 노트 생성
        MyRythm.ReadFile("NoteText/" + MyRythm.info.title);
        circleNote = new GameObject[MyRythm.info.totalCount];

        GameObject.Find("Circle Original").GetComponent<Renderer>().enabled = true;
        GameObject original = GameObject.Find("Circle Original");
        for (int i = 0; i < MyRythm.info.totalCount; i++)
        {
            circleNote[i] = Instantiate(original, new Vector3(MyRythm.data[i].x * MULTISPEED, original.transform.position.y, MyRythm.data[i].z * MULTISPEED), new Quaternion(0, 0, 0, 0), GameObject.Find("Game").transform);
            circleNote[i].name = MyRythm.data[i].way;
        }
        GameObject.Find("Circle Original").GetComponent<Renderer>().enabled = false;

        //노래 생성
        anvilAudio = gameObject.AddComponent<AudioSource>();
        anvilAudio.playOnAwake = false;
        anvilAudio.loop = false;
        anvilAudio.clip = Resources.Load("Song/" + MyRythm.info.title, typeof(AudioClip)) as AudioClip;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CameraEffect.fade && CameraEffect.alph != 0) return;

        if (Scenes.Scenes.present == Scenes.Scene.SmithRythm)
        {
            if (startTime < 3)
            {
                startTime += Time.deltaTime;
                MoveCircle(Time.deltaTime);

                if (startTime >= 3)
                {
                    if (startTime != 3)
                    {
                        MoveCircle(3 - startTime);
                        startTime = 3;
                    }

                    anvilAudio.Play();
                }
            }
            else if (anvilAudio.isPlaying)
            {
                if (!SmithAnimation.isPlaying)
                {
                    hammerEffect.Stop();
                }

                if (circleIndex == circleNote.Length) return;

                Judge temp = JudgeCircle(circleIndex, MULTISPEED / 4);
                if (temp != Judge.Fail)
                {
                    bool bTmp = false;

                    if (Input.GetKey(KeyCode.Space))
                    {
                        bTmp = true;
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        GameObject target = GetClickedObject();
                        if (target != null && target.Equals(gameObject))
                        {
                            bTmp = true;
                        }
                    }

                    if (bTmp)
                    {
                        //bpm을 맞추어야 하기 떄문에 표시를 없앰
                        circleNote[circleIndex++].GetComponent<Renderer>().enabled = false;
                        score += (int)temp * ++combo;
                        
                        SmithAnimation.Play();
                        hammerEffect.Play();
                    }
                }

                if (JudgeCircle(bpmIndex, 0) != Judge.Fail)
                {
                    //중앙에 도달했으면 제거
                    Destroy(circleNote[bpmIndex++]);

                    //현 인덱스가 bpm인덱스보다 작으면 원 노트를 터치 하지 못한 것
                    if (circleIndex < bpmIndex)
                    {
                        circleIndex++;
                        combo = 0;
                        energy -= 100f / MyRythm.info.totalCount;
                    }

                    if (energy < endEnergy)
                    {
                        anvilAudio.Stop();
                    }
                }

                MoveCircle(Time.deltaTime);
            }
            else if (startTime < 6)
            {
                startTime += Time.deltaTime;
            }
            else
            {
                Scenes.Scenes.present = energy >= 80 ? Scenes.Scene.ShowItem : Scenes.Scene.InSmithy;

                if (energy < endEnergy)
                {
                    //죽어서 게임을 나온 경우
                    //GameOver가 나오도록 구현할 것

                }

                CameraEffect.fade = true;
            }
        }

        if (CameraEffect.fade && CameraEffect.alph == 1)
        {
            if (Scenes.Scenes.present == Scenes.Scene.ShowItem)
            {
                SceneManager.LoadScene("ItemScene");
            }
            else if (Scenes.Scenes.present == Scenes.Scene.InSmithy)
            {
                SceneManager.LoadScene("SmithyScene");
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

    void OnGUI()
    {
        if (!CameraEffect.fade && CameraEffect.alph != 0) return;

        if (Scenes.Scenes.present == Scenes.Scene.SmithRythm)
        {
            float textWidth = Screen.width / 7f, textHeight = Screen.height / 8f;

            GUI.Label(new Rect(10, 10, textWidth, textHeight),
                "점수 : " + score + "\n" +
                "콤보 : " + combo + "\n" + 
                "에너지 : " + energy.ToString("N2"),
                Scenes.Scenes.GUIAlign("label", (int)textWidth / 8));

            float BtnWidth = Screen.width / 7f, BtnHeight = Screen.height / 16f;

            if (GUI.Button(new Rect(Screen.width - BtnWidth * 1.05f, BtnHeight / 10, BtnWidth, BtnHeight), "나가기", Scenes.Scenes.GUIAlign("button", (int)BtnWidth / 8)))
            {
                Scenes.Scenes.present = Scenes.Scene.InSmithy;
                CameraEffect.fade = true;
            }
        }
    }

    Judge JudgeCircle(int index, float max) //index 판정할 인덱스, max 허용 범위
    {
        if (index >= circleNote.Length) return Judge.Fail;

        string name = circleNote[index].name;
        Vector3 position = circleNote[index].transform.position;
        if (max / 4 >= GetDistance(name, position))
        {
            return Judge.Perfect;
        }
        else if (max / 2 >= GetDistance(name, position))
        {
            return Judge.Great;
        }
        else if (max >= GetDistance(name, position))
        {
            return Judge.Normal;
        }

        return Judge.Fail;
    }

    float GetDistance(string name, Vector3 position)
    {
        switch (name)
        {
            case "Left": return -position.x;
            case "Right": return position.x;
            case "Down": return -position.z;
            case "Up": return position.z;
            default: return 0;
        }
    }

    void MoveCircle(float time)
    {
        for (int i = bpmIndex; i < circleNote.Length; i++)
        {
            //bpm / 60 * 업데이트 함수 호출시간
            if (bpmIndex != 0) speed = MyRythm.data[bpmIndex - 1].bpm;
            else speed = MyRythm.data[0].bpm;

            speed *= 1f / 60 * time * MULTISPEED;

            //이동
            Vector3 position = circleNote[i].transform.position;
            float x = position.x, y = position.y, z = position.z;
            switch (circleNote[i].name)
            {
                case "Left": position = new Vector3(x + speed, y, z); break;
                case "Right": position = new Vector3(x - speed, y, z); break;
                case "Down": position = new Vector3(x, y, z + speed); break;
                case "Up": position = new Vector3(x, y, z - speed); break;
            }

            circleNote[i].transform.position = position;
        }
    }
}