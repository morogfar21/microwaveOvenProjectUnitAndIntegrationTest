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

namespace Microwave.test.integration
{
    [TestFixture]
    public class Step2
    {
        //top level modules
        private IDoor door;

        //included modules
        private UserInterface sut_;

        //stubs
        private IButton powerButton;
        private IButton startCancelButton;
        private IButton timeButton;

        private ICookController stubCookController;
        private IDisplay stubDisplay;
        private ILight stubLight;

        [SetUp]
        public void SetUp()
        {
            powerButton = Substitute.For<IButton>();
            startCancelButton = Substitute.For<IButton>();
            timeButton = Substitute.For<IButton>();

            

            stubCookController = Substitute.For<ICookController>();
            stubDisplay = Substitute.For<IDisplay>();
            stubLight = Substitute.For<ILight>();
            
            door = new Door();
            sut_ = new UserInterface(powerButton, timeButton, startCancelButton, door, stubDisplay, stubLight, stubCookController);
        }

        [Test]
        public void Open_OpenDoor_LightTurnOn()
        {
            //act
            door.Open();
            
            //assert
            stubLight.Received(1).TurnOn();

        }

        [Test]
        public void Close_CloseDoor_LightTurnOff()
        {
            //act
            door.Open();
            door.Close();

            //assert
            stubLight.Received(1).TurnOff();
        }
    }
}
