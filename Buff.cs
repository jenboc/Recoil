using System;
using System.Collections.Generic;
using System.Text;

namespace Recoil
{
    abstract class Buff
    {
        public float Duration { get; set; }
        public float TimeActive { get; set; }

        public Buff(float duration)
        {
            Duration = duration;
            TimeActive = 0;
        }

        public bool Update(float elapsedTime, Player player)
        {
            TimeActive += elapsedTime;

            if (TimeActive >= Duration)
            {
                RemoveEffect(player);
                return true;
            }
            return false;
        }

        public abstract void AddEffect(Player player);
        public abstract void RemoveEffect(Player player);
    }

    class AntiGravBuff : Buff
    {
        public float GravityChange { get; set; }
        public float GravityBefore { get; set; }

        public AntiGravBuff(float duration, float newGravValue) : base(duration)
        {
            GravityChange = newGravValue;
        }

        public override void AddEffect(Player player)
        {
            GravityBefore = player.gravityValue;
            player.gravityValue = GravityChange;

            TimeActive = 0;
        }

        public override void RemoveEffect(Player player)
        {
            player.gravityValue = GravityBefore;
        }
    }
}
