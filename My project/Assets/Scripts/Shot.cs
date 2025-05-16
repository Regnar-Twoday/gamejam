using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Shot : MonoBehaviour
    {

        public float lifetime;
        public float speed;

        void Start()
        {
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
    }
}