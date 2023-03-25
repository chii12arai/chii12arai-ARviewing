using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class MyStageDirector : MonoBehaviour
{
    // Control options.
    public bool ignoreFastForward = true;

    // Prefabs.
    [SerializeField]
    GameObject musicPlayer;
    public GameObject mainCameraRigPrefab;
    public GameObject[] prefabsNeedsActivation;
    public GameObject[] prefabsOnTimeline;
    float fadespeed = 0.65f;

    // Camera points.
    public Transform[] cameraPoints;

    // Exposed to animator.
    public float overlayIntensity = 1.0f;

    // Objects to be controlled.
    CameraSwitcher mainCameraSwitcher;
    ScreenOverlay[] screenOverlays;
    GameObject[] objectsNeedsActivation;
    GameObject[] objectsOnTimeline;

    void Awake()
    {
        // Instantiate the prefabs.

        //var cameraRig = mainCameraRigPrefab;
        //mainCameraSwitcher = cameraRig.GetComponentInChildren<CameraSwitcher>();
        //screenOverlays = cameraRig.GetComponentsInChildren<ScreenOverlay>();

        objectsNeedsActivation = prefabsNeedsActivation;

        objectsOnTimeline = prefabsOnTimeline;

    }

    void Update()
    {
        if(overlayIntensity > 0) overlayIntensity -= Time.deltaTime * fadespeed;
        //foreach (var so in screenOverlays)
        //{
          //  so.intensity = overlayIntensity;
          //  so.enabled = overlayIntensity > 0.01f;
        //}
    }

    public void StartMusic()
    {
        foreach (var source in musicPlayer.GetComponentsInChildren<AudioSource>())
            source.Play();
    }

    public void ActivateProps()
    {
        foreach (var o in objectsNeedsActivation) o.BroadcastMessage("ActivateProps");
    }

    public void SwitchCamera(int index)
    {
        //if (mainCameraSwitcher)
          //  mainCameraSwitcher.ChangePosition(cameraPoints[index], true);
    }

    public void StartAutoCameraChange()
    {
        //if (mainCameraSwitcher)
          //  mainCameraSwitcher.StartAutoChange();
    }

    public void StopAutoCameraChange()
    {
        //if (mainCameraSwitcher)
          //  mainCameraSwitcher.StopAutoChange();
    }

    public void FastForward(float second)
    {
        if (!ignoreFastForward)
        {
            FastForwardAnimator(GetComponent<Animator>(), second, 0);
            foreach (var go in objectsOnTimeline)
                foreach (var animator in go.GetComponentsInChildren<Animator>())
                    FastForwardAnimator(animator, second, 0.5f);
        }
    }

    void FastForwardAnimator(Animator animator, float second, float crossfade)
    {
        for (var layer = 0; layer < animator.layerCount; layer++)
        {
            var info = animator.GetCurrentAnimatorStateInfo(layer);
            if (crossfade > 0.0f)
                animator.CrossFade(info.fullPathHash, crossfade / info.length, layer, info.normalizedTime + second / info.length);
            else
                animator.Play(info.fullPathHash, layer, info.normalizedTime + second / info.length);
        }
    }

    public void EndPerformance()
    {
        //Application.LoadLevel(0);
        SceneManager.LoadScene(0);
    }
}