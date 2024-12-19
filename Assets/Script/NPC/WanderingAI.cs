using System.Collections;
using UnityEngine;

public class WanderingAI : Enemy_AI
{
    new void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        StartCoroutine(ChangeDirectionRoutine());

        Name = "ƒƒhƒ‹ƒfƒB";
    }


}
