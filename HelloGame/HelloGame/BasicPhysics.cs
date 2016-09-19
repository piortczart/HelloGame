namespace HelloGame
{
    public class BasicPhysics
    {
        public Real2DVector SelfPropelling { get; set; }
        public Real2DVector Interia { get; set; }
        public Real2DVector Drag { get; set; }

        public Real2DVector TotalForce { get { return Real2DVector.Combine(SelfPropelling, Interia); } }
    }
}
