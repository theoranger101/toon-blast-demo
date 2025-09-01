using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Blocks.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class BlockViewAnimator : MonoBehaviour
    {
        public RectTransform RectTransform;
        public Image Image;

        private Sequence m_Seq;

        private void Awake()
        {
            if (RectTransform == null)
            {
                RectTransform = GetComponent<RectTransform>();
            }

            if (Image == null)
            {
                Image = GetComponent<Image>();
            }
        }

        public void KillAll()
        {
            if (m_Seq != null && m_Seq.IsActive())
            {
                m_Seq.Kill();
            }

            m_Seq = null;
        }

        public Tween SpawnIn(float dist, float dur, Ease ease)
        {
            KillAll();

            var start = RectTransform.anchoredPosition + (Vector2.up * dist);
            RectTransform.anchoredPosition = start;

            m_Seq = DOTween.Sequence().SetRecyclable();
            m_Seq.Append(RectTransform.DOAnchorPosY(start.y - dist, dur).SetEase(ease));

            return m_Seq;
        }

        public Tween FallTo(Vector2 target, float dur, Ease ease)
        {
            KillAll();

            m_Seq = DOTween.Sequence().SetRecyclable();
            m_Seq.Append(RectTransform.DOAnchorPos(target, dur, true).SetEase(ease));

            return m_Seq;
        }

        public Tween Pop(float dur, Ease ease)
        {
            KillAll();

            m_Seq = DOTween.Sequence().SetRecyclable();
            m_Seq.Append(RectTransform.DOScale(0, dur).SetEase(ease));

            return m_Seq;
        }

        public Tween Bump(float offset, float dur, Ease ease = Ease.Linear)
        {
            KillAll();

            var pos = RectTransform.anchoredPosition;

            m_Seq = DOTween.Sequence().SetRecyclable();
            m_Seq.Append(RectTransform.DOAnchorPosY(pos.y + offset, dur).SetEase(ease));
            m_Seq.Append(RectTransform.DOAnchorPosX(pos.x, dur).SetEase(ease));

            return m_Seq;
        }

        private void OnDisable()
        {
            KillAll();
        }
    }
}