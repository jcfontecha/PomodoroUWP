using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PomodoroUWP.Models;

namespace PomodoroUWP
{
    public interface IPomodoroSerializer
    {
        Task<string> SerializePomodoroSessionsAsync(List<PomodoroSession> list);
        Task<List<PomodoroSession>> DeserializePomodoroSessionsAsync();
    }
}
