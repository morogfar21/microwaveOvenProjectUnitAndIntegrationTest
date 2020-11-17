using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using MicrowaveOvenClasses.Boundary;

namespace Microwave.Test.Unit
{
    [TestFixture]
    class UIIntegrationTest
    {
        private UserInterface sut_;
        private IButton powerButton;
        private IButton startCancelButton;
        private IButton timeButton;
        private IDoor door;

        private ICookController stubCookController;
        private IDisplay stubDisplay;
        private ILight stubLight;

        [SetUp]
        public void SetUp()
        {
            powerButton = new Button();
            startCancelButton = new Button();
            timeButton = new Button();

            door = new Door();

            stubCookController = Substitute.For<ICookController>();
            stubDisplay = Substitute.For<IDisplay>();
            stubLight = Substitute.For<ILight>();

            sut_ = new UserInterface(powerButton, timeButton, startCancelButton, door, stubDisplay, stubLight, stubCookController);

        }


        [Test]
        public void Press_pressPowerButtonTwice_ShowPowerCalled()
        {
            powerButton.Press();
            stubDisplay.Received(1).ShowPower(Arg.Any<int>());
            //sut_.Received(1).OnPowerPressed(Arg.Any<object>(),Arg.Any<EventArgs>());
            
        }

        [Test]
        public void Press_pressPowerButtonOnce_ShowPowerCalled()
        {
            powerButton.Press();
            powerButton.Press();
            stubDisplay.Received(2).ShowPower(Arg.Any<int>());
            //sut_.Received(1).OnPowerPressed(Arg.Any<object>(),Arg.Any<EventArgs>());

        }


        [Test]
        public void Press_pressTimeButtonOnce_ShowTimeCalled()
        {
            powerButton.Press();
            timeButton.Press();
            stubDisplay.Received(1).ShowTime(Arg.Any<int>(), Arg.Any<int>());
            //sut_.Received(1).OnTimePressed(Arg.Any<object>(), Arg.Any<EventArgs>());

        }

        [Test]
        public void Press_pressTimeButtonAndPowerButton_ShowTimeCalled()
        {
            powerButton.Press();
            timeButton.Press();
            timeButton.Press();
            

            stubDisplay.Received(2).ShowTime(Arg.Any<int>(),Arg.Any<int>());
            //sut_.Received(1).OnTimePressed(Arg.Any<object>(), Arg.Any<EventArgs>());

        }

        [Test]
        public void Press_pressButtonToActivateEvent_casecalled1()
        {
            powerButton.Press();
            startCancelButton.Press();

            stubLight.Received(1).TurnOff();
            stubDisplay.Received(1).Clear();
        }

        [Test]
        public void Press_pressButtonToActivateEvent_casecalled2()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();

            stubDisplay.Received(1).Clear();
            stubLight.Received(1).TurnOn();
            stubCookController.Received(1).StartCooking(Arg.Any<int>(),Arg.Any<int>());

            
        }

        [Test]
        public void Press_pressButtonToActivateEvent_casecalled3()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            startCancelButton.Press();

            stubDisplay.Received(2).Clear();
            stubLight.Received(1).TurnOn();
            stubCookController.Received(1).StartCooking(Arg.Any<int>(), Arg.Any<int>());

            stubCookController.Received(1).Stop();
            stubLight.Received(1).TurnOff();

        }

        [Test]
        public void Press_pressButtonToActivateEvent_casecalled4()
        {
            startCancelButton.Press();
            stubDisplay.Received(1).Clear();
        }


        [Test]
        public void Close_openDoorAndThenClose_TurnOffCalled()
        {
            door.Open();
            door.Close();
            stubLight.Received(1).TurnOff();
        }




        [Test]
        public void Open_openDoorAndThen1_Called()
        {
            door.Open();
            
            stubLight.Received(1).TurnOn();
        }

        [Test]
        public void Open_openDoorAndThen2_Called()
        {
            powerButton.Press();
            door.Open();
            
            stubLight.Received(1).TurnOn();
            stubDisplay.Received(1).Clear();
        }

        [Test]
        public void Open_openDoorAndThen3_Called()
        {
            powerButton.Press();
            timeButton.Press();
            door.Open();


            stubLight.Received(1).TurnOn();
            stubDisplay.Received(1).Clear();
        }

        [Test]
        public void Open_openDoorAndThen4_Called()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            door.Open();


            stubCookController.Received(1).Stop();
        }
    }
}
