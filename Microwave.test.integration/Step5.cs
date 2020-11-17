using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.test.integration
{
    [TestFixture]
    public class Step5
    {

        //top modules
        private IButton powerButton;
        private IButton startCancelButton;
        private IButton timeButton;
        

        //module included
        private UserInterface sut_;
        private CookController sutCookController;
        private ITimer sutTimer;
        private IDisplay sutDisplay;



        //stubs
        private IDoor door;
        private ILight stubLight;
        private IOutput stubOutput;
        private IPowerTube stubPowerTube;

        [SetUp]
        public void SetUp()
        {
            stubLight = Substitute.For<ILight>();
            stubOutput = Substitute.For<IOutput>();
            door = Substitute.For<IDoor>();
            stubPowerTube = Substitute.For<IPowerTube>();

            powerButton = new Button();
            startCancelButton = new Button();
            timeButton = new Button();

            sutTimer = new Timer();
            sutDisplay = new Display(stubOutput);
            sutCookController = new CookController(sutTimer,sutDisplay,stubPowerTube);
            sut_ = new UserInterface(powerButton, timeButton, startCancelButton, door, sutDisplay, stubLight, sutCookController);
            sutCookController.UI = sut_;

        }


        [Test]
        public void Press_StartTimerAndWaitToEnd_TurnOffPowertube()
        {
            //act
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();

            Thread.Sleep(61000);
            
            //assert
            //stubOutput.Received().OutputLine(Arg.Is<string>("PowerTube turned off"));
            stubPowerTube.Received(1).TurnOff();

        }


        [Test]
        public void Press_StartMicrowaveWith1Minute_LogLineCalled()
        {
            //act
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();

            Thread.Sleep(3000);
            
            //assert
            stubOutput.Received().OutputLine(Arg.Is<string>(s => s.StartsWith("Display shows:") && s.Length-s.Replace(":","").Length==2));
        }
    }
}
