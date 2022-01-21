using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
namespace QFramework
{
    #region Architecture
    public interface IArchitecture
    {
        /// <summary>
        /// 注册System系统层
        /// </summary>
        void RegisterSystem<S>(S system) where S : ISystem;
        S GetSystem<S>() where S : class, ISystem;
        /// <summary>
        /// 注册Model数据层
        /// </summary>
        void RegisterModel<M>(M model) where M : IModel;
        M GetModel<M>() where M : class, IModel;
        /// <summary>
        /// 注册Utility工具层
        /// </summary>
        void RegisterUtility<U>(U utility) where U : IUtility;
        /// <summary>
        /// 获取Utility工具
        /// 获取API
        /// </summary>
        C GetUtility<C>() where C : class, IUtility;
        /// <summary>
        /// 创建Command，并将Command发送给Architecture
        /// </summary>
        void SendCommand<N>() where N : ICommand, new();
        /// <summary>
        /// 将Command发送给Architecture
        /// </summary>
        void SendCommand<N>(N command) where N : ICommand;
        TResult SendQuery<TResult>(IQuery<TResult> query);
        void SendEvent<E>() where E : new();
        void SendEvent<E>(E e);
        ICancel RegisterEvent<E>(Action<E> onEvent);
        void CancelEvent<E>(Action<E> onEvent);
    }
    /// <summary>
    /// 架构
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Architecture<T> : IArchitecture where T : Architecture<T>, new()
    {
        // 是否初始化完成
        private bool mInited;
        // 缓存要初始化的Model
        private List<IModel> mModelList = new List<IModel>();
        // 缓存要初始化的System
        private List<ISystem> mSystemList = new List<ISystem>();
        // 注册补丁
        public static Action<T> OnRegisterPatch = architecture => { };
        // 类似单例，仅可在内部访问。与单例没有访问限制不同
        private static T mArchitecture;
        public static IArchitecture Interface {
            get {
                if (mArchitecture == null) MakeSureArchitecture();
                return mArchitecture;
            }
        }
        /// <summary>
        /// 确保Container有实例
        /// </summary>
        private static void MakeSureArchitecture()
        {
            if (mArchitecture != null) return;
            mArchitecture = new T();
            mArchitecture.Init();
            //调用
            OnRegisterPatch?.Invoke(mArchitecture);
            //初始化Model，此处先初始化Model，因为Model比System更底层，System是会访问Model的，所以要早于初始化System
            foreach(var model in mArchitecture.mModelList) model.Init();
            //清空Model
            mArchitecture.mModelList.Clear();
            //初始化System，初始化Model之后进行System的初始化
            foreach(var system in mArchitecture.mSystemList) system.Init();
            //清空System
            mArchitecture.mSystemList.Clear();
            mArchitecture.mInited = true;
        }
        /// <summary>
        /// 子类注册模块
        /// </summary>
        protected abstract void Init();
        private IOCContainer mContainer = new IOCContainer();
        /// <summary>
        /// 注册模块API
        /// </summary>
        [UsedImplicitly]
        private static void Register<K>(K instance)
        {
            MakeSureArchitecture();
            mArchitecture.mContainer.Register(instance);
        }
        /// <summary>
        /// 注册System层
        /// 注册API
        /// </summary>
        public void RegisterSystem<S>(S system) where S : ISystem
        {
            //给System赋值
            system.SetArchitecture(this);
            mContainer.Register(system);
            if (!mInited) mSystemList.Add(system);
            else system.Init();
        }
        /// <summary>
        /// 注册Model层
        /// 注册API
        /// </summary>
        public void RegisterModel<M>(M model) where M : IModel
        {
            //给Model赋值
            model.SetArchitecture(this);
            mContainer.Register(model);
            if (!mInited) mModelList.Add(model);
            else model.Init();
        }
        public void RegisterUtility<U>(U utility) where U : IUtility => mContainer.Register(utility);
        /// <summary>
        /// 获取System模块
        /// </summary>
        public S GetSystem<S>() where S : class, ISystem => mContainer.Get<S>();
        /// <summary>
        /// 获取Model模块
        /// </summary>
        public M GetModel<M>() where M : class, IModel => mContainer.Get<M>();
        /// <summary>
        /// 获取Utility模块
        /// </summary>
        public U GetUtility<U>() where U : class, IUtility => mContainer.Get<U>();
        public void SendCommand<N>() where N : ICommand, new()
        {
            var command = new N();
            command.SetArchitecture(this);
            command.Execute();
            // command.SetArchitecture(null);
        }
        public void SendCommand<N>(N command) where N : ICommand
        {
            command.SetArchitecture(this);
            command.Execute();
            // command.SetArchitecture(null);
        }
        public TResult SendQuery<TResult>(IQuery<TResult> query)
        {
            query.SetArchitecture(this);
            return query.Do();
        }
        private readonly ITypeEventSystem mTypeEventSystem = new TypeEventSystem();
        public void SendEvent<E>() where E : new() => mTypeEventSystem.Send<E>();
        public void SendEvent<E>(E e) => mTypeEventSystem.Send<E>(e);
        public ICancel RegisterEvent<E>(Action<E> onEvent) => mTypeEventSystem.Register(onEvent);
        public void CancelEvent<E>(Action<E> onEvent) => mTypeEventSystem.Cancel(onEvent);
    }
    #endregion

    #region Controller
    public interface IController : ICanGetModel, ICanGetSystem, ICanSendCommand, ICanRegisterEvent, ICanSentQuery
    {
    }
    #endregion

    #region System
    public interface ISystem : ICanSetArchitecture, ICanGetModel, ICanGetUtility, ICanRegisterEvent, ICanSendEvent, ICanGetSystem
    {
        /// <summary>
        /// System本身需要有状态，需要有初始化
        /// </summary>
        void Init();
    }
    public abstract class AbstractSystem : ISystem
    {
        private IArchitecture mArchitecture;
        IArchitecture IBelongToArchitecture.GetArchitecture() => mArchitecture;
        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture) => mArchitecture = architecture;
        /// <summary>
        /// 这里使用接口的显式实现，为了隔离功能，限制子类的调用
        /// </summary>
        void ISystem.Init() => OnInit();
        protected abstract void OnInit();
    }
    #endregion

    #region Model
    public interface IModel : ICanSetArchitecture, ICanGetUtility, ICanSendEvent
    {
        void Init();
    }
    public abstract class AbstractModel : IModel
    {
        private IArchitecture mArchitecture;
        IArchitecture IBelongToArchitecture.GetArchitecture() => mArchitecture;
        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture) => mArchitecture = architecture;
        /// <summary>
        /// 这里使用接口的显式实现，为了隔离功能，限制子类的调用
        /// </summary>
        void IModel.Init() { OnInit(); }
        protected abstract void OnInit();
    }
    #endregion

    #region Utility
    public interface IUtility
    {

    }
    #endregion

    #region Command
    public interface ICommand : ICanSetArchitecture, ICanGetModel, ICanGetSystem, ICanSendCommand, ICanGetUtility, ICanSendEvent, ICanSentQuery
    {
        void Execute();
    }
    public abstract class AbstractCommand : ICommand
    {
        private IArchitecture mArchitecture;
        IArchitecture IBelongToArchitecture.GetArchitecture() => mArchitecture;
        void ICanSetArchitecture.SetArchitecture(IArchitecture architecture) => mArchitecture = architecture;
        /// <summary>
        /// 这里使用接口的显式实现，为了隔离功能，限制子类的调用
        /// </summary>
        void ICommand.Execute() => OnExecute();
        protected abstract void OnExecute();
    }
    #endregion

    #region Query
    public interface IQuery<TResult> : ICanSetArchitecture, ICanGetModel, ICanGetSystem, ICanSentQuery
    {
        TResult Do();
    }
    public abstract class AbstractQuery<T> : IQuery<T>
    {
        public T Do() => OnDo();
        protected abstract T OnDo();
        private IArchitecture mArchitecture;
        public void SetArchitecture(IArchitecture architecture) => mArchitecture = architecture;
        public IArchitecture GetArchitecture() => mArchitecture;
    }
    #endregion

    #region Rule
    public interface IBelongToArchitecture
    {
        IArchitecture GetArchitecture();
    }
    public interface ICanSetArchitecture
    {
        void SetArchitecture(IArchitecture architecture);
    }
    public interface ICanGetModel : IBelongToArchitecture
    {
    }
    public static class CanGetModelExtension
    {
        public static M GetModel<M>(this ICanGetModel self) where M : class, IModel => self.GetArchitecture().GetModel<M>();
    }
    public interface ICanGetSystem : IBelongToArchitecture
    {
    }
    public static class CanGetSystemExtension
    {
        public static T GetSystem<T>(this ICanGetModel self) where T : class, ISystem => self.GetArchitecture().GetSystem<T>();
    }
    public interface ICanGetUtility : IBelongToArchitecture
    {
    }
    public static class CanGetUtilityExtension
    {
        public static T GetUtility<T>(this IBelongToArchitecture self) where T : class, IUtility => self.GetArchitecture().GetUtility<T>();
    }
    public interface ICanRegisterEvent : IBelongToArchitecture
    {
    }
    public static class CanRegisterEventExtension
    {
        public static ICancel RegisterEvent<E>(this ICanRegisterEvent self, Action<E> onEvent) => self.GetArchitecture().RegisterEvent<E>(onEvent);
        public static void CancelEvent<E>(this ICanRegisterEvent self, Action<E> onEvent) => self.GetArchitecture().CancelEvent(onEvent);
    }
    public interface ICanSendCommand : IBelongToArchitecture
    {
    }
    public static class CanSendCommandExtension
    {
        public static void SendCommand<T>(this ICanSendCommand self) where T : ICommand, new() => self.GetArchitecture().SendCommand<T>();
        public static void SendCommand<T>(this ICanSendCommand self, T command) where T : ICommand => self.GetArchitecture().SendCommand<T>(command);
    }
    public interface ICanSendEvent : IBelongToArchitecture
    {
    }
    public static class CanSendEventExtension
    {
        public static void SendEvent<E>(this ICanSendEvent self) where E : new() => self.GetArchitecture().SendEvent<E>();
        public static void SendEvent<E>(this ICanSendEvent self, E onEvent) => self.GetArchitecture().SendEvent<E>(onEvent);
    }
    public interface ICanSentQuery : IBelongToArchitecture
    {
    }
    public static class CanSendQueryExtension
    {
        public static TResult SendQuery<TResult>(this ICanSentQuery self, IQuery<TResult> query) => self.GetArchitecture().SendQuery(query);
    }
    #endregion

    #region TypeEventSystem
    public interface ITypeEventSystem
    {
        void Send<T>() where T : new();
        void Send<T>(T e);
        ICancel Register<T>(Action<T> onEvent);
        void Cancel<T>(Action<T> onEvent);
    }
    public interface ICancel
    {
        void Cancel();
    }
    public struct TypeEventSystemCancel<T> : ICancel
    {
        public ITypeEventSystem typeEventSystem;
        public Action<T> OnEvent;
        public void Cancel()
        {
            typeEventSystem.Cancel<T>(OnEvent);
            typeEventSystem = null;
            OnEvent = null;
        }
    }
    public class CancelOnDestroyTrigger : MonoBehaviour
    {
        private HashSet<ICancel> mCancel = new HashSet<ICancel>();
        public void AddCancel(ICancel cancel) => mCancel.Add(cancel);
        private void OnDestroy()
        {
            foreach(var cancel in mCancel) cancel.Cancel();
            mCancel.Clear();
        }
    }
    public static class CancelExtension
    {
        public static void CancelWhenGameObjectDestroy(this ICancel cancel, GameObject go)
        {
            var trigger = go.GetComponent<CancelOnDestroyTrigger>();
            if (!trigger) go.AddComponent<CancelOnDestroyTrigger>();
            else trigger.AddCancel(cancel);
        }
    }
    public class TypeEventSystem : ITypeEventSystem
    {
        public interface IRegistrations
        {
        }
        public class Registrations<T> : IRegistrations
        {
            public Action<T> OnEvent = e => { };
        }
        public readonly Dictionary<Type, IRegistrations> RegistrationsMap = new Dictionary<Type, IRegistrations>();
        public void Send<T>() where T : new()
        {
            var e = new T();
            Send<T>(e);
        }
        public void Send<T>(T e)
        {
            var type = typeof(T);
            if (RegistrationsMap.TryGetValue(type, out var registrations)) ((Registrations<T>)registrations).OnEvent(e);
        }
        public ICancel Register<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            if (!RegistrationsMap.TryGetValue(type, out var registrations))
            {
                registrations = new Registrations<T>();
                RegistrationsMap.Add(type, registrations);
            }
            ((Registrations<T>)registrations).OnEvent += onEvent;
            return new TypeEventSystemCancel<T>()
            {
                OnEvent = onEvent,
                typeEventSystem = this
            };
        }
        public void Cancel<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            if (RegistrationsMap.TryGetValue(type, out var registrations)) ((Registrations<T>)registrations).OnEvent -= onEvent;
        }
    }
    #endregion

    #region IOC
    public class IOCContainer
    {
        private Dictionary<Type, object> mInstance = new Dictionary<Type, object>();
        public void Register<T>(T instance)
        {
            var key = typeof(T);
            if (!mInstance.ContainsKey(key)) mInstance.Add(key, instance);
            else mInstance[key] = instance;
        }
        public T Get<T>() where T : class
        {
            var key = typeof(T);
            if (mInstance.TryGetValue(key, out var retInstance)) return retInstance as T;
            return null;
        }
    }
    #endregion

    #region BindableProperty
    /// <summary>
    /// 数据 + 数据变更事件，节省代码量
    /// </summary>
    public class BindableProperty<T>
    {
        private T mValue;
        public T Value {
            get => mValue;
            set {
                if (value == null && mValue == null || value != null && value.Equals(mValue)) return;
                mValue = value;
                mOnValueChanged?.Invoke(value);
            }
        }
        private Action<T> mOnValueChanged = v => { };
        public BindableProperty() { }
        public BindableProperty(T value) { Value = value; }
        public ICancel Register(Action<T> onValueChanged)
        {
            mOnValueChanged += onValueChanged;
            return new BindablePropertyCancel<T>()
            {
                BindableProperty = this,
                OnValueChanged = onValueChanged
            };
        }
        public void Cancel(Action<T> onValueChanged) => mOnValueChanged -= onValueChanged;
    }
    public class BindablePropertyCancel<T> : ICancel
    {
        public BindableProperty<T> BindableProperty { get; set; }
        public Action<T> OnValueChanged { get; set; }
        public void Cancel()
        {
            BindableProperty.Cancel(OnValueChanged);
            BindableProperty = null;
            OnValueChanged = null;
        }
    }
    #endregion
}