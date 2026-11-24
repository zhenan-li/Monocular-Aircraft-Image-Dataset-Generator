<h1 align="center">飞机单目测距数据集生成器</h1>

<p align="center">
  <strong>简体中文</strong> · <a href="./README.md">English</a>
</p>

<p align="center">
  <a href="../../releases/latest"><strong>下载 Windows x64 版</strong></a> ·
  <a href="#输入">输入</a> ·
  <a href="#输出">输出</a> ·
  <a href="#性能">性能</a> ·
  <a href="#应用">应用</a>
</p>

这是我在2024年秋和华东理工大学计算机系的一个合作项目“基于单目摄像头的飞行器实时测高测距”中，用 Unity 和 C# 实现的3d图像数据集生成器。它通过仿真空中的飞行物，批量生成带高度和距离标签的图像，极大减少了获取视觉模型训练数据的成本。核心算法由我手写，用户界面借助 AI 制作。

软件中的飞机模型，地形都可以在editor中方便地更换。目前版本内置一架客机模型和仿真海面。

## 快速开始

1. 从 [Releases](../../releases/latest) 下载 Windows ZIP 并完整解压。
2. 运行 `AircraftDatasetGenerator.exe`。试跑时可按界面顺序填入 `200`、`600`、`60`、`2`、`1`。
3. 点击 **Start**。本批生成结束后，窗口会自动关闭。

## 输入

| 界面字段 | 控制内容 |
| --- | --- |
| Aircraft Height | 飞机固定的世界坐标 Y 值，使用场景单位 |
| Aircraft Range | 飞机随机 X、Z 坐标的上限，使用场景单位 |
| Spawn Speed | 两次生成步骤之间的帧数 |
| Spawn Number | 要保存的 PNG 张数 |
| Size Multiplier | 相对于渲染窗口的截图倍率。窗口为 1920 × 1080 时，倍率 2 输出 3840 × 2160。 |

五项都填正整数。

## 输出

每次运行会在以下位置创建按 UTC 时间命名的目录，并保存 PNG 图像：

```text
%USERPROFILE%\AppData\LocalLow\DefaultCompany\AircraftDistantEstimate_DataBaseCreater\GeneratedImages
```

文件名格式为 `test<N>高度<H>距离<D>.png`。其中 `N` 是图片编号，`H` 是飞机的世界坐标 Y 值，`D` 是飞机到世界原点的直线距离取整。场景相机位于 `(0, 10, 0)`，精确的相机到飞机距离应以相机位置为起点计算。

## 性能

2026 年 11 月在 Windows 上复测，生成间隔为 1 帧时，实测吞吐量为 **5.15 张带标签的 4K PNG／秒**。每档参数各跑 3 批，每批 30 张。从触发 **Start** 到最后一张 PNG 写入完成计时，包含随机姿态筛选、渲染和保存。

| Spawn Speed | 每批 30 张耗时 | 三批合计吞吐量 |
| --- | ---: | ---: |
| 1 帧 | 5.64–6.01 秒 | 5.15 张／秒 |
| 60 帧 | 6.35–7.51 秒 | 4.21 张／秒 |

测试使用后台运行的 Windows 版，环境为 Unity 2021.3.45f1、Direct3D 11、Intel Core i7-12700H 和 NVIDIA GeForce RTX 3070 Ti Laptop GPU。渲染分辨率为 1920 × 1080，截图倍率为 2，输出 3840 × 2160。飞机高度和水平采样范围分别为 200 和 600 场景单位。

## 应用

- 为飞机单目测高和测距实验生成带监督标签的数据。
- 改变高度和水平采样范围，研究目标尺寸、画面位置对模型表现的影响。
- 在采集和标注实拍图像前，验证训练数据的输入和标签流程。

## 从源码构建

用 **Unity 2021.3.45f1** 打开仓库，在 **Window → TextMeshPro → Import TMP Essential Resources** 导入字体资源。原场景需要从已获许可的原版素材中恢复 `Assets/Aircrafts/A320.obj` 和 `Assets/Standard Assets/Environment/Water (Basic)/`，并保留对应的 `.meta` 文件。Windows ZIP 已将这些依赖打包进场景。[Unity Asset Store 条款](https://unity.com/legal/as-terms)

打开 [Sea-Sunny-Scene](Assets/Scenes/Sea-Sunny-Scene.unity)，即可在编辑器中运行或构建 Windows x86-64 版。仓库中的天空素材保留了原许可证。[Dynamic Sky](https://github.com/nvjob/nvjob-sky-shader-simple-and-fast)
