// Copyright (c) 2023 NicoIer and Contributors.
// Licensed under the MIT License (MIT). See LICENSE in the repository root for more information.
#if UNITY_2021_3_OR_NEWER

using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace UnityToolkit
{
    [RequireComponent(typeof(Collider2D))]
    public class Trigger2DEventEmitter : MonoBehaviour
    {
        public event Action<Collider2D> TriggerEnter = delegate { };
        public event Action<Collider2D> TriggerExit = delegate { };
        public event Action<Collider2D> TriggerStay = delegate { };
        public new Collider2D collider2D { get; private set; }
        public LayerMask layerMask;

        protected virtual void Awake()
        {
            collider2D = GetComponent<Collider2D>();
            Assert.IsNotNull(collider2D);
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (layerMask == (layerMask | (1 << other.gameObject.layer)))
            {
                TriggerEnter(other);
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (layerMask == (layerMask | (1 << other.gameObject.layer)))
            {
                TriggerExit(other);
            }
        }

        protected virtual void OnTriggerStay2D(Collider2D other)
        {
            if (layerMask == (layerMask | (1 << other.gameObject.layer)))
            {
                TriggerStay(other);
            }
        }

        protected virtual void OnValidate()
        {
            collider2D = GetComponent<Collider2D>();
            Assert.IsNotNull(collider2D);
            collider2D.isTrigger = true;
        }
    }
}
#endif
