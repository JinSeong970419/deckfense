using Protocol.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Deckfense
{
    public class Monster : MonoBehaviour
    {
        private const float timeBodyRemains = 3f;

        private MonsterDataEx data;
        private Animator animator;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float hp;
        [SerializeField] private float movementSpeed;
        private bool isMoving;

        private float deadTimer;

        #region HpBar
        private Slider hpBar;
        private GameObject hpBarObj;
        #endregion
        public MonsterDataEx Data { get { return data; } set { data = value; } }

        private void Awake()
        {
            mainCamera = Camera.main;
            animator = GetComponentInChildren<Animator>();
        }

        void Update()
        {
            if (hp > 0f)
            {
                Move();
            }
            else
            {
                UpdateDead();
            }

        }

        public void Init()
        {
            animator.SetBool("Dead", false);
            hpBarObj = ObjectPool.Instance.Allocate("HpBar");
            hpBarObj.transform.SetParent(GameManager.Instance.UIPanel.transform);
            hpBarObj.transform.localScale = Vector3.one;
            hpBar = hpBarObj.GetComponent<Slider>();
            HpBarAdjustment();
            deadTimer = timeBodyRemains;

            isMoving = true;
        }

        private void Move()
        {
            if (!isMoving)
            {
                return;
            }

            transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
            hpBarObj.transform.position = mainCamera.WorldToScreenPoint(transform.position + new Vector3(0, 1.5f, 0));
        }

        public void LoadData(string monsterId)
        {
            MonsterDataEx data = DataManager.Instance.MonsterTable.Data[monsterId];
            this.data = data;
            this.hp = data.Hp;
            this.movementSpeed = data.MovementSpeed;
        }

        public void SetHp(float hp)
        {
            this.hp = hp;
            if (this.hp <= 0)
            {
                Dead();
            }
            HpBarAdjustment();
        }

        private void UpdateDead()
        {
            deadTimer -= Time.deltaTime;
            if (deadTimer < 0f)
            {
                Free();
            }
        }

        private void Dead()
        {
            animator.SetBool("Dead", true);
            hpBarObj.SetActive(false);
            isMoving = false;
        }

        private void Free()
        {
            hpBarObj.transform.SetParent(ObjectPool.Instance.transform);
            ObjectPool.Instance.Free(hpBarObj);
            ObjectPool.Instance.Free(gameObject);
        }

        private void HpBarAdjustment()
        {
            hpBar.value = hp / data.Hp;
        }

    }
}
