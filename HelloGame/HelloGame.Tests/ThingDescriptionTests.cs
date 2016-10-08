using System.Drawing;
using HelloGame.Common;
using HelloGame.Common.Extensions;
using HelloGame.Common.Model;
using HelloGame.Common.Model.GameObjects;
using HelloGame.Common.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Ninject.Syntax;

namespace HelloGame.Tests
{
    [TestClass]
    public class ThingDescriptionTests
    {
        [TestMethod]
        public void ThingDescription_SerializeDeserialize()
        {
            IResolutionRoot ninject =
                new StandardKernel(new HelloGameCommonNinjectBindings(GeneralSettings.Gameplay, true, true));

            var facto = ninject.Get<ThingFactory>();
            BigMass mass = facto.GetBigMass(1, Point.Empty, Color.AliceBlue);

            ThingDescription massDescription = new ThingDescription(mass, false);
            string serialized = massDescription.SerializeJson();

            ThingDescription deserialized = serialized.DeSerializeJson<ThingDescription>();

            Assert.AreEqual(mass.Physics.Size, deserialized.AlmostPhysics.Size);
            Assert.AreEqual("BigMass", deserialized.Type);
            Assert.AreEqual(3, deserialized.ConstructParams.Length);
        }
    }
}