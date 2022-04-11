using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx.Triggers;
using UniRx;
using UnityEngine.EventSystems;

// 마우스 클릭하면 네비게이션에 따라 이동함 
public class PlayerMove : MonoBehaviour
{    
    private Camera m_mainCamera;
    private NavMeshAgent m_navAgent;
    private Animator m_animator;
    public PhotonView PV;

    public void Start()
    {
       
        //m_mainCamera = Camera.main;
        m_mainCamera = GameObject.Find("Main Camera")?.GetComponent<Camera>();

        m_navAgent = GetComponent<NavMeshAgent>();
        PV = GetComponent<PhotonView>();
        m_animator = GetComponent<Animator>();

        this.UpdateAsObservable()
        //this.FixedUpdateAsObservable()                                            // 마우스 클릭 이벤트가 안들어옴
            .Where(_ => Input.GetMouseButtonDown(0))                                // 마우스 왼쪽클릭일때
            .Where(_ => !EventSystem.current.IsPointerOverGameObject())             // ui를 클릭한게 아닐때
            .Where(_ =>  PV.IsMine)             
            .Select(_ => m_mainCamera.ScreenPointToRay(Input.mousePosition))        // 카메라에서 레이정보
            .Subscribe(ray => 
                {
                    if (Physics.Raycast(ray, out RaycastHit raycastHit))
                    {
                        // 이동 목적지
                        m_navAgent.SetDestination(raycastHit.point);

                        //Debug.Log("m_navAgent.velocity: " + raycastHit.point);

                    }
                });

        this.UpdateAsObservable()
            .Where(_ => PV.IsMine && m_animator)
            //.Where(_ => m_navAgent.velocity != Vector3.zero)                    // 네비 메쉬의 속도가 0이 아니면               
            .Select(_ => m_navAgent.velocity)                                   // 네비 메쉬의 속도가 변화시               
            .DistinctUntilChanged()
            .Select(_ => m_navAgent.velocity.magnitude)                         // 네비메쉬 벡터크기를 전달
            .Subscribe(magnitude =>                                             // 벡터 크기를 애니메이터 값에 넣음
            {
                m_animator?.SetFloat("MoveSpeed", magnitude);

                //Debug.Log("m_navAgent.velocity: " + magnitude);
            });


    }

}


