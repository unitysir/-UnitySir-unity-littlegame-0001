using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DSFramework.Example
{
    public class TestTimerBehaviour : DSWindowBase
    {
        #region Unity Inspector Fields

        [Header("Controls")] public InputField DurationField;

        public Button StartTimerButton;
        public Button CancelTimerButton;
        public Button PauseTimerButton;
        public Button ResumeTimerButton;

        public Toggle IsLoopedToggle;
        public Toggle UseGameTimeToggle;

        public Slider TimescaleSlider;

        public Text NeedsRestartText;

        [Header("Stats Texts")] public Text TimeElapsedText;
        public Text TimeRemainingText;
        public Text PercentageCompletedText;
        public Text PercentageRemainingText;

        public Text NumberOfLoopsText;
        public Text IsCancelledText;
        public Text IsCompletedText;
        public Text IsPausedText;
        public Text IsDoneText;
        public Text UpdateText;

        #endregion

        public override bool isInit { get; set; }

        public override void InitCmpts()
        {
            base.InitCmpts();
            StartTimerButton = DSCmpt<Button>("StartTimerButton");
            CancelTimerButton = DSCmpt<Button>("CancelTimerButton");
            PauseTimerButton = DSCmpt<Button>("PauseTimerButton");
            ResumeTimerButton = DSCmpt<Button>("ResumeTimerButton");

            IsLoopedToggle = DSCmpt<Toggle>("IsLooped");
            UseGameTimeToggle = DSCmpt<Toggle>("UseGameTime");

            TimescaleSlider = DSCmpt<Slider>("TimescaleSlider");
            NeedsRestartText = DSCmpt<Text>("RestartText");

            TimeElapsedText = DSCmpt<Text>("TimeElapsed");
            TimeRemainingText = DSCmpt<Text>("TimeRemaining");
            PercentageCompletedText = DSCmpt<Text>(" PercentageCompleted");
            PercentageRemainingText = DSCmpt<Text>("PercentageRemainingT");
            NumberOfLoopsText = DSCmpt<Text>("NumberOfLoops");
            IsCancelledText = DSCmpt<Text>("IsCancelled");
            IsCompletedText = DSCmpt<Text>("IsCompleted");
            IsPausedText = DSCmpt<Text>("IsPaused");
            IsDoneText = DSCmpt<Text>("IsDone");
            UpdateText = DSCmpt<Text>("Update");
        }


        private int _numLoops;
        private DSTimer _testDsdsDSTimer;

        private void Awake()
        {
            ResetState();
        }

        private void ResetState()
        {
            _numLoops = 0;
            CancelTestTimer();
        }

        public void StartTestTimer()
        {
            ResetState();

            // 这是重要的代码示例位，我们在其中注册了一个新计时器
            _testDsdsDSTimer = DSTimer.Register(
                GetDurationValue(),
                () => _numLoops++,
                secondsElapsed => { UpdateText.text = $"Timer ran update callback: {secondsElapsed:F2} seconds"; },
                IsLoopedToggle.isOn,
                !UseGameTimeToggle.isOn);

            CancelTimerButton.interactable = true;
        }

        public void CancelTestTimer()
        {
            DSTimer.Cancel(_testDsdsDSTimer);
            CancelTimerButton.interactable = false;
            NeedsRestartText.gameObject.SetActive(false);
        }

        public void PauseTestTimer()
        {
            DSTimer.Pause(_testDsdsDSTimer);
        }

        public void ResumeTestTimer()
        {
            DSTimer.Resume(_testDsdsDSTimer);
        }

        private void Update()
        {
            if (_testDsdsDSTimer == null)
            {
                return;
            }

            Time.timeScale = TimescaleSlider.value;
            _testDsdsDSTimer.isLooped = IsLoopedToggle.isOn;

            TimeElapsedText.text = $"已用时间: {_testDsdsDSTimer.GetTimeElapsed():F2} seconds";
            TimeRemainingText.text = $"剩余时间: {_testDsdsDSTimer.GetTimeRemaining():F2} seconds";
            PercentageCompletedText.text = $"完成百分比: {_testDsdsDSTimer.GetRatioComplete() * 100:F4}%";
            PercentageRemainingText.text = $"剩余百分比: {_testDsdsDSTimer.GetRatioRemaining() * 100:F4}%";
            NumberOfLoopsText.text = $"循环次数: {_numLoops}";
            IsCancelledText.text = $"是否取消: {_testDsdsDSTimer.isCancelled}";
            IsCompletedText.text = $"是否执行: {_testDsdsDSTimer.isCompleted}";
            IsPausedText.text = $"是否暂停: {_testDsdsDSTimer.isPaused}";
            IsDoneText.text = $"是否完成: {_testDsdsDSTimer.isDone}";

            PauseTimerButton.interactable = !_testDsdsDSTimer.isPaused;
            ResumeTimerButton.interactable = _testDsdsDSTimer.isPaused;

            NeedsRestartText.gameObject.SetActive(ShouldShowRestartText());
        }

        private bool ShouldShowRestartText()
        {
            var isOn = UseGameTimeToggle.isOn;
            return !_testDsdsDSTimer.isDone && // 计时器正在进行中...
                   (isOn && _testDsdsDSTimer.usesRealTime || // 切换到实时
                    !isOn && !_testDsdsDSTimer.usesRealTime || // 切换到游戏时间
                    Mathf.Abs(GetDurationValue() - _testDsdsDSTimer.duration) >= Mathf.Epsilon); // 时间改变
        }

        /// <summary>
        /// 获取工期值
        /// </summary>
        /// <returns></returns>
        private float GetDurationValue()
        {
            return float.TryParse(DurationField.text, out var duration) ? duration : 0;
        }
    }
}