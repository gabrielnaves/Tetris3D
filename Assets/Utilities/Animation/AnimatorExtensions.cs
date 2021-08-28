using UnityEngine;

static public class AnimatorExtensions {

    static public bool IsCurrentState(this Animator animator, string name, int layerIndex=0) {
        return animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(name);
    }
}
