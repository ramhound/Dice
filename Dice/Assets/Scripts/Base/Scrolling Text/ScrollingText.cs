using UnityEngine;
using System.Collections;
using System.Text;

//[ExecuteInEditMode]
public class ScrollingText : MonoBehaviour {
    /// <summary>
    /// The underlying TextMesh that will be used to display our text
    /// </summary>
    private TextMesh textMesh;

    /// <summary>
    /// The builder for appending the chars
    /// </summary>
    private StringBuilder builder;

    /// <summary>
    /// The characters that make up either the text from the textFile
    /// of from the supplied text on the mesh itself
    /// </summary>
    private char[] chars;

    private float animSpeed;

    /// <summary>
    /// Used internally for the properties
    /// </summary>
    private TextAsset _textFile;
    private float _scrollSpeed;
    private bool _isScrolling;
    private bool shouldScroll;

    /// <summary>
    /// The text file used is setting this objects text 
    /// </summary>
    public TextAsset textFile;

    /// <summary>
    /// Scrolling speed will control how fast the text scrolls across the screen.
    /// The number supplied shou8ld be between 0f - 1f; 1 being instant display;
    /// </summary>
    public float scrollSpeed;

    /// <summary>
    /// Should the text start scrolling upon being created.
    /// </summary>
    public bool scrollOnStart;

    public delegate void ScrollingTextDelegate(ScrollingText scrollingText);
    public ScrollingTextDelegate onScrollStart;
    public ScrollingTextDelegate onScrollFinish;

    public bool isScrolling { get { return _isScrolling; } }

    // Use this for initialization
    void Start() {
        textMesh = gameObject.GetComponent<TextMesh>();
        Refresh();

        Mathf.Clamp(scrollSpeed, 0f, 1f);
        _scrollSpeed = scrollSpeed;
        animSpeed = 1f - scrollSpeed;

        if(scrollOnStart)
            StartScroll();
    }

    // Update is called once per frame
    void Update() {
        if(textFile != _textFile)
            Refresh();
        if(scrollSpeed != _scrollSpeed) {
            Mathf.Clamp(scrollSpeed, 0f, 1f);
            _scrollSpeed = scrollSpeed;
            animSpeed = 1f - scrollSpeed;
        }
    }

    private IEnumerator ScrollText() {
        for(int i = 0; i < chars.Length; i++) {
            if(shouldScroll) {
                builder.Append(chars[i]);
                textMesh.text = builder.ToString();
                yield return new WaitForSeconds(animSpeed);
            } else break;
        }
        ScrollFinished();
        yield break;
    }

    private void Refresh() {
        if(textFile == null) {
            if(textMesh.text == "")
                chars = "Hello World".ToCharArray();
            else chars = textMesh.text.ToCharArray();
        } else {
            _textFile = textFile;
            chars = textFile.text.ToCharArray();
        }
        textMesh.text = "";
        builder = new StringBuilder(chars.Length);
    }

    /// <summary>
    /// Starts the scrolling of the text
    /// </summary>
    public void StartScroll() {
        shouldScroll = true;
        StartCoroutine(ScrollText());
        _isScrolling = true;
        if(onScrollStart != null)
            onScrollStart(this);
    }

    /// <summary>
    /// Stops the scrolling and instantly displays the text.
    /// This is good if you want to skip the scrolling of text
    /// by clicking or a button press.
    /// </summary>
    public void StopScroll() {
        _isScrolling = false;
        shouldScroll = false;
        textMesh.text = textFile.text;
    }

    private void ScrollFinished() {
        if(onScrollFinish != null)
            onScrollFinish(this);
    }
}
