using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using JetBrains.Annotations;
using Object = UnityEngine.Object;

namespace DSFramework
{
    public class DSTimer : MonoBehaviour
    {
        #region Public Properties/Fields

        /// <summary>
        /// 计时器从开始到完成需要多长时间。
        /// </summary>
        public float duration { get; private set; }

        /// <summary>
        /// 计时器在完成后是否将再次运行。
        /// </summary>
        public bool isLooped { get; set; }

        /// <summary>
        /// 计时器是否完成运行。 如果计时器已取消，则为false。
        /// </summary>
        public bool isCompleted { get; private set; }

        /// <summary>
        /// 计时器使用实时还是游戏时间。 实时时间不受游戏时间尺度变化的影响（例如暂停，慢动作），而游戏时间会受到影响。
        /// </summary>
        public bool usesRealTime { get; private set; }

        /// <summary>
        /// 计时器当前是否暂停。
        /// </summary>
        public bool isPaused => _timeElapsedBeforePause.HasValue;

        /// <summary>
        /// 计时器是否已取消。
        /// </summary>
        public bool isCancelled => _timeElapsedBeforeCancel.HasValue;

        /// <summary>
        /// 计时器是否完成
        /// </summary>
        public bool isDone => isCompleted || isCancelled || isOwnerDestroyed;

        #endregion

        #region Public Static Methods

        /// <summary>
        /// 注册一个新的计时器，该计时器应在经过一定时间后触发一个事件。
        /// 当场景改变时，已注册的计时器将被销毁。
        /// </summary>
        /// <param name="duration">计时器启动之前要等待的时间（以秒为单位），</param>
        /// <param name="onComplete">计时器完成时触发的动作。</param>
        /// <param name="onUpdate">每次更新计时器时应触发的操作。 自计时器电流循环开始以来经过的时间（以秒为单位）。</param>
        /// <param name="isLooped">执行后计时器是否应重复。</param>
        /// <param name="useRealTime">计时器使用的是实时时间（即不受暂停，慢动作/快动作的影响）还是游戏时间（受暂停和慢动作/快动作的影响）。</param>
        /// <param name="autoDestroyOwner">将此计时器附加到的对象。 对象销毁后，计时器将到期且不执行。 这样可以避免烦人</param>
        /// <returns>一个计时器对象，使您可以检查统计信息并停止/恢复进度。</returns>
        public static DSTimer Register(float duration, Action onComplete, Action<float> onUpdate = null,
            bool isLooped = false, bool useRealTime = false, MonoBehaviour autoDestroyOwner = null)
        {
            // 创建一个管理器对象以更新所有计时器（如果尚不存在）。
            if (DSTimer._manager == null)
            {
                TimerManager managerInScene = Object.FindObjectOfType<TimerManager>();
                if (managerInScene != null)
                {
                    DSTimer._manager = managerInScene;
                }
                else
                {
                    GameObject managerObject = new GameObject {name = "TimerManager"};
                    DSTimer._manager = managerObject.AddComponent<TimerManager>();
                }
            }

            DSTimer dsDSTimer =
                new DSTimer(duration, onComplete, onUpdate, isLooped, useRealTime, autoDestroyOwner);
            DSTimer._manager.RegisterTimer(dsDSTimer);
            return dsDSTimer;
        }

        /// <summary>
        /// 取消计时器
        /// </summary>
        public static void Cancel(DSTimer dsDSTimer)
        {
            if (dsDSTimer != null)
            {
                dsDSTimer.Cancel();
            }
        }

        /// <summary>
        /// 暂停计时器
        /// </summary>
        public static void Pause(DSTimer dsDSTimer)
        {
            if (dsDSTimer != null)
            {
                dsDSTimer.Pause();
            }
        }

        /// <summary>
        /// 重置计算器
        /// </summary>
        public static void Resume(DSTimer dsDSTimer)
        {
            if (dsDSTimer != null)
            {
                dsDSTimer.Resume();
            }
        }

        public static void CancelAllRegisteredTimers()
        {
            if (DSTimer._manager != null)
            {
                DSTimer._manager.CancelAllTimers();
            }
        }

        public static void PauseAllRegisteredTimers()
        {
            if (DSTimer._manager != null)
            {
                DSTimer._manager.PauseAllTimers();
            }
        }

        public static void ResumeAllRegisteredTimers()
        {
            if (DSTimer._manager != null)
            {
                DSTimer._manager.ResumeAllTimers();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 停止正在进行或暂停的计时器。 计时器的完成时回调将不会被调用。
        /// </summary>
        public void Cancel()
        {
            if (this.isDone)
            {
                return;
            }

            this._timeElapsedBeforeCancel = this.GetTimeElapsed();
            this._timeElapsedBeforePause = null;
        }

        /// <summary>
        /// 暂停运行计时器。 暂停的计时器可以从暂停的同一点恢复。
        /// </summary>
        public void Pause()
        {
            if (this.isPaused || this.isDone)
            {
                return;
            }

            this._timeElapsedBeforePause = this.GetTimeElapsed();
        }

        /// <summary>
        /// 继续一个暂停的计时器。 如果计时器尚未暂停，则不执行任何操作。
        /// </summary>
        public void Resume()
        {
            if (!this.isPaused || this.isDone)
            {
                return;
            }

            this._timeElapsedBeforePause = null;
        }

        /// <summary>
        /// 获取自此计时器当前周期开始以来已经过的秒数。
        /// </summary>
        /// <returns>
        /// 自计时器当前周期开始以来经过的秒数，即如果计时器已循环则为当前循环，否则为开始。
        ///
        /// 如果计时器已完成运行，则该时间等于持续时间。
        ///
        /// 如果计时器被取消/暂停，则等于计时器启动到取消/暂停之间经过的秒数。
        /// </returns>
        public float GetTimeElapsed()
        {
            if (this.isCompleted || this.GetWorldTime() >= this.GetFireTime())
            {
                return this.duration;
            }

            return this._timeElapsedBeforeCancel ??
                   this._timeElapsedBeforePause ??
                   this.GetWorldTime() - this._startTime;
        }

        /// <summary>
        /// 获取剩余的秒数，直到计时器完成。
        /// </summary>
        public float GetTimeRemaining()
        {
            return this.duration - this.GetTimeElapsed();
        }

        /// <summary>
        /// 获取计时器从开始到结束所取得的进度（作为比率）。
        /// </summary>
        /// <returns>从0到1的值表示计时器的持续时间已过。</returns>
        public float GetRatioComplete()
        {
            return this.GetTimeElapsed() / this.duration;
        }

        /// <summary>
        /// 获得计时器剩余的进度比例。
        /// </summary>
        /// <returns>从0到1的值指示剩余的计时器持续时间。</returns>
        public float GetRatioRemaining()
        {
            return this.GetTimeRemaining() / this.duration;
        }

        #endregion

        #region Private Static Properties/Fields

        // 负责更新所有注册的计时器
        private static TimerManager _manager;

        #endregion

        #region Private Properties/Fields

        private bool isOwnerDestroyed
        {
            get { return this._hasAutoDestroyOwner && this._autoDestroyOwner == null; }
        }

        private readonly Action _onComplete;
        private readonly Action<float> _onUpdate;
        private float _startTime;
        private float _lastUpdateTime;

        //为了暂停，我们将开始时间提前了过去的时间。 
        //如果只检查开始时间与当前世界时间，
        //这将与取消或暂停时所经过的时间混淆，
        //因此我们需要缓存在暂停/取消之前所经过的时间
        private float? _timeElapsedBeforeCancel;
        private float? _timeElapsedBeforePause;

        //自动销毁所有者被销毁后，
        //计时器将以这种方式过期，
        //这样您就不会在计时器销毁后运行并访问对象时遇到任何烦人的错误
        private readonly MonoBehaviour _autoDestroyOwner;
        private readonly bool _hasAutoDestroyOwner;

        #endregion

        #region Private Constructor (使用静态Register方法创建新计时器)

        private DSTimer(float duration, Action onComplete, Action<float> onUpdate,
            bool isLooped, bool usesRealTime, MonoBehaviour autoDestroyOwner)
        {
            this.duration = duration;
            this._onComplete = onComplete;
            this._onUpdate = onUpdate;

            this.isLooped = isLooped;
            this.usesRealTime = usesRealTime;

            this._autoDestroyOwner = autoDestroyOwner;
            this._hasAutoDestroyOwner = autoDestroyOwner != null;

            this._startTime = this.GetWorldTime();
            this._lastUpdateTime = this._startTime;
        }

        #endregion

        #region Private Methods

        private float GetWorldTime()
        {
            return this.usesRealTime ? Time.realtimeSinceStartup : Time.time;
        }

        private float GetFireTime()
        {
            return this._startTime + this.duration;
        }

        private float GetTimeDelta()
        {
            return this.GetWorldTime() - this._lastUpdateTime;
        }

        private void Update()
        {
            if (this.isDone)
            {
                return;
            }

            if (this.isPaused)
            {
                this._startTime += this.GetTimeDelta();
                this._lastUpdateTime = this.GetWorldTime();
                return;
            }

            this._lastUpdateTime = this.GetWorldTime();

            if (this._onUpdate != null)
            {
                this._onUpdate(this.GetTimeElapsed());
            }

            if (this.GetWorldTime() >= this.GetFireTime())
            {
                if (this._onComplete != null)
                {
                    this._onComplete();
                }

                if (this.isLooped)
                {
                    this._startTime = this.GetWorldTime();
                }
                else
                {
                    this.isCompleted = true;
                }
            }
        }

        #endregion

        #region Manager Class (实现细节，自动生成并更新所有已注册的计时器)

        /// <summary>
        /// 首次创建计时器时将实例化该实例-无需手动将其添加到场景中。
        /// </summary>
        private class TimerManager : MonoBehaviour
        {
            private List<DSTimer> _timers = new List<DSTimer>();

            // 缓冲添加计时器，以便我们在迭代期间不编辑集合
            private List<DSTimer> _timersToAdd = new List<DSTimer>();

            public void RegisterTimer(DSTimer dsDSTimer)
            {
                this._timersToAdd.Add(dsDSTimer);
            }

            public void CancelAllTimers()
            {
                foreach (DSTimer timer in this._timers)
                {
                    timer.Cancel();
                }

                this._timers = new List<DSTimer>();
                this._timersToAdd = new List<DSTimer>();
            }

            public void PauseAllTimers()
            {
                foreach (DSTimer timer in this._timers)
                {
                    timer.Pause();
                }
            }

            public void ResumeAllTimers()
            {
                foreach (DSTimer timer in this._timers)
                {
                    timer.Resume();
                }
            }

            // 更新每一帧上所有已注册的计时器
            [UsedImplicitly]
            private void Update()
            {
                this.UpdateAllTimers();
            }

            private void UpdateAllTimers()
            {
                if (this._timersToAdd.Count > 0)
                {
                    this._timers.AddRange(this._timersToAdd);
                    this._timersToAdd.Clear();
                }

                foreach (DSTimer timer in this._timers)
                {
                    timer.Update();
                }

                this._timers.RemoveAll(t => t.isDone);
            }
        }

        #endregion
    }

    public static class TimerExtensions
    {
        /// <summary>
        /// 将计时器附加到该行为。 如果在计时器完成之前行为已被破坏，例如 通过场景更改，计时器回调将不会执行。
        /// </summary>
        public static DSTimer AttachTimer(this MonoBehaviour behaviour, float duration, Action onComplete,
            Action<float> onUpdate = null, bool isLooped = false, bool useRealTime = false)
        {
            return DSTimer.Register(duration, onComplete, onUpdate, isLooped, useRealTime, behaviour);
        }
    }
}