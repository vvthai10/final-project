using PlayerControl;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class GhostController : MonoBehaviour
{
    [Header("Attributes for chase and jumpscare")]
    public Transform player;
    public float jumpScareOffset = 0.5f;
    //public Transform cam;
    public Transform jumpscareLookAtPoint;
    public Light jumpscareLight;

    [Space]
    [Header("Footstep")]
    public AudioSource leftFootAudioSource;
    public AudioSource rightFootAudioSource;
    public AudioClip leftFootstep;
    public AudioClip rightFootstep;

    [Space]
    [Header("Jumpscare")]
    public AudioSource jumpscareAudioSource;
    public AudioClip jumpscareAudioClip;

    [Space]


    private Animator animator;


    private enum LinkTraversalStatus { Start, Mid, End, Idle };
    private enum LinkType { Normal, Door, FloorWall, WallCeiling };
    private NavMeshAgent navMeshAgent;
    private OffMeshLinkData offMeshLinkData;
    private LinkTraversalStatus linkTraversalStatus;
    private LinkType linkType;
    private bool correctedRotationOnLink = false;

    // door navmesh links
    private struct DoorNavMeshLinkData
    {
        LinkTraversalStatus status;
        OffMeshLinkData linkData;
    }

    private DoorNavMeshLinkData doorNavMeshLinkData;

    // animation state hashes
    private int zombieBitingHash = Animator.StringToHash("Zombie Biting");
    private int runningCrawlHash = Animator.StringToHash("Running Crawl");
    private int zombieAttackHash = Animator.StringToHash("Zombie Attack");
    private int zombieScreamHash = Animator.StringToHash("Zombie Scream");
    private int drunkRunForwardHash = Animator.StringToHash("Drunk Run Forward");
    private int injuredIdleHash = Animator.StringToHash("Injured Idle");
    private int zombieKickingHash = Animator.StringToHash("Zombie Kicking");
    private int openingHash = Animator.StringToHash("Opening");

    private bool chasing = false;

    private void Awake()
    {
        doorNavMeshLinkData = new DoorNavMeshLinkData();
        linkTraversalStatus = LinkTraversalStatus.End;
    }
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


    private void CorrectParentHorizontalRotation(Vector3 direction)
    {
        //ResetLocalTransform();
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        navMeshAgent.transform.rotation = lookRotation;
        //transform.rotation = lookRotation;
    }

    //private void CorrectHorizontalRotation(Vector3 direction)
    //{
    //    direction.y = 0;
    //    Quaternion lookRotation = Quaternion.LookRotation(direction);
    //    transform.rotation = lookRotation;
    //}

    private void ResetLocalTransform()
    {
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    public void RotateTowardsPlayer()
    {
        CorrectParentHorizontalRotation((player.position - transform.position).normalized);
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

    private void StartJumpScareAnimation()
    {
        animator.Play(zombieScreamHash);
    }

    private void StartDoorKickAnimation()
    {
        animator.Play(zombieKickingHash);
    }

    private void StartDoorOpenAnimation()
    {
        animator.Play(openingHash);
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


    /*
     * JUMPSCARE
     */
    public void StartJumpscare()
    {
        player.GetComponent<PlayerControl.PlayerController>().Freeze();
        player.GetComponent<FPSController>().Freeze();
        player.gameObject.SetActive(false);
        this.RotateTowardsPlayer();
        CamerasManager.SwitchTo("JumpscareCamera");
        StartJumpScareAnimation();
    }

    private void EnableJumpscareLight()
    {
        jumpscareLight.enabled = true;
    }

    private void PlayJumpscareSound()
    {
        jumpscareAudioSource.PlayOneShot(jumpscareAudioClip);
    }

    IEnumerator StartChasingAfter(float seconds = 3)
    {
        yield return new WaitForSeconds(seconds);
        StartChasing();
    }

    public void HandleNavmeshLinkTraversal()
    {
        // previously not on any links
        if (linkTraversalStatus == LinkTraversalStatus.End)
        {
            offMeshLinkData = navMeshAgent.currentOffMeshLinkData;
            linkTraversalStatus = LinkTraversalStatus.Start;
            linkType = IsOnDoorNavMeshLink() ? LinkType.Door : LinkType.Normal;
            navMeshAgent.transform.position = offMeshLinkData.startPos;
            ResetLocalTransform();

            navMeshAgent.updateRotation = false;
            CorrectParentHorizontalRotation((offMeshLinkData.endPos - offMeshLinkData.startPos).normalized);
        }
        // moving on link
        else if (linkTraversalStatus == LinkTraversalStatus.Mid)
        {
            Vector3 endpos = offMeshLinkData.endPos + Vector3.up * navMeshAgent.baseOffset;
            navMeshAgent.transform.position = Vector3.MoveTowards(navMeshAgent.transform.position, endpos, navMeshAgent.speed * Time.deltaTime);
            if (navMeshAgent.transform.position == endpos)
            {
                navMeshAgent.CompleteOffMeshLink();
                navMeshAgent.updateRotation = true;
                // reset child object
                transform.localPosition = new Vector3(0, 0, 0);
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                linkTraversalStatus = LinkTraversalStatus.End;
            }
        }
        // starting on link
        else if (linkTraversalStatus == LinkTraversalStatus.Start)
        {
            switch (linkType)
            {
                case LinkType.Door:
                    {
                        DecideDoorOpenAnimation();
                        linkTraversalStatus = LinkTraversalStatus.Idle;
                        break;
                    }
                default:
                    {
                        linkTraversalStatus = LinkTraversalStatus.Mid;
                        break;
                    }
            }
        }
        else
        {
            // LinkTraversalStatus.Idle
        }

        //offMeshLinkData = navMeshAgent.currentOffMeshLinkData;
        //Vector3 endpos = offMeshLinkData.endPos + Vector3.up * navMeshAgent.baseOffset;
        //navMeshAgent.transform.position = Vector3.MoveTowards(navMeshAgent.transform.position, endpos, navMeshAgent.speed * Time.deltaTime);


        //if (!correctedRotationOnLink)
        //{
        //    Vector3 direction = offMeshLinkData.endPos - offMeshLinkData.startPos;
        //    //Debug.Log($"direction: {direction}");
        //    // align with path's rotation towards
        //    transform.rotation = Quaternion.LookRotation(direction);

        //    // flip character on ceiling links
        //    if (IsOnCeilingNavMeshLink())
        //    {
        //        //Debug.Log("OnCeilingNavMeshLink");
        //        transform.Rotate(direction, 180, Space.World);
        //    }

        //    correctedRotationOnLink = true;
        //}

        //if (navMeshAgent.transform.position == endpos)
        //{
        //    //Debug.Log("ending navlink");
        //    correctedRotationOnLink = false;
        //    transform.localRotation = Quaternion.Euler(0, 0, 0);
        //    navMeshAgent.CompleteOffMeshLink();
        //    StartChasingAnimation();
        //}
    }

    private void DecideDoorOpenAnimation()
    {
        MobsDoorController doorController = navMeshAgent.navMeshOwner.GetComponent<MobsDoorController>();
        bool isOutside = Vector3.Distance(transform.position, doorController.outsidePoint.position) < Vector3.Distance(transform.position, doorController.insidePoint.position);
        if (isOutside)
        {
            StartDoorKickAnimation();
        }
        else
        {
            StartDoorOpenAnimation();
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

    private bool IsOnDoorNavMeshLink()
    {
        try
        {
            return navMeshAgent.navMeshOwner.ToString().IndexOf("DoorNavMeshLink", StringComparison.OrdinalIgnoreCase) > -1;
        } catch { return false; }
    }

    private void OnKickLanded()
    {
        navMeshAgent.navMeshOwner.GetComponent<MobsDoorController>().onDoorKicked?.Invoke();
    }

    private void OnKickFinished()
    {
        animator.Play(drunkRunForwardHash);
        StartMidTraversalPhase();
    }

    private void OnDoorGrabbed()
    {
        navMeshAgent.navMeshOwner.GetComponent<MobsDoorController>().onDoorOpened?.Invoke();
    }

    private void OnDoorOpenAnimationFinished()
    {
        animator.Play(drunkRunForwardHash);
        StartMidTraversalPhase();
    }

    private void StartMidTraversalPhase()
    {
        linkTraversalStatus = LinkTraversalStatus.Mid;
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

    /*
     * FOOTSTEP CALLBACKS
     */

    private void OnLeftFootLand()
    {
        leftFootAudioSource.PlayOneShot(leftFootstep);
    }

    private void OnRightFootLand()
    {
        rightFootAudioSource.PlayOneShot(rightFootstep);
    }

}
