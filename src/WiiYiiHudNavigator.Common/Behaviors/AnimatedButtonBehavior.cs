using CommunityToolkit.Maui.Behaviors;

namespace WiiYiiHudNavigator.Common.Behaviors;

/// <summary>
/// Smooth animated button behavior that provides fluid touch effects
/// while preserving Command functionality.
/// </summary>
public sealed class AnimatedButtonBehavior : TouchBehavior
{
    public AnimatedButtonBehavior()
    {
        // Configure animation settings for smooth button interactions
        DefaultAnimationDuration = 150;
        DefaultAnimationEasing = Easing.Linear;
        
        // Configure pressed animation
		PressedAnimationDuration = 100;
        PressedAnimationEasing = Easing.Linear;
        
        // Configure scale and opacity for pressed state
        PressedScale = 0.95;
        PressedOpacity = 0.8;

		HoveredScale = 1.02;
		HoveredAnimationDuration = 50;

		ShouldMakeChildrenInputTransparent = false;
    }
}