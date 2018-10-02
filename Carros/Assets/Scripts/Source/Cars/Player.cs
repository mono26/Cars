using UnityEngine;
using UnityEngine.UI;

public class Player : EntityComponent
{
    // TODO create special attack class.
    [SerializeField] private float specialMeter;

    [Header("Player components")]
    [SerializeField] private ExternalInput inputComponent;
    [SerializeField] private GameObject targetImg;

    private void Start ()
    {
        targetImg.SetActive(false);
        specialMeter = 10;
        return;
	}

    private void HandlePlayerAnimations()
    {
        Animator animatorToHandle = entity.GetAnimatorComponent;
        if (inputComponent.GetAimInput > 0)
        {
            targetImg.SetActive(true);
            animatorToHandle.SetBoolParameterIfExisting("isShooting", AnimatorControllerParameterType.Bool, true);
            animatorToHandle.SetBoolParameterIfExisting("isDriving", AnimatorControllerParameterType.Bool, false);
        }
        else if (inputComponent.GetAimInput == 0)
        {
            targetImg.SetActive(false);
            animatorToHandle.SetBoolParameterIfExisting("isShooting", AnimatorControllerParameterType.Bool, false);
            animatorToHandle.SetBoolParameterIfExisting("isDriving", AnimatorControllerParameterType.Bool, true);
        }
        return;
    }

    private void HandleInput()
    {
        if(CanApplyExternalInput())
        {
            CarInput inputToPass = new CarInput(
                inputComponent.GetMovementInput, 
                inputComponent.GetFootBrakesInput, 
                inputComponent.GetHandBrakeInput, 
                inputComponent.GetSteeringInput
                );
            entity.ReceiveInput(inputToPass);
        }
        return;
    }

    public override void EveryFrame()
    {
        HandleInput();
        return;
    }

    public override void FixedFrame()
    {
        HandlePlayerAnimations();
        return;
    }

    public bool CanApplyExternalInput()
    {
        bool canApply = true;
        if (entity.GetEntityType != EntityType.Playable && inputComponent == null) {
            canApply = false;
        }
        return canApply;
    }
}
