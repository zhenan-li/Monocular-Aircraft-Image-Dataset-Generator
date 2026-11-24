<h1 align="center">Aircraft Dataset Generator</h1>

<p align="center">
  <a href="./README.zh-CN.md">简体中文</a> · <strong>English</strong>
</p>

<p align="center">
  <a href="../../releases/latest"><strong>Download for Windows x64</strong></a> ·
  <a href="#inputs">Inputs</a> ·
  <a href="#outputs">Outputs</a> ·
  <a href="#performance">Performance</a> ·
  <a href="#applications">Applications</a>
</p>

I built this 3D image dataset generator in Unity and C# in fall 2024 as part of a collaboration with the Department of Computer Science at East China University of Science and Technology on real-time aircraft height and distance estimation from a single camera. It simulates airborne objects and batch-generates images labeled with height and distance, substantially reducing the cost of obtaining training data for computer vision models. I wrote the core algorithm by hand and built the UI with AI assistance.

The aircraft model and terrain are easy to swap in the Unity Editor. The current build includes a passenger aircraft model and a simulated ocean.

## Quick start

1. Download the Windows ZIP from [Releases](../../releases/latest) and extract the entire archive.
2. Run `AircraftDatasetGenerator.exe` and enter the five values below. For a short run, use `200`, `600`, `60`, `2`, and `1`, in that order.
3. Click **Start**. The window closes automatically after the batch.

## Inputs

| Field | What it controls |
| --- | --- |
| Aircraft Height | Fixed aircraft Y coordinate in scene units |
| Aircraft Range | Upper bound for random X and Z coordinates, in scene units |
| Spawn Speed | Number of frames between generation steps |
| Spawn Number | Number of PNGs to save |
| Size Multiplier | Screenshot scale relative to the rendered window. At 1920 × 1080, a value of 2 produces 3840 × 2160. |

All five inputs are positive integers.

## Outputs

Each run saves its PNGs in a timestamped subfolder under:

```text
%USERPROFILE%\AppData\LocalLow\DefaultCompany\AircraftDistantEstimate_DataBaseCreater\GeneratedImages
```

Files are named `test<N>高度<H>距离<D>.png`. `N` is the image index, `H` is the aircraft's world-space Y coordinate, and `D` is its rounded Euclidean distance to the world origin. The scene camera is positioned at `(0, 10, 0)`, so exact optical range requires measuring from the camera position.

## Performance

In a November 2026 Windows retest, the generator wrote **5.15 labeled 4K PNGs per second** at a one-frame interval. Each setting was tested in three batches of 30 images. Timing ran from **Start** to the last PNG write, including rejected poses, rendering, and saving.

| Spawn Speed | Time per 30-image batch | Throughput across all three batches |
| --- | ---: | ---: |
| 1 frame | 5.64–6.01 s | 5.15 images/s |
| 60 frames | 6.35–7.51 s | 4.21 images/s |

The benchmark used a Windows player running in the background, Unity 2021.3.45f1, Direct3D 11, an Intel Core i7-12700H, and an NVIDIA GeForce RTX 3070 Ti Laptop GPU. The render size was 1920 × 1080, with a screenshot multiplier of 2 for 3840 × 2160 output. Aircraft height and horizontal sampling range were set to 200 and 600 scene units, respectively.

## Applications

- Generate supervised data for experiments in monocular aircraft height and range estimation.
- Vary height and horizontal sample range to study how aircraft scale and position affect model behavior.
- Prototype a data pipeline before collecting and annotating real aerial imagery.

## Build from source

Open the repository in **Unity 2021.3.45f1**. Import **Window → TextMeshPro → Import TMP Essential Resources**. For the original scene, restore `Assets/Aircrafts/A320.obj` and `Assets/Standard Assets/Environment/Water (Basic)/` with their matching `.meta` files from a licensed copy of the original assets. The Windows ZIP includes these dependencies as built scene content. [Unity Asset Store terms](https://unity.com/legal/as-terms)

Open [Sea-Sunny-Scene](Assets/Scenes/Sea-Sunny-Scene.unity) to run in the editor or build for Windows x86-64. The included sky assets retain their original license notices. [Dynamic Sky](https://github.com/nvjob/nvjob-sky-shader-simple-and-fast)
