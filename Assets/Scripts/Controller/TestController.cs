using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Deckfense
{
    public class TestController : Controller
    {
        private float deadTimer = 3f;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            deadTimer -= Time.deltaTime;
            if(deadTimer < 0f)
            {
                ObjectPool.Instance.Free(gameObject);
                deadTimer = 111110f;
            }
        }
    }
}
