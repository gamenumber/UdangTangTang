using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OwlFly : Interaction
{
    [SerializeField] float glideFallSpeed, glideMoveSpeed, glideJumpPower, glideJumpCooldown, maxGlideSpeed;
    [SerializeField] int glideJumpCount;
    [SerializeField] float toggleCooldown;
    [SerializeField] Glide glide;
    MovementController movements;
    float tmp = 0.0f;
    bool gliding = false;
    public override void Start()
    {
        base.Start();
        remove = false;
        requireType = true;
        requiredType = AnimalType.owl;
        movements = GameManager.Instance.M_PlayerMovements;
        glide.glideFallSpeed = glideFallSpeed;
        glide.glideMoveSpeed = glideMoveSpeed;
        glide.glideJumpPower = glideJumpPower;
        glide.glideJumpCooldown = glideJumpCooldown;
        glide.maxGlideSpeed = maxGlideSpeed;
        glide.glideJumpCount = glideJumpCount;
        GameManager.Instance.M_NewCutsceneManager.atCutsceneStart.AddListener(CutsceneUnglide);
    }
    private void Update()
    {
        if (tmp < toggleCooldown) tmp += Time.deltaTime;
    }
    public override void Interact()
    {
        base.Interact();
        if (tmp < toggleCooldown) return;
        if (glide.enabled == false)
        {
            movements.antiAirMove = false;
            movements.rb.velocityX = Mathf.Clamp(movements.rb.velocityX, -maxGlideSpeed, maxGlideSpeed);
            movements.Switch(glide);
        }
        else
        {
            movements.antiAirMove = true;
            glide.UnGlide();
        }
    }
    void CutsceneUnglide()
    {
        glide.UnGlide();
    }
}
