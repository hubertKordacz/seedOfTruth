using UnityEngine;
using System.Collections;

public class CollectibleItem : MonoBehaviour
{

  public  enum CollectibleType
    {
        HEALTH = 0,
        MACHINEGUN = 1,
        SHOTGUN = 2,
        FLAMETHROWER = 3

    }

    public CollectibleType itemType;
    private Animator animator;
    private bool isActive = false;

    public bool IsActive { get => isActive; }

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
       isActive=false;

        if(animator==null)
            this.gameObject.SetActive(false);

    }

    public void Show()
    {
      isActive = true;
        if (animator)
            animator.SetTrigger("show");
        else
            this.gameObject.SetActive(true);
    }
    public void Hide()
    {
    
        isActive = false;
        if (animator)
            animator.SetTrigger("hide");
        else
            this.gameObject.SetActive(false);
    }
  
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (isActive)
        {
        
            var picker = other.GetComponent<CollectiblePicker>();

            if (picker)
                picker.PickUpCollectible(this);
        }
    }
}
