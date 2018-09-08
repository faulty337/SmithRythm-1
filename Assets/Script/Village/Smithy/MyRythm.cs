using UnityEngine;
using System.Collections.Generic;

namespace RythmData
{
    struct MusicInfo
    {
        public string title; //노래 제목
        public int totalCount; //박자의 총 갯수
    }

    struct RythmData
    {
        public string way; //원이 나올 방향 - Up, Down, Left, Right
        public float x; //x좌표 - 방향에 따라서 부호가 결정됨
        public float z; //z좌표 - 방향에 따라서 부호가 결정됨
        public float bpm; //Beat Per Minute - 악보에서 bpm이 바뀔 때가 있음
    }

    class MyRythm
    {
        public static MusicInfo info;
        public static List<RythmData> data = new List<RythmData>();

        //안드로이드에서 text파일 load하기
        //http://www.devkorea.co.kr/bbs/board.php?bo_table=m03_qna&wr_id=54914
        public static int ReadFile(string fileName)
        {
            TextAsset noteTxtAsset = Resources.Load(fileName, typeof(TextAsset)) as TextAsset;
            string[] buffer = noteTxtAsset.text.Split('\n');
            int index = 0;

            while (!buffer[index].StartsWith("@Information Field"))
            {
                if (index++ >= buffer.Length) return -2;
            }

            while (!buffer[index].StartsWith("@Data Field"))
            {
                if (buffer[index].StartsWith("#"))
                {
                    if (buffer[index].StartsWith("#PLAYER "))
                    {
                        //플레이 방법
                    }
                    else if (buffer[index].StartsWith("#TITLE "))
                    {
                        //곡 제목
                        info.title = buffer[index].Substring(7, buffer[index].Length - 8);
                    }
                    else if (buffer[index].StartsWith("#ARTIST "))
                    {
                        //곡 제작자 - 대장장이 게임이므로 별로 필요 없을 듯 함
                    }
                    else if (buffer[index].StartsWith("#GENRE "))
                    {
                        //곡 장르 - 곡 제작자와 마찬가지
                    }
                    else if (buffer[index].StartsWith("#PLAYLEVEL "))
                    {
                        //곡 난이도 - 주관적이라 사람들의 의견을 듣고 결정
                    }
                }

                index++;
            }

            int count = 0;
            float pos = 0; //데이터 계산 시 파일 포맷 작성이 너무 어려워지므로 박자만 기록하도록하기 위한 변수
            while (index < buffer.Length)
            {
                if (buffer[index].Length == 0) continue;
                if (buffer[index].StartsWith("#"))
                {
                    //데이터 구성은 way x z bpm순으로 구성 되어 있음
                    string[] dataStr = buffer[index].Split(' '); //따라서 4개의 데이터가 있는 배열

                    RythmData temp = new RythmData();
                    temp.way = dataStr[0].Substring(1); //맨 앞에 # 제거
                    if (!dataStr[1].Equals("0"))
                    {
                        pos += float.Parse(dataStr[1]);
                        temp.x = pos;
                    }
                    else
                    {
                        temp.x = 0;
                    }

                    if (!dataStr[2].Equals("0"))
                    {
                        pos += float.Parse(dataStr[2]);
                        temp.z = pos;
                    }
                    else
                    {
                        temp.z = 0;
                    }

                    temp.bpm = int.Parse(dataStr[3]);

                    switch (temp.way)
                    {
                        case "Left":
                            temp.x = -temp.x;
                            break;
                        case "Down":
                            temp.z = -temp.z;
                            break;

                    }

                    data.Add(temp);

                    count++;
                }

                index++;
            }

            info.totalCount = count;

            return 1;
        }
    }
}
