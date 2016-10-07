using System.Drawing;
using HelloGame.Common;
using HelloGame.Common.Extensions;
using HelloGame.Common.Model;
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
            var t = facto.GetBigMass(1, Point.Empty);

            var td = new ThingDescription(t, false);
            var ts = td.SerializeJson();

            var tsd = ts.DeSerializeJson<ThingDescription>();

            Assert.AreEqual(1, tsd.Id);
            Assert.AreEqual("BigMass", tsd.Type);
            Assert.AreEqual(3, tsd.ConstructParams.Length);
        }
    }
}