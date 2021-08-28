using UnityEngine;

public class DespawnOnAnimatorStateEnter : StateMachineBehaviour {

    public float delay = 0;

    float elapsedTime;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        elapsedTime = 0;
        if (delay <= 0)
            SimplePool.Despawn(animator.gameObject);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > delay)
            SimplePool.Despawn(animator.gameObject);
    }
}
