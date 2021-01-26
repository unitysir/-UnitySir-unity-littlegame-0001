using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DSFramework
{
    public class AudioComponent : DSComponent
    {
        private AudioSource bkMusic = null;

        private float bkValue = 1;

        private GameObject soundObj = null;

        private List<AudioSource> soundList = new List<AudioSource>();

        private float soundValue = 1;

        private string bgDir;
        private string soundDir;

        /// <summary>
        /// 只用设置一次即可
        /// </summary>
        /// <param name="background"></param>
        /// <param name="sound"></param>
        public void SetAudioDir(string background, string sound)
        {
            bgDir = background;
            soundDir = sound;
        }

        public override void InitCmpts()
        {
            base.InitCmpts();
            DSEntity.Mono.AddUpdate("AudioUpdate", OnUpdate);
        }

        private void OnUpdate()
        {
            for (int i = soundList.Count - 1; i >= 0; --i)
            {
                if (!soundList[i].isPlaying)
                {
                    Destroy(soundList[i]);
                    soundList.RemoveAt(i);
                }
            }
        }

        public void PlayBgMusic(string name)
        {
            if (bkMusic == null)
            {
                GameObject obj = new GameObject {name = "BkMusic"};
                bkMusic = obj.AddComponent<AudioSource>();
            }

            DSEntity.Resource.LoadResAsync<AudioClip>(bgDir + name, (clip) =>
            {
                bkMusic.clip = clip;
                bkMusic.loop = true;
                bkMusic.volume = bkValue;
                bkMusic.Play();
            });
        }

        public void PauseBgMusic()
        {
            if (bkMusic == null)
                return;
            bkMusic.Pause();
        }

        public void StopBgMusic()
        {
            if (bkMusic == null)
                return;
            bkMusic.Stop();
        }

        public void ChangeBgValue(float v)
        {
            bkValue = v;
            if (bkMusic == null)
                return;
            bkMusic.volume = bkValue;
        }

        public void PlaySound(string name, bool isLoop, UnityAction<AudioSource> callBack = null)
        {
            if (soundObj == null)
            {
                soundObj = new GameObject {name = "Sound"};
            }

            DSEntity.Resource.LoadResAsync<AudioClip>(soundDir + name, (clip) =>
            {
                AudioSource source = soundObj.AddComponent<AudioSource>();
                source.clip = clip;
                source.loop = isLoop;
                source.volume = soundValue;
                source.Play();
                soundList.Add(source);
                callBack?.Invoke(source);
            });
        }

        public void ChangeSoundValue(float value)
        {
            soundValue = value;
            for (int i = 0; i < soundList.Count; ++i)
                soundList[i].volume = value;
        }

        public void StopSound(AudioSource source)
        {
            if (soundList.Contains(source))
            {
                soundList.Remove(source);
                source.Stop();
                GameObject.Destroy(source);
            }
        }

        public override void ShutDown()
        {
            soundList.Clear();
            soundList = null;
        }
    }
}