using System;
using UnityEngine;

namespace Deckfense
{
    public class Actor : Entity
    {
        private GameObject model;
        private bool isMove = false;
        private Vector3 directionMove;

        private Animator animator;
        
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            Move();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
        }

        public void SetAsset(AssetKind kind)
        {
            var assetRef = AssetManager.Instance.Settings.AssetReferences[(int)kind];
            assetRef.InstantiateAsync().Completed += InstantiateCompleted;
        }

        private void InstantiateCompleted(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
        {
            if (obj.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                if (model != null)
                {
                    if (Enum.TryParse(model.name, out AssetKind kind))
                    {
                        var assetRef = AssetManager.Instance.Settings.AssetReferences[(int)kind];
                        assetRef.ReleaseInstance(model);
                        Destroy(model);
                        model = null;
                    }
                }

                GameObject prefab = obj.Result;
                prefab.transform.SetParent(transform);
                prefab.transform.localPosition = Vector3.zero;
                prefab.transform.localRotation = Quaternion.identity;
                model = prefab.gameObject;
                model.name = model.name.Replace("(Clone)", "").Trim();
            }
        }

        private void Move()
        {
            if (!isMove)
            {
                return;
            }

            transform.Translate(directionMove * Time.fixedDeltaTime * 10f);
        }

        public void MoveStart(Vector3 pos, Vector3 dir, Vector3 rot)
        {
            directionMove = dir;

            transform.position = pos;
            transform.rotation = Quaternion.Euler(rot);
            

            isMove = true;
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
            animator.SetBool("Walk", true);
        }

        public void MoveEnd(Vector3 pos, Vector3 dir, Vector3 rot)
        {
            directionMove = Vector3.zero;

            transform.position = pos;
            transform.rotation = Quaternion.Euler(rot);

            isMove = false;
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
            animator.SetBool("Walk", false);
        }

        public void Rotate(Vector3 eulerAngle)
        {
            transform.rotation = Quaternion.Euler(eulerAngle);
        }
    }
}
