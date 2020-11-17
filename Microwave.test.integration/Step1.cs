using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Microwave.test.integration
{
    [TestFixture]
    public class Step1
    {
        //Top level modules
        private IButton powerButton;
        private IButton startCancelButton;
        private IButton timeButton;

        //Included modules
        private UserInterface sut_;
        
        //Stubs
        private ICookController stubCookController;
        private IDisplay stubDisplay;
        private ILight stubLight;
        private IDoor door;

        [SetUp]
        public void SetUp()
        {
            stubCookController = Substitute.For<ICookController>();
            stubDisplay = Substitute.For<IDisplay>();
            stubLight = Substitute.For<ILight>();
            door = Substitute.For<IDoor>();

            powerButton = new Button();
            startCancelButton = new Button();
            timeButton = new Button();

            sut_ = new UserInterface(powerButton,timeButton,startCancelButton,door,stubDisplay,stubLight,stubCookController);
        }


        [Test]
        public void Press_pressPowerButtonTOnce_ShowPowerCalled()
        {
            //act
            powerButton.Press();
            
            //assert
            stubDisplay.Received(1).ShowPower(Arg.Any<int>());
        }

        [Test]
        public void Press_pressPowerButtonTwice_ShowPowerCalled()
        {
            //act
            powerButton.Press();
            powerButton.Press();

            //assert
            stubDisplay.Received(2).ShowPower(Arg.Any<int>());
        }


        [Test]
        public void Press_pressTimeButtonOnce_ShowTimeCalled()
        {
            //act
            powerButton.Press();
            timeButton.Press();

            //assert
            stubDisplay.Received(1).ShowTime(Arg.Any<int>(), Arg.Any<int>());

        }

        [Test]
        public void Press_pressTimeButtonTwice_ShowTimeCalled()
        {
            //act
            powerButton.Press();
            timeButton.Press();
            timeButton.Press();

            //assert
            stubDisplay.Received(2).ShowTime(Arg.Any<int>(), Arg.Any<int>());
        }

        [Test]
        public void Press_pressButtonToActivateEvent_casecalled1()
        {
            //act
            powerButton.Press();
            startCancelButton.Press();

            //assert
            stubLight.Received(1).TurnOff();
            stubDisplay.Received(1).Clear();
        }

        [Test]
        public void Press_pressButtonToActivateEvent_casecalled2()
        {
            //act
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();

            //assert
            stubDisplay.Received(1).Clear();
            stubLight.Received(1).TurnOn();
            stubCookController.Received(1).StartCooking(Arg.Any<int>(), Arg.Any<int>());


        }

        [Test]
        public void Press_pressButtonToActivateEvent_casecalled3()
        {
            //act
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            startCancelButton.Press();

            //assert
            stubDisplay.Received(2).Clear();
            stubLight.Received(1).TurnOn();
            stubCookController.Received(1).StartCooking(Arg.Any<int>(), Arg.Any<int>());

            stubCookController.Received(1).Stop();
            stubLight.Received(1).TurnOff();

        }

        [Test]
        public void Press_pressButtonToActivateEvent_casecalled4()
        {
            //act
            startCancelButton.Press();
            
            //assert
            stubDisplay.Received(1).Clear();
        }


    }
}
