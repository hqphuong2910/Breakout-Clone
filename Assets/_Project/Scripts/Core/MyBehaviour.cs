using UnityEngine;

namespace _Project.Scripts.Core
{
    public abstract class MyBehaviour : MonoBehaviour
    {
        protected virtual void Awake()
        {
            LoadComponents();
        }

        protected virtual void Reset()
        {
            LoadComponents();
        }

        protected virtual void Start()
        {
            LoadDependencies();
        }

        protected virtual void OnEnable()
        {
            SubscribeEvents();
        }

        protected virtual void OnDisable()
        {
            UnsubscribeEvents();
        }

        protected virtual void LoadComponents()
        {
        }

        protected virtual void LoadDependencies()
        {
        }

        protected virtual void SubscribeEvents()
        {
        }

        protected virtual void UnsubscribeEvents()
        {
        }
    }
}