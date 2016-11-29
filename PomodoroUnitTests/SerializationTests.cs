using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using PomodoroUWP.Models;
using PomodoroUWP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace PomodoroUnitTests
{
    [TestClass]
    public class SerializationTests
    {
        private SerializationTestsHelper CreateHelper()
        {
            return new SerializationTestsHelper();
        }

        [TestMethod]
        public void SavePomodoroToFile_FileWrittenSuccessfully()
        {
            var helper = CreateHelper();
            Task.Run(() => helper.SavePomodoroToFileAsync());
        }
    }

    public class SerializationTestsHelper
    {
        private TimerViewModel CreateViewModelForTests()
        {
            TimerViewModel vm = new TimerViewModel();
            return vm;
        }

        private PomodoroSession CreatePomodoroSessionForTests()
        {
            PomodoroSession session = new PomodoroSession();
            session.Date = DateTime.Today;
            session.Duration = 1500;
            session.Title = "TEST";

            return session;
        }

        public async void SavePomodoroToFileAsync()
        {
            StorageFile file = await TimerViewModel.GetSaveFileAsync();
            PomodoroSession session = CreatePomodoroSessionForTests();

            await FileIO.AppendTextAsync(file, session.Serialize());

            string content = await FileIO.ReadTextAsync(file);
            string expected = "TEST," + DateTime.Today.ToString() + ",1500";

            Assert.AreEqual(expected, content);
        }
    }
}
