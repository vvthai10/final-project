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
    [Header("Attributes for chase and jumpscare")]
    public Transform player;
    public float jumpScareOffset = 0.5f;
    public Transform cam;
    public Transform jumpscareLookAtPoint;
    public Light jumpscareLight;

    private Animator animator;


    private NavMeshAgent navMeshAgent;
    private OffMeshLinkData offMeshLinkData;
    private bool correctedRotationOnLink = false;


    // animation state hashes
    private int zombieBitingHash = Animator.StringToHash("Zombie Biting");
    private int runningCrawlHash = Animator.StringToHash("Running Crawl");
    private int zombieAttackHash = Animator.StringToHash("Zombie Attack");
    private int zombieScreamHash = Animator.StringToHash("Zombie Scream");
    private int drunkRunForwardHash = Animator.StringToHash("Drunk Run Forward");
    private int injuredIdleHash = Animator.StringToHash("Injured Idle");

    private bool chasing = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
        StartChasing();
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


    public void StartChasing()
    {
        chasing = true;
        navMeshAgent.enabled = true;
        StartChasingAnimation();
    }

    public void StopChasing()
    {
        chasing = false;
        navMeshAgent.enabled = false;
    }

    public void StartJumpscare()
    {
        player.GetComponent<PlayerControl.PlayerController>().Freeze();
        FPSController playerFPSController = player.GetComponent<FPSController>();
        playerFPSController.Freeze();
        this.RotateTowardsCamera();
        this.StartAttackAnimation();
        playerFPSController.StartRotatingTowards(this.jumpscareLookAtPoint);
        jumpscareLight.enabled = true;
    }

    IEnumerator StartChasingAfter(float seconds = 3)
    {
        yield return new WaitForSeconds(seconds);
        StartChasing();
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

    private void Update()
    {
        if (chasing && Vector3.Distance(transform.position, player.position) <= jumpScareOffset)
        {
            this.StopChasing();
            this.StartJumpscare();
        }

        if (chasing)
        {
            navMeshAgent.SetDestination(player.position);
        }

        if (navMeshAgent.isOnOffMeshLink)
        {
            HandleNavmeshLinkTraversal();
        }
    }
}
