using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    private Player player;
    private ProjectileSpawner rangedSpawner;
    private MeleeSpawner meleeSpawner;
    private Animator animator;
    
    private bool isSpecialAttack = false;
    private bool canMelee = true;

    private ResourceBarController resourceBar;
    private SpecialAttackBarController specialBar;

    private bool activeMessage = false;

    private void Start()
    {
        player = GetComponent<Player>();
        rangedSpawner = GetComponent<ProjectileSpawner>();
        meleeSpawner = GetComponent<MeleeSpawner>();

        animator = GetComponentInChildren<Animator>();
        resourceBar = FindObjectOfType<ResourceBarController>();
        specialBar = FindObjectOfType<SpecialAttackBarController>();
    }
    
    private void Update()
    {
        if (Time.timeScale < 1)
        {
            return;
        }
        
        
        if (player.health > 0)
        {
            Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            player.SetDirectionalInput(directionalInput);
            
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }

            if (Input.GetButtonDown("Jump"))
            {
                player.OnJumpInputDown();
                animator.SetBool("isJumping", true);
            }

            if (Input.GetButtonUp("Jump"))
            {
                player.OnJumpInputUp();
            }

            // Left Mouse Button
            if (Input.GetMouseButtonDown(0))
            {
                if (!isSpecialAttack)
                {
                    if (resourceBar.CanAttackMelee())
                    {
                        if (canMelee)
                        {
                            StartCoroutine(coMeleeCooldown());
                        }
                    }
                    else
                    {
                        StartCoroutine(coNotEnoughResource());
                    }
                }
                else
                {
                    if (resourceBar.CanAttackAltMelee())
                    {
                        meleeSpawner.AltMelee();
                        specialBar.UseSpecial();
                        isSpecialAttack = false;
                    }
                    else
                    {
                        StartCoroutine(coNotEnoughResource());
                    }
                }
            }

            // Right Mouse Button
            if (Input.GetMouseButtonDown(1))
            {
                if (!isSpecialAttack)
                {
                    if (resourceBar.CanAttackRanged())
                    {
                        rangedSpawner.Fire(Input.mousePosition);
                    }
                    else
                    {
                        StartCoroutine(coNotEnoughResource());
                    }
                }
                else
                {
                    if (resourceBar.CanAttackAltRanged())
                    {
                       
                        rangedSpawner.AltFire(Input.mousePosition);
                        specialBar.UseSpecial();
                        isSpecialAttack = false;
                    }
                    else
                    {
                        StartCoroutine(coNotEnoughResource());
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (specialBar.CanUseSpecial())
                {
                    isSpecialAttack = !isSpecialAttack;
                    specialBar.ToggleActivate();
                }
                else
                {
                    StartCoroutine(coSpecialNotReady());
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Instantiate(Resources.Load("menus/pauseMenu"), GameObject.FindObjectOfType<Canvas>().transform);
            }
        }
        
        else
        {
            rangedSpawner.enabled = false;
            meleeSpawner.enabled = false;
            animator.enabled = false;
        }
        
    }
    
    /*
    public IEnumerator coNotReady()
    {
        if (activeMessage == false)
        {
            activeMessage = true;
            GameObject notReadyMessage = Instantiate(Resources.Load("menus/notReadyMessage"), GameObject.FindWithTag("MainCanvas").transform) as GameObject;
            float waitTime = Time.time - (lastMelee + meleeCooldown);
            yield return new WaitForSeconds(waitTime + 0.25f);
            Destroy(notReadyMessage);
            activeMessage = false;
        }
    }
    */

    public IEnumerator coNotEnoughResource()
    {
        //if (activeMessage == false)
        //{
        //    activeMessage = true;
        //    GameObject message = Instantiate(Resources.Load("menus/notEnoughResourceMessage"), GameObject.FindWithTag("MainCanvas").transform) as GameObject;
        //    yield return new WaitForSeconds(1);
        //    Destroy(message);
        //    activeMessage = false;
        //}

        if (activeMessage == false)
        {
            activeMessage = true;
            if (!resourceBar.CanAttackMelee())
            {
                GameObject message = Instantiate(Resources.Load("menus/meleeOveruseMessage"), GameObject.FindWithTag("MainCanvas").transform) as GameObject;
                yield return new WaitForSeconds(1);
                Destroy(message);
            }
            else if (!resourceBar.CanAttackRanged())
            {
                GameObject message = Instantiate(Resources.Load("menus/rangeOveruseMessage"), GameObject.FindWithTag("MainCanvas").transform) as GameObject;
                yield return new WaitForSeconds(1);
                Destroy(message);
            }
            
            //Destroy(message);
            activeMessage = false;
        }
    }
    
    public IEnumerator coSpecialNotReady()
    {
        if (activeMessage == false)
        {
            activeMessage = true;
            GameObject message = Instantiate(Resources.Load("menus/specialNotReadyMessage"), GameObject.FindWithTag("MainCanvas").transform) as GameObject;
            yield return new WaitForSeconds(1);
            Destroy(message);
            activeMessage = false;
        }
    }

    public IEnumerator coMeleeCooldown()
    {
        canMelee = false;
        meleeSpawner.DirectionalMelee(Input.mousePosition);
        yield return new WaitForSeconds(0.5f);
        canMelee = true;
    }

    public void Restart()
    {
        rangedSpawner.enabled = true;
        meleeSpawner.enabled = true;
        animator.enabled = true;
    }
}