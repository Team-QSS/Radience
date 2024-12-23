using System;
using System.Collections;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Enemy.TheExecutor
{
    public class ExBehavior : EnemyBehavior
    {
        [SerializeField] private GameObject energyBall;
        private GameObject player;
        private Vector2 playerPos;
        private bool isFirst; 
        [SerializeField]private float[] moveSet;
        private float attackRange;
        private Rigidbody2D rb2D;
        private Animator animator;
        private int randomBehavior;
        private float playerFlipDirection;
        private bool isDashing;
        private bool isUnder;
        private bool isBehaving;
        private bool isSpiting;
        [SerializeField] private BoxCollider2D _collider2D;
        //private TrailRenderer tr;
        private void Start()
        {
            isSpiting = false;
            isUnder = false;
            isBehaving = false;
            isDashing = false;
            animator = GetComponent<Animator>();
            rb2D = GetComponent<Rigidbody2D>();
            isFirst = true;
            player = GameObject.FindWithTag("Player");
            _collider2D.enabled = !_collider2D.enabled;
            //tr = GetComponent<TrailRenderer>();
            //tr.emitting = false;
            Dash();
            stunAni = animator;
            _collider2D.enabled = false;
            maxHp = 7;
            curHp = maxHp;
            isDamaged = false;
            inFiniteTime = 1.2f;
            bossName = "집행자";
            cleared = false;
            SetUpBoss();
        }

        private void FixedUpdate()
        {
            var position = player.transform.position;
            playerPos = new Vector2(position.x, position.y);
            if (playerPos.y>gameObject.transform.position.y)
            {
                isUnder = true;
            }
            else
            {
                isUnder = false;
            }

            if (cleared)
            {
                SceneManager.LoadScene("ExcuterEnd");
            }
        }

        private void Update()
        {
            var playerPosition = player.transform.position;
            playerPos = new Vector2(playerPosition.x, playerPosition.y);
            var position = gameObject.transform.position;
            attackRange = Vector2.Distance(position, playerPos);
            playerFlipDirection = position.x - playerPos.x;
            if (!isDashing)
            {
                gameObject.transform.localScale = playerFlipDirection < 0 ? new Vector3(-1.3f,1.3f,1.3f) : new Vector3(1.3f,1.3f,1.3f);
            }

            if (isStun)
            {
                HideHitBox();
                if (isDamaged) return;
                StartCoroutine(BossGetDamage(inFiniteTime));
            }

        }

        private void NextPattern(float atkRange)
        {
            if (isStun) return;
            if (isUnder)
            {
                Spit();
            }
            else
            {
                if (atkRange < moveSet[0])
                {
                    Attack();
                }

                else if (atkRange > moveSet[1] && atkRange < moveSet[2])
                {
                
                    Dash();
                }
                else if (atkRange > moveSet[0] && atkRange < moveSet[1])
                {
                    randomBehavior = Random.Range(0, 3);
                    switch (randomBehavior)
                    {
                        default:
                            Chase();
                            break;
                        case 2:
                            Dash();
                            break;
                    }
                }
                else
                {
                    Chase();
                }
            }
            
        }

        public void ShowHitBox()
        {
            _collider2D.enabled = true;
        }

        public void HideHitBox()
        {
            _collider2D.enabled = false;
        }

        private void Spit()
        {
            if (isSpiting) return;
            StartCoroutine(SpitFlow());
        }

        public void Fire()
        {
            Instantiate(energyBall, gameObject.transform.position, Quaternion.identity);
        }
        private void Chase()
        {
            StartCoroutine(ChaseFlow());
        }
        
        private void Dash()
        {
            if (isDashing) return;
            isAttacking = true;
            StartCoroutine(DashFlow());
        }

        private void Attack()
        {
            StartCoroutine(AttackFlow());
        }

        
        public void PatternEnd()
        {
            isStun = false;
            HideHitBox();
            NextPattern(attackRange);
        }

        private IEnumerator AttackFlow()
        {

            animator.SetBool("isidle",false);
            animator.SetBool("ischase",false);
            animator.SetBool("isdash",false);
            animator.SetTrigger("isattack");
            yield return new WaitForSeconds(0.7f);
            animator.SetBool("isidle",false);
            animator.SetBool("ischase",false);
            animator.SetBool("isdash",false);

        }

        private IEnumerator SpitFlow()
        {
            isSpiting = true;
            animator.SetBool("isidle",false);
            animator.SetBool("ischase",false); 
            animator.SetBool("isdash",false);
            animator.SetTrigger("isspit");
            yield return new WaitForSeconds(0.8f);
            animator.SetBool("isidle",true);
            animator.SetBool("ischase",false);
            animator.SetBool("isdash",false);
            yield return new WaitForSeconds(1f);
            isSpiting = false;
        }

        private IEnumerator DashFlow()
        {

            isDashing = true;
            var originalGravity = rb2D.gravityScale;
            animator.SetBool("isidle",false);
            animator.SetBool("ischase",false);
            animator.SetBool("isdash",true);
            yield return new WaitForSeconds(0.9f);
            rb2D.gravityScale = 0f;
            //tr.emitting = true;
            rb2D.velocity = new Vector2(gameObject.transform.localScale.x * -20f, 0f);
            //rb2D.AddForce(new Vector2(gameObject.transform.localScale.x * -7f,0),ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.26f);
            rb2D.velocity = new Vector2(0f, 0f);
            rb2D.gravityScale = originalGravity;
            //tr.emitting = false;
            animator.SetBool("isidle",true);
            animator.SetBool("ischase",false);
            animator.SetBool("isdash",false);
            isDashing = false;
            isAttacking = false;

        }

        IEnumerator ChaseFlow()
        {
            
            animator.SetBool("isidle",false);
            animator.SetBool("ischase",true);
            animator.SetBool("isdash",false);
            rb2D.velocity = new Vector2(gameObject.transform.localScale.x * -3f, 0f);
            yield return new WaitForSeconds(0.6f);
            rb2D.velocity = new Vector2(0f, 0f);
            animator.SetBool("isidle",true);
            animator.SetBool("ischase",false);
            animator.SetBool("isdash",false);
        }
        

    }
}
