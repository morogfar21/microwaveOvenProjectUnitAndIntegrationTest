using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.test.integration
{
    [TestFixture]
    public class Step8
    {
        //top level modules
        private Button powerButton;
        private Button timeButton;
        private Button startCancelButton;
        private Door door;

        //included modules
        private Output output;
        private Display sut;
        private UserInterface userInterface;

        //stubs
        private ICookController fakeCookController;
        private ILight fakeLight;

        private StringWriter stringWriter;

        [SetUp]
        public void SetUp()
        {
            //Setup driven classes
            powerButton = new Button();
            timeButton = new Button();
            startCancelButton = new Button();
            door = new Door();



            //Setup classes required by sut
            output = new Output();
            sut = new Display(output);


            //Setup fakes required by classes required by sut
            fakeLight = Substitute.For<ILight>();
            fakeCookController = Substitute.For<ICookController>();

            userInterface = new UserInterface(powerButton, timeButton, startCancelButton, door, sut, fakeLight, fakeCookController);

            //Setup stringWriter to test output is correct
            stringWriter = new StringWriter();

            //Override stdout
            Console.SetOut(stringWriter);
        }

        //Test that output outputs the expected string when Display.Clear is called.
        [Test]
        public void PressStartCancelButton_DisplayIsCleared()
        {
            //act
            startCancelButton.Press();

            //assert
            Assert.That(stringWriter.ToString(), Does.Contain("Display cleared"));
        }


        //Test that output outputs the expected string when Display.ShowPower() is called.
        [Test]
        public void PressPowerButton_DisplayShowsPowerLevel()
        {
            //act
            powerButton.Press();

            //assert
            string outputtedString = stringWriter.ToString();
            Assert.That(Helper.RegexMatchWithWildCard(outputtedString, "Display shows: * W\r\n"),Is.True);
        }

        [Test]
        public void PressPowerAndTimeButton_DisplayShowsTime()
        {
            //act
            powerButton.Press();
            StringBuilder stringBuilder = stringWriter.GetStringBuilder();

            Helper.ClearStringWriter(stringWriter);

            timeButton.Press();

            //assert
            string outputtedString = stringWriter.ToString();
            Assert.That(Helper.RegexMatchWithWildCard(outputtedString, "Display shows: *:*\r\n"),Is.True);
        }
    }
}
