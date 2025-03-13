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

            Vector3 targetPos = nextLocation.transform.position;
            movementSequence.Append(target.DOMove(targetPos, nextLocation.duration)
                .SetEase(nextLocation.ease)
                .SetDelay(nextLocation.delay));

            Vector3 targetRot = nextLocation.transform.rotation.eulerAngles;
            movementSequence.Join(target.DORotate(targetRot, nextLocation.duration)
                .SetEase(nextLocation.ease)
                .SetDelay(nextLocation.delay));

            movementSequence.Play();
        }
    }
}