using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GailunController : MonoBehaviour
{
    public Animator animator;

    public Animator enemyAnimator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartAttack();
        }
    }

    public void StartAttack()
    {
        animator.SetTrigger("StartAttack");
    }

    public void EndAttack()
    {
        enemyAnimator.SetTrigger("BeHit");
    }
}
