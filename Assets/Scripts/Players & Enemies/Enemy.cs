using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum State { Moving, Targeting, Attacking }
    public State CurrentState = State.Moving;

    public float Health = 100;
    public float MovementSpeed = 20;
    public int GoldValue = 3;
    public float ExperienceValue = 1;
    public float DamageToPlayer;

    public float AttackDistance = 1;
    public bool CanMove = true;
    public bool CanAttack = true;

    Coroutine attackRoutine;

    void FixedUpdate()
    {
        if (CurrentState == State.Moving)
            MoveToPlayer();

        if (Vector3.Distance(transform.position, Player.Instance.GameObject.transform.position) <= AttackDistance)
        {
            if (CanAttack)
            {
                CurrentState = State.Attacking;
                CanAttack = false;

                if (attackRoutine != null)
                    StopCoroutine(attackRoutine);

                attackRoutine = StartCoroutine(IAttack());
            }
        }

        else
        {
            CurrentState = State.Moving;
            if (attackRoutine != null)
            {
                StopCoroutine(attackRoutine);
                CanAttack = true;
            }

        }
    }

    void MoveToPlayer()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = (Player.Instance.GameObject.transform.position - transform.position).normalized / 10 * MovementSpeed;
    }

    IEnumerator IAttack()
    {
        yield return new WaitForSeconds(1);

        Debug.Log("Damaging");
        Player.Instance.TakeDamage(DamageToPlayer);
        CanAttack = true;
    }

    public void TakeDamage(float _damage)
    {
        StartCoroutine(IDamageVisual(Color.red, 0.5f));
        Health -= _damage;
        if (Health < 0)
        {
            Die();
        }
    }

    IEnumerator IDamageVisual(Color _color, float _duration)
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(_duration);
        GetComponent<SpriteRenderer>().color = Color.white;

        yield return null;
    }

    public void Die()
    {
        Player.Instance.GainReward(GoldValue, ExperienceValue);
        WaveManager.Instance.RemoveEnemy(this);
    }
}

public class DefaultEnemy : Enemy
{
    void Awake()
    {
        Health = 100;
        MovementSpeed = 20;
        GoldValue = 3;
        ExperienceValue = 1;
        DamageToPlayer = 15;
    }
}

public class LargeEnemy : Enemy
{
    void Awake()
    {
        Health = 150;
        MovementSpeed = 5;
        GoldValue = 10;
        ExperienceValue = 5;
        DamageToPlayer = 50;
    }
}

public class SmallEnemy : Enemy
{
    void Awake()
    {
        Health = 10;
        MovementSpeed = 50;
        GoldValue = 1;
        ExperienceValue = 0.25f;
        DamageToPlayer = 5;
    }
}
