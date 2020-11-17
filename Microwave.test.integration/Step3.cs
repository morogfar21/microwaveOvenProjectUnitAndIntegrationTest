using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MicrowaveOvenClasses;
using NSubstitute;
using NUnit.Framework;
using MicrowaveOvenClasses.Interfaces;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;

namespace Microwave.test.integration
{
    [TestFixture]
    public class Step3
    {
        //top level modules
        private Button powerButton;
        private Button timeButton;
        private Button startCancelButton;
        private Door door;

        //included modules
        private Light light;
        private Display display;
        private CookController cookController;
        private UserInterface sut;

        //stubs
        private IOutput fakeOutput;
        private ITimer fakeTimer;
        private IPowerTube fakePowerTube;

        [SetUp]
        public void SetUp()
        {
            powerButton = new Button();
            timeButton = new Button();
            startCancelButton = new Button();
            door = new Door();

            fakeOutput = Substitute.For<IOutput>();
            fakeTimer = Substitute.For<ITimer>();
            fakePowerTube = Substitute.For<IPowerTube>();

            light = new Light(fakeOutput);
            display = new Display(fakeOutput);
            cookController = new CookController(fakeTimer, display, fakePowerTube);
            sut = new UserInterface(powerButton, timeButton, startCancelButton, door, display, light, cookController);

            cookController.UI = sut;
        }

        //ILight
        [Test]
        public void OpenDoor_LightIsTurnedOn()
        {
            //Test that ILight.TurnOn() is called correctly
            door.Open();
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("on")));
        }

        [Test]
        public void OpenAndCloseDoor_LightIsTurnedOff()
        {
            //Test that ILight.TurnOff() is called correctly
            door.Open();

            door.Close();

            fakeOutput.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }

        //IDisplay
        [Test]
        public void PressStartCancelButton_DisplayIsCleared()
        {
            //Test that IDisplay.Clear() is called correctly
            startCancelButton.Press();

            fakeOutput.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("cleared")));
        }

        [Test]
        public void PressPowerButton_DisplayShowsPowerLevel()
        {
            powerButton.Press();

            fakeOutput.Received(1).OutputLine(Arg.Is<string>(str => Helper.RegexMatchWithWildCard(str, "Display shows: * W")));
        }

        [Test]
        public void PressPowerAndTimeButton_DisplayShowsTime()
        {
            powerButton.Press();
            timeButton.Press();
            fakeOutput.Received(1).OutputLine(Arg.Is<string>(str => Helper.RegexMatchWithWildCard(str, "Display shows: *:*")));
        }

        //ICookController
        [Test]
        public void PressPowerTimeAndStartCancelButton_CookControllerStartsTimerAndTurnsOnPowerTube()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();

            fakeTimer.Received(1).Start(Arg.Any<int>());
            fakePowerTube.Received(1).TurnOn(Arg.Any<int>());
        }

        [Test]
        public void PressPowerOnceTimeOnceAndStartCancelTwiceButton_CookControllerStopsTimerAndTurnsOffPowerTube()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            startCancelButton.Press();

            fakeTimer.Received(1).Stop();
            fakePowerTube.Received(1).TurnOff();
        }
    }
}
