using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Server;
using Vintagestory.API.Config;
using Vintagestory.API.Common;

namespace BoatDecay;

public class BoatDecayModSystem : ModSystem
{
    private const string HarmonyId = "boatdecay";
    private Harmony harmony;
    // Called on server and client
    // Useful for registering block/entity classes on both sides
    public override void Start(ICoreAPI api)
    {
        api.RegisterEntityBehaviorClass("boatdecay:boatdecay", typeof(BehaviorBoatDecay));
        this.harmony = new Harmony("boatdecay");
        SailingSpeedPatch.Apply(harmony, Mod.Logger);
    }

    public override void StartServerSide(ICoreServerAPI api)
    {
        base.StartServerSide(api);
    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        base.StartClientSide(api);
    }
}