using System.Drawing;
using HelloGame.Common.Model.GameObjects;
using HelloGame.Common.Model.GameObjects.Ships;

namespace HelloGame.Common.Model
{
    public interface IThingFactory
    {
        ThingBase CreateFromDescription(ThingDescription description);
        PlayerShipMovable GetPlayerShipMovable(int size, Point location, string name, int? id = null);
        PlayerShipAny GetPlayerShip(int size, Point location, string name, int? id = null);
        AiShip GetAiShip(int size, Point point, string name, int? id = null);
        BigMass GetBigMass(int? size = null, Point? point = null, int? id = null);
    }
}