using DG.Tweening;
using UnityEngine;

namespace Content.Scripts.Components
{
    public class RigidbodyMoverComponent : MoverComponent
    {
        [SerializeField] private Rigidbody target;

        public override void GoToNext()
        {
            MoverLocation nextLocation = GetNextLocation();

            Sequence movementSequence = DOTween.Sequence();
            movementSequence.SetUpdate(updateType);

            movementSequence.Append(target.DOMove(nextLocation.transform.position, nextLocation.duration)
                .SetEase(nextLocation.ease)
                .SetDelay(nextLocation.delay));

            movementSequence.Join(target.DORotate(nextLocation.transform.rotation.eulerAngles, nextLocation.duration)
                .SetEase(nextLocation.ease)
                .SetDelay(nextLocation.delay));

            movementSequence.Play();
        }
    }
}