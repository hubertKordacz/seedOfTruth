using UnityEngine;
using System.Collections;

public class CollectibleItem : MonoBehaviour
{

  public  enum CollectibleType
    {
        HEALTH = 0,
        MACHINEGUN = 1,
        SHOTGUN = 2,
        FLAMETHROWER = 3,
        SNIPERRIFFLE = 4,
        PISTOL = 5,
        ROCKETLAUNCHER = 6


    }

    public CollectibleType itemType;
    public AudioClip showSound;
    public AudioClip hideSound;

    private Animator animator;
    private bool isActive = false;
    private AudioSource audioSource;

    public ParticleSystem spawnParticle;

    public bool IsActive { get => isActive; }

    private void Awake()
    {

        audioSource = GetComponent<AudioSource>();
        animator = this.GetComponent<Animator>();
       isActive=false;

        if(animator==null)
            this.gameObject.SetActive(false);

    }

    public void Show()
    {
      isActive = true;
        if (audioSource != null && showSound != null)
            audioSource.PlayOneShot(showSound);

        if (animator)
            animator.SetTrigger("show");
        else 
            this.gameObject.SetActive(true);
        spawnParticle.Play(true);
    }
    public void Hide()
    {
        if (audioSource != null && hideSound != null)
            audioSource.PlayOneShot(hideSound);

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
