using ServiceKeeper.Producer.Sample.Domain.EFCore;
using ServiceKeeper.Core;
using ServiceKeeper.Core.Entity;

namespace ServiceKeeper.Producer.Sample.Domain
{
    /// <summary>
    /// 管理任务配置
    /// </summary>
    public class TaskEntityDomainService
    {
        private readonly ServiceScheduler scheduler;
        private readonly LocalSourceOptions options;
        private readonly SaveHelper saver;
        private readonly Dictionary<Guid, TaskEntity> currentTaskEntities = new();
        public TaskEntityDomainService(ServiceScheduler scheduler, LocalSourceOptions options, SaveHelper saver)
        {
            this.scheduler = scheduler;
            this.options = options;
            this.saver = saver;
            Task.Run(() => Run());
        }

        public async void Run()
        {
            while (true)
            {
                try
                {
                    GetTasks();
                    await Task.Delay(30000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
            }
        }

        public void GetTasks()
        {
            using var context = new TaskDbContext();
            Dictionary<Guid, TaskEntity>? tempTaskEntities = context.TaskEntity.ToDictionary(config => config.Id);
            if (tempTaskEntities != null && tempTaskEntities.Count != 0) saver.Save(tempTaskEntities, options.TaskEntitiesSaveName);
            else tempTaskEntities = saver.Load<Dictionary<Guid, TaskEntity>>(options.TaskEntitiesSaveName);

            if (tempTaskEntities == null || tempTaskEntities.Count == 0) { ServiceScheduler.ClearTask(); return; }

            if (currentTaskEntities.Count == 0 && tempTaskEntities.Count != 0)      //本地无配置,从接口获取并初始化
            {
                foreach (var task in tempTaskEntities)
                {
                    scheduler.AddTask(task.Value);
                    //mediator.Publish(new TaskCreatedEvent(task.Value));
                    currentTaskEntities.TryAdd(task.Key, task.Value);
                }
            }
            else if (currentTaskEntities.Count != 0 && tempTaskEntities.Count != 0) //本地有配置,对配置进行更新
            {
                foreach (var item in currentTaskEntities)
                {
                    if (!tempTaskEntities.ContainsKey(item.Key))                    //在当前使用配置中找到新配置中不存在的,触发事件并删除
                    {
                        scheduler.DeleteTask(item.Value);
                        //mediator.Publish(new TaskDeletedEvent(item.Value));
                        currentTaskEntities.Remove(item.Key);
                    }
                }
                foreach (var item in tempTaskEntities)
                {
                    if (currentTaskEntities.ContainsKey(item.Key))                  //在新配置中找到与当前使用配置ID相同但内容不同的,触发事件并更新
                    {
                        if (!currentTaskEntities[item.Key].Equals(item.Value))
                        {
                            scheduler.AddTask(item.Value);
                            //mediator.Publish(new TaskUpdatedEvent(item.Value));
                            currentTaskEntities.Remove(item.Key);
                            currentTaskEntities.Add(item.Key, item.Value);
                        }
                    }
                    else
                    {
                        scheduler.AddTask(item.Value);
                        //mediator.Publish(new TaskCreatedEvent(item.Value));         //在新配置中找到当前使用配置中不存在的,触发事件并添加
                        currentTaskEntities.Add(item.Key, item.Value);
                    }
                }
            }
        }
    }
}
