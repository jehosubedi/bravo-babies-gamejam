using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [System.Serializable]
    public enum MobState { Idle, Stroll, Contemplating, Hungry, Cashless, GoToVendor, WaitForVendor, Queueing, Satisfied, Unsatisfied }

    public MobState currentState;
    public float hungerMeter = 1;
    public int cash;
    public Animator bubble;
    public Vector2 velo;

    private bool buying = false;
    private bool canBuy = true;
    private bool satisfied = false;
    private float satisfyDuration;
    private Transform targetVendor;
    private Transform targetPOI;
    private Transform originPOI;
    private MobState defaultState;
    private BoxCollider2D col;
    private MobSpawnController spawnController;
    private NavMeshAgent agent;
    private float idleTime;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = Random.Range(1.2f,3.5f);
    }

    public void Initialize(MobSpawnController controller, Transform destination, Transform origin)
    {
        targetPOI = destination;
        originPOI = origin;
        spawnController = controller;
        currentState = (MobState)Random.Range(0, 2);
        if (targetPOI != null && currentState != MobState.Stroll)
            currentState = MobState.Stroll;
        defaultState = currentState;
        cash = Random.Range(0, 75);
        hungerMeter = Random.Range(0f,0.7f);
        col = GetComponent<BoxCollider2D>();
        idleTime = Random.Range(5, 8);
    }
    public void FulfillOrder()
    {
        cash -= 15;
        buying = false;
        MoodSwitch(MobState.Satisfied);
    }

    public void UnfulfilleOrder()
    {
        buying = false;
        MoodSwitch(MobState.Unsatisfied);
    }

    private void Update()
    {
        if (satisfied && satisfyDuration > 0)
            satisfyDuration -= Time.deltaTime;
        else
            satisfied = false;

        anim.SetBool("IsMoving", agent.velocity.normalized.magnitude > 0);

        if(agent.velocity.normalized.magnitude > 0)
        {
            anim.SetFloat("Y", agent.velocity.normalized.y);
            anim.SetFloat("X", agent.velocity.normalized.x);
        }

        hungerMeter = Mathf.Clamp(hungerMeter -= Time.deltaTime / 200, 0, 100);

        switch (currentState)
        {
            case MobState.GoToVendor:
                if (Mathf.Abs((transform.position - targetVendor.position).magnitude) > 1f)
                {
                    agent.SetDestination(targetVendor.position);
                    agent.isStopped = false;
                }
                else if(buying && !agent.isStopped)
                {
                    targetVendor.TryGetComponent(out PlayerController p);
                    currentState = MobState.Queueing;
                    bubble.gameObject.SetActive(false);
                    agent.isStopped = true;
                    if (!p.Queue(this))
                        MoodSwitch(MobState.Unsatisfied);
                }
                break;
            case MobState.WaitForVendor:
                if (buying && Mathf.Abs((transform.position - targetVendor.position).magnitude) <= 1f)
                {
                    targetVendor.TryGetComponent(out PlayerController p);
                    currentState = MobState.Queueing;
                    bubble.gameObject.SetActive(false);
                    agent.isStopped = true;
                    if (!p.Queue(this))
                        MoodSwitch(MobState.Unsatisfied);
                }
                break;
            case MobState.Stroll:
                agent.SetDestination(targetPOI.position);
                if(Mathf.Abs((transform.position - targetPOI.position).magnitude) < .5f)
                {
                    spawnController.PopNPC(gameObject);
                    Destroy(gameObject);
                }
                break;
            case MobState.Idle:
                if (idleTime > 0)
                    idleTime -= Time.deltaTime;
                else
                    MoodSwitch(MobState.Stroll);

                break;
        }
    }

    private void MoodSwitch(MobState state)
    {
        StopCoroutine(nameof(MoodSwitch));

        StartCoroutine(ChangeMood(state));

        IEnumerator ChangeMood(MobState state)
        {
            currentState = state;
                
            switch (state)
            {
                case MobState.Contemplating:
                    if(agent != null)
                        agent.isStopped = true;
                    float r = Random.Range(2, 5);
                    ShowBubble(0,r);
                    yield return new WaitForSeconds(r);
                    if (cash >= 15 && hungerMeter <= 0.25)
                        MoodSwitch(MobState.Hungry);
                    else if (cash < 15)
                        MoodSwitch(MobState.Cashless);
                    else if (hungerMeter > 0.25)
                        MoodSwitch(defaultState);
                    break;
                case MobState.Hungry:
                    ShowBubble(1, loop: true);
                    buying = true;
                    col.size = Vector2.one * 0.25f;
                    // Decide if going to follow the vendor or wait for the vendor
                    if (Random.value > .5)
                        MoodSwitch(MobState.GoToVendor);
                    else
                        MoodSwitch(MobState.WaitForVendor);
                    break;
                case MobState.Cashless:
                    ShowBubble(2);
                    buying = false;
                    canBuy = false;
                    MoodSwitch(defaultState);
                    // Go back to previous state and disable function for buying
                    break;
                case MobState.Satisfied:  /// UPDATE DESTINATION HERE AND THE STATE
                    ShowBubble(3);
                    buying = false;
                    satisfied = true;
                    satisfyDuration = Random.Range(5, 20);
                    yield return new WaitForSeconds(3);
                    if (defaultState == MobState.Stroll && targetPOI != null)
                        agent.SetDestination(targetPOI.position);

                    MoodSwitch(defaultState);
                    // Go back to previous state and increase hunger meter
                    break;
                case MobState.Unsatisfied:
                    ShowBubble(4);
                    buying = false;
                    canBuy = false;
                    MoodSwitch(defaultState);
                    // Go back to previous state and disable function for buying
                    break;
                case MobState.Idle:
                    targetVendor = null;
                    break;
                case MobState.Stroll:
                    agent.isStopped = false;
                    targetVendor = null;
                    break;
            }

            yield return null;
        }
    }

    private void ShowBubble(int status, float delay = 1.5f,bool loop = false)
    {
        StopCoroutine(nameof(ShowBubble));

        StartCoroutine(ShowBubble(status));

        IEnumerator ShowBubble(int status)
        {
            bubble.gameObject.SetActive(true);
            bubble.SetInteger("AnimIndex", status);
            yield return new WaitForSeconds(delay);
            if(!loop)
                bubble.gameObject.SetActive(false);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Vendor") && canBuy)
        {
            if (currentState != MobState.Contemplating)
                if (!satisfied && !buying)
                {
                    targetVendor = collision.transform;
                    MoodSwitch(MobState.Contemplating);
                }
        }
    }
}
