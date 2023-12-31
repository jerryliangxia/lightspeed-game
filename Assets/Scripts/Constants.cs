using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    // Color Emission
    private static readonly Color CosmicRed = new (0.8f, 0.0f, 0.2f);
    private static readonly Color CosmicOrange = new (1.0f, 0.5f, 0.2f);
    private static readonly Color CosmicYellow = new (1.0f, 0.8f, 0.2f);
    private static readonly Color CosmicGreen = new (0.0f, 0.4f, 0.2f);
    private static readonly Color CosmicBlue = new (0.2f, 0.3f, 0.8f);
    private static readonly Color CosmicTeal = new (0.0f, 0.5f, 0.5f);
    private static readonly Color CosmicCyan = new (0.2f, 0.8f, 0.8f);
    private static readonly Color CosmicPink = new (1.0f, 0.4f, 0.6f);
    private static readonly Color CosmicMagenta = new (0.8f, 0.2f, 0.8f);
    private static readonly Color CosmicPurple = new (0.5f, 0.0f, 0.5f);

    public const float InvincibleDuration = 10.0f;
    public const float ColorFlashDuration = 0.1f; // Duration for each color change
    public const float Scale = 1f;
    
    public static readonly List<Color[]> Colors = new()
    {
        new[] { CosmicRed, CosmicOrange },
        new[] { CosmicOrange, CosmicYellow },
        new[] { CosmicYellow, CosmicGreen },
        new[] { CosmicGreen, CosmicBlue },
        new[] { CosmicBlue, CosmicTeal },
        new[] { CosmicTeal, CosmicCyan },
        new[] { CosmicCyan, CosmicPink },
        new[] { CosmicPink, CosmicMagenta },
        new[] { CosmicMagenta, CosmicPurple }
    };

    // Game Controller
    public const int MaxLevelCapForIncrease = 8;
    public const float EmissionRateDecrease = 0.0375f;
    public const float StartEmissionRate = 0.4f;
    
    // High Score
    public const string HighScore = "HighScore";
    
    // Music and Sfx Constants
    public const string MusicToggledOn = "MusicToggledOn";
    public const string SfxToggledOn = "SfxToggledOn";
    public const string MusicVolumeValue = "MusicVolumeValue";
    public const string SfxVolumeValue = "SfxVolumeValue";
    
    // Scene Names
    public const string MainMenuScene = "MainMenu";
    public const string GameScene = "Game";
}