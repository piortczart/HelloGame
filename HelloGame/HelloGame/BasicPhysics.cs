using HelloGame.MathStuff;

namespace HelloGame
{
    public class BasicPhysics
    {
        /// <summary>
        /// Anything propeling the object.
        /// </summary>
        public Real2DVector SelfPropelling { get; set; }
        /// <summary>
        /// Current interia of the object.
        /// </summary>
        public Real2DVector Interia { get; set; }

        public Real2DVector TotalForce => Real2DVector.Combine(SelfPropelling, Interia);
    }
}
