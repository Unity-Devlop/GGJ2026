using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityToolkit;

namespace Jump
{
    public class AssetSystem : ISystem, IOnInit
    {
        public void OnInit()
        {
        }

        public void Dispose()
        {
            // TODO release managed resources here
        }

        public GameObject Instantiate(string address)
        {
            var handle = UnityEngine.AddressableAssets.Addressables.InstantiateAsync(address);
            handle.WaitForCompletion();
            if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }

            return null;
        }

        public UniTask<GameObject> InstantiateAsync(string address)
        {
            var handle = UnityEngine.AddressableAssets.Addressables.InstantiateAsync(address);
            return handle.ToUniTask();
        }

        public void Instantiate(string address, Action<GameObject> callback)
        {
            var handle = UnityEngine.AddressableAssets.Addressables.InstantiateAsync(address);
            handle.Completed += op =>
            {
                if (op.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                {
                    callback?.Invoke(op.Result);
                }
                else
                {
                    callback?.Invoke(null);
                }
            };
        }

        public T LoadAsset<T>(string address) where T : UnityEngine.Object
        {
            var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<T>(address);
            handle.WaitForCompletion();
            if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }

            return null;
        }

        public UniTask<T> LoadAssetAsync<T>(string address) where T : UnityEngine.Object
        {
            var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<T>(address);
            return handle.ToUniTask();
        }

        public void ReleaseAsset<T>(T asset) where T : UnityEngine.Object
        {
            UnityEngine.AddressableAssets.Addressables.Release(asset);
        }
    }
}