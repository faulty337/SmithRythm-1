using UnityEngine;
using UnityEngine.SceneManagement;
using RythmData;

//http://itpangpang.xyz/177 - 유니티 마우스 관련 함수
//https://blog.naver.com/live_for_dream/220883895920 - 유니티 화면 분할

public class SmithRythm : MonoBehaviour
{
    AudioSource anvilAudio;

    bool gameStart = false;
    float speed;
    const float MULTISPEED = 20;
    float presentBPM;
    int circleIndex;
    float rank;

    GameObject[] circleTmp; // 나오는 원들을 의미
    GameObject perfectCircle, judgeCircle;

    float energy; //진행도
    int score; //게임 점수
    int combo;

    // Update is called once per frame
    void Update()
    {
        if (Scenes.Scenes.present == Scenes.Scene.SmithRythm)
        {
            if (!gameStart)
            {
                MyRythm.ReadFile(Application.dataPath + "/Resources/" + MyRythm.info.title + ".txt");
                circleTmp = new GameObject[MyRythm.info.totalCount];

                //게임 요소 초기화
                circleIndex = 0;
                energy = 100;
                score = 0;
                combo = 0;
                rank = 1.8f * MULTISPEED / 4f; //5가 퍼펙트 기준
                
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

                //노래에 맞추어 나올 노트(?) 생성
                GameObject.Find("Circle Original").GetComponent<Renderer>().enabled = true;
                GameObject original = GameObject.Find("Circle Original");
                presentBPM = MyRythm.data[0].bpm;
                for (int i = 0; i < MyRythm.info.totalCount; i++)
                {
                    circleTmp[i] = Instantiate(original, new Vector3(MyRythm.data[i].x * MULTISPEED, original.transform.position.y, MyRythm.data[i].z * MULTISPEED), new Quaternion(0, 0, 0, 0), GameObject.Find("Game").transform);
                    circleTmp[i].name = MyRythm.data[i].way;
                }
                GameObject.Find("Circle Original").GetComponent<Renderer>().enabled = false;

                
                PlayMusic(MyRythm.info.title);
                gameStart = true;
            }
            else
            {
                if (anvilAudio.isPlaying)
                {
                    if (circleIndex == circleTmp.Length) return;

                    presentBPM = MyRythm.data[circleIndex].bpm;

                    if (Input.GetKey(KeyCode.Space))
                    {
                        string circleName = circleTmp[circleIndex].name;
                        Vector3 circlePosition = circleTmp[circleIndex].transform.position;
                        if (circleName.Equals("Left") && -rank <= circlePosition.x ||
                            circleName.Equals("Right") && circlePosition.x <= rank ||
                            circleName.Equals("Down") && -rank <= circlePosition.z ||
                            circleName.Equals("Up") && circlePosition.z <= rank)
                        {
                            //중앙과 가까울 때 클릭하면
                            Destroy(circleTmp[circleIndex++]);
                            combo++;
                            score += 10 * combo;
                        }
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        GameObject target = GetClickedObject();
                        if (target != null && target.Equals(gameObject))
                        {
                            string circleName = circleTmp[circleIndex].name;
                            Vector3 circlePosition = circleTmp[circleIndex].transform.position;
                            if (circleName.Equals("Left") && -rank <= circlePosition.x ||
                                circleName.Equals("Right") && circlePosition.x <= rank ||
                                circleName.Equals("Down") && -rank <= circlePosition.z ||
                                circleName.Equals("Up") && circlePosition.z <= rank)
                            {
                                //중앙과 가까울 때 클릭하면
                                Destroy(circleTmp[circleIndex++]);
                                combo++;
                                score += 10 * combo;
                            }
                        }
                    }

                    string name = circleTmp[circleIndex].name;
                    Vector3 position = circleTmp[circleIndex].transform.position;
                    float judge = MULTISPEED / 4f - (rank - MULTISPEED / 4f) / 2;
                    if (name.Equals("Left") && -judge <= position.x ||
                        name.Equals("Right") && position.x <= judge ||
                        name.Equals("Down") && -judge <= position.z ||
                        name.Equals("Up") && position.z <= judge)
                    {
                        //중앙에 도달했으면 제거
                        Destroy(circleTmp[circleIndex++]);
                        combo = 0;

                        //원을 클릭하지 못했으므로 에너지 감소
                        energy -= 100f / MyRythm.info.totalCount;

                        if (energy <= 80)
                        {
                            anvilAudio.Stop();
                        }
                    }

                    MoveCircle(Time.deltaTime);
                }
                else
                {
                    //제작 완료된 무기가 보이는 화면으로 이동하도록 구성할 것
                    if (energy > 80)
                    {
                        //요구 내용 : 아이템 제작 완료 창 및 아이템을 획득 시킬 것
                        //추가해야할 내용 : 경험치 증가 관련
                        Scenes.Scenes.present = Scenes.Scene.ShowItem;
                        Scenes.Scenes.ConvertCamera(GameObject.Find("Item Camera"));
                    }
                    else
                    {
                        Scenes.Scenes.present = Scenes.Scene.InSmithy;
                        Scenes.Scenes.ConvertCamera(GameObject.Find("Smithy Camera"));

                        if (energy != 0)
                        {
                            //죽어서 게임을 나온 경우
                            //GameOver가 나오도록 구현할 것
                        }
                    }

                    Destroy(anvilAudio);
                    for (int i = circleIndex; i < circleTmp.Length; i++) Destroy(circleTmp[i]);
                    Destroy(perfectCircle);
                    Destroy(judgeCircle);
                    gameStart = false;
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

    void OnGUI()
    {
        if (Scenes.Scenes.present == Scenes.Scene.SmithRythm)
        {
            const int TEXTWIDTH = 100, TEXTHEIGHT = 50;
            GUI.Label(new Rect(10, 10, TEXTWIDTH, TEXTHEIGHT),
                "점수 : " + score + "\n" +
                "콤보 : " + combo + "\n" + 
                "에너지 : " + energy.ToString("N2"));

            const int BTNWIDTH = 100, BTNHEIGHT = 20;
            if (GUI.Button(new Rect(Screen.width - BTNWIDTH - 10, 10, BTNWIDTH, BTNHEIGHT), "Exit"))
            {
                energy = 0;
            }
        }
    }

    void PlayMusic(string musicStr)
    {
        anvilAudio = gameObject.AddComponent<AudioSource>();
        anvilAudio.playOnAwake = false;
        anvilAudio.loop = false;
        anvilAudio.clip = Resources.Load(musicStr, typeof(AudioClip)) as AudioClip;
        anvilAudio.Play();
    }

    void MoveCircle(float time)
    {
        for (int i = circleIndex; i < circleTmp.Length; i++)
        {
            speed = (presentBPM / 60) * time * MULTISPEED; //bpm / 60 * 업데이트 함수 호출시간

            //이동
            GameObject gObj = circleTmp[i];
            Vector3 position = gObj.transform.position;
            float x = position.x;
            float y = position.y;
            float z = position.z;
            if (circleTmp[i].name.Equals("Left")) circleTmp[i].transform.position = new Vector3(x + speed, y, z);
            else if (gObj.name.Equals("Right")) circleTmp[i].transform.position = new Vector3(x - speed, y, z);
            else if (gObj.name.Equals("Down")) circleTmp[i].transform.position = new Vector3(x, y, z + speed);
            else if (gObj.name.Equals("Up")) circleTmp[i].transform.position = new Vector3(x, y, z - speed);
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
 