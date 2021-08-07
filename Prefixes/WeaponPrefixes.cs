using Terraria;
using Terraria.ModLoader;

namespace RothurMod.Prefixes
{
    public class BoringPrefix : ModPrefix 
    {
        public override void SetDefaults() 
        {
            DisplayName.SetDefault("Boring");
        }
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult = 1.07f;
            useTimeMult = 1.12f;
        }
    }
    public class SinisterPrefix : ModPrefix 
    {
        public override void SetDefaults() 
        {
            DisplayName.SetDefault("Sinister");
        }
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult = 1.10f;
            critBonus = 8;
            useTimeMult = .88f;
        }
    }
    public class DisgustingPrefix : ModPrefix 
    {
        public override void SetDefaults() 
        {
            DisplayName.SetDefault("Disgusting");
        }
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult = .88f;
            critBonus = -8;
        }
    }
    public class BloodthirstyPrefix : ModPrefix 
    {
        public override void SetDefaults() 
        {
            DisplayName.SetDefault("Bloodthirsty");
        }
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult = .88f;
            critBonus = 20;
        }
    }
}