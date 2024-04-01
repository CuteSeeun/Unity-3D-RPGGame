using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SingletonLazy<T> : MonoBehaviour where T : class
{
    private static readonly Lazy<T> _instance =
        new Lazy<T>(() =>
        {
            T instance = FindObjectOfType(typeof(T)) as T;

            if (instance == null)
            {
                GameObject obj = new GameObject("SingletonLazy");
                instance = obj.AddComponent(typeof(T)) as T;

                DontDestroyOnLoad(obj);
            }
            else
            {
                Destroy(instance as GameObject);
            }

            return instance;
        });

    public static T Instance
    {
        get
        {
            return _instance.Value;
        }
    }
}

public class SFX_Manager : SingletonLazy<SFX_Manager>
{
    //사운드 매니저 싱글톤 선언
    
    //카메라 변수 선언
    public CameraMove Camera;
    //오디오 소스 변수 선언
    public AudioSource BGM;


    
    //유니티에서 접근 가능한 프라이빗 오디오클립 리스트 컴포넌넌트 SFX변수 선언
    [SerializeField]
    private List<AudioClip> _audioClips_sfx;
    //유니티에서 접근 가능한 프라이빗 오디오클립 리스트 컴포넌넌트 BGM 변수 선언
    [SerializeField]
    private List<AudioClip> _audioBGM;

    //브금 오디오소스 변수가 널일때 카메라에 있는 오디오소스를 열고 브금플레이( 브금 확정 시 변경 )함수를 실행
    public void Start()
    {
        if (BGM == null)
        {
            Camera.GetComponent<AudioSource>();
        }
        BGMPLAY("bgm1");


    }

    //브금플레이 함수는 브금 오디오소스가 플레이중일때 멈추면
    public void BGMPLAY(string bgmname)
    {
        if (BGM.isPlaying) BGM.Stop();

        //오디오클립의 브금클립이 널이고
        AudioClip bgmclip = null;

        //i가 0이고, i 가 오디오부금변수보다 작을때 i 는 1씩 커지고 그동안
        for (int i = 0; i < _audioBGM.Count; ++i)
        {

            //i에 해당하는 변수이름이 브금네임이랑 같을 때
            if (_audioBGM[i].name == bgmname)
            {
                //브금클립 변수는 브금네임과 같게 하고 for루틴을 끝낸다.
                bgmclip = _audioBGM[i];
                break;

                //그렇게 되면 브금플레이 함수는 해당이름이 매칭된 함수다.
            }
        }

        //브금클립을 할당하고
        BGM.clip = bgmclip;
        //브금을 플레이한다.
        BGM.Play();
    }
    public void VFX(string soundname)
    {

        // clip 선언
        AudioClip clip = null;

        //i 가 0이고 i가 _audioClips_sfx.Count보다 작다, i는 1씩늘어남<<이 조건동안 for를 반복한다
        for (int i = 0; i < _audioClips_sfx.Count; ++i)
        {
            //브금버전과 마찬가지로 SFX도 매칭한다.
            if (_audioClips_sfx[i].name == soundname)
            {
                clip = _audioClips_sfx[i];
                break;
            }
        }

        // audiosource 선언, destroysfx 선언
        AudioSource audiosource = null;
        DestroySFX destroysfx = null;

        //go 가 soundname 쓰는 게임오브젝트임
        GameObject go = new GameObject(soundname);

        // 위에서 선언한 audiosource에 go 게임오브젝트(오디오소스 인스펙터창을 추가한)넣음
        audiosource = go.AddComponent<AudioSource>();
        //위에서 선언한 destrosfx에 go 게임오브젝트(DestroySFX 다른 스크립트 인스펙터창)를 넣음
        destroysfx = go.AddComponent<DestroySFX>();

        //destroysfx에 있는 _audioSource는 audiosource이다
        destroysfx._audioSource = audiosource;
        //audiosource에 클립은 사용할 clip를 받아온다
        audiosource.clip = clip;

        //go의 생성위치(카메라 애착)
        go.transform.parent = Camera.transform;
        go.transform.position = Vector3.zero;

        //audiosource.volume = 0.5f;  
        //audiosource 실행
        audiosource.Play();
    }

    //BGM.volume 을 volume변수로 할당한 함수 SetVolume 생성
    public void SetVolume(float volume)
    {
        BGM.volume = volume;
    }

    private void OnGUI()
    {
        //if 안에 있는 조건 변경 필요 => 플레이어가 마을에서 상호작용하고 대화든 강화나 제작이든 할 때                                                    
        if (GUI.Button(new Rect(200, 0, 100, 100), "bgmdown"))
        {
            //코루틴을 시작한다(BGM.Volume 을 volume변수로 할당한 셋볼륨 함수의 값을 1에서부터0.005로 0.5속도로)
            StartCoroutine(SetVolume(0.005f, 0.2f));
        }
        if (GUI.Button(new Rect(300, 0, 100, 100), "bgmup"))
        {
            //코루틴을 시작한다(BGM.Volume 을 volume변수로 할당한 셋볼륨 함수의 값을 0에서부터1로 0.5속도로)
            StartCoroutine(SetVolume(1f, 0.2f));
        }
        //상호작용 끝나고 나와서도 다시한번 코루틴 시작
    }

    //코루틴 내용 설명 : 볼륨크기 변수 와 볼륨 커지는 속도 변수를 가진 셋 볼륨은
    public IEnumerator SetVolume(float volume, float speed)
    {
        //템프변수가 BGM.volume에서 volume를 뺀 값이고
        float temp = BGM.volume - volume;

        //템프가 영보다 작을 때 셋 볼륨 브금업(볼륨크기와 커지는속도변수가 할당된)을 실행한다.
        if (temp < 0)
        {
            yield return SetVolume_BGMUP(volume, speed);
        }

        //그게 아닌 경우 볼륨 브금다운을 실행한다.
        else
        {
            yield return SetVolume_BGMDOWN(volume, speed);
        }

        //그러고 한 프레임 쉰다
        yield return null;
    }


    //볼륨 브금업은 볼륨크기변수와 커지는 속도 변수를 가지고 있으며
    private IEnumerator SetVolume_BGMUP(float volume, float speed)
    {
        //max 변수는 1이고
        float max = 1f;

        //버퍼변수는 브금 볼륨값과 같다
        float buffer = BGM.volume;

        //버퍼가 볼륨보다 작으면 
        while (buffer < volume)
        {
            //버퍼를 (타임델타타임*커지는속도로 조절) 하는만큼 값을 키운다
            buffer += (Time.deltaTime * speed);

            //버퍼가 맥스인 1보다 커지면 버퍼는 맥스로 고정하고 1이된다.
            if (buffer > max)
            {
                buffer = max;
            }

            BGM.volume = buffer;


            yield return null;
        }

        yield return null;
    }


    //볼륨 업의 마이너스버전
    private IEnumerator SetVolume_BGMDOWN(float volume, float speed)
    {
        float min = 0f;

        float buffer = BGM.volume;
        while (buffer > volume)
        {
            buffer -= (Time.deltaTime * speed);

            if (buffer < min)
            {
                buffer = min;
            }
            BGM.volume = buffer;


            yield return null;
        }

        yield return null;
    }
}
