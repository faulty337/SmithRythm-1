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
    float presentBPM;
    int circleIndex;
    float rank;

    GameObject[] circleNote; // 나오는 원들을 의미
    GameObject perfectCircle, judgeCircle;

    float energy; //진행도
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
        rank = 1.8f * MULTISPEED / 4f; //5가 퍼펙트 기준
        SmithAnimation.Play();
        hammerEffect.Stop();

        //판정원이 그려질 오브젝트 생성
        perfectCircle = new GameObject("PerfectCircle");
        judgeCircle = new GameObject("JudgeCircle");
        perfectCircle.transform.parent = gameObject.transform;
        judgeCircle.transform.parent = gameObject.transform;

        //지정
        Vector3 tempPos = gameObject.transform.position;
        perfectCircle.transform.position = new Vector3(0, tempPos.y + 0.15f * gameObject.transform.localScale.y, 0);
        judgeCircle.transform.position = new Vector3(0, tempPos.y + 0.15f * gameObject.transform.localScale.y, 0);

        LineRenderer tempLine = perfectCircle.AddComponent<LineRenderer>();
        DrawCircle(ref tempLine, MULTISPEED / 4f);
        tempLine = judgeCircle.AddComponent<LineRenderer>();
        DrawCircle(ref tempLine, rank);

        //노트 파일 읽고 노트 생성
        MyRythm.ReadFile("NoteText/" + MyRythm.info.title);
        circleNote = new GameObject[MyRythm.info.totalCount];

        GameObject.Find("Circle Original").GetComponent<Renderer>().enabled = true;
        GameObject original = GameObject.Find("Circle Original");
        presentBPM = MyRythm.data[0].bpm;
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

                presentBPM = MyRythm.data[circleIndex].bpm;

                Judge temp = JudgeCircle(MULTISPEED / 4f - (rank - MULTISPEED / 4f) / 2, MULTISPEED / 4f, rank);

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
                        //중앙과 가까울 때 클릭하면
                        Destroy(circleNote[circleIndex++]);
                        combo++;
                        score += (int)temp * combo;

                        SmithAnimation.Play();
                        hammerEffect.Play();
                    }
                }

                if (JudgeCircle(0, 0, MULTISPEED / 4f - (rank - MULTISPEED / 4f) / 2) != Judge.Fail)
                {
                    //중앙에 도달했으면 제거
                    Destroy(circleNote[circleIndex++]);
                    combo = 0;

                    //원을 클릭하지 못했으므로 에너지 감소
                    energy -= 100f / MyRythm.info.totalCount;

                    if (energy < 80)
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

                if (energy < 80)
                {
                    //죽어서 게임을 나온 경우
                    //GameOver가 나오도록 구현할 것

                }

                CameraEffect.fade = true;

                Destroy(anvilAudio);
                
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

    Judge JudgeCircle(float min, float mark, float max) //min, max 허용 범위, mark 점수대
    {
        if (circleIndex >= circleNote.Length) return Judge.Fail;

        string name = circleNote[circleIndex].name;
        Vector3 position = circleNote[circleIndex].transform.position;
        if ((max + mark) / 4f >= GetDistance(name, position) && GetDistance(name, position) >= (min + mark) / 4f)
        {
            return Judge.Perfect;
        }
        else if ((max + mark) / 2f >= GetDistance(name, position) && GetDistance(name, position) >= (min + mark) / 2f)
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
        for (int i = circleIndex; i < circleNote.Length; i++)
        {
            //bpm / 60 * 업데이트 함수 호출시간
            speed = (presentBPM / 60) * time * MULTISPEED;

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

    //http://www.devkorea.co.kr/bbs/board.php?bo_table=m03_qna&wr_id=52770 - 원 그리기 예제
    void DrawCircle(ref LineRenderer circle, float radius)
    {
        int index = 0;
        Vector3 firstPoint = Vector3.zero;
        float thetaInterval = 0.1f;

        circle.positionCount = ((int)(2 * Mathf.PI / thetaInterval) + 2);// 정점 갯수 설정
        circle.useWorldSpace = false;
        circle.startWidth = 0.1f;
        circle.endWidth = 0.1f;

        for (float theta = 0f; theta < (2 * Mathf.PI); theta += thetaInterval)
        {
            float x = radius * Mathf.Cos(theta); //현재각도에-> 원의좌표로 변환
            float z = radius * Mathf.Sin(theta);

            Vector3 pos = new Vector3(x, 0, z);

            circle.SetPosition(index, pos);

            if (index++ == 0) firstPoint = pos;
        }

        circle.SetPosition(index, firstPoint);
    }
}