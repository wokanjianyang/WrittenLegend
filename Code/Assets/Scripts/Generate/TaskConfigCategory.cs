using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class TaskConfigCategory
    {
        public TaskConfig GetById(int taskId)
        {
            try
            {
                TaskConfig config = Get(taskId);

                return config;
            }
            catch 
            {
            }

            return null;
        }
    }

    public class TaskHelper
    {

        public static void CheckTask(TaskType type, long condition)
        {
            User user = GameProcessor.Inst.User;

            TaskConfig config = TaskConfigCategory.Instance.GetById(user.TaskId);

            if (config == null)
            {
                return;
            }

            if (config.Type != (int)type)
            {
                return;
            }

            if (config.Condition <= condition)
            {
                user.TaskLog[user.TaskId] = true;
            }

            GameProcessor.Inst.EventCenter.Raise(new TaskChangeEvent() { });

            return;
        }
    }
}
