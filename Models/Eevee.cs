﻿namespace PokemonPocket
{
    public class Eevee:Pokemon
    {
        public Eevee(int Hp, int Exp) : base(Hp, Exp)
        {
            Name = "Eevee";
            Skill = "Run Away";
            Damage = 20;
        }

        public void calculateDamage(int damage_taken)
        {
            this.Hp = this.Hp - (damage_taken * 2);
        }
    }
}
