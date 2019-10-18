using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowManAnimatorBehaviour : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponent<Animator>();
        StartCoroutine(this.SetAnimState());
    }

    private IEnumerator SetAnimState()
    {
        this.SetVariableStates(true, false, false);
        yield return new WaitForSeconds(4.20f);
        Debug.Log("eat");
        this.SetVariableStates(false, true, false);
        yield return new WaitForSeconds(1.61f);
        Debug.Log("idle");

        this.SetVariableStates(false, false, true);


    }

    private void SetVariableStates(bool isRuning, bool isSnowEat, bool idle)
    {
        this.animator.SetBool("isRuning", isRuning);
        this.animator.SetBool("isSnowEat", isSnowEat);
        this.animator.SetBool("idle", idle);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
