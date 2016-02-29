using UnityEngine;
using System.Collections;

enum state{
    HALL,
    ROOM1,
    ROOM1TOHALL,
    ROOM2,
    ROOM2TOHALL
};

public class HallManager : MonoBehaviour {

    /*
     #3 복도1의 모습이 나타난다
    #4 복도1중 교실a의 문을 클릭한다
    #5 교실a의 모습 나타난다
     */

    private bool isPrint = false;
    public int testValue = 0;
    private int currentState = 0;
    private int objectCount = 4;

    public float waitForGhost = 2.0f;

	// Use this for initialization
	void Start () {
        //print(Application.loadedLevel.ToString());
        testValue = PlayerPrefs.GetInt("paperNumber");
        currentState = PlayerPrefs.GetInt("state");
        //PlayerPrefs.SetInt("state", 0);

        //초기셋팅 :: 모든 문충돌체 비활성화
        for (int i = (int)state.ROOM1; i <= (int)state.ROOM2TOHALL; i++)
            cantClick("door_0" + i);
	}
	
	// Update is called once per frame
	void Update () {
        //씬체인지
        changeRoom();
        //씬 상태
        sceneState();
	}

    //씬 상태
    void sceneState()
    {
        print(PlayerPrefs.GetInt("state"));
        
        switch (currentState)
        {
            //복도->교실1
            case (int)state.HALL:
                room1();
                break;
            //교실1 -> 복도
            case (int)state.ROOM1TOHALL:
                room1ToHall();
                break;
           //복도->교직원실
            case (int)state.ROOM2:
                room2();
                break;
            case (int)state.ROOM2TOHALL:
                break;
        }
    }

    void room1()
    {
        print("room1()");
        //교실1의 문충돌체 활성화
        canClick("door_0" + (int)state.ROOM1);
    }

    void room1ToHall()
    {
        print("room1ToHall()");
        //1.교실1의 문 충돌체를 없애줌
        deleteCollider("door_left_01");
        //2.애니메이션 시작
        StartCoroutine("runningGhost");
        //3.애니가 끝나면 씬상태의 변경
        currentState = PlayerPrefs.GetInt("state");
    }

    void room2()
    {
        print("room2()");
        //교직원실 문충돌체 활성화
        canClick("door_0" + (int)state.ROOM2);
    }

    void room2ToHall()
    {
        currentState = PlayerPrefs.GetInt("state");
    }

    //체인지 씬
    void changeRoom()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();

        //마우스 클릭 :: 나중에 모바일 버전으로 바꿀것
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                switch (hit.collider.name)
                {
                    case "door_01":
                        print("좌측_01");
                        //Room01로 씬전환
                        StartCoroutine("ChangeLevel_forFirst");
                        break;
                    case "door_02":
                        print("좌측_02");
                        break;
                    case "door_03":
                        print("우측_02");
                        StartCoroutine("ChangeLevel");
                        break;
                    case "door_04":
                        print("우측_01");
                        break;
                }
            }
        }
    }

    //콜리더 삭제
    void deleteCollider(string objectName)
    {
        Destroy(GameObject.Find(objectName));
    }

    //콜리더 클릭불가
    void cantClick(string objectName)
    {
        GameObject.Find(objectName).GetComponent<MeshCollider>().enabled = false;
    }

    //콜리더 클릭가능
    void canClick(string objectName)
    {
        GameObject.Find(objectName).GetComponent<MeshCollider>().enabled = true;
    }

    IEnumerator ChangeLevel_forFirst()
    {
        float fadeTime = GameObject.Find("Background").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(2.5f);
        Application.LoadLevel(Application.loadedLevel + (currentState+1));
    }

    IEnumerator ChangeLevel()
    {
        float fadeTime = GameObject.Find("Background").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(2.5f);
        Application.LoadLevel(Application.loadedLevel + (currentState-1));
    }

    //S_Room01
    IEnumerator runningGhost()
    {
        //public
        yield return new WaitForSeconds(waitForGhost);
        //귀신 달려나가는 애니메숑
        GameObject.Find("runningGhost").GetComponent<Animator>().SetBool("showGhost", true);
    }
}
