﻿using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using PomodoroUWP.ViewModels;
using PomodoroUWP.Models;

namespace PomodoroUnitTests
{
    [TestClass]
    class TimerTests
    {
        [TestMethod]
        public void CreateNewTimer_ResultMatchesExpected()
        {
            var timerService = new TimerService(1500); // 25 minutes


        }
    }
}
