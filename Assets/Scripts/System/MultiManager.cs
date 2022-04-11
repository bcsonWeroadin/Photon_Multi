using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

/// MonoBehaviourPunCallbacks 사용
public class MultiManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerOrigin;

    [SerializeField] private CameraManager MainCamera;
    [SerializeField] private GameObject StartCanvas;
    [SerializeField] private ChatManager chatManager;

    private string gameVersion = "1"; // 게임 버전

    //public Text connectionInfoText; // 네트워크 정보를 표시할 텍스트
    public TextMeshProUGUI connectionInfoText; // 네트워크 정보를 표시할 텍스트

    public Button joinButton; // 룸 접속 버튼

    private GameObject PlayerInstance;

    // 게임 실행과 동시에 마스터 서버 접속 시도
    void Start()
    {
        DontDestroyOnLoad(this);

        if (StartCanvas)
        {
            DontDestroyOnLoad(StartCanvas);
        }


        joinButton.onClick.AddListener(Connect);

        //접속에 필요한 정보 (게임 버전) 설정
        PhotonNetwork.GameVersion = this.gameVersion;
        //설정한 정보로 마스터 서버 접속 시도
        PhotonNetwork.ConnectUsingSettings();


        this.joinButton.interactable = false;
        this.connectionInfoText.text = "마스터 서버에 접속중...";
    }

    // 마스터 서버 접속 성공시 자동 실행
    public override void OnConnectedToMaster()
    {
        this.joinButton.interactable = true;
        this.connectionInfoText.text = "온라인 : 마스터 서버와 연결 됨";
    }

    // 마스터 서버 접속 실패시 자동 실행
    public override void OnDisconnected(DisconnectCause cause)
    {
        this.joinButton.interactable = false;
        this.connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음\n 접속 재시도중... ";
        //설정한 정보로 마스터 서버 접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    // 룸 접속 시도
    public void Connect()
    {
        // 중복 접속 막기
        this.joinButton.interactable = false;
        this.joinButton.gameObject.SetActive(false);

        // 마스터 서버에 접속 중이라면
        if (PhotonNetwork.IsConnected)
        {

            //룸에 접속한다.
            this.connectionInfoText.text = "룸에 접속....";
            PhotonNetwork.JoinOrCreateRoom("Main", new RoomOptions { MaxPlayers  = 0}, TypedLobby.Default);
        }
        else
        {
            this.connectionInfoText.text = "오프라인 : 마스터 서버와 연결 끊킴 \n 다시 접속 시도합니다.";
            //설정한 정보로 마스터 서버 접속 시도
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // (빈 방이 없어)랜덤 룸 참가에 실패한 경우 자동 실행
/*    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        this.connectionInfoText.text = "빈 방 없음, 새로운방 생성...";
        //최대 인원을 4명으로 설정 + 방을 만듦
        //방이름 , 4명 설정
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });

    }
*/

    // 룸에 참가 완료된 경우 자동 실행
    public override void OnJoinedRoom()
    {
        this.connectionInfoText.text = "방 참가 성공!";

        // 채팅 접속시도
        chatManager.Connect();

        //모든 룸 참가자가 Main 씬을 로드하게 함
        //PhotonNetwork.LoadLevel("Main");

        // 호스트가 아니면 스폰
        /*        if(!PhotonNetwork.IsMasterClient)
                {
                    // Resources 폴더에 프리팹이 있어야 한다
                    //PlayerInstance = PhotonNetwork.Instantiate(this.playerPrefab.name, Vector3.zero, Quaternion.identity);
                    PlayerInstance = PhotonNetwork.Instantiate(this.playerPrefab.name, this.transform.position, this.transform.localRotation);

                    MainCamera.init(ref PlayerInstance);

                    //PhotonNetwork.Instantiate(this.playerPrefab.name, Vector3.zero, Quaternion.identity);
                }*/

        // 새로 생성후 원래것 삭제
        //PlayerInstance = PhotonNetwork.Instantiate(this.playerPrefab.name, playerOrigin.transform.position, playerOrigin.transform.localRotation);
        PlayerInstance = PhotonNetwork.Instantiate("Player 1", playerOrigin.transform.position, playerOrigin.transform.localRotation);
        Destroy(playerOrigin);
        MainCamera.ReTargeting(ref PlayerInstance);

        //Debug.Log(PhotonNetwork.NickName);

        //Debug.Log(Path.Combine(@"..\Prefabs", this.playerPrefab.name));

        //PhotonNetwork.Instantiate(Path.Combine(@"..\Prefabs", this.playerPrefab.name), Vector3.zero, Quaternion.identity);
        //PhotonNetwork.Instantiate(this.playerPrefab.name, Vector3.zero, Quaternion.identity);

    }
}
