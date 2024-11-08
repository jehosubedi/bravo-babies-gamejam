using System.Collections;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [System.Serializable]
    public enum MobState { Idle, Stroll, Contemplating, Hungry, Cashless, GoToVendor, WaitForVendor, Queueing, Satisfied, Unsatisfied }

    public MobState currentState;
    public float hungerMeter = 1;
    public int cash;

    bool switchingState = false;
    bool canBuy = true;
    bool satisfied = false;
    float satisfyDuration;
    Transform targetVendor;
    MobState defaultState;

    private void Awake()
    {
        currentState = (MobState)Random.Range(0, 2);
        defaultState = currentState;
        cash = Random.Range(0, 20);
        hungerMeter = Random.value;
    }

    private void Update()
    {
        if (satisfied && satisfyDuration > 0)
            satisfyDuration -= Time.deltaTime;
        else
            satisfied = false;

        if(!switchingState)
            hungerMeter = Mathf.Clamp(hungerMeter -= Time.deltaTime / 250, 0, 100);
    }

    private void MoodSwitch(MobState state)
    {
        StopAllCoroutines();

        StartCoroutine(ChangeMood(state));

        IEnumerator ChangeMood(MobState state)
        {
            currentState = state;
                
            switch (state)
            {
                case MobState.Contemplating:
                    Debug.Log("Mob is contemplating");
                    yield return new WaitForSeconds(Random.Range(2, 5));
                    if (cash > 5 && hungerMeter <= 0.25)
                        MoodSwitch(MobState.Hungry);
                    else if (cash < 5)
                        MoodSwitch(MobState.Cashless);
                    else if (hungerMeter > 0.25)
                        MoodSwitch(defaultState);
                    break;
                case MobState.Hungry:
                    // Popup the speech bubble
                    Debug.Log("Mob is hungry");
                    yield return new WaitForSeconds(1);

                    // Decide if going to follow the vendor or wait for the vendor
                    if (Random.value > .5)
                        MoodSwitch(MobState.GoToVendor);
                    break;
                case MobState.Cashless:
                    // Popup the speech bubble
                    Debug.Log("Mob is broke");
                    canBuy = false;
                    MoodSwitch(defaultState);
                    // Go back to previous state and disable function for buying
                    break;
                case MobState.Satisfied:
                    // Pop the speech bubble
                    Debug.Log("Mob is satisfied");
                    satisfied = true;
                    satisfyDuration = Random.Range(5, 20);
                    MoodSwitch(defaultState);
                    // Go back to previous state and increase hunger meter
                    break;
                case MobState.Unsatisfied:
                    Debug.Log("Mob is unsatisfied and will not buy again");
                    canBuy = false;
                    MoodSwitch(defaultState);
                    // Go back to previous state and disable function for buying
                    break;
                case MobState.Idle:
                    Debug.Log("Mob is idle");
                    break;
                case MobState.Stroll:
                    Debug.Log("Mob is strolling");
                    break;
            }

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Vendor"))
        {
            Debug.Log("Mob found a vendor");
            targetVendor = collision.transform;
            if(!satisfied)
                MoodSwitch(MobState.Contemplating);
        }
    }
}
