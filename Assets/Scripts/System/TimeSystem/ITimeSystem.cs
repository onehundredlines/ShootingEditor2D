using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
namespace ShootingEditor2D
{
    public interface ITimeSystem : ISystem
    {
        float CurrentSeconds { get; }
        void AddTimeDelay(float seconds, Action onDelayFinish);
    }
    public enum DelayTaskState
    {
        NotStart,
        Start,
        Finish
    }
    public class DelayTask
    {
        public float Seconds;
        private float startSeconds;
        public Action OnFinish { get; set; }

        public float StartSeconds { get; set; }
        public float FinishSeconds { get; set; }
        public DelayTaskState State { get; set; }
    }
    public class TimeSystem : AbstractSystem, ITimeSystem
    {
        /// <summary>
        /// 只有注册，没有销毁
        /// 有需要销毁的求，需要在TimeSystem添加销毁函数
        /// 并在在TimeSystemUpdateMonoBehaviour中添加成员变量
        /// </summary>
        public class TimeSystemUpdateMonoBehaviour : MonoBehaviour
        {
            public event Action OnUpdate;
            private void Update() => OnUpdate?.Invoke();
        }
        protected override void OnInit()
        {
            var updateMonoBehaviourGameObject = new GameObject(nameof(TimeSystemUpdateMonoBehaviour));
            var timeSystemUpdateMonoBehaviour = updateMonoBehaviourGameObject.AddComponent<TimeSystemUpdateMonoBehaviour>();
            timeSystemUpdateMonoBehaviour.OnUpdate += OnUpdate;
            CurrentSeconds = 0f;
        }
        private void OnUpdate()
        {
            CurrentSeconds += Time.deltaTime;
            if (delayTasks.Count > 0)
            {
                var currentNode = delayTasks.First;
                while(currentNode != null)
                {
                    var nextNode = currentNode.Next;
                    var delayTask = currentNode.Value;
                    if (delayTask.State == DelayTaskState.NotStart)
                    {
                        delayTask.State = DelayTaskState.Start;
                        delayTask.StartSeconds = CurrentSeconds;
                        delayTask.FinishSeconds = CurrentSeconds + delayTask.Seconds;
                    } else if (delayTask.State == DelayTaskState.Start)
                    {
                        if (CurrentSeconds >= delayTask.FinishSeconds)
                        {
                            delayTask.State = DelayTaskState.Finish;
                            delayTask.OnFinish();

                            delayTask.OnFinish = null;
                            delayTasks.Remove(currentNode);
                        }
                    }
                    currentNode = nextNode;
                }
            }
        }
        public float CurrentSeconds { get; private set; }
        private LinkedList<DelayTask> delayTasks = new LinkedList<DelayTask>();
        public void AddTimeDelay(float seconds, Action onDelayFinish)
        {
            var task = new DelayTask()
            {
                Seconds = seconds,
                OnFinish = onDelayFinish,
                State = DelayTaskState.NotStart
            };
            delayTasks.AddLast(task);
        }
    }
}