using Microsoft.Maui.Controls;

namespace WiiYiiHudNavigator.Common.Behaviors;

/// <summary>
/// Simple manual button animation behavior using TapGestureRecognizer and Button events
/// </summary>
public class ManualButtonAnimationBehavior : Behavior<Button>
{
    private Button? _button;
    private TapGestureRecognizer? _tapGestureRecognizer;
    private bool _isAnimating = false;

    protected override void OnAttachedTo(Button bindable)
    {
        base.OnAttachedTo(bindable);
        _button = bindable;
        
        // Add a tap gesture recognizer for manual animation control
        _tapGestureRecognizer = new TapGestureRecognizer();
        _tapGestureRecognizer.Tapped += OnTapped;
        
        _button.GestureRecognizers.Add(_tapGestureRecognizer);
        
        // Add pointer pressed/released events if available
        _button.Pressed += OnPressed;
        _button.Released += OnReleased;
        
        System.Diagnostics.Debug.WriteLine($"ManualButtonAnimationBehavior attached to: {_button.Text}");
    }

    protected override void OnDetachingFrom(Button bindable)
    {
        if (_button != null)
        {
            _button.Pressed -= OnPressed;
            _button.Released -= OnReleased;
            
            if (_tapGestureRecognizer != null)
            {
                _button.GestureRecognizers.Remove(_tapGestureRecognizer);
                _tapGestureRecognizer.Tapped -= OnTapped;
            }
        }
        
        _button = null;
        _tapGestureRecognizer = null;
        base.OnDetachingFrom(bindable);
    }

    private async void OnPressed(object? sender, EventArgs e)
    {
        if (_button != null && !_isAnimating)
        {
            System.Diagnostics.Debug.WriteLine($"Button pressed: {_button.Text}");
            await AnimatePressed();
        }
    }

    private async void OnReleased(object? sender, EventArgs e)
    {
        if (_button != null && !_isAnimating)
        {
            System.Diagnostics.Debug.WriteLine($"Button released: {_button.Text}");
            await AnimateReleased();
        }
    }

    private async void OnTapped(object? sender, TappedEventArgs e)
    {
        if (_button != null && !_isAnimating)
        {
            System.Diagnostics.Debug.WriteLine($"Button tapped: {_button.Text}");
            // Quick press and release animation
            _isAnimating = true;
            await AnimatePressed();
            await Task.Delay(100);
            await AnimateReleased();
            _isAnimating = false;
        }
    }

    private async Task AnimatePressed()
    {
        if (_button != null)
        {
            System.Diagnostics.Debug.WriteLine($"Animating pressed state for: {_button.Text}");
            var scaleTask = _button.ScaleTo(0.85, 150, Easing.CubicInOut);
            var opacityTask = _button.FadeTo(0.7, 150, Easing.CubicInOut);
            await Task.WhenAll(scaleTask, opacityTask);
        }
    }

    private async Task AnimateReleased()
    {
        if (_button != null)
        {
            System.Diagnostics.Debug.WriteLine($"Animating released state for: {_button.Text}");
            var scaleTask = _button.ScaleTo(1.0, 150, Easing.CubicOut);
            var opacityTask = _button.FadeTo(1.0, 150, Easing.CubicOut);
            await Task.WhenAll(scaleTask, opacityTask);
        }
    }
}