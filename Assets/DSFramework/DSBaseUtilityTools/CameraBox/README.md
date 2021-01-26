# 自制相机插件

## 2020年08月05日 10:29:51

将 prefabs 目录中的 CameraBox 拖入场景即可

```C#
            CameraCtrl.Instance.SetCameraRotate(1); //相机旋转
            CameraCtrl.Instance.SetCameraZoom(1, 10f, -6f, -2f); // 相机缩放
            CameraCtrl.Instance.SetCameraUpAndDown(1); 	// 相机高度
```

