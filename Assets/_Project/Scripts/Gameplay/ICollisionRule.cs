using UnityEngine;

namespace DefaultNamespace
{
    public interface ICollisionRule
    {
        void Resolve(Animal a, Animal b);
    }

    public class PreyPreyRule : ICollisionRule
    {
        public void Resolve(Animal a, Animal b)
        {
            var direction = (b.transform.position - a.transform.position).normalized;
            const float BOUNCE_BACK_DISTANCE = 1f;

            a.transform.position -= direction * BOUNCE_BACK_DISTANCE;
            b.transform.position += direction * BOUNCE_BACK_DISTANCE;
            
            a.SetMoveDirection(-direction);
            b.SetMoveDirection(direction);

            a.GetMovementBehavior()?.OnInterrupted();
        }
    }
    
    public class PredatorPreyRule : ICollisionRule
    {
        public void Resolve(Animal a, Animal b)
        {
            if (a.IsPrey())
                a.Die();
            else
                b.Die();
        }
    }
    
    public class PredatorPredatorRule : ICollisionRule
    {
        public void Resolve(Animal a, Animal b)
        {
            if (Random.value < 0.5f)
                a.Die();
            else
                b.Die();
        }
    }
}