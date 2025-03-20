using UnityEngine;

public class StoveBurnFlashingBarUI : MonoBehaviour
{

    private const string IS_FLASHING = "IsFlashing";

    [SerializeField] private StoveCounter stoveCounter;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;

        animator.SetBool(IS_FLASHING, false);
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float _burnShowProgressAmount = .5f;
        bool _show = stoveCounter.IsFried() && e._progressNormalized >= _burnShowProgressAmount;


        animator.SetBool(IS_FLASHING, _show);
    }
}
