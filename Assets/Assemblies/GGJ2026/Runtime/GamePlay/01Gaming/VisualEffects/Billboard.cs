namespace GGJ2026.GamePlay
{
    using UnityEngine;

    public class Billboard : MonoBehaviour
    {
        public enum BillboardType
        {
            LookAtPosition, // 盯着相机的位置（会随相机高低产生俯仰角）
            CameraForward, // 对齐相机的正方向（无论高低，始终保持垂直平行）
            AxisY // 只绕 Y 轴旋转（类似森林里的树木效果）
        }

        [Header("设置")] public BillboardType billboardType = BillboardType.CameraForward;
        public bool useMainCamera = true;
        public Camera targetCamera;

        [Header("旋转微调")] [Tooltip("是否反向面向相机（有些模型初始背对相机）")]
        public bool flip = false;

        private Transform _camTransform;

        void Start()
        {
            if (useMainCamera)
            {
                if (Camera.main != null)
                    _camTransform = Camera.main.transform;
            }
            else if (targetCamera != null)
            {
                _camTransform = targetCamera.transform;
            }
        }

        // 使用 LateUpdate 确保在相机移动之后再更新旋转，防止抖动
        void LateUpdate()
        {
            if (_camTransform == null) return;

            switch (billboardType)
            {
                case BillboardType.LookAtPosition:
                    // 逻辑 1：LookAt 目标点
                    Vector3 targetPos = _camTransform.position;
                    if (flip)
                    {
                        // 计算反向位置
                        targetPos = transform.position + (transform.position - _camTransform.position);
                    }

                    transform.LookAt(targetPos);
                    break;

                case BillboardType.CameraForward:
                    // 逻辑 2：直接对齐相机的朝向
                    // 这是 UI 或 2D 效果最平滑的方式
                    if (flip)
                        transform.forward = -_camTransform.forward;
                    else
                        transform.forward = _camTransform.forward;
                    break;

                case BillboardType.AxisY:
                    // 逻辑 3：只绕 Y 轴转动
                    Vector3 direction = _camTransform.position - transform.position;
                    direction.y = 0; // 抹掉高度差

                    if (direction != Vector3.zero)
                    {
                        Quaternion rotation = Quaternion.LookRotation(flip ? direction : -direction);
                        transform.rotation = rotation;
                    }

                    break;
            }
        }
    }
}