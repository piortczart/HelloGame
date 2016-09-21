namespace HelloGame.MathStuff
{
    public class Circle : Shape
    {
        public decimal Radius { get; set; }

        public Circle(decimal radius){
            Radius = radius;
        }
    }

    public abstract class Shape
    {
    }
}
