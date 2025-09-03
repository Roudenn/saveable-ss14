using Content.Shared.FixedPoint;
using Content.Shared.Mobs;
using Robust.Shared.Network;
using Robust.Shared.Serialization;

namespace Content.Server.KillTracking;

/// <summary>
/// This is used for entities that track player damage sources and killers.
/// </summary>
[RegisterComponent, Access(typeof(KillTrackingSystem))]
public sealed partial class KillTrackerComponent : Component
{
    /// <summary>
    /// The mobstate that registers as a "kill"
    /// </summary>
    [DataField("killState")]
    public MobState KillState = MobState.Critical;

    /// <summary>
    /// A dictionary of sources and how much damage they've done to this entity over time.
    /// </summary>
    [DataField("lifetimeDamage")]
    public Dictionary<KillSource, FixedPoint2> LifetimeDamage = new();
}

[ImplicitDataDefinitionForInheritors]
public abstract partial record KillSource;

/// <summary>
/// A kill source for players
/// </summary>
public sealed partial record KillPlayerSource : KillSource
{
    [DataField("playerId")]
    public NetUserId PlayerId;

    public KillPlayerSource(NetUserId playerId)
    {
        PlayerId = playerId;
    }
}

/// <summary>
/// A kill source for non-player controlled entities
/// </summary>
public sealed partial record KillNpcSource : KillSource
{
    [DataField("npcEnt")]
    public NetEntity NpcEnt;

    public KillNpcSource(NetEntity npcEnt)
    {
        NpcEnt = npcEnt;
    }
}

/// <summary>
/// A kill source for kills with no damage origin
/// </summary>
public sealed partial record KillEnvironmentSource : KillSource;
