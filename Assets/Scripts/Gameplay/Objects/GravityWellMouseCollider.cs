﻿using System;
using System.Collections;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts {
    public class GravityWellMouseCollider : MonoBehaviour,
        IPointerDownHandler, IPointerUpHandler, IPointerClickHandler,
        IPointerEnterHandler, IPointerExitHandler, IDragHandler, IDropHandler {
        private GravityWell well;
        public Sprite AttractSprite;
        public Sprite RepelSprite;
        public Sprite NoneSprite;

        private SpriteRenderer _spriteRenderer;
        private float _forceStrength = 20;

        public delegate void OnWellChangeDelegate(Transform trans, GravityWellModifierStatus curStatus, float strength);
        public event OnWellChangeDelegate OnWellChangeEvent;

        private void Awake() {
            well = GetComponentInParent<GravityWell>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            OnWellChangeEvent += ChangeSprite;
            OnWellChangeEvent += GameManager.Instance.MainCamera.GetComponent<TwistEffect>().ChangeTwistVal;
        }

        private void OnMouseBehaviourChanged() {
            bool isLmbDown = Input.GetMouseButtonDown(0);
            bool isRmbDown = Input.GetMouseButtonDown(1);
            if (isLmbDown && isRmbDown) {
                // The player is messing with the game
                return;
            }
            GravityWellModifierStatus objectStatus = GravityWellModifierStatus.None;
            if (isLmbDown) {
                objectStatus = GravityWellModifierStatus.Attract;
            } else if (isRmbDown) {
                objectStatus = GravityWellModifierStatus.Repel;
            }
            BroadcastMouseHitEvent(objectStatus);
        }

        private void BroadcastMouseHitEvent(GravityWellModifierStatus status) {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(clickPosition, Vector2.zero);
            foreach (RaycastHit2D hit in hits) {
                if (hit.collider == null) {
                    continue;
                }
                GravityWellMouseCollider hitMouseCollider = hit.collider.GetComponent<GravityWellMouseCollider>();
                if (hitMouseCollider == null) {
                    continue;
                }
                GameObject hitObj = hit.collider.gameObject;
                hitMouseCollider.SwitchModifierStatus(status);
            }
        }

        public void SwitchModifierStatus(GravityWellModifierStatus status) {
            if (well.status == status) {
                return;
            }
            OnWellChangeEvent?.Invoke(well.transform, status, _forceStrength);
            well.status = status;
            Debug.LogFormat("{0} status set to {1}", well.gameObject.name, status);
        }

        #region IPointerDelegate

        public void OnPointerDown(PointerEventData eventData) {
            OnMouseBehaviourChanged();
        }
        public void OnPointerUp(PointerEventData eventData) {
            OnMouseBehaviourChanged();
        }
        public void OnPointerClick(PointerEventData eventData) {
            OnMouseBehaviourChanged();
        }
        public void OnPointerEnter(PointerEventData eventData) {
            OnMouseBehaviourChanged();
        }
        public void OnPointerExit(PointerEventData eventData) {
            OnMouseBehaviourChanged();
        }
        public void OnDrag(PointerEventData eventData) {
            OnMouseBehaviourChanged();
        }
        public void OnDrop(PointerEventData eventData) {
            OnMouseBehaviourChanged();
        }
        #endregion


        private void ChangeSprite(Transform trans, GravityWellModifierStatus curStatus, float strength)
        {
            switch (curStatus)
            {
                case (GravityWellModifierStatus.Attract):
                    _spriteRenderer.sprite = AttractSprite;
                    break;
                case (GravityWellModifierStatus.Repel):
                    _spriteRenderer.sprite = RepelSprite;
                    break;
                case (GravityWellModifierStatus.None):
                    _spriteRenderer.sprite = NoneSprite;
                    break;
                default: break;

            }
        }
    }
}