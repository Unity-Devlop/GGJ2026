using System.Numerics;
using UnityToolkit;

namespace Jump
{
    // 基于FMOD的音频系统，打包时放到StreamingAssets目录下 不用AssetBundle
    public class AudioSystem : ISystem, IOnInit
    {
        public enum VolumeType
        {
            BGM,
            SFX,
        }

        public void OnInit()
        {
        }

        public void Dispose()
        {
            // TODO release managed resources here
        }


        public float GetVolume(VolumeType type)
        {
            return 1;
        }

        public void SetVolume(VolumeType type, float volume)
        {
        }

        public bool IsPlayingBGM(string path)
        {
            return false;
        }

        public bool GetBackGroundInstance<T>(string path, out T instance)
        {
            instance = default;
            return false;
        }

        public void PlayBGM(string path)
        {
        }

        public void StopBGM(string path)
        {
        }

        public void PauseBGM(string path)
        {
        }

        public void ResumeBGM(string path)
        {
        }

        public void PlayOneShot(string path)
        {
        }

        public void PlayOneShotAt(string path, Vector3 pos)
        {
        }
    }
}