using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogeBeats
{
    public static class Player
    {
        public static readonly int MAX_HEALTH = 10;

        public static int CurrentHealth { get; set; }

        static Player()
        {
            CurrentHealth = MAX_HEALTH;
        }

        public static void Hurt(int damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth < 0)
            {
                CurrentHealth = 0;
                Kill();
            }
        }

        private static void Kill()
        {
            
        }

        public static void Heal(int heal)
        {
            CurrentHealth += heal;
            if (CurrentHealth > MAX_HEALTH)
                CurrentHealth = MAX_HEALTH;
        }
    }
}
