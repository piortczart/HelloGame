using HelloGame.MathStuff;

namespace HelloGame
{
    public class BasicPhysics
    {
        /// <summary>
        /// The more the worse.
        /// </summary>
        public decimal Aerodynamism { get; set; }

        public decimal Mass { get; set; }
        /// <summary>
        /// Anything propeling the object.
        /// </summary>
        public Real2DVector SelfPropelling { get; set; }
        /// <summary>
        /// Current interia of the object.
        /// </summary>
        public Real2DVector Interia { get; set; }
        /// <summary>
        /// Current drag.
        /// </summary>
        public Real2DVector Drag { get; set; }

        public Real2DVector TotalForce => Real2DVector.Combine(SelfPropelling, Interia, Drag);

        public BasicPhysics()
        {
            Interia = new Real2DVector();
            SelfPropelling = new Real2DVector();
        }
    }
}
