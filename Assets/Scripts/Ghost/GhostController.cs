using PlayerControl;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class GhostController : MonoBehaviour
{
    public enum State
    {
        Chasing,
        Wandering,
        Idle,
        None
    }

    public static GhostController instance;
    [Header("Attributes for chase and jumpscare")]
    public Transform player;
    public float jumpScareOffset = 0.5f;
    public Transform cam;
    public Light jumpscareLight;

    [Header("NavMeshAgent")]
    public float chaseSpeed = 3f;
    public float wanderSpeed = 1.5f;

    [Header("Wandering Configs")]
    public Transform[] wanderPositions;
    public List<int> visitedWanderPositions;

    private Animator animator;

    private GhostVisionField visionField;

    private NavMeshAgent navMeshAgent;
    private OffMeshLinkData offMeshLinkData;
    private bool correctedRotationOnLink = false;

    private bool stopUpdate = false;

    // animation state hashes
    private int zombieBitingHash = Animator.StringToHash("Zombie Biting");
    private int runningCrawlHash = Animator.StringToHash("Running Crawl");
    private int zombieAttackHash = Animator.StringToHash("Zombie Attack");
    private int zombieScreamHash = Animator.StringToHash("Zombie Scream");
    private int drunkRunForwardHash = Animator.StringToHash("Drunk Run Forward");
    private int injuredIdleHash = Animator.StringToHash("Injured Idle");
    private int drunkWalkHash = Animator.StringToHash("Drunk Walk");

    private bool idling = false;
    private bool chasing = false;
    private bool wandering = false;


    private float idlingTimer = 0;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
        visionField = GetComponent<GhostVisionField>();
        gameObject.SetActive(false);
        //  StartChasing();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void RotateTowardsCamera()
    {
        Vector3 orgDirection = (cam.position - transform.position).normalized;
        orgDirection.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(orgDirection);
        transform.rotation = lookRotation;
    }

    public void StartChaseSequence()
    {
        //this.StartBitingAnimation();
        StartCoroutine(StartChasingAfter(3));
    }

    public void StartBitingAnimation()
    {
        animator.Play(zombieBitingHash);
    }

    public void StartChasingAnimation()
    {
        animator.Play(drunkRunForwardHash);
    }

    public void StartAttackAnimation()
    {
        animator.Play(zombieAttackHash);
    }

    private void StartJumpscareAnimation()
    {
        animator.Play(zombieScreamHash);
    }

    public void StartChasing()
    {
        chasing = true;
        navMeshAgent.enabled = true;
        navMeshAgent.speed = chaseSpeed;
        StartChasingAnimation();
    }

    public void StopChasing()
    {
        chasing = false;
        //navMeshAgent.enabled = false;
    }

    public void StartWandering()
    {
        visitedWanderPositions = new List<int>();
        wandering = true;
        navMeshAgent.enabled = true;
        navMeshAgent.speed = wanderSpeed;
        animator.Play(drunkWalkHash);
    }

    public void StopWandering()
    {
        wandering = false;
    }

    private int? GetNearestUnvisitedWanderPosition()
    {
        float shortestDistance = Mathf.Infinity;
        int? foundIdx = null;

        int n = wanderPositions.Length;

        for (int i = 0; i < n; i++)
        {
            if (!visitedWanderPositions.Contains(i))
            {
                float distanceToTarget = Vector3.Distance(transform.position, wanderPositions[i].position);
                if (distanceToTarget < shortestDistance)
                {
                    shortestDistance = distanceToTarget;
                    foundIdx = i; 
                }
            }
        }

        return foundIdx;
    }

    public void StartIdling()
    {
        idlingTimer = 0;
        idling = true;
        animator.Play(injuredIdleHash);
    }

    public void StopIdling()
    {
        idling = false;
    }

    public void StartJumpscare()
    {
        //SwitchState(State.None);
        player.GetComponent<PlayerControl.PlayerController>().Freeze();
        FPSController playerFPSController = player.GetComponent<FPSController>();
        playerFPSController.Freeze();
        player.gameObject.SetActive(false);
        navMeshAgent.enabled = false;
        
        CamerasController.instance.SwitchTo("JumpscareCamera");
        StartJumpscareAnimation();
        //jumpscareLight.enabled = true;
    }

    public void EnableJumpscareLight()
    {
        jumpscareLight.enabled = true;
    }

    public void DisableJumpscareLight()
    {
        jumpscareLight.enabled = false;
    }

    IEnumerator StartChasingAfter(float seconds = 3)
    {
        yield return new WaitForSeconds(seconds);
        StartChasing();
    }

    public void SwitchState(GhostController.State state)
    {
        switch (state)
        {
            case State.Chasing:
                { StartChasing(); StopIdling(); StopWandering(); break; }
            case State.Wandering:
                { StartWandering(); StopChasing(); StopIdling(); break; }
            case State.Idle:
                {
                    StartIdling(); StopChasing(); StopWandering(); break;
                }
            case State.None:
                {
                    StopIdling(); StopChasing(); StopWandering(); break;
                }
            default: { break; }
        }
    }

    public void HandleNavmeshLinkTraversal()
    {

        offMeshLinkData = navMeshAgent.currentOffMeshLinkData;
        Vector3 endpos = offMeshLinkData.endPos + Vector3.up * navMeshAgent.baseOffset;
        navMeshAgent.transform.position = Vector3.MoveTowards(navMeshAgent.transform.position, endpos, navMeshAgent.speed * Time.deltaTime);


        if (!correctedRotationOnLink)
        {
            Vector3 direction = offMeshLinkData.endPos - offMeshLinkData.startPos;
            //Debug.Log($"direction: {direction}");
            // align with path's rotation towards
            transform.rotation = Quaternion.LookRotation(direction);
            
            // flip character on ceiling links
            if (IsOnCeilingNavMeshLink())
            {
                //Debug.Log("OnCeilingNavMeshLink");
                transform.Rotate(direction, 180, Space.World);
            }

            correctedRotationOnLink = true;
        }
        
        if (navMeshAgent.transform.position == endpos)
        {
            //Debug.Log("ending navlink");
            correctedRotationOnLink = false;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            navMeshAgent.CompleteOffMeshLink();
            StartChasingAnimation();
        }
    }

    private bool IsOnCeilingNavMeshLink()
    {
        try
        {
            return navMeshAgent.navMeshOwner.ToString().IndexOf("WallCeilingNavMeshLink", StringComparison.OrdinalIgnoreCase) > -1;
        } catch
        {
            return false;
        }
    }

    public void SendMainToStart()
    {
        player.gameObject.SetActive(true);
        PlayerController.instance.gameObject.transform.position = new Vector3(12.14f, -0.94f, -8.4f);
        player.GetComponent<PlayerControl.PlayerController>().Unfreeze();
        FPSController playerFPSController = player.GetComponent<FPSController>();
        playerFPSController.Unfreeze();
        CamerasController.instance.SwitchTo("MainCamera");
        this.stopUpdate = false;
        this.SwitchState(State.Idle);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, jumpScareOffset);
    }

    private void Update()
    {
        if (!stopUpdate)
        {
            if (!chasing && visionField.CanSeePlayer())
            {
                SwitchState(State.Chasing);
                return;
            }

            if (chasing)
            {
                if (Vector3.Distance(transform.position, player.position) <= jumpScareOffset)
                {
                    this.StopChasing();
                    stopUpdate = true;
                    this.StartJumpscare();
                    return;
                }

                if (visionField.CanSeePlayer())
                {
                    navMeshAgent.SetDestination(visionField.LastSeenPosition());
                }

                else
                {
                    if (navMeshAgent.remainingDistance == 0)
                    {
                        SwitchState(State.Idle);
                    }
                }

                if (navMeshAgent.isOnOffMeshLink)
                {
                    HandleNavmeshLinkTraversal();
                }
                return;
            }

            if (wandering)
            {
                int nVisited = visitedWanderPositions.Count;
                Debug.Log(nVisited);
                if (nVisited == 0)
                {
                    int? next = GetNearestUnvisitedWanderPosition();
                    if (next != null)
                    {
                        visitedWanderPositions.Add((int)next);
                        navMeshAgent.SetDestination(wanderPositions[(int)next].position);
                    }
                    else
                        SwitchState(State.Idle);
                }
                else if (nVisited == wanderPositions.Length)
                {
                    SwitchState(State.Idle);
                }
                else
                {
                    if (navMeshAgent.remainingDistance == 0)
                    {
                        int? next = GetNearestUnvisitedWanderPosition();
                        if (next != null)
                        {
                            visitedWanderPositions.Add((int)next);
                            navMeshAgent.SetDestination(wanderPositions[(int)next].position);
                        }
                        else
                            SwitchState(State.Idle);
                    }
                }

            }

            if (idling)
            {
                idlingTimer += Time.deltaTime;
                if (idlingTimer > 3)
                    SwitchState(State.Wandering);
            }
        }

        
    }
}
