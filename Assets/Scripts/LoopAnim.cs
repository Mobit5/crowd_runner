using UnityEngine;

public class LoopAnim : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        if (state.normalizedTime >= 1f)
            animator.Play(state.fullPathHash, 0, 0f);
    }
}