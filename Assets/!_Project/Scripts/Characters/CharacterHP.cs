using System;
using UnityEngine;
using UnityEngine.Events;

namespace Character
{
    public class CharacterHP : MonoBehaviour, ITakeDamage
    {
        [SerializeField] private float _maxHP = 10f;
        [SerializeField] private float _initialHp = 5f;

        public ReactiveListener<float> HPListener
        {
            get
            {
                _hpListener ??= new(_hpField);
                return _hpListener;
            }
        }

        private ReactiveField<float> _hpField = new();

        private ReactiveListener<float> _hpListener;

        private void OnValidate()
        {
            _initialHp = Mathf.Min(_maxHP, _initialHp);
        }

        private void Awake()
        {
            _hpField.Value = _initialHp;
        }

        public void TakeDamage(float damage)
        {
            _hpField.Value -= Mathf.Abs(damage);
        }

        public void AddHeal(float heal)
        {
            _hpField.Value = Mathf.Min(_maxHP, _hpField.Value + Mathf.Abs(heal));
        }
    }
}