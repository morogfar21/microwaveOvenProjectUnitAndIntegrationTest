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
using NUnit.Framework.Internal.Execution;

namespace Microwave.test.integration
{
    [TestFixture]
    public class Step4
    {

        //top modules
        private IButton powerButton;
        private IButton startCancelButton;
        private IButton timeButton;
        

        //module included
        private UserInterface sut_;
        private CookController sutCookController;
        private IPowerTube sutPowerTube;


        //stubs
        private IDoor door;
        private ILight stubLight;
        private IOutput stubOutput;
        private ITimer stubTimer;
        private IDisplay stubDisplay;

        [SetUp]
        public void SetUp()
        {
            stubLight = Substitute.For<ILight>();
            stubOutput = Substitute.For<IOutput>();

            powerButton = new Button();
            startCancelButton = new Button();
            timeButton = new Button();

            door = Substitute.For<IDoor>();

            stubTimer = Substitute.For<ITimer>();

            stubDisplay = Substitute.For<IDisplay>();
            sutPowerTube = new PowerTube(stubOutput);

            sutCookController = new CookController(stubTimer,stubDisplay,sutPowerTube);
            sut_ = new UserInterface(powerButton, timeButton, startCancelButton, door, stubDisplay, stubLight, sutCookController);
            sutCookController.UI = sut_;
            



        }

        [Test]
        public void Press_StartMicrowaveWithOneMinuteAndWait_LightTurnOff()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();

            //don't want to wait too long
            stubTimer.Expired += Raise.Event();
            stubLight.Received(1).TurnOff();
            
            //sutCookController.OnTimerExpired += Raise.EventWith(new object());

        }

        

        [Test]
        public void Press_TurnOnPowerTube_OutputLineCalled()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();

            //don't want to wait too long
            stubTimer.Expired += Raise.Event();

            //stubOutput.Received(1).OutputLine("");
            stubOutput.Received(1).OutputLine($"PowerTube works with 50 W");
        }

        [Test]
        public void Press_TurnOffPowertube_OutputLineCalled()
        {
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();

            //don't want to wait too long
            stubTimer.Expired += Raise.Event();
            

            stubOutput.Received(1).OutputLine($"PowerTube turned off");
        }


        
    }
}
