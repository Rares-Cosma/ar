using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections;
using System.Collections;
using TMPro;

public class GrassDetector : MonoBehaviour
{
    [Header("AR Camera")]
    public ARCameraManager cameraManager;  // AR camera manager (mobile)

    [Header("Sampling Settings")]
    public int sampleRadius = 5;           // Radius in pixels (screen or AR image)
    public int downsampleFactor = 4;       // Downscale factor for AR image sampling

    [Header("Debug")]
    public Color currentColor;             // Current sampled color
    public LayerMask grassLayer;
    public bool inGrass;
    public TMP_Text testText;
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
#if UNITY_EDITOR
        // Editor: sample screen pixels at center
        StartCoroutine(SampleScreenCenterColor());
#else
        // Mobile AR: sample AR camera feed
        if (cameraManager != null)
        {
            SampleARCameraColor();
        }
#endif
        RaycastHit hit;
        if(Physics.Raycast(transform.position,-Vector3.up,out hit,2f, grassLayer))
        {
            inGrass = true;
            testText.text = "Pe Iarba";
        }
        else
        {
            inGrass = false;
            testText.text = "Nu Pe Iarba";
        }
    }

    // --- Editor / Desktop: sample screen pixels at center ---
    IEnumerator SampleScreenCenterColor()
    {
        yield return new WaitForEndOfFrame();

        int centerX = Screen.width / 2;
        int centerY = Screen.height / 2;
        int size = sampleRadius * 2 + 1;

        Texture2D tex = new Texture2D(size, size, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(centerX - sampleRadius, centerY - sampleRadius, size, size), 0, 0);
        tex.Apply();

        Color sum = Color.black;
        int count = 0;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                sum += tex.GetPixel(x, y);
                count++;
            }
        }

        currentColor = sum / count;

        Destroy(tex);
    }

    // --- Mobile AR: sample color from ARCameraManager ---
    void SampleARCameraColor()
    {
        if (!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
            return;

        using (image)
        {
            var conversionParams = new XRCpuImage.ConversionParams
            {
                inputRect = new RectInt(0, 0, image.width, image.height),
                outputDimensions = new Vector2Int(image.width / downsampleFactor, image.height / downsampleFactor),
                outputFormat = TextureFormat.RGB24,
                transformation = XRCpuImage.Transformation.MirrorY
            };

            int size = image.GetConvertedDataSize(conversionParams);
            var buffer = new NativeArray<byte>(size, Allocator.Temp);
            image.Convert(conversionParams, buffer);

            int width = conversionParams.outputDimensions.x;
            int height = conversionParams.outputDimensions.y;
            int bytesPerPixel = 3;

            int centerX = width / 2;
            int centerY = height / 2;

            int rSum = 0, gSum = 0, bSum = 0, count = 0;

            for (int y = -sampleRadius; y <= sampleRadius; y++)
            {
                for (int x = -sampleRadius; x <= sampleRadius; x++)
                {
                    if (x * x + y * y > sampleRadius * sampleRadius) continue;

                    int px = centerX + x;
                    int py = centerY + y;
                    if (px < 0 || py < 0 || px >= width || py >= height) continue;

                    int index = (py * width + px) * bytesPerPixel;
                    byte r = buffer[index];
                    byte g = buffer[index + 1];
                    byte b = buffer[index + 2];

                    rSum += r;
                    gSum += g;
                    bSum += b;
                    count++;
                }
            }

            buffer.Dispose();

            if (count > 0)
            {
                currentColor = new Color(rSum / (255f * count), gSum / (255f * count), bSum / (255f * count));
            }
        }
    }
}
