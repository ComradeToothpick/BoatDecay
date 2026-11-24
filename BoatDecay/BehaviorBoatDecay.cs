using Vintagestory.API.Common.Entities;

namespace BoatDecay;

public class BehaviorBoatDecay : EntityBehavior
{
    private float timeSinceLastDamage = 0.0f;
    private EntityPos lastPos;
    private const float timeInterval = 10.0f;
    private const float decayDistance = 30.0f;
    
    public BehaviorBoatDecay(Entity entity) : base(entity)
    {
    }
    
    
    private float GetDurability()
    {
        return entity.WatchedAttributes.GetFloat("boatdecay:durability", 1.0f);
    }

    public float GetMaxDurabilityRatio()
    {
        return GetDurability() / GetMaxDurability();
    }
    
    private float GetMaxDurability()
    {
        return entity.WatchedAttributes.GetFloat("boatdecay:maxdurability", 1.0f);
    }

    private void SetDurability(float Durability)
    {
        if (entity.WatchedAttributes == null || float.IsNaN(Durability) || Durability < 0.0)
            return;
        if (Durability > GetMaxDurability()) Durability = GetMaxDurability();
        entity.WatchedAttributes.SetFloat("boatdecay:durability", Durability);
    }
    
    private void SetMaxDurability(float Durability)
    {
        if (entity.WatchedAttributes == null || float.IsNaN(Durability) || Durability < 0.0)
            return;
        entity.WatchedAttributes.SetFloat("boatdecay:maxdurability", Durability);
    }
    
    private float GetDurabilityDecay()
    {
        return entity.WatchedAttributes.GetFloat("boatdecay:durabilitydecay", 0.0f);
    }
    
    private void SetDurabilityDecay(float DurabilityDecay)
    {
        if (entity.WatchedAttributes == null || float.IsNaN(DurabilityDecay) || DurabilityDecay < 0.0)
            return;
        entity.WatchedAttributes.SetFloat("boatdecay:durabilitydecay", DurabilityDecay);
    }

    private void DecayDurability()
    {
        SetDurability(GetDurability() - GetDurabilityDecay());
    }

    public override void FromBytes(bool isSync)
    {
        base.FromBytes(isSync);
    }

    public override void ToBytes(bool forClient)
    {
        base.ToBytes(forClient);
    }

    public override void OnGameTick(float deltaTime)
    {
        timeSinceLastDamage -= deltaTime;
        if (timeSinceLastDamage <= 0.0f)
        {
            if (lastPos == null) lastPos = entity.Pos;
            if (entity.Pos.HorDistanceTo(lastPos) > decayDistance)
            {
                lastPos = entity.Pos;
                DecayDurability();
            }
            timeSinceLastDamage = timeInterval;
        }
        base.OnGameTick(deltaTime);
    }

    public override string PropertyName()
    {
        return "BoatDecay";
    }
}