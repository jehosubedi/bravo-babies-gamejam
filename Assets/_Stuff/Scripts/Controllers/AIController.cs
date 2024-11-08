using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AIController : MonoBehaviour
{
    [System.Serializable]
    public enum MobState { Idle, Stroll, Contemplating, Hungry, Cashless, GoToVendor, WaitForVendor, Queueing, Satisfied, Unsatisfied }

    public MobState currentState;
    public float hungerMeter = 1;
    public int cash;
    public Image bubble;
    public Sprite[] status;

    bool buying = false;
    bool canBuy = true;
    bool satisfied = false;
    float satisfyDuration;
    Transform targetVendor;
    MobState defaultState;
    BoxCollider2D col;

    private void Awake()
    {
        currentState = (MobState)Random.Range(0, 2);
        defaultState = currentState;
        cash = Random.Range(0, 20);
        hungerMeter = Random.value;
        col = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (satisfied && satisfyDuration > 0)
            satisfyDuration -= Time.deltaTime;
        else
            satisfied = false;

        hungerMeter = Mathf.Clamp(hungerMeter -= Time.deltaTime / 200, 0, 100);

        switch (currentState)
        {
            case MobState.GoToVendor:
                Debug.Log("Going to vendor");
                break;
            case MobState.WaitForVendor:
                Debug.Log("Waiting for vendor");
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
                    float r = Random.Range(2, 5);
                    ShowBubble(0,r);
                    yield return new WaitForSeconds(r);
                    if (cash > 5 && hungerMeter <= 0.25)
                        MoodSwitch(MobState.Hungry);
                    else if (cash < 5)
                        MoodSwitch(MobState.Cashless);
                    else if (hungerMeter > 0.25)
                        MoodSwitch(defaultState);
                    break;
                case MobState.Hungry:
                    ShowBubble(1);
                    buying = true;
                    col.size = Vector2.one * 3;
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
                case MobState.Satisfied:
                    ShowBubble(3);
                    buying = false;
                    satisfied = true;
                    satisfyDuration = Random.Range(5, 20);
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
                    targetVendor = null;
                    break;
            }

            yield return null;
        }
    }

    private void ShowBubble(int status, float delay = 1.5f)
    {
        StopCoroutine(nameof(ShowBubble));

        StartCoroutine(ShowBubble(status));

        IEnumerator ShowBubble(int status)
        {
            bubble.sprite = this.status[status];
            bubble.gameObject.SetActive(true);
            yield return new WaitForSeconds(delay);
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

            if (buying)
            {
                targetVendor.TryGetComponent(out PlayerController p);
                currentState = MobState.Queueing;
                p.Queue(this);
            }
        }
    }
}
