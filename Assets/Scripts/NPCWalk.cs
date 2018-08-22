using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCWalk : MonoBehaviour {
    private bool Flag;
    Vector3 startPos;
    float distance;
    Vector3 _currTarget;
    [SerializeField]
    Transform _destination;
    NavMeshAgent _navMeshAgent;
	// Use this for initialization
	void Start () {
        startPos = gameObject.transform.position;
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        SetDestination();
        _currTarget = _destination.transform.position;
        Flag = true;
        distance = Vector3.Distance(this.transform.position, _currTarget);
	}
    private void Update()
    {
        distance = Vector3.Distance(this.transform.position, _currTarget);
        if(distance<5)
        {
            SetDestination();
        }
    }
    private void SetDestination()
    {
        if(_destination !=null)
        {
            if (Flag==false)
            {
                _currTarget = _destination.transform.position;
                _navMeshAgent.SetDestination(_currTarget);
                Flag = true;
            }
            else
            {
                _currTarget = startPos;
                _navMeshAgent.SetDestination(_currTarget);
                Flag = false;
            }
        }
    }
}
