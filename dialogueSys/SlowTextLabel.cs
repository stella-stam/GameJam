using Godot;
using System;

public partial class SlowTextLabel : RichTextLabel
{
    // Signal emitted when the text finishes printing or is skipped
    [Signal]
    public delegate void DialogueFinishedEventHandler();

    [Export] public float CharactersPerSecond { get; set; } = 30.0f;
    [Export] public string SkipAction { get; set; } = "ui_accept"; // Space / Enter by default

    //private AudioStreamPlayer _blipSound;
    private Tween _textTween;
    private bool _isDisplaying = false;

    public override void _Ready()
    {
        // Fetch the sibling AudioStreamPlayer node
        // _blipSound = GetNode<AudioStreamPlayer>("../BlipSound");

        // Start completely empty
        VisibleCharacters = 0;
        Text = "";
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        // If text is currently crawling and player hits skip, instant finish
        if (_isDisplaying && @event.IsActionPressed(SkipAction))
        {
            GetViewport().SetInputAsHandled();
            SkipToEnd();
        }
    }

    /// <summary>
    /// Call this from external scripts to print out a line of text.
    /// </summary>
    public void DisplayText(string newText)
    {
        // Kill any ongoing animation safely
        if (_textTween != null && _textTween.IsValid())
        {
            _textTween.Kill();
        }

        Text = newText;
        VisibleCharacters = 0;
        _isDisplaying = true;

        // GetParsedText() strips out BBCode tags so our math matches visible letters
        int rawTextLength = GetParsedText().Length;
        if (rawTextLength == 0)
        {
            OnTweenFinished();
            return;
        }

        float duration = rawTextLength / CharactersPerSecond;

        // Animate the visible character count linearly
        _textTween = CreateTween();
        _textTween.TweenProperty(this, "visible_characters", rawTextLength, duration)
            .SetTrans(Tween.TransitionType.Linear)
            .SetEase(Tween.EaseType.InOut);

        // Connect C# Actions to the tween steps
        _textTween.StepFinished += OnCharacterRevealed;
        _textTween.Finished += OnTweenFinished;
    }

    private void OnCharacterRevealed(long idx)
    {
        // Trigger sound only if it's a non-space character and audio isn't already overlapping
        // if (VisibleCharacters > 0)
        // {
        //     string parsedText = GetParsedText();
        //     if (VisibleCharacters <= parsedText.Length)
        //     {
        //         char currentChar = parsedText[VisibleCharacters - 1];
        //         if (currentChar != ' ' && _blipSound != null && !_blipSound.Playing)
        //         {
        //             _blipSound.Play();
        //         }
        //     }
        // }
    }

    private void SkipToEnd()
    {
        if (_textTween != null && _textTween.IsValid())
        {
            _textTween.Kill();
        }
        VisibleCharacters = -1; // -1 displays the full string instantly
        OnTweenFinished();
    }

    private void OnTweenFinished()
    {
        // Disconnect events to prevent memory leaks with recycled tweens
        if (_textTween != null)
        {
            _textTween.StepFinished -= OnCharacterRevealed;
            _textTween.Finished -= OnTweenFinished;
        }

        _isDisplaying = false;
        EmitSignal(SignalName.DialogueFinished);
    }
}