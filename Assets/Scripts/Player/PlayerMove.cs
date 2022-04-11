using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx.Triggers;
using UniRx;
using UnityEngine.EventSystems;

// ���콺 Ŭ���ϸ� �׺���̼ǿ� ���� �̵��� 
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
        //this.FixedUpdateAsObservable()                                            // ���콺 Ŭ�� �̺�Ʈ�� �ȵ���
            .Where(_ => Input.GetMouseButtonDown(0))                                // ���콺 ����Ŭ���϶�
            .Where(_ => !EventSystem.current.IsPointerOverGameObject())             // ui�� Ŭ���Ѱ� �ƴҶ�
            .Where(_ =>  PV.IsMine)             
            .Select(_ => m_mainCamera.ScreenPointToRay(Input.mousePosition))        // ī�޶󿡼� ��������
            .Subscribe(ray => 
                {
                    if (Physics.Raycast(ray, out RaycastHit raycastHit))
                    {
                        // �̵� ������
                        m_navAgent.SetDestination(raycastHit.point);

                        //Debug.Log("m_navAgent.velocity: " + raycastHit.point);

                    }
                });

        this.UpdateAsObservable()
            .Where(_ => PV.IsMine && m_animator)
            //.Where(_ => m_navAgent.velocity != Vector3.zero)                    // �׺� �޽��� �ӵ��� 0�� �ƴϸ�               
            .Select(_ => m_navAgent.velocity)                                   // �׺� �޽��� �ӵ��� ��ȭ��               
            .DistinctUntilChanged()
            .Select(_ => m_navAgent.velocity.magnitude)                         // �׺�޽� ����ũ�⸦ ����
            .Subscribe(magnitude =>                                             // ���� ũ�⸦ �ִϸ����� ���� ����
            {
                m_animator?.SetFloat("MoveSpeed", magnitude);

                //Debug.Log("m_navAgent.velocity: " + magnitude);
            });


    }

}


