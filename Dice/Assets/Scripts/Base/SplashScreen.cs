using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;

public class SplashScreen : MonoBehaviour {
    private Color transparent = new Color(1f, 1f, 1f, 0f);
    private float finishedTime = 0f;
    private bool startTracking = false;
    private volatile bool operationCompleted;
    private volatile object operationData;

    public static SplashScreen instance { get; private set; }
    public Thread concurrentThread { get; private set; }
    public Transform[] children { get; private set; }
    public delegate void SplashDelegate();
    public delegate void OperationDelegate(object data);
    public SplashDelegate onSplashStart;
    public SplashDelegate onSplashFinished;
    public SplashDelegate onSplashFadeInFinished;
    public SplashDelegate onSplashFadeOutFinished;
    public OperationDelegate onStartOperationStarted;
    public OperationDelegate onStartOperationFinished;
    public bool disabled;
    public bool fadeIn = true;
    public bool fadeOut = true;
    public bool fadeBackground = true;
    public ConcurrentOperation startingOperation;
    public bool destroyOnFinish;
    public bool loadObjectOnFinish;
    public string objectToLoad = "Main Menu";
    public float fadeInTime = 2f;
    public float fadeOutTime = 2f;
    public float idleTime = 2f;

    void Awake() {
        instance = this;
        transform.position = Vector3.zero;
    }

    void Start() {
        if(startingOperation.className != String.Empty && startingOperation.isThreaded) {
            concurrentThread = new Thread(StartOperation);
            concurrentThread.Name = "Splash Screen Thread";
            concurrentThread.IsBackground = true;
            if(onStartOperationStarted != null)
                onStartOperationStarted(this);
            concurrentThread.Start();
		} else if(startingOperation.className != String.Empty) {
            if(onStartOperationStarted != null)
                onStartOperationStarted(this);
            StartOperation();
        }
        if(disabled) {
            SplashDone();
            if(startingOperation != null && !startingOperation.isThreaded) //may need to stay alive to signal when the start operation is finished when threading
                enabled = false;
            return;
        }

        children = GetComponentsInChildren<Transform>();

        if(fadeIn) FadeIn();
        else {
            finishedTime = Time.time + idleTime;
            startTracking = true;
        }

        if(onSplashStart != null)
            onSplashStart();
    }

    private void StartOperation() {
        Type type = Type.GetType(startingOperation.className);
        if(type != null) {
            MethodInfo method = type.GetMethod(startingOperation.method);
            if(method != null) {
                ParameterInfo[] parms = method.GetParameters();
                object instance = null;
                if(!method.IsStatic) {
                    if(typeof(MonoBehaviour).IsAssignableFrom(type)) Debug.LogWarning("It is highly recommended to call static methods if the type is a MonoBehaiour." +
                        " Calling the constructor of this type is unsupported in unity.");
                    instance = Activator.CreateInstance(type, null);
                }
                if(parms.Length == 0) operationData = method.Invoke(instance, null);
                else operationData = method.Invoke(instance, new object[1] { startingOperation.argument });
            } else throw new Exception("No method by that name found. Method name: " + startingOperation.method);
        } else throw new Exception("No class by that name found. class name: " + startingOperation.className);
        operationCompleted = true;
        concurrentThread = null;

        if((disabled && !startingOperation.isThreaded) || !startingOperation.isThreaded)
            OperationFinished();
    }

    void Update() {
        //this one is only used when threading the start operation
        if(operationCompleted)
            OperationFinished();

        if(startTracking && Time.time >= finishedTime) {
            startTracking = false;

            if(fadeOut) FadeOut();
            else SplashDone();

            if(startingOperation != null && !startingOperation.isThreaded) enabled = false; //may need to stay alive to signal when the start operation is finished when threading
        }
    }

    private void FadeIn() {
        for(int i = 0; i < children.Length; i++) {
            if(!fadeBackground && children[i].name.Equals("Background")) continue;
            if(i != children.Length - 1) iTween.ColorFrom(children[i].gameObject, iTween.Hash(
                "color", transparent,
                "time", fadeInTime,
                "easetype", iTween.EaseType.easeInQuad,
                "includechildren", false));
            else iTween.ColorFrom(children[i].gameObject, iTween.Hash(
                "color", transparent,
                "time", fadeInTime,
                "easetype", iTween.EaseType.easeInQuad,
                "includechildren", false,
                "oncomplete", "FadeInDone",
                "oncompletetarget", gameObject));
        }
    }

    public void FadeInDone() {
        if(onSplashFadeInFinished != null)
            onSplashFadeInFinished();
        startTracking = true;
        finishedTime = Time.time + idleTime;
    }

    private void FadeOut() {
        for(int i = 0; i < children.Length; i++) {
            if(!fadeBackground && children[i].name.Equals("Background")) continue;
            if(i != children.Length - 1) iTween.ColorTo(children[i].gameObject, iTween.Hash(
                "color", transparent,
                "time", fadeOutTime,
                "easetype", iTween.EaseType.easeOutQuad,
                "includechildren", false));
            else iTween.ColorTo(children[i].gameObject, iTween.Hash(
                "color", transparent,
                "time", fadeOutTime,
                "easetype", iTween.EaseType.easeOutQuad,
                "includechildren", false,
                "oncomplete", "FadeOutDone",
                "oncompletetarget", gameObject));
        }
    }

    public void FadeOutDone() {
        if(onSplashFadeOutFinished != null)
            onSplashFadeOutFinished();
        SplashDone();
    }

    private void SplashDone() {
        if(onSplashFinished != null)
            onSplashFinished();
        enabled = false;
        if(loadObjectOnFinish) SOS.CreateObjectAt(objectToLoad, Vector2.zero);
        if(destroyOnFinish) Destroy(gameObject);
    }

    private void OperationFinished() {
#if UNITY_EDITOR
        if(!EditorApplication.isPlaying)
            return;
#endif
        operationCompleted = false;
        concurrentThread = null;
        if(onStartOperationFinished != null)
            onStartOperationFinished(operationData);
    }

    [System.Serializable]
    public sealed class ConcurrentOperation {
        public string className;
        public string method;
        public string argument;
        public bool isThreaded;
    }
}