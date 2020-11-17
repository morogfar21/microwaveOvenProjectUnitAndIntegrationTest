using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses;
using NUnit.Framework;

namespace Microwave.Test.Unit
{
    [TestFixture]
    public class HelperTest
    {
        //uut is a static class, thus no instance of it is created.
        private StringWriter stringWriter;

        [SetUp]
        public void SetUp()
        {
            stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
        }

        [TestCase("Display Shows: lorem W", "Display Shows: * W", true)]
        [TestCase("loremDisplay Shows: lorem Wlorem", "Display Shows: * W", false)]
        [TestCase("loremDisplay Shows: lorem Wlorem", "*Display Shows: * W*", true)]
        [TestCase("loremDisplay lorem Shows: lorem Wlorem", "*Display Shows: * W*", false)]
        public void RegexMatchWithWildCard_aMatchWithb_Expectc(string a, string b, bool c)
        {
            Assert.That(Helper.RegexMatchWithWildCard(a, b), Is.EqualTo(c));
        }

        [TestCase]
        public void ClearStringWriter()
        {
            Console.WriteLine("Lorem Ipsum Dolos Sit Amet");

            Assert.That(stringWriter.ToString(), Does.Contain("Ipsum"));

            Helper.ClearStringWriter(stringWriter);

            Assert.That(stringWriter.ToString(),Is.Empty);
        }
    }
}
