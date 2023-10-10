using UnityEngine;

public static class AnimatorUtils
{
    public const string ANIMATION_BASE_LAYER = "Base Layer";
    public const string ANIMATION_PATH_SEPARATOR = ".";

    public static void ResetAllTriggers(this Animator animator)
    {
        foreach (var trigger in animator.parameters)
        {
            if (trigger.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(trigger.name);
            }
        }
    }

    public static bool IsTriggerOrStateActive(
       this Animator animator,
       string name)
    {
        return animator.IsTriggerOrStateActive(name, name);
    }

    public static bool IsTriggerOrStateActive(
        this Animator animator,
        string triggerName,
        string stateName)
    {
        return animator.IsTriggerEnabled(triggerName)
            || animator.IsStateActive(stateName);
    }

    public static bool IsTriggerEnabled(
        this Animator animator,
        string triggerName)
    {
        var fullName
            = ANIMATION_BASE_LAYER
            + ANIMATION_PATH_SEPARATOR
            + triggerName;
        return animator.GetBool(triggerName);
    }

    public static bool IsStateActive(
        this Animator animator,
        string stateName)
    {
        var fullStateName
            = ANIMATION_BASE_LAYER
            + ANIMATION_PATH_SEPARATOR
            + stateName;
        return animator
                .GetCurrentAnimatorStateInfo(0)
                .IsName(fullStateName)
            || animator
                .GetNextAnimatorStateInfo(0)
                .IsName(fullStateName);
    }
}
