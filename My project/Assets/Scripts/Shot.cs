using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Shot : MonoBehaviour
    {
        public ScoreObject scoreObject;
        public float lifetime;
        public float speed;
        

        [SerializeField] private CircleCollider2D collider;
        
        private Rigidbody2D rb;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            StartCoroutine(ShotLifeTime(lifetime));
        }

        // Update is called once per frame
        void Update()
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }

        public IEnumerator ShotLifeTime(float lifestime)
        {
            yield return new WaitForSeconds(lifestime);
            Destroy(gameObject);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Asteroid"))
            {
                scoreObject.AddToScore();
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
    }
}