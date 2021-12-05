using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour {

    [SerializeField] Transform[] Targets;
    NavMeshAgent agent;

    [SerializeField] float minWaitTime;
    [SerializeField] float maxWaitTime;

    [SerializeField] float remainingDistance = 1;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        setRandomTarget();
        StartCoroutine("WaklAround");
    }


    IEnumerator WaklAround() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));
            setRandomTarget();
            while(agent.remainingDistance < remainingDistance) {
                yield return new WaitForSeconds(.5f);
            }
        }
    }

    void setRandomTarget() {
        agent.SetDestination(Targets[Random.Range(0, Targets.Length)].position);
    }
}