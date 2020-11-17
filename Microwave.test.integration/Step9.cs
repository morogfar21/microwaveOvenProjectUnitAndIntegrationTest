using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.test.integration
{
    [TestFixture]
    public class Step9
    {
        //top modules
        private IButton powerButton;
        private IButton startCancelButton;
        private IButton timeButton;
        

        //module included
        private UserInterface sut_;
        private CookController sutCookController;
        private ITimer sutTimer;
        private IOutput sutOutput;
        private IPowerTube sutPowerTube;
        
        //stubs
        private ILight stubLight;
        private IDisplay stubDisplay;
        private IDoor door;

        private StringWriter stringWriter;

        [SetUp]
        public void SetUp()
        {
            stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            
            stubLight = Substitute.For<ILight>();
            stubDisplay = Substitute.For<IDisplay>();
            door = Substitute.For<IDoor>();
            
            
            
            powerButton = new Button();
            timeButton = new Button();
            startCancelButton = new Button();
            
            sutTimer = new MicrowaveOvenClasses.Boundary.Timer();
            sutOutput = new Output();
            sutPowerTube = new PowerTube(sutOutput);
            
            sutCookController = new CookController(sutTimer, stubDisplay, sutPowerTube);
            sut_ = new UserInterface(powerButton, timeButton, startCancelButton, door, stubDisplay, stubLight, sutCookController);
            sutCookController.UI = sut_;


        }

        [Test]
        public void Press_startMicrowaveWith1Minute_TurnOn()
        {
            //act
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();

            //assert
            Assert.That(stringWriter.ToString(),Does.StartWith("PowerTube works with "));
        }

        [Test]
        public void Press_startMicrowaveWith1Minute_TurnOff()
        {
            //act
            powerButton.Press();
            timeButton.Press();
            startCancelButton.Press();
            Thread.Sleep(61000);
            

            //assert
            Assert.That(stringWriter.ToString(), Does.Contain("PowerTube turned off"));
        }
        
    }
}
